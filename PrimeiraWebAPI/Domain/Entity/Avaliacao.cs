using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace PrimeiraWebAPI.Domain.Entity
{
    [Table("Avaliacoes")]
    public class Avaliacao
    {
        [Key]
        public int IdAvaliacao { get; set; }
        [Required]
        public int IdAlbum { get; set; } //A avaliação deverá ter a chave estrangeria do álbum. - creio ser o IdAlbum
        [Required]
        public int Nota { get; set; }
        [StringLength(255)]
        public string Comentario { get; set; }
        // virtual para indicar o relacionamento
        public virtual Album Album { get; set; } 
    }
}
