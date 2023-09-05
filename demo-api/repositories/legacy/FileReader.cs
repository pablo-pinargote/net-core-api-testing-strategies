using System.IO;

namespace demo_api.repositories.legacy
{
    public interface IFileReader
    {
        string Read(string filepath);
    }
    
    public class FileReader: IFileReader
    {
        public string Read(string filepath)
        {
            return File.ReadAllText(filepath);
        }
    }
}