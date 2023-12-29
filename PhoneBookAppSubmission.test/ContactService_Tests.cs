using PhoneBookAppSubmission.Interfaces;
using PhoneBookAppSubmission.Models;
using PhoneBookAppSubmission.Services;

namespace PhoneBookAppSubmission.test;

public class ContactService_Tests
{
    [Fact]
    public void AddContactToList_ShouldAddContactToList_ReturnTrue()
    {
        IContactUser contact = new ContactUser
        {
            //Skapar en kontakt för test
            FirstName = "Hugo",
            LastName = "Spångberg",
            Email = "Hugo@domain.com",
            PhoneNumber = "1234567890",
            Country = "Sverige",
            City = "Göteborg",
            Street = "Engogata 34",
            PostalCode = "123 45",

        };
        IContactService contactService = new ContactService();

        bool result = contactService.AddTestContactToList(contact);

        Assert.True(result);
    }
}
