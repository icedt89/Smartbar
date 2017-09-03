namespace JanHafner.Smartbar.Common
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using JetBrains.Annotations;

    public static class PathUtilities
    {
        [NotNull]
        public static readonly String ExplorerExeName = "explorer.exe";

        [NotNull]
        public static String GetExplorer()
        {
            var explorerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), ExplorerExeName);
            if (!File.Exists(explorerPath))
            {
                throw new ExplorerExeNotFoundException(ExplorerExeName, explorerPath);
            }

            return explorerPath;
        }

        public static Boolean PathExists(String path)
        {
            return Directory.Exists(path) || File.Exists(path);
        }

        [NotNull]
        public static String GetDefaultDirectoryIcon(out Int32 iconIndex)
        {
            iconIndex = 0;
            var iconFile = GetExplorer();

            return iconFile;
        }

        [CanBeNull]
        public static String GetIdealFileDisplayName([NotNull] String file)
        {
            if (String.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentNullException(nameof(file));    
            }

            var name = FileVersionInfo.GetVersionInfo(file).FileDescription;
            if (String.IsNullOrWhiteSpace(name))
            {
                name = Path.GetFileName(file);
            }

            return name;
        }

        [CanBeNull]
        public static String GetIdealDirectoryDisplayName([NotNull] String directory)
        {
            if (String.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            return new DirectoryInfo(directory).Name;
        }

        [NotNull]
        public static String GetIdealDisplayName([NotNull] String something)
        {
            if (String.IsNullOrWhiteSpace(something))
            {
                throw new ArgumentNullException(nameof(something));
            }

            if (Directory.Exists(something))
            {
                return GetIdealDirectoryDisplayName(something);
            }
            else if (File.Exists(something))
            {
                return GetIdealFileDisplayName(something);
            }

            return something;
        }
    }
}
