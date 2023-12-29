using PhoneBookAppSubmission.Interfaces;

namespace PhoneBookAppSubmission.Models;


//-- Container för att skapa en kontakt. --
public class ContactUser : IContactUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Street { get; set; } = null!;
    public string? City { get; set; } = null!;
    public string? PostalCode { get; set; } = null!;
    public string? Country { get; set; } = null!;
}
