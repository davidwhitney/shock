using System.IO;

namespace Shock.AppDomainShims
{
    /// <summary>
    /// Would much rather use System.Io.Abstractions but I want a tiny footprint
    /// </summary>
    public class FileSystemWrapper : IFileSystemWrapper
    {
        public string[] DirectoryGetFiles(string path = "")
        {
            path = string.IsNullOrWhiteSpace(path) ? Directory.GetCurrentDirectory() : path;
            return Directory.GetFiles(path);
        }
    }
}
