using PhoneBookAppSubmission.Interfaces;
using PhoneBookAppSubmission.Models;
using System.Runtime.CompilerServices;

namespace PhoneBookAppSubmission.Services;

public interface IMenuService
{
    void ShowMainMenu();

}

public class MenuService : IMenuService
{
    private readonly IFileService _fileService = new FileService(@"C:\ProjectsCode\PhoneBookAppSubmission\content.json");
    private IContactService _contactService = new ContactService();
    


    public void ShowMainMenu()
    {
        while (true)
        {
            DisplayMenuTitle("ADDRESSBOOK APPLICATION");
            Console.WriteLine("\t[1]\t Add Contact");
            Console.WriteLine("\t[2]\t View All contacts");
            Console.WriteLine("\t[3]\t View contact");

            Console.WriteLine("\t[4]\t Delete Contact");
            Console.WriteLine("\t[0]\t Exit");


            Console.Write("\nEnter Menu Option: ");

            var menuOption = Console.ReadLine();

            switch (menuOption)
            {
                case "1":
                    ShowAddContact();
                    break;
                case "2":
                    ShowViewAllContacts();
                    break;
                case "3":
                    ShowSearchContact();
                    break;
                case "4":
                    ShowDeleteContact();
                    break;
                case "0":
                    ShowExitApplication();
                    break;
                default:
                    Console.WriteLine("Invalid option selected. Please try again");
                    Console.ReadKey();
                    break;
            }

        }
    }
    private void ShowAddContact()
    {
        ContactUser contact = new ContactUser();
        DisplayMenuTitle("Add new Contact");
        Console.Write("First Name: ");
        contact.FirstName = Console.ReadLine()!.Trim();

        Console.Write("Last Name: ");
        contact.LastName = Console.ReadLine()!.Trim();

        Console.Write("Email: ");
        contact.Email = Console.ReadLine()!.Trim().ToLower();

        Console.Write("Phone Number: ");
        contact.PhoneNumber = "0706840803";

        Console.Write("Country: ");
        contact.Country = "Sverige";

        Console.Write("City: ");
        contact.City = "Växjö";

        Console.Write("Street Name: ");
        contact.Street = "Sandgärdsgatan 42";

        Console.Write("Postal Code: ");
        contact.PostalCode = "35232";
        Console.WriteLine();

        IServiceResult result = _contactService.AddContactToList(contact);

        switch (result.Status)
        {
            case Enums.ServiceStatus.SUCCESSED:
                Console.WriteLine("Contact successfully created");
                break;
            case Enums.ServiceStatus.FAILED:
                Console.WriteLine("Something went wrong");
                break;
            case Enums.ServiceStatus.ALREADY_EXISTS:
                Console.WriteLine($"Contact with the same Email: <{contact.Email}> Already exist");
                break;
        }

        Console.ReadKey();
    }

    private void ShowDeleteContact()
    {
        {
            DisplayMenuTitle("Delete a Contact by Email");
            Console.Write("Write the email of the contact you want to delete: ");
            var email = Console.ReadLine()!;

            var result = _contactService.DeleteContactFromListbyEmail(email);


            switch (result.Status)
            {
                case Enums.ServiceStatus.DELETED:
                    Console.WriteLine($"Contact with email <{email}> has been deleted.");
                    break;
                case Enums.ServiceStatus.FAILED:
                    Console.WriteLine("Failed.");
                    break;
                default:
                    Console.WriteLine($"Contact not found. Check if <{email}> was spelled correctly.");
                    break;
            }

            Console.ReadKey();
        }

    }


    private void ShowViewAllContacts()
    {
        Console.Clear();
        var res = _contactService.GetAllContactsFromList();
        DisplayMenuTitle("View All Contacts");
        if(res is List<ContactUser> contactList) 
        {
            
            foreach (var contact in contactList)
            {
                string UpperFirstNameLetter = char.ToUpper(contact.FirstName[0]) + contact.FirstName.Substring(1).ToLower();
                string UpperLastNameLetter = char.ToUpper(contact.LastName[0]) + contact.LastName.Substring(1).ToLower();
                string LowerEmailWord = contact.Email.ToLower();


                ContactCard(UpperFirstNameLetter, UpperLastNameLetter, LowerEmailWord, contact.PhoneNumber,null!,null!,null!,null!);
                
            }
            Console.Write("View more details? y/n: ");
            var detailOption = Console.ReadLine() ?? "";
            if (detailOption.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                Console.Clear();
                DisplayMenuTitle("View All Contacts");
                foreach (var contact in contactList)
                {
                    string UpperFirstNameLetter = char.ToUpper(contact.FirstName[0]) + contact.FirstName.Substring(1).ToLower();
                    string UpperLastNameLetter = char.ToUpper(contact.LastName[0]) + contact.LastName.Substring(1).ToLower();
                    string LowerEmailWord = contact.Email.ToLower();
                    string UpperCountryWord = contact.Country.ToUpper();
                    string UpperCityLetter = char.ToUpper(contact.City[0]) + contact.City.Substring(1).ToLower();
                    string UpperStreetWord = contact.Street.ToUpper();

                    ContactCard(UpperFirstNameLetter, UpperLastNameLetter, LowerEmailWord, contact.PhoneNumber, UpperCountryWord, UpperCityLetter, UpperStreetWord, contact.PostalCode);   
                }
                Console.Write("Press any key to continue");
                Console.ReadKey();
            }
        }
    }

    private void ShowSearchContact()
    {
        
        DisplayMenuTitle("SEARCH CONTACT");
        Console.Write("Enter full Name: ");
        var email = Console.ReadLine()!.ToLower().Trim();
        var res = _contactService.GetContactFromList(email);

        if (res.Status == Enums.ServiceStatus.SUCCESSED)
        {
            if (res.Result is IContactUser contact)
            {
                string UpperFirstNameLetter = char.ToUpper(contact.FirstName[0]) + contact.FirstName.Substring(1).ToLower();
                string UpperLastNameLetter = char.ToUpper(contact.LastName[0]) + contact.LastName.Substring(1).ToLower();
                string LowerEmailWord = contact.Email.ToLower();
                string UpperCountryWord = contact.Country.ToUpper();
                string UpperCityLetter = char.ToUpper(contact.City[0]) + contact.City.Substring(1).ToLower();
                string UpperStreetWord = contact.Street.ToUpper();

                ContactCard(UpperFirstNameLetter, UpperLastNameLetter, LowerEmailWord, contact.PhoneNumber, UpperCountryWord, UpperCityLetter, UpperStreetWord, contact.PostalCode);

                Console.Write("Press any key to continue");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("No contact found.");
                Console.Write("Press any key to continue");
                Console.ReadKey();
            }
        }
        else if (res.Status == Enums.ServiceStatus.NOT_FOUND)
        {
            Console.WriteLine("No contact found.");
            Console.Write("Press any key to continue");
            Console.ReadKey();
        }
        
    }

    private void ShowExitApplication()
    {
        DisplayMenuTitle("Exit Application");
        Console.Write("Exit application y/n: ");
        var exitOption = Console.ReadLine() ?? "";

        if (exitOption.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            Environment.Exit(0);
        }
    }


    private void DisplayMenuTitle (string title)
    {
        Console.Clear();
        Console.WriteLine($"\t#--- {title} ---#");
        Console.WriteLine();
    }

    private void ContactCard(string FirstName, string LastName, string Email, string PhoneNumber, string Country, string City, string Street, string PostalCode)
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine();
        Console.WriteLine($"{FirstName} {LastName}");
        Console.WriteLine($"<{Email}> | {PhoneNumber}");
        Console.Write($"{Country} {City} ");
        Console.Write($"{Street} {PostalCode}");
        Console.WriteLine();
        Console.WriteLine();

    }
}
