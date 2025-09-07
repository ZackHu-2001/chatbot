using Microsoft.AspNetCore.Mvc;
using LlLambBackend.DTOs;
using LlLambBackend.Services;

namespace LlLambBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);
        if (result == null)
        {
            return BadRequest(new { message = "用户名或邮箱已存在" });
        }
        
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        if (result == null)
        {
            return Unauthorized(new { message = "用户名或密码错误" });
        }
        
        return Ok(result);
    }
    
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        var result = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
        if (result == null)
        {
            return Unauthorized(new { message = "无效的刷新令牌" });
        }
        
        return Ok(result);
    }
    
    [HttpPost("revoke")]
    public async Task<ActionResult> RevokeToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        var result = await _authService.RevokeTokenAsync(refreshTokenDto.RefreshToken);
        if (!result)
        {
            return BadRequest(new { message = "令牌撤销失败" });
        }
        
        return Ok(new { message = "令牌已成功撤销" });
    }
}

public class RefreshTokenDto
{
    public string RefreshToken { get; set; } = string.Empty;
}