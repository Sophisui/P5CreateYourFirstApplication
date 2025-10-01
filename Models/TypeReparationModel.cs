using System.ComponentModel.DataAnnotations;

namespace P5CreateYourFirstApplication.Models
{
    public class TypeReparationModel
    {
        [Key]
        public int Id { get; set; }
        public int IdReparation { get; set; }
        public string NomReparation { get; set; }
        public string Description { get; set; }
        public double CoutTypeReparation { get; set; }
    }
}
