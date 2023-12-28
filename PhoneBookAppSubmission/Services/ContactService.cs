using Newtonsoft.Json;
using PhoneBookAppSubmission.Interfaces;
using PhoneBookAppSubmission.Models;
using PhoneBookAppSubmission.Models.Responses;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;


namespace PhoneBookAppSubmission.Services;

public class ContactService : IContactService
{
    private readonly IFileService _fileService = new FileService(@"C:\ProjectsCode\PhoneBookAppSubmission\content.json");
    private static List<ContactUser> _contacts = [];
    public IServiceResult AddContactToList(ContactUser contact)
    {

        IServiceResult response = new ServiceResult();
        try
        {
            if(!_contacts.Any(x => x.Email == contact.Email)) 
            {
                _contacts.Add(contact);
                _fileService.SaveContentToFile(JsonConvert.SerializeObject(_contacts));
                response.Status = Enums.ServiceStatus.SUCCESSED;
            }
            else
            {
                response.Status = Enums.ServiceStatus.ALREADY_EXISTS;
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

    public IServiceResult DeleteContactFromListbyEmail(string email)
    {
        IServiceResult response = new ServiceResult();
        try
        {
            var ContactDelete = _contacts.FirstOrDefault(x => x.Email == email);

            if (ContactDelete != null)
            {
                _contacts.Remove(ContactDelete);
              
                response.Status = Enums.ServiceStatus.DELETED;
            }
            else
            {
                response.Status= Enums.ServiceStatus.NOT_FOUND;
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



    public IEnumerable<IContactUser> GetAllContactsFromList()
    {

        try
        {
            var content = _fileService.GetContentFromFile();
            if (!string.IsNullOrEmpty(content)) 
            {
                _contacts = JsonConvert.DeserializeObject<List<ContactUser>>(content)!;

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);

        }
        return _contacts;
       
    }

    public IServiceResult GetContactFromList(string email)
    {
        IServiceResult response = new ServiceResult();
        try
        {
            var ContactFound = _contacts.FirstOrDefault(x => x.Email == email);

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


}
