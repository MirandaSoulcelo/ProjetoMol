using System.Security.Principal;
using MediatR;
using TheProject.Application.Handlers.Auth;




public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly TokenService _tokenService;



    public LoginHandler(IUsuarioRepository usuarioRepository, TokenService tokenService)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;

    }



    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
            throw new ArgumentException("Email ou senha não foram informados :( ");


        var usuario = await _usuarioRepository.ObterPorEmailESenhaAsync(request.Email, request.Senha);

        if (usuario == null)
            throw new Exception("Nenhum usuário encontrado");


        var token = _tokenService.GetToken(usuario.Nome, usuario.Email);

        return new LoginResponse { Token = token };
    }
}