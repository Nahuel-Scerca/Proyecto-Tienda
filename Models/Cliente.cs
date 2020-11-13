using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Tienda_MAWS.Models
{
    public class Cliente
    {
		[Key]
		[Display(Name = "Código")]
		public int Id { get; set; }

		[Required]
		public string Nombre { get; set; }
		[Required]
		public string Apellido { get; set; }
		[Required]
		public string Dni { get; set; }
		public string Telefono { get; set; }
		[Required, EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Direccion { get; set; }
		
	}
}
