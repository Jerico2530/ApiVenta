using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecApi.Models
{
    [Table("Prenda")]
    public class Prenda
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int IdPrenda { get; set; }
        [Required]
        public string NombrePrenda { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Precio { get; set; }
        [Required]
        public double Stock { get; set; }
        [Required]
        public int IdCategoria { get; set; }
        [ForeignKey("IdCategoria")]
        public Categoria Categoria { get; set; }
        [Required]
        public int IdColor { get; set; }
        [ForeignKey("IdColor")]
        public Color Color { get; set; }
        [Required]
        public int IdMarca {  get; set; }
        [ForeignKey("IdMarca")]
        public Marca Marca { get; set; }
        [Required]
        public int IdTalla {  get; set; }
        [ForeignKey("IdTalla")]
        public Talla Talla { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaCreacion { get; set; }
    }
}
