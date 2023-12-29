using System.Diagnostics;

namespace PhoneBookAppSubmission.Services;


public interface IFileService
{

    bool SaveContentToFile (string content);
    string GetContentFromFile();
}

public class FileService(string filePath) : IFileService
{
   
    private readonly string _filePath = filePath; //sökväg för filen.

    public bool SaveContentToFile(string content) //Sparar ner kontakten till .json.
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
    public string GetContentFromFile() //Hämtar .json filen.
    {
        try
        {
            if (File.Exists(_filePath)) //kontrollerar ifall filen finns.
            {
                using var sr = new StreamReader(_filePath);
                return sr.ReadToEnd();
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }


}
