using Microsoft.EntityFrameworkCore;
using MimicAPI.Data;
using MimicAPI.Helpers;
using MimicAPI.Models;
using MimicAPI.Repositories.Contracts;
using System;
using System.Linq;


namespace MimicAPI.Repositories
{
    public class PalavraRepository : IPalavraRepository
    {
        private readonly MimicContext _banco;

        public PalavraRepository(MimicContext banco)
        {
            _banco = banco;
        }


        public PaginacaoList<Palavras> ObterPalavras(PalavraUrlQuery query)
        {
            var lista = new PaginacaoList<Palavras>();

            var item = _banco.Palavras.AsNoTracking().AsQueryable();

            if (query.Data.HasValue)
            {
                item = item.Where(a => a.Criado > query.Data.Value || a.Atualizado > query.Data.Value);
            }

            if (query.NumPagina.HasValue)
            {
                Paginacao paginacao = new Paginacao();
                int qtdTotalRegistros = item.Count();

                item = item.Skip((query.NumPagina.Value - 1) * query.QtdRegistros.Value).Take(query.QtdRegistros.Value);

                paginacao.NumeroPagina = query.NumPagina.Value;
                paginacao.RegistroPagina = query.QtdRegistros.Value;
                paginacao.TotalRegistros = qtdTotalRegistros;
                paginacao.TotalPaginas = (int)Math.Ceiling((double)paginacao.TotalRegistros / paginacao.RegistroPagina);

                lista.Paginacao = paginacao;
            }

            lista.Results.AddRange(item.ToList());

            return lista;
        }


        public Palavras Obter(int id)
        {
           return _banco.Palavras.AsNoTracking().FirstOrDefault(a => a.Id == id);
        }
        public void Cadastrar(Palavras palavra)
        {
            _banco.Palavras.Add(palavra);
            _banco.SaveChanges();
        }
        public void Atualizar(Palavras palavra)
        {
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();
        }
        public void Deletar(int id)
        {
            Palavras palavra = Obter(id);
            palavra.Ativo = false;

            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();

        }

    }
}
