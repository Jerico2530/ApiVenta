using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models.Dto
{
    public class CategoriaDto
    {

        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
    }
}
