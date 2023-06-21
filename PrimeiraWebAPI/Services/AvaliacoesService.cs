using PrimeiraWebAPI.DAL;
using PrimeiraWebAPI.Domain.DTO;
using PrimeiraWebAPI.Domain.Entity;
using PrimeiraWebAPI.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeiraWebAPI.Services
{
    public class AvaliacoesService
    {
        private readonly AppDbContext _dbContext;
        public AvaliacoesService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ServiceResponse<AvaliacaoResponse> CadastrarNovo(AvaliacaoCreateRequest model)
        {
            // validação de integridade
            var resultado = _dbContext?.Albuns?.FirstOrDefault(x => x.IdAlbum == model.IdAlbum);
            if (resultado == null)
                return new ServiceResponse<AvaliacaoResponse>("Album não encontrado");
            //Regra de negócio
            // A nota da avaliação deve ser entre 1 e 5
            if (model.Nota < 1 || model.Nota > 5)
            {
                return new ServiceResponse<AvaliacaoResponse>("A nota da avaliação deve ser um número entre 1 e 5");
            }
            //tudo certo, só cadastrar
            var nova = new Avaliacao()
            {
                Nota = model.Nota,
                Comentario = model.Comentario,
                IdAlbum = model.IdAlbum
            };
            _dbContext.Add(nova);
            _dbContext.SaveChanges();
            var retorno = new AvaliacaoResponse(nova);
            return new ServiceResponse<AvaliacaoResponse>(retorno);
        }
    }
}