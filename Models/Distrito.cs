using System.ComponentModel.DataAnnotations;

namespace Provincias.Models
{
    public class Distrito
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del distrito es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El código postal es obligatorio")]
        [StringLength(20)]
        [Display(Name = "Código postal")]
        public string CodigoPostal { get; set; }

        [Required(ErrorMessage = "El área del distrito es obligatoria")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El área debe ser mayor que cero")]
        [Display(Name = "Área (km²)")]
        public decimal AreaKm2 { get; set; }

        [Required(ErrorMessage = "La población del distrito es obligatoria")]
        [Range(0, int.MaxValue)]
        [Display(Name = "Población")]
        public int Poblacion { get; set; }

        [Required(ErrorMessage = "El cantón es obligatorio")]
        [Display(Name = "Cantón")]
        public int CantonId { get; set; }

        public Canton Canton { get; set; }
        public ICollection<CentroSalud> CentrosSalud { get; set; } = new List<CentroSalud>();
    }
}
