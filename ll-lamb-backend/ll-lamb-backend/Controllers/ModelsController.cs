using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LlLambBackend.DTOs;
using LlLambBackend.Services;

namespace LlLambBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ModelsController : ControllerBase
{
    private readonly IChatService _chatService;
    
    public ModelsController(IChatService chatService)
    {
        _chatService = chatService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ModelDto>>> GetModels()
    {
        var models = await _chatService.GetModelsAsync();
        return Ok(models);
    }
}