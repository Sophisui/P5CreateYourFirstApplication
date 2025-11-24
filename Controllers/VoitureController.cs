using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using P5CreateYourFirstApplication.Data;
using P5CreateYourFirstApplication.Models;
using P5CreateYourFirstApplication.Services.Interfaces;

namespace P5CreateYourFirstApplication.Controllers
{
    public class VoituresController : Controller
    {
        private readonly IVoitureService _voitureService;
        private readonly ApplicationDbContext _context;

        public VoituresController(IVoitureService voitureService, ApplicationDbContext context)
        {
            _voitureService = voitureService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new VoitureViewModel
            {
                Marques = _context.MarqueModel.ToList(),

                Modeles = _context.ModeleModel.ToList(),

                Finitions = _context.FinitionModel.ToList(),
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddVoiture(
            VoitureViewModel voitureViewModel,
            IFormFile photo,
            string[] RepairsDescriptions,
            decimal[] RepairsCosts)
        {
            if (!ModelState.IsValid)
            {
                voitureViewModel.Marques = _context.MarqueModel.ToList();
                voitureViewModel.Modeles = _context.ModeleModel.ToList();
                voitureViewModel.Finitions = _context.FinitionModel.ToList();
            }

            var voiture = _voitureService.MapToEntity(voitureViewModel);

            if (photo != null && photo.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/voitures", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                voiture.Visuel = "/images/voitures/" + fileName;
            }

            voiture.CodeVin = voitureViewModel.CodeVin;
            voiture.Couleur = voitureViewModel.Couleur;
            await _voitureService.AddAsync(voiture);

            decimal totalRepairs = 0;

            if (RepairsDescriptions != null && RepairsCosts != null)
            {
                for (int i = 0; i < RepairsDescriptions.Length; i++)
                {
                    var rep = new ReparationModel
                    {
                        Id = voiture.Id,
                        Description = RepairsDescriptions[i],
                        CoutReparation = RepairsCosts[i]
                    };

                    totalRepairs += rep.CoutReparation;
                    _context.ReparationModel.Add(rep);
                }

                await _context.SaveChangesAsync();
            }

            voiture.PrixVente = voiture.PrixAchat + totalRepairs + 500m;
            await _voitureService.UpdateAsync(voiture);

            return View("AddSuccess", voiture);
        }

        [HttpGet]
        public JsonResult GetModelesByMarque(int idMarque)
        {
            var modeles = _context.ModeleModel
                .Where(m => m.IdMarque == idMarque)
                .Select(m => new { m.Id, m.NomModele })
                .ToList();

            return Json(modeles);
        }

        // GetAll (Voitures)
        public async Task<IActionResult> Index()
        {
            var voitures = await _voitureService.GetAllAsync();
            return View(voitures);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var voiture = await _context.VoitureModel
                .Include(v => v.Marque)
                .Include(v => v.Modele)
                .Include(v => v.Finition)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (voiture == null)
                return NotFound();

            return View(voiture);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var voiture = _context.VoitureModel
                .Include(v => v.Marque)
                .Include(v => v.Modele)
                .Include(v => v.Finition)
                .FirstOrDefault(v => v.Id == id);

            if (voiture == null) return NotFound();

            ViewBag.Marques = _context.MarqueModel.ToList();
            ViewBag.Modeles = _context.ModeleModel.ToList();
            ViewBag.Finitions = _context.FinitionModel.ToList();

            return View(voiture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(VoitureModel voiture, IFormFile photo)
        {
            voiture.Modele = _context.ModeleModel.Where(x => x.Id == voiture.IdModele).First();
            voiture.Marque = _context.MarqueModel.Where(x => x.Id == voiture.IdMarque).First();
            voiture.Finition = _context.FinitionModel.Where(x => x.Id == voiture.IdFinition).First();


            //if (!ModelState.IsValid)
            //{
            //    ViewBag.Marques = _context.MarqueModel.ToList();
            //    ViewBag.Modeles = _context.ModeleModel.ToList();
            //    ViewBag.Finitions = _context.FinitionModel.ToList();
            //    return View(voiture);
            //}

            var voitureToUpdate = await _voitureService.GetByIdAsync(voiture.Id);
            if (voitureToUpdate == null) return NotFound();

            voitureToUpdate.Annee = voiture.Annee;
            voitureToUpdate.Couleur = voiture.Couleur;
            voitureToUpdate.IdMarque = voiture.IdMarque;
            voitureToUpdate.IdModele = voiture.IdModele;
            voitureToUpdate.IdFinition = voiture.IdFinition;

            if (photo != null && photo.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/voitures", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                voitureToUpdate.Visuel = "/images/voitures/" + fileName;
            }

            await _voitureService.UpdateAsync(voitureToUpdate);

            return RedirectToAction("Details", new { id = voiture.Id });
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var voiture = await _context.VoitureModel
                .Include(v => v.Marque)
                .Include(v => v.Modele)
                .Include(v => v.Finition)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (voiture == null)
                return NotFound();

            return View(voiture);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Charger la voiture avec les relations
            var voiture = await _context.VoitureModel
                .Include(v => v.Marque)
                .Include(v => v.Modele)
                .Include(v => v.Finition)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (voiture == null)
                return NotFound();

            // Supprimer la voiture
            _context.VoitureModel.Remove(voiture);
            await _context.SaveChangesAsync();

            // Passer le modèle chargé à la vue DeleteSuccess
            return View("DeleteSuccess", voiture);
        }
    }
}
