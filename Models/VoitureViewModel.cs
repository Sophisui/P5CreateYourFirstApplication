using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace P5CreateYourFirstApplication.Models
{
    public class VoitureViewModel
    {
        [Required]
        public string CodeVin { get; set; }

        [Required]
        public decimal? PrixAchat { get; set; }

        [Required]
        public string Couleur { get; set; }

        [Required]
        public DateTime Annee { get; set; } = DateTime.Now;

        [Required]
        public int IdMarque { get; set; }
        public IEnumerable<MarqueModel>? Marques { get; set; }

        [Required]
        public int IdModele { get; set; }
        public IEnumerable<ModeleModel>? Modeles { get; set; }

        [Required]
        public int IdFinition { get; set; }
        public IEnumerable<FinitionModel>? Finitions { get; set; }

        [Required]
        public IFormFile Visuel { get; set; }
    }

}
