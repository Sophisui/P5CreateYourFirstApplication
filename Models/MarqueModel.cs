using System.ComponentModel.DataAnnotations;

namespace P5CreateYourFirstApplication.Models
{
    public class MarqueModel
    {
        [Key]
        public int Id { get; set; }
        public string NomMarque { get; set; }
        public ICollection<VoitureModel> Voitures { get; set; } = new List<VoitureModel>();
    }
}
