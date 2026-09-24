namespace AerationSterilize.Application.Abstractions;
public interface ICurrentUserService
{
    Guid? UserId { get; }
    Guid UserIdRequired { get; }
    string UserName { get; }
    bool IsAuthenticated { get; }
}
