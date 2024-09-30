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
using Microsoft.AspNetCore.Hosting;
using System.Configuration;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = CreateHostBuilder(args).Build();
        builder.Run();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.Configure<JwtOptions>(Configuration.GetSection(nameof(JwtOptions)));
        services.AddApiAuthentication(services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>());
        services.AddSingleton<IAuthorizationHandler, RoleHierarchyHandler>();
        services.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DataBaseConfig")));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();

        services.AddControllers()
            .AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssemblyContaining<AuthorModelValidator>();
                config.RegisterValidatorsFromAssemblyContaining<BookModelValidator>();
                config.RegisterValidatorsFromAssemblyContaining<LoginUserRequestValidator>();
                config.RegisterValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();
                config.RegisterValidatorsFromAssemblyContaining<RoleModelValidator>();
                config.RegisterValidatorsFromAssemblyContaining<UpdateAuthorRequestValidator>();
                config.RegisterValidatorsFromAssemblyContaining<UpdateBookRequestValidator>();
                config.RegisterValidatorsFromAssemblyContaining<RefreshTokenRequestValidator>();
            });

        services.AddScoped<AddNewBookUseCase>();
        services.AddScoped<BookShareUseCase>();
        services.AddScoped<DeleteBookUseCase>();
        services.AddScoped<GetBooksUseCase>();
        services.AddScoped<UpdateBookUseCase>();
        services.AddScoped<UpdateImageUseCase>();
        services.AddScoped<AddNewAuthorUseCase>();
        services.AddScoped<DeleteAuthorUseCase>();
        services.AddScoped<GetAllBooksByAuthorUseCase>();
        services.AddScoped<GetAuthorsUseCase>();
        services.AddScoped<UpdateAuthorUseCase>();
        services.AddScoped<LoginUseCase>();
        services.AddScoped<RegisterUseCase>();
        services.AddScoped<UpdateTokensUseCase>();
        services.AddScoped<UpdateUserUseCase>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRefreshProvider, RefreshProvider>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandlerMiddleware();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

}
