namespace AerationSterilize.Application.Features.V1.Auth.Common.Dtos;
public sealed record UserDto
(
    Guid Id,
    string UserName,
    string FullName,
    string Email,
    IList<string> Roles
 );
