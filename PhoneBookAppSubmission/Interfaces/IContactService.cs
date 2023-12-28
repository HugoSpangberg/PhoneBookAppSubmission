using PhoneBookAppSubmission.Models;

namespace PhoneBookAppSubmission.Interfaces;

public interface IContactService
{
    IServiceResult AddContactToList(ContactUser contact);
    IServiceResult GetContactFromList(string email);
    IEnumerable<IContactUser> GetAllContactsFromList();
    IServiceResult DeleteContactFromListbyEmail(string email);


}
