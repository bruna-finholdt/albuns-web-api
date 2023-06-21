using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeiraWebAPI.Domain.DTO
{
    public class AvaliacaoCreateRequest
    {
        [Required]
        public int IdAlbum { get; set; }
        [Required]
        public int Nota { get; set; }
        public string Comentario { get; set; }
    }
}