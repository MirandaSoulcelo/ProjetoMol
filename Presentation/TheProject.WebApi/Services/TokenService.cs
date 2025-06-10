using System;
using System.IdentityModel.Tokens.Jwt; //fornece a funcionalidade de criar e manipular tokens JWT
using System.Security.Claims; // permite trabalhar com "claims" (informações embutidas no token), não posso esquecer.
using System.Text;
using Microsoft.IdentityModel.Tokens; // define estruturas e classes para criptografia e segurança dos tokens.

public class TokenService
{
    private readonly string _secretKey;

    public TokenService(string secretKey)
    {
        _secretKey = secretKey;
    }


    public string GetToken(string nome, string email)
    {

        //Cria um manipulador de tokens. Ele que vai criar e serializar (converter para string) o token.
        var tokenHandler = new JwtSecurityTokenHandler();

        //aqui converte para um array de bytes
        var key = Encoding.UTF8.GetBytes(_secretKey); ;

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            //aqui eu to definindo como o token será criado
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("user_name", nome),
                new Claim("user_email", email)

            }),

            Expires = DateTime.UtcNow.AddMinutes(15),

            // define o algoritmo de assinatura do token (HMAC-SHA256) e a chave secreta.
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        //criando uum token com base no descriptor
        var token = tokenHandler.CreateToken(tokenDescriptor);

        //se não me engano WriteToken serve pra converter pra string
        return tokenHandler.WriteToken(token);
    }
 }