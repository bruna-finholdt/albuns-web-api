using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PrimeiraWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbunsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<String> Get()
        {
            List<string> listaDeDiscos = new List<string>();
            listaDeDiscos.Add("s");
            return listaDeDiscos;
        }

        [HttpGet("{id}")]
        public String GetById(int id)
        {
            return "Album tal " + id;
        }

        [HttpGet("nome/{nome}")]
        public String GetByNome(string nome) // nome do parametro deve ser o mesmo do {}
        {
            return "Album tal " + nome;
        }
    }
}
