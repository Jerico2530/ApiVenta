using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models.Dto
{
    public class ColorDto
    {
        public int IdColor { get; set; }
        public string NombreColor { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
