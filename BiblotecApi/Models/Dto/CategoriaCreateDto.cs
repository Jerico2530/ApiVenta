using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models.Dto
{
    public  class CategoriaCreateDto
    {
        [Required(ErrorMessage = "El campo IdCategoria es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de IdCategoria debe ser mayor que cero.")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El campo Nombre Categoria es obligatorio.")]
        [StringLength(50, ErrorMessage = "La longitud del campo Nombre Categoria no puede ser mayor de 50 caracteres.")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "El campo Nombre Categoria no puede contener números.")]
        public string NombreCategoria { get; set; }
    }
}

