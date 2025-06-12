using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components;
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


/*
cd caminho/para/TheProject.WebApi
dotnet add package FluentValidation
dotnet add package FluentValidation.AspNetCore só lembrando os pacotes que usei pra validar erros
*/


var builder = WebApplication.CreateBuilder(args);
//Aqui estou definindo a chave 'secreta' para assinar o token
var key = Encoding.ASCII.GetBytes("123");
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


builder.Services
    .AddFluentValidationAutoValidation() // ativa a validação automática no pipeline
    .AddFluentValidationClientsideAdapters(); // ativa suporte à validação client-side (opcional, útil se tiver front-end Razor/Blazor)

builder.Services.AddValidatorsFromAssemblyContaining<ProductUpdateValidator>(); // registra os validators



builder.Services.AddSingleton<TokenService>(new TokenService("123"));

builder.Services.AddAuthorization();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//criando a forma de comunição entre interface e serviços
builder.Services.AddScoped<IProductsInterface, ProductsService>();
builder.Services.AddScoped<IUsersInterface, UsersService>();
builder.Services.AddScoped<ICategoriesInterface, CategoriesService>();




var app = builder.Build();



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


app.MapControllers();
// Configure the HTTP request pipeline.


app.Run();

