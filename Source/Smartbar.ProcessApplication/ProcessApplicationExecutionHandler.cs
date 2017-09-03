namespace JanHafner.Smartbar.ProcessApplication
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Model;
    using JanHafner.Toolkit.Windows;
    using JetBrains.Annotations;

    [Export(typeof(IApplicationExecutionHandler))]
    public class ProcessApplicationExecutionHandler : IApplicationExecutionHandler
    {
        public virtual Boolean CanExecute(Application application)
        {
            return application is ProcessApplication;
        }

        public virtual void Execute(Application application)
        {
            var processApplication = application as ProcessApplication;
            if (processApplication == null)
            {
                throw new ArgumentException("Invalid argument supplied.", nameof(application));
            }

            var password = processApplication.GetPassword();
            var processStartInfo = new ProcessStartInfo
            {
                WorkingDirectory = processApplication.WorkingDirectory,
                Arguments = processApplication.Arguments,
                WindowStyle = processApplication.WindowStyle,
                UseShellExecute = String.IsNullOrEmpty(processApplication.Username) || password == null
            };

            if (!processStartInfo.UseShellExecute)
            {
                processStartInfo.UserName = processApplication.Username;
                processStartInfo.Password = password;
            }

            if (processStartInfo.UseShellExecute)
            {
                processStartInfo.FileName = processApplication.Execute;
            }
            else
            {
                var executable = SafeNativeMethods.FindExecutable(processApplication.Execute);
                if (executable.Equals(processApplication.Execute))
                {
                    processStartInfo.FileName = processApplication.Execute;
                }
                else
                {
                    processStartInfo.FileName = executable;
                    processStartInfo.Arguments = $"\"{processApplication.Execute}\" {processApplication.Arguments}";
                }
            }

            Process process;
            try
            {
                process = Process.Start(processStartInfo);
            }
            catch (Win32Exception win32Exception)
            {
                if (win32Exception.NativeErrorCode == (Int32)NativeProcessStartErrorCode.Cancelled)
                {
                    return;
                }

                throw;
            }

            if (process != null)
            {
                this.ConfigurePostExecution(processApplication, process);
            }
        }

        private void ConfigurePostExecution([NotNull] ProcessApplication processApplication, [NotNull] Process process)
        {
            if (process.HasExited)
            {
                return;
            }

            if (process.PriorityClass != processApplication.Priority)
            {
                process.PriorityClass = processApplication.Priority;
            }

            if (processApplication.ProcessAffinityMask.HasValue && processApplication.ProcessAffinityMask.Value > 0)
            {
                process.ProcessorAffinity = new IntPtr(processApplication.ProcessAffinityMask.Value);
            }
        }
    }
}
