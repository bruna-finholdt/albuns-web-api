using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeiraWebAPI.Domain.Entity
{
    public class Album
    {
        public int IdAlbum { get; set; }
        public string? Nome { get; set; } //? pois devemos fazer uma validação para propriedades que possam receber 
                                          //valores null  
        public string? Artista { get; set; } //
        public int Anolancamento { get; set; }
    }
}




