using System.IO;

namespace DigiX.Web.Global.Helpers
{
    public class Utilities
    {
        public static string ReadFileContent(string filePath, string fileName)
        {
            return File.ReadAllText(string.Format("{0}{1}", filePath.EndsWith("/") ? filePath : (filePath + "/"), fileName));
        }

        public static void WriteFileContent(string filePath, string fileName, string fileContent)
        {
            File.WriteAllText(string.Format("{0}{1}", filePath.EndsWith("/") ? filePath : (filePath + "/"), fileName), fileContent);
        }
    }
}
