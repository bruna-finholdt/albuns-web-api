using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrimeiraWebAPI.Domain.DTO;
using PrimeiraWebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeiraWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacoesController : ControllerBase
    {
        private readonly AvaliacoesService avaliacaoService;
        public AvaliacoesController(AvaliacoesService avaliacaoService)
        {
            this.avaliacaoService = avaliacaoService;
        }
        [HttpPost]
        // FromBody para indicar de o corpo da requisição deve ser mapeado para o modelo
        public IActionResult Post([FromBody] AvaliacaoCreateRequest postModel)
        {
            //Validação modelo de entrada
            if (ModelState.IsValid)
            {
                var retorno = avaliacaoService.CadastrarNovo(postModel);
                if (!retorno.Sucesso)
                    return BadRequest(retorno);
                else
                    return Ok(retorno.ObjetoRetorno);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
