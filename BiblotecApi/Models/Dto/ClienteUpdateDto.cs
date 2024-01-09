using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models.Dto
{
    public class ClienteUpdateDto
    {
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public int Edad { get; set; }
        public int DNI { get; set; }
        public string Direccion { get; set; }
    }

}
