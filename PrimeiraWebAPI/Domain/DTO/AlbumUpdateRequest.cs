using System.ComponentModel.DataAnnotations;

namespace PrimeiraWebAPI.Domain.DTO
{
    public class AlbumUpdateRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "O Artista é obrigatório")]
        public string? Artista { get; set; }
    }
}

//Não é são todos os campos que devemos permitir atualização. Dessa forma, é importante ter
//outra DTO só com os campos permitidos.