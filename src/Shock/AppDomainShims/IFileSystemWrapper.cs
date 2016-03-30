namespace Shock.AppDomainShims
{
    public interface IFileSystemWrapper
    {
        string[] DirectoryGetFiles(string path = "");
    }
}