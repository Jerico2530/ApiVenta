using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models.Dto
{
    public class PrendaCreateDto
    {
        [Required(ErrorMessage = "El campo IdPrenda es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de IdPrenda debe ser mayor que cero.")]
        public int IdPrenda { get; set; }


        [Required(ErrorMessage = "El campo Nombre Prenda es obligatorio.")]
        [StringLength(50, ErrorMessage = "La longitud del campo Nombre Prenda no puede ser mayor de 50 caracteres.")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "El campo Nombre Prenda no puede contener números.")]
        public string NombrePrenda { get; set; }

        [Required(ErrorMessage = "El campo Precio es obligatorio.")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El campo IdCategoria es obligatorio.")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El campo IdColor es obligatorio.")]
        public int IdColor { get; set; }

        [Required(ErrorMessage = "El campo IdMarca es obligatorio.")]
        public int IdMarca { get; set; }

        [Required(ErrorMessage = "El campo IdTalla es obligatorio.")]
        public int IdTalla { get; set; }

        [Required(ErrorMessage = "El campo Stock es obligatorio.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "El valor de Stock debe ser mayor o igual a 0.")]
        public double Stock { get; set; }


    }

}
