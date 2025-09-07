using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LlLambBackend.Data;
using LlLambBackend.DTOs;
using LlLambBackend.Models;
using LlLambBackend.Repositories;

namespace LlLambBackend.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    
    public AuthService(IUserRepository userRepository, ApplicationDbContext context, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _context = context;
        _configuration = configuration;
    }
    
    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
        if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash, user.Salt))
        {
            return null;
        }
        
        return await GenerateAuthResponseAsync(user);
    }
    
    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userRepository.ExistsAsync(registerDto.Username, registerDto.Email))
        {
            return null;
        }
        
        var salt = GenerateSalt();
        var passwordHash = HashPassword(registerDto.Password, salt);
        
        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            Salt = salt
        };
        
        var createdUser = await _userRepository.CreateAsync(user);
        return await GenerateAuthResponseAsync(createdUser);
    }
    
    public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
    {
        var userSession = await _context.UserSessions
            .Include(us => us.User)
            .FirstOrDefaultAsync(us => us.RefreshToken == refreshToken 
                && us.ExpiresAt > DateTime.UtcNow 
                && !us.IsRevoked);
                
        if (userSession == null) return null;
        
        return await GenerateAuthResponseAsync(userSession.User);
    }
    
    public async Task<bool> RevokeTokenAsync(string refreshToken)
    {
        var userSession = await _context.UserSessions
            .FirstOrDefaultAsync(us => us.RefreshToken == refreshToken);
            
        if (userSession == null) return false;
        
        userSession.IsRevoked = true;
        await _context.SaveChangesAsync();
        return true;
    }
    
    private async Task<AuthResponseDto> GenerateAuthResponseAsync(User user)
    {
        var accessToken = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddDays(7);
        
        var userSession = new UserSession
        {
            UserId = user.UserId,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt
        };
        
        _context.UserSessions.Add(userSession);
        await _context.SaveChangesAsync();
        
        return new AuthResponseDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt
        };
    }
    
    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };
        
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
    
    private static string GenerateSalt()
    {
        var saltBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }
    
    private static string HashPassword(string password, string salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);
        return Convert.ToBase64String(hash);
    }
    
    private static bool VerifyPassword(string password, string hash, string salt)
    {
        var hashBytes = Convert.FromBase64String(hash);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256);
        var computedHash = pbkdf2.GetBytes(32);
        return hashBytes.SequenceEqual(computedHash);
    }
}