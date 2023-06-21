using PrimeiraWebAPI.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeiraWebAPI.Domain.DTO
{
    public class AlbumResponse
    {
        //construtor
        public AlbumResponse(Album album)
        {
            IdAlbum = album.IdAlbum;
            Nome = album.Nome;
            Artista = album.Artista;
            AnoLancamento = album.AnoLancamento;

            if (album.Avaliacoes != null && album.Avaliacoes.Any())
            {

                Avaliacoes = new List<AvaliacaoResponse>();
                Avaliacoes.AddRange(album.Avaliacoes.Select(x => new AvaliacaoResponse(x)));
            }
        }

        //atributos
        public int IdAlbum { get; set; }
        public string Nome { get; set; }
        public string Artista { get; set; }
        public int AnoLancamento { get; set; }
        public List<AvaliacaoResponse> Avaliacoes { get; set; }
    }
}