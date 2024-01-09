using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models.Dto
{
    public class MarcaUpdateDto
    {

        public int IdMarca { get; set; }

        public string NombreMarca { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
