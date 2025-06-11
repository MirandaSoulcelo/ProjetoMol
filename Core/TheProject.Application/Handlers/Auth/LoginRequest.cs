using MediatR;

namespace TheProject.Application.Handlers.Auth;

public class LoginRequest : IRequest<LoginResponse>
{
    public string Email { get; set; }
    public string Senha { get; set; }
}