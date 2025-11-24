using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P5CreateYourFirstApplication.Models
{
    public class VoitureModel
    {
        [Key]
        public int Id { get; set; } 
        public string? CodeVin { get; set; }
        public int Annee { get; set; }
        public string? Couleur { get; set; }
        public string? Visuel { get; set; }
        public bool EnReparation { get; set; }
        public bool Disponible { get; set; }
        public bool Vendu { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrixAchat { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrixVente { get; set; }
        public DateOnly DateAchat { get; set; }
        public DateOnly? DateVente { get; set; }

        public int IdMarque { get; set; }
        [ForeignKey("IdMarque")]
        public MarqueModel Marque { get; set; }

        public int IdModele { get; set; }
        [ForeignKey("IdModele")]
        public ModeleModel Modele { get; set; }

        public int IdFinition { get; set; }
        [ForeignKey("IdFinition")]
        public FinitionModel Finition { get; set; }
        public ICollection<ReparationModel> Reparations { get; set; } = new List<ReparationModel>();
    }

}