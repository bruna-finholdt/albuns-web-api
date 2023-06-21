using PrimeiraWebAPI.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PrimeiraWebAPI.Domain.DTO
{
    public class AvaliacaoResponse
    {
        //construtor
        public AvaliacaoResponse(Avaliacao avaliacao)
        {
            IdAvaliacao = avaliacao.IdAvaliacao;
            IdAlbum = avaliacao.IdAlbum;
            Nota = avaliacao.Nota;
            Comentario = avaliacao.Comentario;
        }

        //atributos
        public int IdAvaliacao { get; set; }
        public int IdAlbum { get; set; }
        public int Nota { get; set; }
        public string Comentario { get; set; }
    }
}
