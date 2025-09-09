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
            _logger.LogInformation("Starting database health check");
            
            // Test database connectivity
            var canConnect = await _context.Database.CanConnectAsync();
            _logger.LogInformation($"Can connect to database: {canConnect}");
            
            if (!canConnect)
            {
                return StatusCode(503, new { 
                    status = "database_unavailable", 
                    message = "Cannot connect to database",
                    connectionString = _context.Database.GetConnectionString()?.Substring(0, 50) + "..." // Only show first 50 chars for security
                });
            }

            // Test a simple query
            var userCount = await _context.Users.CountAsync();
            _logger.LogInformation($"User count: {userCount}");
            
            return Ok(new { 
                status = "database_healthy", 
                canConnect = true,
                userCount = userCount,
                timestamp = DateTime.UtcNow,
                connectionString = _context.Database.GetConnectionString()?.Substring(0, 50) + "..."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed");
            return StatusCode(500, new { 
                status = "database_error", 
                error = ex.Message,
                innerError = ex.InnerException?.Message,
                stackTrace = ex.StackTrace?.Split('\n')[0..3] // First 3 lines of stack trace
            });
        }
    }
}