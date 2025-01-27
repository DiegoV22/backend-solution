using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_model
{
    public class Jugadores
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Clave primaria

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(100)]
        public string Apellido { get; set; }

        [Required]
        [MaxLength(10)]
        public string Cedula { get; set; }

        [Required]
        public int NumCamiseta { get; set; }

        [Required]
        [MaxLength(10)]
        public string Lateralidad { get; set; }

        // Relación con Equipo
        public int? EquipoId { get; set; } // Clave foránea opcional
        [ForeignKey("EquipoId")]
        public Equipos Equipo { get; set; } // Propiedad de navegación hacia Equipos
    }
}
