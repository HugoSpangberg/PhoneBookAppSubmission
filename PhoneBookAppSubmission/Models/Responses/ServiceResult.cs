using PhoneBookAppSubmission.Enums;
using PhoneBookAppSubmission.Interfaces;

namespace PhoneBookAppSubmission.Models.Responses;

public class ServiceResult : IServiceResult
{
    public ServiceStatus Status { get; set; }
    public object Result { get; set; } = null!;
}
