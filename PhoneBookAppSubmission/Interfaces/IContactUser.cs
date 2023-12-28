
namespace PhoneBookAppSubmission.Interfaces
{
    public interface IContactUser
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string PhoneNumber { get; set; }
        string City { get; set; }
        string Country { get; set; }
        string PostalCode { get; set; }
        string Street { get; set; }
    }
}