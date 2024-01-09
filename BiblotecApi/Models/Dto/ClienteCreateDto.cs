using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models.Dto
{
    public class ClienteCreateDto
    {
        [Required(ErrorMessage = "El campo Nombre Cliente es obligatorio.")]
        [StringLength(50, ErrorMessage = "La longitud del campo Nombre Cliente no puede ser mayor de 50 caracteres.")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "El campo Nombre Cliente no puede contener números.")]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "El campo Apellido Cliente es obligatorio.")]
        [StringLength(50, ErrorMessage = "La longitud del campo Apellido Cliente no puede ser mayor de 50 caracteres.")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "El campo Apellido Cliente no puede contener números.")]
        public string ApellidoCliente { get; set; }

        [Required(ErrorMessage = "El campo Edad es obligatorio.")]
        [Range(1, 100, ErrorMessage = "La Edad debe estar entre 1 y 100.")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "El campo DNI es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de DNI debe ser mayor que cero.")]
        public int DNI { get; set; }

        [Required(ErrorMessage = "El campo Direccion es obligatorio.")]
        public string Direccion { get; set; }

    }
}
