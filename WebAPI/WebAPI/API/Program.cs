using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebAPI.API.Extensions;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services.Authors;
using WebAPI.Application.Interfaces.Services.Book;
using WebAPI.Application.Interfaces.Services.Users;
using WebAPI.Application.Interfaces.UnitOfWork;
using WebAPI.Application.UseCases.Authors;
using WebAPI.Application.UseCases.Books;
using WebAPI.Application.UseCases.Users;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructures.Interfaces;
using WebAPI.Infrastructures.Persistence;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddApiAuthentication(builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>());
builder.Services.AddSingleton<IAuthorizationHandler, RoleHierarchyHandler>();
builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataBaseConfig")));
builder.Services.AddScoped<IAddNewBookService, AddNewBookUseCase>();
builder.Services.AddScoped<IBookShareService, BookShareUseCase>();
builder.Services.AddScoped<IDeleteBookService, DeleteBookUseCase>();
builder.Services.AddScoped<IGetBooksService, GetBooksUseCase>();
builder.Services.AddScoped<IUpdateBookService, UpdateBookUseCase>();
builder.Services.AddScoped<IUpdateImageService, UpdateImageUseCase>();
builder.Services.AddScoped<IAddNewAuthorService, AddNewAuthorUseCase>();
builder.Services.AddScoped<IDeleteAuthorService, DeleteAuthorUseCase>();
builder.Services.AddScoped<IGetAllBooksByAuthorService, GetAllBooksByAuthorUseCase>();
builder.Services.AddScoped<IGetAuthorsService, GetAuthorsUseCase>();
builder.Services.AddScoped<IUpdateAuthorService, UpdateAuthorUseCase>();
builder.Services.AddScoped<ILoginService, LoginUseCase>();
builder.Services.AddScoped<IRegisterService, RegisterUseCase>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
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
