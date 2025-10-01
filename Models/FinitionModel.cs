using System.ComponentModel.DataAnnotations;

namespace P5CreateYourFirstApplication.Models
{
    public class FinitionModel
    {
        [Key]
        public int Id { get; set; }
        public int IdModele { get; set; }
        public string NomFinition { get; set; }
        public ICollection<VoitureModel> Voitures { get; set; } = new List<VoitureModel>();

    }
}
