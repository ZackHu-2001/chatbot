using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LlLambBackend.Data;

namespace LlLambBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HealthController> _logger;

    public HealthController(ApplicationDbContext context, ILogger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            // Test basic API functionality
            return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            return StatusCode(500, new { status = "unhealthy", error = ex.Message });
        }
    }

    [HttpGet("database")]
    public async Task<IActionResult> CheckDatabase()
    {
        try
        {
            // Test database connectivity
            var canConnect = await _context.Database.CanConnectAsync();
            
            if (!canConnect)
            {
                return StatusCode(503, new { 
                    status = "database_unavailable", 
                    message = "Cannot connect to database" 
                });
            }

            // Test a simple query
            var userCount = await _context.Users.CountAsync();
            
            return Ok(new { 
                status = "database_healthy", 
                canConnect = true,
                userCount = userCount,
                timestamp = DateTime.UtcNow 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed");
            return StatusCode(500, new { 
                status = "database_error", 
                error = ex.Message,
                innerError = ex.InnerException?.Message
            });
        }
    }
}