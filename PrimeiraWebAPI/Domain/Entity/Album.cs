using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeiraWebAPI.Domain.Entity
{
    [Table("Albuns")]
    public class Album
    {
        [Key]
        public int IdAlbum { get; set; }
        public string? Nome { get; set; } //? pois devemos fazer uma validação para propriedades que possam receber 
                                          //valores null  
        public string? Artista { get; set; } //
        public int AnoLancamento { get; set; }
        public virtual ICollection<Avaliacao>? Avaliacoes { get; set; } //Esta coleção virtual será utilizada para
                                                                        //definir o vinculo da chave estrangeira
    }
}




