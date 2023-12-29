using PhoneBookAppSubmission.Models;

namespace PhoneBookAppSubmission.Interfaces;

public interface IContactService
{
    // Lägger till en kontakt i listan.
    IServiceResult AddContactToList(ContactUser contact);
    // Hämtar kontakt från listan via e-post.
    IServiceResult GetContactFromList(string email);
    // Hämtar alla kontaker via lista.
    IEnumerable<ContactUser> GetAllContactsFromList();
    // Tar bort kontakt via e-post.
    IServiceResult DeleteContactFromListbyEmail(string email);
    //Enhetstest för att lägga till en kontakt.
    bool AddTestContactToList(IContactUser contact);
}
