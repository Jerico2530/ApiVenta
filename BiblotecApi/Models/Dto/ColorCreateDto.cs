using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models.Dto
{
    public class ColorCreateDto
    {
        [Required(ErrorMessage = "El campo IdColor es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de IdColor debe ser mayor que cero.")]
        public int IdColor { get; set; }

        [Required(ErrorMessage = "El campo Nombre Color es obligatorio.")]
        [StringLength(10, ErrorMessage = "La longitud del campo Nombre Color no puede ser mayor de 50 caracteres.")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "El campo Nombre Color no puede contener números.")]
        public string NombreColor { get; set; }
    }
}
