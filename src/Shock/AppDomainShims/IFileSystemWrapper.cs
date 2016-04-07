namespace Shock.AppDomainShims
{
    public interface IFileSystemWrapper
    {
        string[] DirectoryGetFiles(string path = "");
        bool Exists(string path);
    }
}