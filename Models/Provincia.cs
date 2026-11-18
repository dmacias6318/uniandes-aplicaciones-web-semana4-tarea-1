
using System.ComponentModel.DataAnnotations;

namespace Provincias.Models
{
    public class Provincia
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "El nombre de la provincia es obligatorio")]
        [Length(3, 100, ErrorMessage = "El nombre de la provincia " +
            "debe tener entre 3 y 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La capital de la provincia es obligatoria")]
        [StringLength(100)]
        public string Capital { get; set; }

        [Required(ErrorMessage = "La extensión de la provincia es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La extensión de la provincia " +
            "debe ser un valor positivo")]
        public int Extension { get; set; }


        [Required(ErrorMessage = "El número de cantones de la provincia es obligatorio")]
        [Display(Name = "Número de Cantones")]
        [Range(1, int.MaxValue)]
        public int Numero_Cantones { get; set; }

        [Required(ErrorMessage = "La población de la provincia es obligatoria")]
        [Display(Name = "Población")]
        [Range(0, int.MaxValue)]
        public int Poblacion { get; set; }

        public ICollection<Canton> Cantones { get; set; } = new List<Canton>();
    }
}
