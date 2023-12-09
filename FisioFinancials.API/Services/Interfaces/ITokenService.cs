using FisioFinancials.API.Config;
using FisioFinancials.Domain.Model.Entities;

namespace FisioFinancials.API.Services.Interfaces;

public interface ITokenService
{
    public string GenerateToken(User user, JwtBearerTokenSettings jwtBearerTokenSettings);
}
