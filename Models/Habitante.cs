using System.ComponentModel.DataAnnotations;

namespace Provincias.Models
{
    public class Habitante
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del habitante es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido del habitante es obligatorio")]
        public string Apellido { get; set; }
        
        [Required(ErrorMessage = "La fecha de nacimiento del habitante es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime Fecha_Nacimiento { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
        [Display(Name = "Correo electrónico")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "El cantón del habitante es obligatorio")]
        [Display(Name = "Cantón")]
        public int CantonId { get; set; }

        public Canton Canton { get; set; }


    }
}
