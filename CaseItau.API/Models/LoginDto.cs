namespace CaseItau.API.Models;

public record LoginDto(string Username, string Password);
public record TokenDto(string Token, DateTime ExpiresAt);
