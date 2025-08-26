using Application.Driving.Dtos.Auth;
using Core.Ports.Auth;
using Core.Ports.Token;
using Microsoft.AspNetCore.Mvc;

namespace Application.Driving.Routes;

public static class AuthRoutes
{
    public static void ConfigureAuthRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Authentication");

        group.MapPost("/register", RegisterUser)
            .WithName("RegisterUser")
            .Produces<AuthResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Registra um novo usuário")
            .WithDescription("Cria um novo usuário e retorna as informações de autenticação.");

        group.MapPost("/login", LoginUser)
            .WithName("LoginUser")
            .Produces<AuthResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithSummary("Autentica um usuário")
            .WithDescription("Valida as credenciais do usuário e retorna um token de acesso.");
    }

    private static async Task<IResult> RegisterUser(
        [FromBody] RegisterRequest request,
        IAuthService authService,
        IPasswordHasher passwordHasher, 
        ITokenService tokenService) 
    {
        var hashedPassword = passwordHasher.HashPassword(request.Password);
            
        var user = await authService.RegisterAsync(request.Username, request.Email, hashedPassword);

        var token = tokenService.GenerateToken(user);
        var response = new AuthResponse { Username = user.Username, Token = token };

        return Results.Created($"/api/users/{user.Id}", response);
    }

    private static async Task<IResult> LoginUser(
        [FromBody] LoginRequest request,
        IAuthService authService,
        ITokenService tokenService) 
    {
        var user = await authService.ValidateUserAsync(request.Username, request.Password);

        if (user is null)
        {
            return Results.Unauthorized();
        }

        var token = tokenService.GenerateToken(user);
        var response = new AuthResponse { Username = user.Username, Token = token };

        return Results.Ok(response);
    }
}