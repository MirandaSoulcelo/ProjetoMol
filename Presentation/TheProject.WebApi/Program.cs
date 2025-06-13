using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TheProject.Infrastructure.Data;
using TheProject.Application.Interfaces;
using TheProject.Infrastructure.Services.Product;
using TheProject.Infrastructure.Services.User;
using TheProject.Infrastructure.Services.Categories;
using FluentValidation.AspNetCore;
using FluentValidation;
using TheProject.Application.DTOs;
using TheProject.Application.Validators.Products;
using TheProject.Application.DTOs.UsersDTO;
using TheProject.Application.Validators.Users;
using MediatR;
using TheProject.Application.Features.Categories.Commands.AddCategory;


/*
cd caminho/para/TheProject.WebApi
dotnet add package FluentValidation
dotnet add package FluentValidation.AspNetCore só lembrando os pacotes que usei pra validar erros
*/


var builder = WebApplication.CreateBuilder(args);
//Aqui estou definindo a chave 'secreta' para assinar o token
var secretKey = builder.Configuration["Jwt:SecretKey"];
var key = Encoding.ASCII.GetBytes(secretKey);



builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
 });

builder.Services.AddAuthentication(options =>
{
    //definindo qual esquema de autenticação que o meu sistema vai usar como padrão
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    //caso um user tentar acessar sem estar autenticado, vai levar um belo de um 404
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{   //configuração do middleware JWT Bearer, Marcelo. Não esquecer disso
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //o sistema vai ter que validar se esse carinha(token) foi de fato assinado O.o
        ValidateIssuerSigningKey = true,

        //passando minha chave nada secreta para ser usada para validar a assinatura
        IssuerSigningKey = new SymmetricSecurityKey(key),

        //aqui eu não to validando quem emitiu o token, pelo que entendi
        ValidateIssuer = false,

        // nem o público
        ValidateAudience = false,
        //sem tolerância, igual a igreja católica no período da inquisição
        ClockSkew = TimeSpan.Zero
    };
});


builder.Services.AddMediatR(typeof(AddCategoryHandler).Assembly);

builder.Services
    .AddFluentValidationAutoValidation() // ativa a validação automática no pipeline
    .AddFluentValidationClientsideAdapters(); 

builder.Services.AddValidatorsFromAssemblyContaining<ProductUpdateValidator>(); // registra os validators
builder.Services.AddScoped<IValidator<ProductDeleteDTO>, ProductDeleteValidator>();
builder.Services.AddScoped<IValidator<UsersDTO>, UsersValidator>();
builder.Services.AddScoped<IValidator<UserDeleteDTO>, UsersDeleteValidator>();





builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "TheProject.WebApi", Version = "v1" });

    // Configura o Swagger para usar JWT
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header usando o esquema Bearer. 
                        Exemplo: 'Bearer {seu_token_jwt}'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});





builder.Services.AddScoped<TokenService>(provider => new TokenService(secretKey));

builder.Services.AddAuthorization();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//criando a forma de comunição entre interface e serviços
builder.Services.AddScoped<IProductsInterface, ProductsService>();
builder.Services.AddScoped<IUsersInterface, UsersService>();
builder.Services.AddScoped<ICategoriesInterface, CategoriesService>();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
              //Eu sei que isso não é recomendado em produção, só permiti pois meu método update preciso dessa permissão
    });
});

var app = builder.Build();


app.UseCors();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});


app.UseHttpsRedirection();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
// Configure the HTTP request pipeline.


app.Run();

