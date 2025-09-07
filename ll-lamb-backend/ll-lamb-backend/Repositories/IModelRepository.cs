using LlLambBackend.Models;

namespace LlLambBackend.Repositories;

public interface IModelRepository
{
    Task<List<Model>> GetAllActiveAsync();
    Task<Model?> GetByIdAsync(Guid modelId);
    Task<Model> CreateAsync(Model model);
    Task<Model> UpdateAsync(Model model);
    Task<bool> DeleteAsync(Guid modelId);
}