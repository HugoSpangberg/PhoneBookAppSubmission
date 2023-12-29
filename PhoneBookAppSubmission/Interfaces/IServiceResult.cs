using PhoneBookAppSubmission.Enums;

namespace PhoneBookAppSubmission.Interfaces
{
    
    public interface IServiceResult
    {
        object Result { get; set; }
        ServiceStatus Status { get; set; }
    }
}