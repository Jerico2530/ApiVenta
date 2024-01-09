using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models.Dto
{
    public class PrendaUpdateDto
    {
        public int IdPrenda { get; set; }

        public string NombrePrenda { get; set; }


        public decimal Precio { get; set; }

        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }

        public int IdColor { get; set; }
        public string NombreColor { get; set; }

        public int IdMarca { get; set; }
        public string NombreMarca { get; set; }

        public int IdTalla { get; set; }
        public string NumeroTalla { get; set; }
        public double Stock { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
