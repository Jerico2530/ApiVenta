using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models.Dto
{
    public class VentaCreateDto
    {
        [Required(ErrorMessage = "El campo Descripcion es obligatorio.")]
        [MaxLength(100, ErrorMessage = "La longitud máxima de la descripción es 100 caracteres.")]
        [MinLength(5, ErrorMessage = "La longitud mínima de la descripción es 5 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo IdEmpleado es obligatorio.")]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "El campo IdPrenda es obligatorio.")]
        public int IdPrenda { get; set; }

        [Required(ErrorMessage = "El campo IdCliente es obligatorio.")]
        public int IdCliente { get; set; }

    }
}
