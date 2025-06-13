using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheProject.Application.DTOs.UsersDTO;
using TheProject.Application.Interfaces;
using TheProject.Application.Validators.Users;
using TheProject.Domain.Entities;
using TheProject.Infrastructure.Data;


namespace TheProject.Infrastructure.Services.User
{
    public class UsersService : IUsersInterface
    {
        private readonly AppDbContext _context;
        private readonly IValidator<UsersDTO> _validator;
        private readonly PasswordHasher<Users> _passwordHasher = new PasswordHasher<Users>();

        public UsersService(AppDbContext context, IValidator<UsersDTO> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Response<List<UsersDTO>>> GetAll()
        {
            Response<List<UsersDTO>> response = new Response<List<UsersDTO>>();
            try
            {

                var users = await _context.Users
                    .Where(u => u.Ativo)
                    .Select(u => new UsersDTO
                    {
                        Id = (int)u.Id,
                        Name = u.Name
                    })
                    .ToListAsync();

                response.Data = users;
                response.Message = "users coletados";
                response.Status = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<Response<UsersDTO>> Add(UsersDTO dto)
        {
            var response = new Response<UsersDTO>();
            try
            {
                // Validação SINCRONA do DTO via FluentValidation (regras locais)
                var validationResult = await _validator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    response.Status = false;
                    response.Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return response;
                }


                var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
                if (emailExists)
                {
                    response.Status = false;
                    response.Message = "Já existe um email cadastrado";
                    return response;
                }


                // Criar novo usuário
                var newUser = new Users
                {

                    Name = dto.Name.Trim(),
                    Email = dto.Email,

                };

                var passwordHasher = new PasswordHasher<Users>();
                // Gerar o hash com a senha do DTO
                newUser.Password = passwordHasher.HashPassword(newUser, dto.Password);


                // Salvar no banco
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                // Retornar o User (apenas Id e Name)
                response.Data = new UsersDTO
                {
                    Id = (int)newUser.Id,
                    Name = newUser.Name,
                    Email = newUser.Email,


                };
                response.Message = "Usuário criado :)";
                response.Status = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }





        public async Task<Response<Users>> Update(UsersDTO dto)
        {
            var response = new Response<Users>();

            // Validação SINCRONA do DTO via FluentValidation (regras locais)
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                response.Status = false;
                response.Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                return response;
            }


            var user = await _context.Users.FindAsync(dto.Id);
            if (user == null)
            {
                response.Status = false;
                response.Message = "usuário não encontrado";
                return response;
            }




            var duplicateEmail = await _context.Users
                .AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower() && u.Id != dto.Id);
            if (duplicateEmail)
            {
                response.Status = false;
                response.Message = "Já existe um email como esse cadastrado.";
                return response;
            }

            // Atualizar Usuário
            user.Id = dto.Id;
            user.Name = dto.Name.Trim();
            user.Email = dto.Email;
            user.Password = dto.Password;
            user.Ativo = dto.Ativo;

            var passwordHasher = new PasswordHasher<Users>();
                // Gerar o hash com a senha do DTO
            user.Password = passwordHasher.HashPassword(user, dto.Password);

            await _context.SaveChangesAsync();

            response.Data = user;
            response.Message = "Usuário atualizado com sucesso.";
            response.Status = true;
            return response;
        }




        public async Task<Response<bool>> Delete(UserDeleteDTO dto)
        {
            var response = new Response<bool>();
            var allErrors = new List<string>();

            try
            {
                // Validação do DTO via FluentValidation
                var deleteValidator = new UsersDeleteValidator();
                var validationResult = await deleteValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    allErrors.AddRange(validationResult.Errors.Select(e => e.ErrorMessage));
                }


                Users user = null;
                if (!allErrors.Any())
                {
                    user = await _context.Users.FindAsync(dto.Id);
                    if (user == null)
                    {
                        allErrors.Add("usuário informado não encontrado");
                    }
                }

                // Se houver erros, retornar todos
                if (allErrors.Any())
                {
                    response.Status = false;
                    response.Message = string.Join(" | ", allErrors);
                    response.Data = false;
                    return response;
                }



                user.Ativo = false;

                // Inativa o usuário do banco de dados
                _context.Users.Update(user); //Aqui eu to usando soft delete para manter registro de usuários e emails cadastrados
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Message = "Usuário inativado com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                response.Data = false;
                return response;
            }
        }





        public async Task<Domain.Entities.Users?> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            try
            {
                // Primeiro, busca o usuário apenas pelo email
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                // Se não encontrou o usuário, retorna null
                if (user == null)
                    return null;

                // Cria uma instância do PasswordHasher
                var passwordHasher = new PasswordHasher<Users>();

                // Verifica se a senha informada confere com o hash salvo
                var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);

                // Se a senha não confere, retorna null
                if (result != PasswordVerificationResult.Success)
                    return null;

                return user;
            }
            catch (Exception)
            {
                return null;
            }
    }  }
}