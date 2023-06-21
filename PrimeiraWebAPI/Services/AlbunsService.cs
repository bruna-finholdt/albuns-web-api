using Microsoft.EntityFrameworkCore;
using PrimeiraWebAPI.DAL;
using PrimeiraWebAPI.Domain.DTO;
using PrimeiraWebAPI.Domain.Entity;
using PrimeiraWebAPI.Services.Base;
using PrimeiraWebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PrimeiraWebAPI.Services
{
    public class AlbunsService //é a classe responsável por realizar as regras de negócio relacionadas à entidade Album
    {
        //private static List<Album>? listaDeAlbuns;  
        //private static int proximoId = 1;
        private readonly AppDbContext? _dbContext;

        public AlbunsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;

            //if (listaDeAlbuns == null)
            //{
            //    listaDeAlbuns = new List<Album>();
            //    listaDeAlbuns.Add(new Album()
            //    {
            //        IdAlbum = proximoId++,
            //        Nome = "Da Lama ao Caos",
            //        Artista = "Chico Science & Nação Zumbi",
            //        AnoLancamento = 1994
            //    });
            //    listaDeAlbuns.Add(new Album()
            //    {
            //        IdAlbum = proximoId++,
            //        Nome = "Fragile",
            //        Artista = "Yes",
            //        AnoLancamento = 1971
            //    });
            //    listaDeAlbuns.Add(new Album()
            //    {
            //        IdAlbum = proximoId++,
            //        Nome = "This Is Acting",
            //        Artista = "Dia",
            //        AnoLancamento = 2016
            //    });
            //    listaDeAlbuns.Add(new Album()
            //    {
            //        IdAlbum = proximoId++,
            //        Nome = "Clube da Esquina",
            //        Artista = "Milton Nascimento e Lô Borges",
            //        AnoLancamento = 1972
            //    });
            //}


        }
        public ServiceResponse<Album> CadastrarNovo(AlbumCreateRequest model) //alteração pós criação ServiceResponse Base
        //public Album CadastrarNovo(AlbumCreateRequest model)
        {
            //regra de negócio: somente albuns lançados entre 1950 e o ano atual (2023)
            if (!model.AnoLancamento.HasValue || model.AnoLancamento < 1950 || model.AnoLancamento > DateTime.Today.Year)
            //se não tiver ano de lanc ou se o ano de lanc for anterior a 1950 ou se ano lanc for posterior ao ano atual
            {
                //return null //alteração aqui tb
                return new ServiceResponse<Album>("Somente é possível cadastrar albuns lançados entre 1950 e o ano atual");
            }

            //após passar pela verificação das regras de negócio, cadastrar o album:
            //obg não continuamos a execução de CadastrarNovo caso seja inválido (não passa pela validação do if)
            var novoAlbum = new Album()
            {
                //IdAlbum = proximoId++,
                Nome = model.Nome,
                Artista = model.Artista,
                AnoLancamento = model.AnoLancamento.Value
            };

            //listaDeAlbuns?.Add(novoAlbum);
            //return novoAlbum; alteração aqui tb
            _dbContext?.Add(novoAlbum);
            _dbContext?.SaveChanges();
            return new ServiceResponse<Album>(novoAlbum);
        }

        //public List<Album>? ListarTodos()
        public IEnumerable<AlbumResponse> ListarTodos()
        {
            //return listaDeAlbuns;
            // select  * from albuns
            //return _dbContext?.Albuns?.ToList();
            // left join avaliacoes a on a.idAlbum = x.idAlbum

            var retornoDoBanco = _dbContext.Albuns.Include(x => x.Avaliacoes).ToList();

            // Conveter para AlbumResponse
            IEnumerable<AlbumResponse> lista = retornoDoBanco.Select(x => new AlbumResponse(x));

            return lista;
        }

        //public ServiceResponse<Album> PesquisarPorId(int id)
        public ServiceResponse<AlbumResponse> PesquisarPorId(int id)
        {
            //var resultado = listaDeAlbuns?.Where(x => x.IdAlbum == id).FirstOrDefault(); 
            //var resultado = _dbContext?.Albuns?.FirstOrDefault(x => x.IdAlbum == id);

            // left join avaliacoes a on a.idAlbum = x.idAlbum
            // where x.IdAlbum == id 
            var resultado = _dbContext.Albuns.Include(x => x.Avaliacoes).FirstOrDefault(x => x.IdAlbum == id);
            if (resultado == null)
            {
                //return new ServiceResponse<Album>("Não encontrado!");
                return new ServiceResponse<AlbumResponse>("Não encontrado!");

            }
            else
            {
                //return new ServiceResponse<Album>(resultado);
                return new ServiceResponse<AlbumResponse>(new AlbumResponse(resultado));
            }
        }

        //public ServiceResponse<Album> PesquisarPorNome(string nome)
        public ServiceResponse<AlbumResponse> PesquisarPorNome(string nome)
        {
            //var resultado = listaDeAlbuns?.Where(x => x.Nome == nome).FirstOrDefault();
            //var resultado = _dbContext?.Albuns?.FirstOrDefault(x => x.Nome == nome);

            // left join avaliacoes a on a.idAlbum = x.idAlbum
            // where x.nome == no,e 
            var resultado = _dbContext.Albuns.Include(x => x.Avaliacoes).FirstOrDefault(x => x.Nome == nome);
            if (resultado == null)
            {
                //return new ServiceResponse<Album>("Não encontrado!");
                return new ServiceResponse<AlbumResponse>("Não encontrado!");
            }
            else
            {
                //return new ServiceResponse<Album>(resultado);
                return new ServiceResponse<AlbumResponse>(new AlbumResponse(resultado));
            }
        }

        public ServiceResponse<Album> Editar(int id, AlbumUpdateRequest model)
        {
            //var resultado = listaDeAlbuns?.Where(x => x.IdAlbum == id).FirstOrDefault();
            var resultado = _dbContext?.Albuns?.FirstOrDefault(x => x.IdAlbum == id);
            if (resultado == null)
            {
                return new ServiceResponse<Album>("Album não encontrado!");
            }
            //sendo encontrado => atualizando...
            resultado.Artista = model.Artista;

            _dbContext.Albuns.Add(resultado).State = EntityState.Modified; //_dbContext.Albuns - Acessamos
                                                                           //o DBSet para conseguirmos manipular a entidade.
            _dbContext?.SaveChanges();
            //Sempre que fizermos qualquer alteração, teremos que executar _dbContext.SaveChanges(), para
            //"enviar" as alterações para o banco de dados.

            return new ServiceResponse<Album>(resultado);
        }

        public ServiceResponse<bool> Deletar(int id)
        {
            //var resultado = listaDeAlbuns?.Where(x => x.IdAlbum == id).FirstOrDefault();
            var resultado = _dbContext?.Albuns?.FirstOrDefault(x => x.IdAlbum == id);
            if (resultado == null)
            {
                return new ServiceResponse<bool>("Album não encontrado!");
            }
            //sendo encontrado => deletando...
            //listaDeAlbuns?.Remove(resultado);

            _dbContext?.Albuns?.Remove(resultado);
            _dbContext?.SaveChanges();
            //Sempre que fizermos qualquer alteração, teremos que executar _dbContext.SaveChanges(), para
            //"enviar" as alterações para o banco de dados.

            return new ServiceResponse<bool>(true);
        }
    }
}





