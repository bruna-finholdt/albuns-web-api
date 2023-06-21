using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeiraWebAPI.Services.Base
{
    public class ServiceResponse<T> //< T> é uma forma de indicar que a classe deve receber um tipo como parâmetro.
    {
        //contrutores da classe ServiceResponse
        public ServiceResponse(T objeto)
        {
            Sucesso = true;
            Mensagem = string.Empty;
            ObjetoRetorno = objeto;
        }

        public ServiceResponse(string mensagemDeErro)
        {
            Sucesso = false;
            mensagemDeErro = mensagemDeErro;
            ObjetoRetorno = default;
        }

        //atributos da classe ServiceResponse
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public T ObjetoRetorno { get; set; } //irá assumir o tipo indicado por T.


    }
}
