using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend_model
{
    public class Torneos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Clave primaria

        [Required]
        [MaxLength(100)]
        public string NombreTorneo { get; set; } // Nombre del torneo

        [Required]
        public DateTime FechaInicio { get; set; } // Fecha de inicio

        [Required]
        public DateTime FechaFin { get; set; } // Fecha de fin

        // Relación con Equipos
        [JsonIgnore] // Ignorar la propiedad Equipos para evitar ciclos
        public ICollection<Equipos> Equipos { get; set; } // Lista de equipos asociados
    }
}
