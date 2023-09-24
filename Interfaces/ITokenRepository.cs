using Microsoft.AspNetCore.Identity;

namespace go_bus_backend.Interfaces;

public interface ITokenRepository
{
   string CreateJwtToken(IdentityUser user, List<string> roles);

}