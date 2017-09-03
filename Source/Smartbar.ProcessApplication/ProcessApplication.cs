namespace JanHafner.Smartbar.ProcessApplication
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Security;
    using System.Security.Cryptography;
    using System.Windows.Input;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Model;
    using JanHafner.Toolkit.Windows.HotKey;
    using JetBrains.Annotations;

    public sealed class ProcessApplication : Application, IApplicationWithImage, ICloneable
    {
        private ProcessApplication()
        {
        }

        internal ProcessApplication(Guid id, [NotNull] String execute, [NotNull] String name)
            : base(id)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (String.IsNullOrWhiteSpace(execute))
            {
                throw new ArgumentNullException(nameof(execute));    
            }

            this.Priority = ProcessPriorityClass.Normal;
            this.Name = name;
            this.Execute = execute;
            this.HotKeyModifier = HotKeyModifier.None;
            this.HotKey = Key.None;
        }

        [Required]
        [CanBeNull]
        public String Name { get; private set; }

        [Required]
        [CanBeNull]
        public String Execute { get; private set; }

        [CanBeNull]
        public String WorkingDirectory { get; private set; }

        [CanBeNull]
        public String Arguments { get; private set; }

        public ProcessPriorityClass Priority { get; private set; }

        [CanBeNull]
        public String Username { get; private set; }

        [CanBeNull]
        public Int64? ProcessAffinityMask { get; private set; }

        public ProcessWindowStyle WindowStyle { get; private set; }

        [CanBeNull]
        public Byte[] Password { get; private set; }

        public Boolean StretchSmallImage { get; private set; }

        public HotKeyModifier HotKeyModifier { get; private set; }

        public Key HotKey { get; private set; }

        public Boolean HasHotKey
        {
            get { return this.HotKey != Key.None && this.HotKeyModifier != HotKeyModifier.None; }
        }

        [CanBeNull]
        public SecureString GetPassword()
        {
            if (this.Password == null || this.Password.Length == 0)
            {
                return null;
            }

            var data = this.Password;

            try
            {
                data = ProtectedData.Unprotect(data, new Byte[0], DataProtectionScope.LocalMachine);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }

            var securePassword = System.Text.Encoding.Default.GetString(data).ToSecureString();
            securePassword.MakeReadOnly();

            return securePassword;
        }

        private void SetPassword([CanBeNull] SecureString password)
        {
            if (password == null || password.Length == 0)
            {
                this.Password = null;
                return;
            }

            var data = System.Text.Encoding.Default.GetBytes(password.FromSecureString());

            try
            {
                this.Password = ProtectedData.Protect(data, new Byte[0], DataProtectionScope.LocalMachine);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        [Required]
        [NotNull]
        public ApplicationImage Image { get; private set; }

        public void UpdateImage(ApplicationImage applicationImage)
        {
            if (applicationImage == null)
            {
                throw new ArgumentNullException(nameof(applicationImage));
            }

            if (this.Image != null && this.Image.GetType() == applicationImage.GetType())
            {
                this.Image.Update(applicationImage);
            }
            else
            {
                this.Image = applicationImage;
            }
        }

        public void DeleteImage()
        {
            this.Image = null;
        }

        public void Update([NotNull] String path, [CanBeNull] String workingDirectory, [CanBeNull] String arguments, ProcessPriorityClass priority, [CanBeNull] Int64? processAffinityMask, Boolean stretchSmallImage, ProcessWindowStyle windowStyle)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            this.StretchSmallImage = stretchSmallImage;
            this.Execute = path;
            this.Priority = priority;
            this.WindowStyle = windowStyle;
            this.WorkingDirectory = workingDirectory;
            this.Arguments = arguments;
            this.ProcessAffinityMask = processAffinityMask;
        }

        public void UpdateImpersonation([CanBeNull] String username, [CanBeNull] SecureString password)
        {
            this.Username = username;
            this.SetPassword(password);
        }

        public void UpdateHotKey(HotKeyModifier hotKeyModifier, Key hotKey)
        {
            this.HotKeyModifier = hotKeyModifier;
            this.HotKey = hotKey;
        }

        public void Rename([NotNull] String name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
        }

        public Object Clone()
        {
            return new ProcessApplication(Guid.NewGuid(), this.Execute, this.Name)
            {
                Arguments = this.Arguments,
                Password = this.Password,
                Priority = this.Priority,
                ProcessAffinityMask = this.ProcessAffinityMask,
                Username = this.Username,
                WorkingDirectory = this.WorkingDirectory,
                Image = (this.Image as ICloneable)?.Clone() as ApplicationImage,
                StretchSmallImage = this.StretchSmallImage
            };
        }
    }
}
