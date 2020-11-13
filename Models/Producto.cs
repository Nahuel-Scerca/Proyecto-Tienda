using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda_MAWS.Models
{

    public enum enCategoria
    {
        Calzado = 1,
        Remera = 2,
        Pantalon = 3,
    }
    public class Producto
    {
        [Display(Name = "Código")]
        public int Id { get; set; }


        [Required]
        [Display(Name = "Descripcion")]
        public String Descripcion { get; set; }

        [Required]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [Required]
        [Display(Name = "Categoria")]
        public int Categoria { get; set; }

        [Required]
        [Display(Name = "Stock")]
        public int Stock { get; set; }

        [Required]
        [Display(Name = "Talle")]
        public String Talle { get; set; }

        [Required]
        [Display(Name = "Color")]
        public String Color { get; set; }

        [Required]
        [Display(Name = "Foto")]
        public string Foto { get; set; }


        [Display(Name = "Categoria")]
        public string CategoriaNombre => Categoria > 0 ? ((enCategoria)Categoria).ToString().Replace('_', ' ') : "";

        public static IDictionary<int, string> ObtenerRoles()
        {
            SortedDictionary<int, string> categoria = new SortedDictionary<int, string>();
            Type tipoEnumEstado = typeof(enCategoria);
            foreach (var valor in Enum.GetValues(tipoEnumEstado))
            {
                categoria.Add((int)valor, Enum.GetName(tipoEnumEstado, valor).Replace('_', ' '));
            }
            return categoria;
        }
    }
}
