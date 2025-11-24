using P5CreateYourFirstApplication.Models;

namespace P5CreateYourFirstApplication.Services.Interfaces
{
    public interface IVoitureService
    {
        Task<IEnumerable<VoitureModel>> GetAllAsync();
        Task<VoitureModel?> GetByIdAsync(int id);
        Task AddAsync(VoitureModel voiture);
        Task UpdateAsync(VoitureModel voiture);
        Task DeleteAsync(int id);
        VoitureModel MapToEntity(VoitureViewModel vm);
    }
}
