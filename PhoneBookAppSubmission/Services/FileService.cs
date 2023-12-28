using System.Diagnostics;

namespace PhoneBookAppSubmission.Services;


public interface IFileService
{

    bool SaveContentToFile (string content);
    string GetContentFromFile();
}

public class FileService(string filePath) : IFileService
{
   
    private readonly string _filePath = filePath;

    public bool SaveContentToFile(string content)
    {
        try
        {

            using (var sw = new StreamWriter(_filePath)) 
            {
                sw.WriteLine(content);
            }
            return true;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;

    }
    public string GetContentFromFile()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                using var sr = new StreamReader(_filePath);
                return sr.ReadToEnd();
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }


}
