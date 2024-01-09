using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models.Dto
{
    public class TallaCreateDto
    {
        [Required(ErrorMessage = "El campo IdTalla es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de IdTalla debe ser mayor que cero.")]
        public int IdTalla { get; set; }

        [Required(ErrorMessage = "El campo NumeroTalla es obligatorio.")]
        [StringLength(50, ErrorMessage = "La longitud del campo NumeroTalla no puede ser mayor de 50 caracteres.")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "El campo NumeroTalla no puede contener números.")]
        public string NumeroTalla { get; set; }
    }
}
