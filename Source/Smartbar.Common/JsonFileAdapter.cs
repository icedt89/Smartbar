namespace JanHafner.Smartbar.Common
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class JsonFileAdapter
    {
        protected async Task SaveFileToDiskSafeAsync(String file, String content)
        {
            var backupFileName = $"{file}.bak";

            using (var streamWriter = new StreamWriter(backupFileName, false, Encoding.Unicode))
            {
                await streamWriter.WriteAsync(content);
                await streamWriter.FlushAsync();
            }

            File.Replace(backupFileName, file, null);
        }
    }
}