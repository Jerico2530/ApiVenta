using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models
{
    [Table("Empleado")]
    public class Empleado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEmpleado { get; set; }
        [Required]
        public string NombreEmpleado { get; set; }
        [Required]
        public string ApellidoEmpleado { get; set; }
        [Required]
        public int Edad { get; set; }
        [Required]
        public string Contraseña { get; set; }
        [Required]
        public string RepetirContraseña { get; set; }
        [Required]
        public int DNI { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaCreacion { get; set; }
    }
}
