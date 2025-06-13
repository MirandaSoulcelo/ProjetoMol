using Microsoft.AspNetCore.Mvc;
using TheProject.Application.DTOs.Auth;
using TheProject.Application.Interfaces;
using TheProject.Infrastructure.Services;

namespace TheProject.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsersInterface _usersService;
        private readonly TokenService _tokenService;

        public AuthController(IUsersInterface usersService, TokenService tokenService)
        {
            _usersService = usersService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                // Validar se os parâmetros são válidos
                if (!ModelState.IsValid)
                {
                    return BadRequest(new LoginResponseDTO
                    {
                        Success = false,
                        Message = "Dados inválidos",
                        Token = string.Empty
                    });
                }

                // Buscar usuário por email e senha
                var user = await _usersService.GetUserByEmailAndPasswordAsync(loginDto.Email, loginDto.Password);

                
                if (user == null)
                {
                    return Unauthorized(new LoginResponseDTO
                    {
                        Success = false,
                        Message = "Nenhum usuário encontrado",
                        Token = string.Empty
                    });
                }

                // Gerar token de autenticação
                var token = _tokenService.GetToken(user);

                return Ok(new LoginResponseDTO
                {
                    Success = true,
                    Message = "Login realizado com sucesso",
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new LoginResponseDTO
                {
                    Success = false,
                    Message = "Erro interno do servidor",
                    Token = string.Empty
                });
            }
        }
    }
}