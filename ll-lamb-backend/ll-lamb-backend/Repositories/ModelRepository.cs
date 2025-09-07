using Microsoft.EntityFrameworkCore;
using LlLambBackend.Data;
using LlLambBackend.Models;

namespace LlLambBackend.Repositories;

public class ModelRepository : IModelRepository
{
    private readonly ApplicationDbContext _context;
    
    public ModelRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Model>> GetAllActiveAsync()
    {
        return await _context.Models
            .Where(m => m.Status)
            .OrderBy(m => m.Name)
            .ToListAsync();
    }
    
    public async Task<Model?> GetByIdAsync(Guid modelId)
    {
        return await _context.Models
            .FirstOrDefaultAsync(m => m.ModelId == modelId);
    }
    
    public async Task<Model> CreateAsync(Model model)
    {
        model.ModelId = Guid.NewGuid();
        model.CreatedAt = DateTime.UtcNow;
        model.UpdatedAt = DateTime.UtcNow;
        
        _context.Models.Add(model);
        await _context.SaveChangesAsync();
        return model;
    }
    
    public async Task<Model> UpdateAsync(Model model)
    {
        model.UpdatedAt = DateTime.UtcNow;
        _context.Models.Update(model);
        await _context.SaveChangesAsync();
        return model;
    }
    
    public async Task<bool> DeleteAsync(Guid modelId)
    {
        var model = await GetByIdAsync(modelId);
        if (model == null) return false;
        
        model.Status = false;
        model.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}