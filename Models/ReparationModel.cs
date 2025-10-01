using System.ComponentModel.DataAnnotations;

namespace P5CreateYourFirstApplication.Models
{
    public class ReparationModel
    {
        [Key]
        public int Id { get; set; }
        public string CodeVin { get; set; }
        public int IdVoiture { get; set; }
        public VoitureModel Voiture { get; set; }
        public int IdTypeReparation { get; set; }
        public TypeReparationModel TypeReparation { get; set; }
        public string Description { get; set; }
        public double CoutReparation { get; set; }
        public DateOnly DateReparation { get; set; }
        public string Statut { get; set; } // string pour "en cours", "termine"...
    }
}
