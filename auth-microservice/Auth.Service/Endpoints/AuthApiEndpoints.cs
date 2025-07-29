using Auth.Service.ApiModels;
using Auth.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Service.Endpoints;

public static class AuthApiEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/login", async Task<IResult> ([FromServices]ITokenService tokenService, LoginRequest loginRequest) =>
        {
            var loginResult = await tokenService.GenerateAuthenticationToken(loginRequest.Username, loginRequest.Password);

            return loginResult is null ? TypedResults.Unauthorized() : TypedResults.Ok(loginResult);
        });
    }
}
