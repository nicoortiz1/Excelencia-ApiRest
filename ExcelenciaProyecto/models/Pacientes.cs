using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExcelenciaProyecto.Models
{
    public class Pacientes
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [Required, StringLength(100)]
        public string Apellido { get; set; }

        [Required]
        public string DNI { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public string Telefono { get; set; }

        public string Email { get; set; }

        public string Direccion { get; set; }

        //public ICollection<Cita> Citas { get; set; }
    }
}