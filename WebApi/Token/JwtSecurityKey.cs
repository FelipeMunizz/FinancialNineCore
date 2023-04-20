using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi.Token;

public class JwtSecurityKey
{
    public static SymmetricSecurityKey Create(string secret) => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
}
