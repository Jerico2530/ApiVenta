using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models.Dto
{
    public class EmpleadoDto
    {
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string ApellidoEmpleado { get; set; }
        public string Contraseña { get; set; }

        public string RepetirContraseña { get; set; }

        public int Edad { get; set; }
        public int DNI { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
