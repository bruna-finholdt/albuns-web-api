using Microsoft.EntityFrameworkCore;
using PrimeiraWebAPI.DAL;
using PrimeiraWebAPI.Domain.DTO;
using PrimeiraWebAPI.Domain.Entity;
using PrimeiraWebAPI.Services;

namespace PrimeiraWebAPI.Tests.Services
{
    public class AvaliacoesServiceTest : IDisposable
    {
        //Crie o campo do service e do dbcontext

        /// <summary>
        /// DbContext é a camada de acesso ao banco
        /// </summary>
        private readonly AppDbContext _dbContext; //_dbContext is an instance of the AppDbContext class, which represents
                                                  //the database context for accessing the database.

        /// <summary>
        /// Service que iremos testar
        /// </summary>
        private readonly AvaliacoesService _service; //_service is an instance of the AlbunsService class, which is the
                                                     //service being tested.

        /// <summary>
        /// Aqui preparamos os testes
        /// </summary>

        //Crie o construtor que instancie o service e o dbcontext
        public AvaliacoesServiceTest()
        {
            // Criando banco em memória
            var options = new DbContextOptionsBuilder<AppDbContext>()
                // Guid.NewGuid().ToString(): Garantindo a criação de um banco novo
                //  a cada execução de teste, evitando a existência de dados não inseridos durante os testes
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            // Criamos as instâncias que vamos usar nos testes
            _dbContext = new AppDbContext(options);
            _service = new AvaliacoesService(_dbContext);
        } //No construtor criamos um banco de dados em memória do EFCore e usamos o campo _dbContext para armazená-lo 
          //Além disso cada vez que um teste é executado um novo nome aleatório é gerado Guid.NewGuid().ToString()
          //Por fim, instanciamos o _service que vamos testar

        //Execução dos testes:
        [Fact]
        public void Quando_PassadaAvaliacaoComNotaInvalida_Deve_RetornarErro() //Caso de criação de avaliação com nota
                                                                               //inválida
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var mockAlbum = MockAlbumStub();
            var mensagemEsperada = "A nota da avaliação deve ser um número entre 1 e 5";

            // Preparando entrada
            var request = new AvaliacaoCreateRequest()
            {
                IdAlbum = mockAlbum.IdAlbum,
                Nota = 10, //nota inválida
                Comentario = "Ótimo"
            };

            // Executando
            var retorno = _service.CadastrarNovo(request);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
        }

        [Fact]
        public void Quando_PassadaAvaliacao_ComIdDeAlbumInexistente_Deve_RetornarErro() //Caso de criação de avaliação
                                                                                        //com id de álbum inexistente
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var mockAlbum = MockAlbumStub();
            var mensagemEsperada = "Album não encontrado";

            // Preparando entrada
            var request = new AvaliacaoCreateRequest()
            {
                IdAlbum = 154786, //id inválido
                Nota = 3,
                Comentario = "Legalzinho",
            };

            // Executando
            var retorno = _service.CadastrarNovo(request);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
        }

        [Fact]
        public void Quando_PassadaAvaliacaoValida_DeveCadastrar_E_Retornar() //Caso de criação de avaliação com
                                                                             //sucesso
        {
            var mockAlbum = MockAlbumStub();

            // Preparando entrada
            var request = new AvaliacaoCreateRequest()
            {
                IdAlbum = mockAlbum.IdAlbum,
                Nota = 3,
                Comentario = "Legalzinho"
            };

            // Executando
            var retorno = _service.CadastrarNovo(request);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.Comentario, request.Comentario);
            Assert.Equal(retorno.ObjetoRetorno.IdAlbum, request.IdAlbum);
            Assert.Equal(retorno.ObjetoRetorno.Nota, request.Nota);
        }

        /// <summary>
        /// Stub Album
        /// </summary>
        /// <returns>Mock Album para usar em testes</returns>
        private Album MockAlbumStub()
        {
            // Dados para mock
            var mockAlbum = new Album()
            {
                Nome = "Nome do album",
                AnoLancamento = 2000,
                Artista = "Artista do album"
            };

            // Salvamos os dados no banco
            _dbContext.Add(mockAlbum);
            _dbContext.SaveChanges();

            // Retornamos para usar nas validações
            return mockAlbum;
        }

        //Faça a classe implementar a interface IDisposable e implemente o método Dispose
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
