using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models.Dto
{
    public class VentaDto
    {
        public int IdVenta { get; set; }
        public string Descripcion { get; set; }
        public int IdEmpleado { get; set; }
        public int IdPrenda { get; set; }
        public int IdCliente { get; set; }
        public DateTime FechaCreacion { get; set; }

    }
}
