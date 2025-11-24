using Microsoft.EntityFrameworkCore;
using P5CreateYourFirstApplication.Data;
using P5CreateYourFirstApplication.Models;
using P5CreateYourFirstApplication.Services.Interfaces;

namespace P5CreateYourFirstApplication.Services
{
    public class VoitureService : IVoitureService
    {
        private readonly ApplicationDbContext _context;

        public VoitureService(ApplicationDbContext context)
        {
            _context = context;
        }

        public VoitureModel MapToEntity(VoitureViewModel vm)
        {
            return new VoitureModel()
            {
                PrixAchat = vm.PrixAchat ?? 0,
                Annee = vm.Annee.Year,
                DateAchat = DateOnly.FromDateTime(vm.Annee),

                IdMarque = vm.IdMarque,
                IdModele = vm.IdModele,
                IdFinition = vm.IdFinition,

                Disponible = true,
                EnReparation = false,
                Vendu = false
            };
        }

        public async Task<IEnumerable<VoitureModel>> GetAllAsync()
        {
            return await _context.VoitureModel
                .Include(v => v.Marque)
                .Include(v => v.Modele)
                .Include(v => v.Finition)
                .ToListAsync();
        }

        public async Task<VoitureModel?> GetByIdAsync(int id)
        {
            return await _context.VoitureModel
                .Include(v => v.Marque)
                .Include(v => v.Modele)
                .Include(v => v.Finition)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task AddAsync(VoitureModel voiture)
        {
            _context.VoitureModel.Add(voiture);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VoitureModel voiture)
        {
            _context.VoitureModel.Update(voiture);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var voiture = await _context.VoitureModel.FindAsync(id);
            if (voiture != null)
            {
                _context.VoitureModel.Remove(voiture);
                await _context.SaveChangesAsync();
            }
        }
    }
}
