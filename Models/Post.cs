using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiProva.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; }

        [Required]
        public string Conteudo { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        [JsonIgnore]
        public Usuario Usuario { get; set; }

    }
}
