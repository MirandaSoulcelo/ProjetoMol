using System.IdentityModel.Tokens.Jwt; //fornece a funcionalidade de criar e manipular tokens JWT
using System.Security.Claims; // permite trabalhar com "claims" (informações embutidas no token), não posso esquecer.
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TheProject.Domain.Entities; // define estruturas e classes para criptografia e segurança dos tokens.

public class TokenService
{
    private readonly string _secretKey;
    public TokenService(string secretKey)
    {
        if (string.IsNullOrWhiteSpace(secretKey) || secretKey.Length < 16)
            throw new ArgumentException("SecretKey JWT inválida. Ela deve ter pelo menos 16 caracteres.");

        _secretKey = secretKey;
    }

    public string GetToken(Users user)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes(_secretKey);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new Claim[]
        {
            new Claim("user", $"{{\"name\":\"{user.Name}\",\"email\":\"{user.Email}\"}}")
        }),
        Expires = DateTime.UtcNow.AddMinutes(15),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}
 }