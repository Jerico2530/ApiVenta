using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models.Dto
{
    public class TallaDto
    {

        public int IdTalla { get; set; }

        public string NumeroTalla { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
