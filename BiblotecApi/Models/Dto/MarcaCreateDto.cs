using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models.Dto
{
    public class MarcaCreateDto
    {
        [Required(ErrorMessage = "El campo IdMarca es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de IdMarca debe ser mayor que cero.")]
        public int IdMarca { get; set; }
        [Required(ErrorMessage = "El campo NombreMarca es obligatorio.")]
        [MaxLength(20, ErrorMessage = "La longitud máxima del nombre de la Marca es 20 caracteres.")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "El campo Apellido Marca no puede contener números.")]
        public string NombreMarca { get; set; }
    }
}
