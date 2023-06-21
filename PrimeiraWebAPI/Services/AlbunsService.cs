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
        private static List<Album>? listaDeAlbuns; //? colocado 
        private static int proximoId = 1;

        public AlbunsService()
        {
            //<pre><code> 

            if (listaDeAlbuns == null)
            {
                listaDeAlbuns = new List<Album>();
                listaDeAlbuns.Add(new Album()
                {
                    IdAlbum = proximoId++,
                    Nome = "Album 1",
                    Artista = "Artista Album 1",
                    Anolancamento = 1994
                });
                listaDeAlbuns.Add(new Album()
                {
                    IdAlbum = proximoId++,
                    Nome = "Album 2",
                    Artista = "Artista Album 2",
                    Anolancamento = 1971
                });
                listaDeAlbuns.Add(new Album()
                {
                    IdAlbum = proximoId++,
                    Nome = "Album 3",
                    Artista = "Artista Album 3",
                    Anolancamento = 2016
                });
                listaDeAlbuns.Add(new Album()
                {
                    IdAlbum = proximoId++,
                    Nome = "Album 4",
                    Artista = "Artista Album 4",
                    Anolancamento = 1972
                });

            }

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
                IdAlbum = proximoId++,
                Nome = model.Nome,
                Artista = model.Artista,
                Anolancamento = model.AnoLancamento.Value
            };

            listaDeAlbuns?.Add(novoAlbum); //? colocado
            //return novoAlbum; alteração aqui tb
            return new ServiceResponse<Album>(novoAlbum);
        }

        public List<Album>? ListarTodos() //? colocado
        {
            return listaDeAlbuns;
        }

        public ServiceResponse<Album> PesquisarPorId(int id)
        {
            var resultado = listaDeAlbuns?.Where(x => x.IdAlbum == id).FirstOrDefault(); //?
            if (resultado == null)
            {
                return new ServiceResponse<Album>("Não encontrado!");
            }
            else
            {
                return new ServiceResponse<Album>(resultado);
            }
        }

        public ServiceResponse<Album> PesquisarPorNome(string nome)
        {
            var resultado = listaDeAlbuns?.Where(x => x.Nome == nome).FirstOrDefault(); //?
            if (resultado == null)
            {
                return new ServiceResponse<Album>("Não encontrado!");
            }
            else
            {
                return new ServiceResponse<Album>(resultado);
            }
        }

        public ServiceResponse<Album> Editar(int id, AlbumUpdateRequest model)
        {
            var resultado = listaDeAlbuns?.Where(x => x.IdAlbum == id).FirstOrDefault();
            if (resultado == null)
            {
                return new ServiceResponse<Album>("Album não encontrado!");
            }
            //sendo encontrado => atualizando...
            resultado.Artista = model.Artista;

            return new ServiceResponse<Album>(resultado);
        }

        public ServiceResponse<bool> Deletar(int id)
        {
            var resultado = listaDeAlbuns?.Where(x => x.IdAlbum == id).FirstOrDefault();
            if (resultado == null)
            {
                return new ServiceResponse<bool>("Album não encontrado!");
            }
            //sendo encontrado => deletando...
            listaDeAlbuns?.Remove(resultado);

            return new ServiceResponse<bool>(true);
        }
    }
}





