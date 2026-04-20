using CaseItau.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CaseItau.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config) => _config = config;

    /// <summary>Autentica o usuário e retorna um token JWT.</summary>
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var validUser = _config["Auth:Username"] == dto.Username
                     && _config["Auth:Password"] == dto.Password;

        if (!validUser)
            return Unauthorized(new { error = "Credenciais inválidas." });

        var (token, expiresAt) = GenerateToken(dto.Username);
        return Ok(new TokenDto(token, expiresAt));
    }

    private (string Token, DateTime ExpiresAt) GenerateToken(string username)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]!));

        var expiresAt = DateTime.UtcNow.AddHours(
            double.Parse(_config["JwtSettings:ExpirationHours"]!));

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: [new Claim(ClaimTypes.Name, username)],
            expires: expiresAt,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
