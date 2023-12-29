using PhoneBookAppSubmission.Interfaces;
using PhoneBookAppSubmission.Models;

namespace PhoneBookAppSubmission.Services;

public interface IMenuService
{
    void ShowMainMenu();
}

public class MenuService : IMenuService
{
    
    private IContactService _contactService = new ContactService(); // En instans av ContactService.

    public void ShowMainMenu()
    {
        while (true)
        {
            //===============MENY======================

            DisplayMenuTitle("ADDRESSBOOK APPLICATION");
            Console.WriteLine("\t[1]\t Add Contact");
            Console.WriteLine("\t[2]\t View All contacts");
            Console.WriteLine("\t[3]\t View contact");
            Console.WriteLine("\t[4]\t Delete Contact");
            Console.WriteLine("\t[0]\t Exit");


            Console.Write("\n\tEnter Menu Option: ");

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
                    Console.WriteLine("\tInvalid option selected. Please try again");
                    Console.ReadKey();
                    break;
            }

        }
    }
    private void ShowAddContact()
    {
        //Användaren kan lägga in en kontakt till listan

        ContactUser contact = new ContactUser();
        DisplayMenuTitle("Add new Contact");
        Console.WriteLine("\tRequired fields (*) must be filled in.\n");

        Console.Write("\tFirst Name*: ");
        contact.FirstName = Console.ReadLine()!.Trim();

        Console.Write("\tLast Name*: ");
        contact.LastName = Console.ReadLine()!.Trim();

        Console.Write("\tEmail*: ");
        contact.Email = Console.ReadLine()!.Trim().ToLower();

        Console.Write("\tPhone Number*: ");
        contact.PhoneNumber = Console.ReadLine()!.Trim();

        Console.Write("\tCountry: ");
        contact.Country = Console.ReadLine()!.Trim();

        Console.Write("\tCity: ");
        contact.City = Console.ReadLine()!.Trim();

        Console.Write("\tStreet Name and Number: ");
        contact.Street = Console.ReadLine()!.TrimStart().TrimEnd();

        Console.Write("\tPostal Code: ");
        contact.PostalCode = Console.ReadLine()!;
        Console.WriteLine();

        IServiceResult result = _contactService.AddContactToList(contact);

        switch (result.Status) //Felmeddelanden till ifall det lyckades, inte lyckades och kontakten finns redan.
        {
            case Enums.ServiceStatus.SUCCESSED:
                Console.WriteLine("\tContact successfully created!");
                PressAnyKey();
                break;
            case Enums.ServiceStatus.FAILED:
                Console.WriteLine("\tYou must input at least two values in required fields (*)");
                PressAnyKey();
                break;
            case Enums.ServiceStatus.ALREADY_EXISTS:
                Console.WriteLine($"\tContact with the same Email: <{contact.Email}> Already exist");
                PressAnyKey();
                break;
        }
    }

    private void ShowDeleteContact()
    {
        {
            //Användaren kan ta bort sin kontakt genom att ange epost-adressen. Detta tar även bort all annan info om kontakten. 

            DisplayMenuTitle("Delete a Contact by Email");
            Console.Write("\tWrite the email of the contact you want to delete: ");
            var email = Console.ReadLine()!;

            IServiceResult result = _contactService.DeleteContactFromListbyEmail(email);


            switch (result.Status) //Felmeddelanden till ifall det lyckades, inte lyckades och kontakten inte hittades.
            {
                case Enums.ServiceStatus.DELETED:
                    Console.WriteLine($"\tContact with email <{email}> has been deleted.");
                    break;
                case Enums.ServiceStatus.FAILED:
                    Console.WriteLine("\tFailed.");
                    break;
                default:
                    Console.WriteLine($"\tContact not found. Check if <{email}> was spelled correctly.");
                    break;
            }

            Console.ReadKey();
        }

    }

    //Här kan användaren se alla sina kontakter och även se detaljerade information om alla kontakter.
    private void ShowViewAllContacts()
    {
        Console.Clear();
        var res = _contactService.GetAllContactsFromList();
        DisplayMenuTitle("View All Contacts");
        if(res is List<ContactUser> contactList) 
        {
            
            //Loopar ut alla kontaker som finns i listan.
            foreach (var contact in contactList)
            {
                // Detta gör så att kontakten får en stor bokstav i början på namnet, efternamnet och gör email-adressen till små bokstäver

                string UpperFirstNameLetter = char.ToUpper(contact.FirstName[0]) + contact.FirstName.Substring(1).ToLower();
                string UpperLastNameLetter = char.ToUpper(contact.LastName[0]) + contact.LastName.Substring(1).ToLower();
                string LowerEmailWord = contact.Email.ToLower();

                //Metod som kallar på utskriftformat till programmet för användaren.
                ContactCardSmall(UpperFirstNameLetter, UpperLastNameLetter, LowerEmailWord, contact.PhoneNumber);
                
            }
            //Om användaren vill vill se detaljerad information om sina kontakter kan den skriva in y. Annars kan den bara trycka enter för att komma tillbaka till menyn. 
            Console.Write("\tView more details? y/n: ");
            var detailOption = Console.ReadLine() ?? "";

            Console.Write("\f\u001bc\x1b[3J"); //Denna motsvarar Console.clear(). Men console.clear() tog ej bort scrollback buffer. Så saker som låg utanför det man såg i console rutan togs ej bort.
            DisplayMenuTitle("View All Contacts Details");

            if (detailOption.Equals("y", StringComparison.OrdinalIgnoreCase))
            {

                foreach (var contact in contactList)
                {
                    // Gör samma som foreach-loopen ovan. Men gör så att LAND, och GATA blir stora bokstäver och Stad blir stor vokstav i början.
                    string UpperFirstNameLetter = char.ToUpper(contact.FirstName[0]) + contact.FirstName.Substring(1).ToLower();
                    string UpperLastNameLetter = char.ToUpper(contact.LastName[0]) + contact.LastName.Substring(1).ToLower();
                    string LowerEmailWord = contact.Email.ToLower();

                    //På country, city och street så har jag lagt till null or empty då användaren inte nödvändigt behöver göra en kontakt med land stad och gata. Detta förhindrar att programmet craschar.
                    string UpperCountryWord = string.IsNullOrEmpty(contact.Country) ? "" : contact.Country.ToUpper();
                    string UpperCityLetter = string.IsNullOrEmpty(contact.City) ? "" : char.ToUpper(contact.City[0]) + contact.City.Substring(1).ToLower();
                    string UpperStreetWord = string.IsNullOrEmpty(contact.Street) ? "" : contact.Street.ToUpper();

                    //Metod som kallar på utskriftformat till programmet för användaren.
                    ContactCardFull(UpperFirstNameLetter, UpperLastNameLetter, LowerEmailWord, contact.PhoneNumber, UpperCountryWord, UpperCityLetter, UpperStreetWord, contact.PostalCode!);   
                }
                PressAnyKey();
            }
        }
    }

    private void ShowSearchContact()
    {
        //Användaren kan söka på e-post för att hitta en specifik kontakt.
        DisplayMenuTitle("SEARCH CONTACT");
        Console.Write("\tEnter Email: ");
        var email = Console.ReadLine()!.ToLower().Trim();
        IServiceResult res = _contactService.GetContactFromList(email);

        if (res.Status == Enums.ServiceStatus.SUCCESSED)
        {
            if (res.Result is IContactUser contact)
            {
                // Gör samma som foreach-loopen ovan men skriver bara ut en specifik kontakt.

                string UpperFirstNameLetter = char.ToUpper(contact.FirstName[0]) + contact.FirstName.Substring(1).ToLower();
                string UpperLastNameLetter = char.ToUpper(contact.LastName[0]) + contact.LastName.Substring(1).ToLower();
                string LowerEmailWord = contact.Email.ToLower();
                string UpperCountryWord = string.IsNullOrEmpty(contact.Country) ? "" : contact.Country.ToUpper();
                string UpperCityLetter = string.IsNullOrEmpty(contact.City) ? "" : char.ToUpper(contact.City[0]) + contact.City.Substring(1).ToLower();
                string UpperStreetWord = string.IsNullOrEmpty(contact.Street) ? "" : contact.Street.ToUpper();

                //Metod som kallar på utskriftformat till programmet för användaren.
                ContactCardFull(UpperFirstNameLetter, UpperLastNameLetter, LowerEmailWord, contact.PhoneNumber, UpperCountryWord, UpperCityLetter, UpperStreetWord, contact.PostalCode);

                PressAnyKey();
            }

        }
        //Om kontakten inte hittades så skriver programmet ut det.
        else if (res.Status == Enums.ServiceStatus.NOT_FOUND)
        {
            Console.WriteLine();
            Console.WriteLine($"\tContact not found. Check if <{email}> was spelled correctly.");
            PressAnyKey();
        }
        
    }

    private void ShowExitApplication()
    {
        //Användaren kan avsluta applicationen.
        DisplayMenuTitle("Exit Application");
        Console.Write("Exit application y/n: ");
        var exitOption = Console.ReadLine() ?? "";

        if (exitOption.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            Environment.Exit(0);
        }
    }

    // Metod för titlar till programmet.
    private void DisplayMenuTitle (string title)
    {
        Console.Clear();
        Console.WriteLine("");
        Console.WriteLine($"\t#--- {title} ---#");
        Console.WriteLine("");
    }
    // Metod för Press any key to continue.
    private void PressAnyKey()
    {
        Console.WriteLine("\t============================================================");
        Console.WriteLine();
        Console.Write("\tPress any key to continue");
        Console.ReadKey();
    }

    //Metod för kontakt kort. Innehåller INTE all information om en kontakt.
    private void ContactCardSmall(string FirstName, string LastName, string Email, string PhoneNumber)
    {

        Console.WriteLine("\t============================================================");
        Console.WriteLine("\t|");
        Console.WriteLine($"\t|\t{FirstName} {LastName}");
        Console.WriteLine($"\t|\t<{Email}> | {PhoneNumber}");
        Console.WriteLine("\t|");




    }

    //Metod för kontakt kort. Innehåller ALL information om en kontakt.
    private void ContactCardFull(string FirstName, string LastName, string Email, string PhoneNumber, string Country, string City, string Street, string PostalCode)
    {


        Console.WriteLine("\t============================================================");
        Console.WriteLine("\t|");
        Console.WriteLine($"\t|\t{FirstName} {LastName}");
        Console.WriteLine($"\t|\t<{Email}> | {PhoneNumber}");
        Console.WriteLine($"\t|\t{Country} {City} {Street}");
        Console.WriteLine($"\t|\t{PostalCode}");
        Console.WriteLine("\t|"); 
    }

}
