using System.ComponentModel.DataAnnotations;

namespace P5CreateYourFirstApplication.Models
{
    public class ModeleModel
    {
        [Key]
        public int Id { get; set; }
        public int IdMarque { get; set; }
        public string NomModele { get; set; }
        public ICollection<VoitureModel> Voitures { get; set; } = new List<VoitureModel>();
    }
}
