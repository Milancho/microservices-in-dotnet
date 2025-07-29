namespace Auth.Service.Models;

public record AuthToken(string Token, int ExpiresIn);
