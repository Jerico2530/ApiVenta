using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models.Dto
{
    public class EmpleadoCreateDto
    {
        [Required(ErrorMessage = "El campo NombreEmpleado es obligatorio.")]
        [StringLength(50, ErrorMessage = "La longitud del campo NombreEmpleado no puede ser mayor de 50 caracteres.")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "El campo NombreEmpleado no puede contener números.")]
        public string NombreEmpleado { get; set; }

        [Required(ErrorMessage = "El campo ApellidoEmpleado es obligatorio.")]
        [StringLength(50, ErrorMessage = "La longitud del campo ApellidoEmpleado no puede ser mayor de 50 caracteres.")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "El campo ApellidoEmpleado no puede contener números.")]
        public string ApellidoEmpleado { get; set; }

        [Required(ErrorMessage = "El campo Edad es obligatorio.")]
        [Range(1, 100, ErrorMessage = "La Edad debe estar entre 1 y 100.")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [MaxLength(50, ErrorMessage = "La contraseña no puede tener más de 50 caracteres.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "La contraseña debe contener al menos una letra mayúscula y al menos un número.")]
        public string Contraseña { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [Compare("Contraseña", ErrorMessage = "Las contraseñas no coinciden.")]
        public string RepetirContraseña { get; set; }


        [Required(ErrorMessage = "El campo DNI es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de DNI debe ser mayor que cero.")]
        public int DNI { get; set; }

    }
}
