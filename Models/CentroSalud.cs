using System.ComponentModel.DataAnnotations;

namespace Provincias.Models
{
    public class CentroSalud
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del centro de salud es obligatorio")]
        [StringLength(120)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El tipo de centro es obligatorio")]
        [StringLength(60)]
        [Display(Name = "Tipo de centro")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [Phone(ErrorMessage = "Ingrese un número de teléfono válido")]
        [StringLength(30)]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(200)]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El distrito es obligatorio")]
        [Display(Name = "Distrito")]
        public int DistritoId { get; set; }

        public Distrito Distrito { get; set; }
    }
}
