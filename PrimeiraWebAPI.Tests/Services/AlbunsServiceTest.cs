using Microsoft.EntityFrameworkCore;
using PrimeiraWebAPI.DAL;
using PrimeiraWebAPI.Domain.DTO;
using PrimeiraWebAPI.Domain.Entity;
using PrimeiraWebAPI.Services;

namespace PrimeiraWebAPI.Tests.Services
{
    public class AlbunsServiceTest : IDisposable
    {
        /// <summary>
        /// DbContext é a camada de acesso ao banco
        /// </summary>
        private readonly AppDbContext _dbContext; //_dbContext is an instance of the AppDbContext class, which represents
                                                  //the database context for accessing the database.

        /// <summary>
        /// Service que iremos testar
        /// </summary>
        private readonly AlbunsService _service; //_service is an instance of the AlbunsService class, which is the

        /// <summary>
        /// Aqui preparamos os testes
        /// </summary>
        /// 
        //construtor:
        public AlbunsServiceTest()
        {
            // Criando banco em memória
            var options = new DbContextOptionsBuilder<AppDbContext>()
                // Guid.NewGuid().ToString(): Garantindo a criação de um banco novo
                //  a cada execução de teste, evitando a existência de dados não inseridos durante os testes
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            // Criamos as instâncias que vamos usar nos testes
            _dbContext = new AppDbContext(options);
            _service = new AlbunsService(_dbContext);
            // the constructor initializes the _dbContext field with the database context, and initializes the _service
            // field with a new instance of the AlbunsService class.

            //No construtor criamos um banco de dados em memória do EFCore e usamos o campo _dbContext para armazená-lo 
            //Além disso cada vez que um teste é executado um novo nome aleatório é gerado Guid.NewGuid().ToString()
            //Por fim, instanciamos o _service que vamos testar
        }

        //Execução dos testes:
        //Aqui criamos dois testes um pra caso de sucesso e outro para casos de erro
        //Dado uma entrada: Álbum com data válida e outro com data inválida;
        //Esperamos tais comportamentos: Esperamos a inserção no banco do álbum válido e o erro no álbum inválido;
        //E tais saídas: Esperamos receber o álbum cadastrado no sucesso e o erro com mensagem no caso de erro;
        [Fact]
        public void Quando_PassadoAlbumValido_DeveCadastrar_E_Retornar()
        {
            // Preparando entrada
            var request = new AlbumCreateRequest()
            {
                Nome = "Album Test",
                AnoLancamento = 1990,
                Artista = "Artista Test"
            };

            // Executando
            var retorno = _service.CadastrarNovo(request);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.AnoLancamento, request.AnoLancamento);
            Assert.Equal(retorno.ObjetoRetorno.Artista, request.Artista);
            Assert.Equal(retorno.ObjetoRetorno.Nome, request.Nome);
            //Tests the creation of a valid album and verifies if the returned album matches the input.
        }

        [Fact]
        public void Quando_PassadoAlbumInvalido_Deve_RetornarErro()
        {
            var mensagemEsperada = "Somente é possível cadastrar albuns lançados entre 1950 e o ano atual";

            // Preparando entrada
            var request = new AlbumCreateRequest()
            {
                Nome = "Album Test",
                // Ano inválido para provocar erro de validação
                AnoLancamento = 1949,
                Artista = "Artista Test"
            };

            // Executando
            var retorno = _service.CadastrarNovo(request);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada); //Tests the creation of an invalid album and verifies if
                                                              //the returned result indicates an error with the expected
                                                              //error message.
        }

        //Testamos o cadastro de um album acima, agora vamos criar testes para listagem geral, listagem por id
        //e listagem por nome:

        //Execução dos testes: Implementados todos os testes de consultas temos os seguintes casos:
        //Listagem geral: Verifica se todos os itens inseridos pelo ListaAlbunsStub são retornados na consulta;
        //Consulta por id - Caso sucesso: Verifica se o primeiro item inserido é retornado na consulta;
        //Consulta por id - Caso erro: Verifica se o id do último item + 1 dispara erro "Não encontrado!";
        //Consulta por nome - Caso sucesso: Verifica se o nome do primeiro item inserido é retornado na consulta;
        //Consulta por nome - Caso erro: Verifica se um nome aleatório dispara erro "Não encontrado!";

        //Obs: O método ListaAlbunsStub cria, salva no banco e retorna lista de
        //álbuns para que possamos manipulá-la nos nossos testes:
        [Fact]
        public void Quando_ChamadoListarTodos_Deve_RetornarTodos()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();

            // Executa e cria objeto do tipo lista a partir da execução
            var retorno = new List<AlbumResponse>(_service.ListarTodos());

            // Validando resultados
            Assert.Equal(retorno.Count, lista.Count); //Tests the listing of all albums and verifies if the number of
                                                      //returned albums matches the number of albums in the database.
        }

        [Fact]
        public void Quando_ChamadoPesquisaPorId_Com_IdExistente_Deve_RetornarAlbum()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();

            // Executa com id que foi cadastrado no banco
            var retorno = _service.PesquisarPorId(lista[0].IdAlbum);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.IdAlbum, lista[0].IdAlbum);
            Assert.Equal(retorno.ObjetoRetorno.Nome, lista[0].Nome);
            Assert.Equal(retorno.ObjetoRetorno.Artista, lista[0].Artista);
            Assert.Equal(retorno.ObjetoRetorno.AnoLancamento, lista[0].AnoLancamento);
            Assert.Equal(retorno.ObjetoRetorno.Avaliacoes.Average(x => x.Nota).ToString(), 4.ToString());
            //Tests the search for an album by ID when the ID exists in the database and verifies if the returned album
            //matches the expected album.
        }

        [Fact]
        public void Quando_ChamadoPesquisaPorId_Com_IdNaoExistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();
            var mensagemEsperada = "Não encontrado!";

            // Executa com id que foi cadastrado no banco + 1,
            // assim temos certeza que vamos consultar um id que não existe
            var retorno = _service.PesquisarPorId(lista.Count + 1);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
            //Tests the search for an album by ID when the ID does not exist in the database and verifies if the returned
            //result indicates an error with the expected error message.
        }

        [Fact]
        public void Quando_ChamadoPesquisaPorNome_Com_NomeExistente_Deve_RetornarAlbum()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();

            // Executa com nome que foi cadastrado no banco
            var retorno = _service.PesquisarPorNome(lista[0].Nome);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.IdAlbum, lista[0].IdAlbum);
            Assert.Equal(retorno.ObjetoRetorno.Nome, lista[0].Nome);
            Assert.Equal(retorno.ObjetoRetorno.Artista, lista[0].Artista);
            Assert.Equal(retorno.ObjetoRetorno.AnoLancamento, lista[0].AnoLancamento);
            Assert.Equal(retorno.ObjetoRetorno.Avaliacoes.Average(x => x.Nota).ToString(), 4.ToString());
            //Tests the search for an album by name when the name exists in the database and verifies if the returned
            //album matches the expected album.
        }

        [Fact]
        public void Quando_ChamadoPesquisaPorNome_Com_NomeNaoExistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            ListaAlbunsStub();
            var mensagemEsperada = "Não encontrado!";

            // Executa com nome que não foi cadastrado no banco
            var retorno = _service.PesquisarPorNome("Nome inexistente");

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
            //Tests the search for an album by name when the name does not exist in the database and verifies if the
            //returned result indicates an error with the expected error message.
        }

        //Por fim vamos testar os métodos Edição por id e Exclusão por id:
        //Execução dos testes: Implementados todos os testes que faltavam:
        //Edição por id - sucesso: Verifica se o primeiro item inserido consegue ser editado;
        //Edição por id - erro: Verifica se o id do último item + 1 dispara erro "Album não encontrado!";
        //Exclusão por id - sucesso: Verifica se o primeiro item inserido consegue ser deletado;
        //Exclusão por id - erro: Verifica se o id do último item + 1 dispara erro "Album não encontrado!";
        [Fact]
        public void Quando_ChamadoEditar_Com_IdExistente_Deve_RetornarAlbumAtualizado()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();

            // Preparando entrada
            var request = new AlbumUpdateRequest()
            {
                Artista = "Novo nome de artista"
            };

            // Executa com id que foi cadastrado no banco
            var retorno = _service.Editar(lista[0].IdAlbum, request);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.Artista, lista[0].Artista);//Verifica se o artista do álbum foi
                                                                          //atualizado

            //Tests the editing of an album by ID when the ID exists in the database and verifies if the returned album
            //has been updated.                                                              
        }

        [Fact]
        public void Quando_ChamadoEditar_Com_IdNaoExistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();
            var mensagemEsperada = "Album não encontrado!";

            // Preparando entrada
            var request = new AlbumUpdateRequest();

            // Executa com id que foi cadastrado no banco + 1,
            // assim temos certeza que vamos consultar um id que não existe
            var retorno = _service.Editar(lista.Count + 1, request);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
            //Tests the editing of an album by ID when the ID does not exist in the database and verifies if the returned
            //result indicates an error with the expected error message.
        }

        [Fact]
        public void Quando_ChamadoDeletar_Com_IdExistente_Deve_RetornarAlbumAtualizado()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();

            // Executa com id que foi cadastrado no banco
            var retorno = _service.Deletar(lista[0].IdAlbum);

            // Validando resultados
            Assert.True(retorno.ObjetoRetorno);
            // Verifica se existe um álbum a menos na base
            Assert.Equal(_dbContext.Albuns.Count(), lista.Count - 1);//Verifica se existe um álbum a menos na base
        }

        [Fact]
        public void Quando_ChamadoDeletar_Com_IdNaoExistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();
            var mensagemEsperada = "Album não encontrado!";

            // Executa com id que foi cadastrado no banco + 1,
            // assim temos certeza que vamos consultar um id que não existe
            var retorno = _service.Deletar(lista.Count + 1);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
            // Verifica se existe o mesmo número de álbuns na base
            Assert.Equal(_dbContext.Albuns.Count(), lista.Count);//Verifica se existe o mesmo número de álbuns na
                                                                 //base
        }


        /// <summary>
        /// Stub Albuns
        /// </summary>
        /// <returns>Conjunto de dados que mockamos para usar em testes</returns>
        private List<Album> ListaAlbunsStub()
        {
            // Dados para mock
            var lista = new List<Album>()
            {
                new Album()
                {
                    Nome = "Album Test 1",
                    AnoLancamento = 1990,
                    Artista = "Artista Test 1",
                    Avaliacoes = ListaAvaliacoesStub()
                },
                new Album()
                {
                    Nome = "Album Test 2",
                    AnoLancamento = 1991,
                    Artista = "Artista Test 2",
                    Avaliacoes = ListaAvaliacoesStub()
                }
            };

            // Salvamos os dados no banco
            _dbContext.AddRange(lista);
            _dbContext.SaveChanges();

            // Retornamos para usar nas validações
            return lista;
        }

        //Na classe AlbunsServiceTest adicione um Stub para o campo Avaliacoes de Album no método ListaAlbunsStub

        private List<Avaliacao> ListaAvaliacoesStub()
        {
            var listaAvaliacoes = new List<Avaliacao>()
            {
                new Avaliacao()
                {
                    Nota = 5,
                    Comentario = "Ótimo"
                },
                new Avaliacao()
                {
                    Nota = 3,
                    Comentario = "Legalzinho"
                }
            };

            _dbContext.AddRange(listaAvaliacoes);
            _dbContext.SaveChanges();

            return listaAvaliacoes;
        }

        /// <summary>
        /// Método que é executado quando os testes são encerrados.
        /// O XUnit chama o método Dispose definido na interface IDisposable.
        /// </summary>
        public void Dispose()
        {
            // Garante que o banco usado nos testes foi deletado
            _dbContext.Database.EnsureDeleted();
            // Informa pro Garbage Collector que o objeto já foi limpo. Leia mais:
            // - https://docs.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1816
            // - https://stackoverflow.com/a/151244/7467989
            GC.SuppressFinalize(this);
        }//Finalização dos testes: O método Dispose é a implementação da interface IDisposable e é chamado ao
         //final dos testes para que o banco seja excluído.
         //A linha GC.SuppressFinalize(this) é uma recomendação da Microsoft.
    }
}
