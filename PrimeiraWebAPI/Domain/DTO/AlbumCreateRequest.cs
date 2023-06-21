using System;
using System.ComponentModel.DataAnnotations; //para atributos como o required por ex

namespace PrimeiraWebAPI.Domain.DTO
{
    public class AlbumCreateRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Nome { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O Artista é obrigatório!")]
        public string Artista { get; set; }

        [Required] //atributo dedicado à validação que indica que o campo é obgtório
        public int? AnoLancamento { get; set; } //'int?' ao inves de 'int' pq senão o valor padrão vai sempre ver 0
                                                //e nunca saberemos se é o valor passado ou o padrão (só achei desnecessário pra esse caso pois nunca vai
                                                //ter um anod e lançamento pra album que seja 0, mas pra outros casos blz.

    }
}
