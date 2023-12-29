using Newtonsoft.Json;
using PhoneBookAppSubmission.Interfaces;
using PhoneBookAppSubmission.Models;
using PhoneBookAppSubmission.Models.Responses;
using System.Diagnostics;

namespace PhoneBookAppSubmission.Services;

public class ContactService : IContactService
{
    private readonly IFileService _fileService = new FileService(@"C:\ProjectsCode\PhoneBookAppSubmission\content.json"); //Sökvägen för .json filen. Både spara ner och hämta.
    private static List<ContactUser> _contacts = []; //Listan för där kontakterna läggs till i listan.

    //-- När programmet startar så hämtar den .jsonfilen --
    public ContactService()
    {
        LoadContactsFromFile(); //Kallar på metoden för att hämta .json filen.
    }

    //-- Metod för att ladda kontakter från filen --
    private void LoadContactsFromFile() //Metod för att hämta .json filen. 
    {
        try
        {
            var content = _fileService.GetContentFromFile();
            if (!string.IsNullOrEmpty(content))
            {
                _contacts = JsonConvert.DeserializeObject<List<ContactUser>>(content)!; //lägger till .json i listan
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    //-- Lägger till en användare till listan. --
    public IServiceResult AddContactToList(ContactUser contact)
    {

        IServiceResult response = new ServiceResult(); 
        try
        {

            if (!IsValid(contact.FirstName) || !IsValid(contact.LastName) || !IsValid(contact.Email) || !IsValid(contact.PhoneNumber)) //Om namn, efternamn, email och telefonnummer ej uppfyller kraven så skriver programmet ut felmeddelande.
            {
                response.Status = Enums.ServiceStatus.FAILED;
                return response;
            }
            if (!_contacts.Any(x => x.Email == contact.Email)) 
            {
                _contacts.Add(contact);
                _fileService.SaveContentToFile(JsonConvert.SerializeObject(_contacts)); //Konventerar C# object till json
                response.Status = Enums.ServiceStatus.SUCCESSED;
            }
            else
            {
                response.Status = Enums.ServiceStatus.ALREADY_EXISTS; //Om användare med samma mail finns så skriver programmet ut felmeddelande.
            }
            
        }
        catch ( Exception ex ) 
        {
            Debug.WriteLine(ex.Message);
            response.Status = Enums.ServiceStatus.FAILED;
            response.Result = ex.Message;
        }
        return response;

    }

    //-- Ta bort en användare från listan och även i json filen. --
    public IServiceResult DeleteContactFromListbyEmail(string email)
    {
        IServiceResult response = new ServiceResult();
        try
        {
            var ContactDelete = _contacts.FirstOrDefault(x => x.Email == email);//Letar efter epost-adressen som användaren skrivit in.
           

            if (ContactDelete != null)
            {
                _contacts.Remove(ContactDelete);//Tar bort hela kontakten.
                _fileService.SaveContentToFile(JsonConvert.SerializeObject(_contacts));//Uppdaterar .json filen
                response.Status = Enums.ServiceStatus.DELETED;
            }
            else
            {
                response.Status= Enums.ServiceStatus.NOT_FOUND;//Om användaren skriver fel eller en e-post som inte finns så skrivs det ut att kontakten inte finns.
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = Enums.ServiceStatus.FAILED;
            response.Result = ex.Message;
        }
        return response;
    }

    //-- Hämtar alla kontakter från listan. --
    public IEnumerable<ContactUser> GetAllContactsFromList()
    {

        try
        {
            var content = _fileService.GetContentFromFile();//hämtar kontaker från .json filen.
            if (!string.IsNullOrEmpty(content)) 
            {
                _contacts = JsonConvert.DeserializeObject<List<ContactUser>>(content)!;//Konventerar .json till c# object

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);

        }
        return _contacts;
       
    }

    //-- Hämtar en kontakt från listan. --
    public IServiceResult GetContactFromList(string email)
    {
        IServiceResult response = new ServiceResult();
        try
        {
            var ContactFound = _contacts.FirstOrDefault(x => x.Email == email);//Letar efter epost-adressen som användaren skrivit in.

            if (ContactFound != null)
            {
                _contacts.Contains(ContactFound);
                response.Status = Enums.ServiceStatus.SUCCESSED;
                response.Result = ContactFound;
            }
            else
            {
                response.Status = Enums.ServiceStatus.NOT_FOUND;
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = Enums.ServiceStatus.FAILED;
            response.Result = ex.Message;
        }
        return response;
    }
    
    //-- Kod test för att lägga till kontakter i en lista. --
    public bool AddTestContactToList(IContactUser contact)
    {
        try
        {
            _contacts.Add((ContactUser)contact);
            return true;
        }
        catch (Exception ex){ Debug.WriteLine(ex.Message); } 
        return false;
    }

    private bool IsValid(string contact)
    {

        if (string.IsNullOrEmpty(contact)) //Kontrollerar om användaren inte skrivit in något så retunerar det false.
        {
            return false;
        }

        
        if (contact.Trim().Length == 1)// Kontrollera om användaren har endast skrivit in en bokstav. 
        {
            return false;
        }
        return true;
    }
}
