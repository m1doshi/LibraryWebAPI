using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebAPI.API.Extensions;
using WebAPI.Application.DTOs;
using WebAPI.Application.UseCases.Authors;
using WebAPI.Application.UseCases.Books;
using WebAPI.Application.UseCases.Users;
using WebAPI.Application.Validators;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Interfaces.Repositories;
using WebAPI.Domain.Interfaces.UnitOfWork;
using WebAPI.Infrastructure.Interfaces;
using WebAPI.Infrastructure.Persistence;
using WebAPI.Infrastructures.Interfaces;
using WebAPI.Infrastructures.Persistence;
using WebAPI.Infrastructures.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddApiAuthentication(builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>());
builder.Services.AddSingleton<IAuthorizationHandler, RoleHierarchyHandler>();
builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataBaseConfig")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<AuthorModelValidator>();
    });
builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<BookModelValidator>();
    });
builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<LoginUserRequestValidator>();
    });
builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();
    });
builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<RoleModelValidator>();
    });
builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<UpdateAuthorRequestValidator>();
    });
builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<UpdateBookRequestValidator>();
    });
builder.Services.AddScoped<AddNewBookUseCase>();
builder.Services.AddScoped<BookShareUseCase>();
builder.Services.AddScoped<DeleteBookUseCase>();
builder.Services.AddScoped<GetBooksUseCase>();
builder.Services.AddScoped<UpdateBookUseCase>();
builder.Services.AddScoped<UpdateImageUseCase>();
builder.Services.AddScoped<AddNewAuthorUseCase>();
builder.Services.AddScoped<DeleteAuthorUseCase>();
builder.Services.AddScoped<GetAllBooksByAuthorUseCase>();
builder.Services.AddScoped<GetAuthorsUseCase>();
builder.Services.AddScoped<UpdateAuthorUseCase>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegisterUseCase>();
builder.Services.AddScoped<UpdateTokensUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRefreshProvider, RefreshProvider>();
var app = builder.Build();
app.UseExceptionHandlerMiddleware();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
