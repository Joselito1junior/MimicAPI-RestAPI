using MimicAPI.Helpers;
using MimicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Repositories.Contracts
{
    public interface IPalavraRepository
    {
        PaginacaoList<Palavras> ObterPalavras(PalavraUrlQuery query);
        Palavras Obter(int id);
        void Cadastrar(Palavras palavra);
        void Atualizar(Palavras palavra);
        void Deletar(int id);

    }
}
