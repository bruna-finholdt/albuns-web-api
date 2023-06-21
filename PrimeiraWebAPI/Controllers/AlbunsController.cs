using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrimeiraWebAPI.Domain.DTO;
using PrimeiraWebAPI.Domain.Entity;
using PrimeiraWebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeiraWebAPI.Controllers
{
    //attributes ou atributos: Os atributos fornecem um método eficiente de associação de metadados, ou
    //informações declarativas ao código (assemblies, tipos, métodos, propriedades e etc)
    //O atributo [Route("api/[controller]")], por exemplo, indica que o controller em questão será responsável
    //por responder requisições feitas na URL "/api/Albuns". 

    [Route("api/[controller]")] //Atributo que indica qual a URL usada por este controller. IMPORTANTE: Não é
                                //necessário trocar o [controller] pelo nome do controller, ele faz isso em
                                //tempo de execução.

    [ApiController] //Atributo que indica para o framework que esta classe é um controller de Web API.
    public class AlbunsController : ControllerBase //Todo Controller deve herdar da classe abstrada ControllerBase,
                                                   //ela fornece vários métodos base, que vamos utilizar em nossa
                                                   //Web API.
    {
        //usando o AlbunsService via injeção de dependência:
        private readonly AlbunsService albumService;
        public AlbunsController(AlbunsService albumService)
        {
            this.albumService = albumService;
        }

        [HttpGet]
        public IEnumerable<AlbumResponse>? Get() //? colocado
        {
            //List<string> listaDeDiscos = new List<string>();
            //listaDeDiscos.Add("Album 1");
            //listaDeDiscos.Add("Album 2");
            //listaDeDiscos.Add("Album 3");
            //listaDeDiscos.Add("Album 4");
            //return listaDeDiscos;
            return albumService?.ListarTodos(); //?
        }

        [HttpGet("{id}")] //se a seguinte URL for acessada: "/api/Albuns/4", nosso método será executando
                          //recebendo 4 como parâmetro.
        public IActionResult GetById(int id)
        {
            var retorno = albumService.PesquisarPorId(id);
            if (retorno.Sucesso)
            {
                return Ok(retorno.ObjetoRetorno);
            }
            else
            {
                return NotFound(retorno);
            }
        }

        [HttpGet("nome/{nome}")]
        public IActionResult GetByNome(string nome)
        {
            var retorno = albumService.PesquisarPorNome(nome);
            if (retorno.Sucesso)
            {
                return Ok(retorno.ObjetoRetorno);
            }
            else
            {
                return NotFound(retorno);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] AlbumCreateRequest postModel) //FromBody para indicar q o corpo da req deve
                                                                           //ser mapeado para o modelo

        //usa-se IActionResult no retorno pois aqui podemos ter um retorno específico para cada cenário: valid/not valid
        //os de cima foram alterados com os passo-a-passo e tb usam essa interface agora pois tb possuem cenários de 
        //return especificos
        {
            //validação modelo de entrada
            if (ModelState.IsValid)//modelState é o objeto que guarda o estado de validação do modelo de entrada, ou seja
                                   //a validação dos parametros do metodo
            {
                var retorno = albumService.CadastrarNovo(postModel);
                if (!retorno.Sucesso)
                {
                    return BadRequest(retorno.Mensagem);
                }
                else
                {
                    return Ok(retorno);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] AlbumUpdateRequest putModel)
        {
            if (ModelState.IsValid)
            {
                var retorno = albumService.Editar(id, putModel);
                if (!retorno.Sucesso)
                {
                    return BadRequest(retorno.Mensagem);
                }
                else
                {
                    return Ok(retorno.ObjetoRetorno);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var retorno = albumService.Deletar(id);
            if (!retorno.Sucesso)
            {
                return BadRequest(retorno.Mensagem);
            }
            else
            {
                return Ok();
            }
        }

    }
}


