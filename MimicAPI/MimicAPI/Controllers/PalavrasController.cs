using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Data;
using MimicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Controllers
{
    [Route("api/palavras")]
    public class PalavrasController : ControllerBase
    {
        private readonly MimicContext _banco;

        public PalavrasController(MimicContext banco)
        {
            _banco = banco;
        }

        [Route("")]
        [HttpGet]
        public ActionResult ObterTodas(DateTime? data, int? numPagina, int? qtdRegistros)
        {
            var item = _banco.Palavras.AsQueryable();

            if(data.HasValue)
            {
                item = item.Where(a => a.Criado > data.Value || a.Atualizado > data.Value);
            }

            if(numPagina.HasValue)
            {
                item = item.Skip((numPagina.Value - 1) * qtdRegistros.Value).Take(qtdRegistros.Value);
            }

            return Ok(item);
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult Obter(int id)
        {
            Palavras obj = _banco.Palavras.Find(id);

            if (obj == null)
                return StatusCode(404);

            return Ok();
        }

        [Route("")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody] Palavras palavra)
        {
            _banco.Palavras.Add(palavra);
            _banco.SaveChanges();
            return Created($"/ApiBehaviorOptions/Palavras/{palavra.Id}", palavra);
        }

        [Route("{id}")]
        [HttpPut]
        public ActionResult Atualizar(int id, [FromBody] Palavras palavra)
        {
            Palavras obj = _banco.Palavras.AsNoTracking().FirstOrDefault(a => a.Id == id);

            if (obj == null)
                return StatusCode(404);

            palavra.Id = id;
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();
            return Ok();
        }

        [Route("{id}")]
        [HttpDelete]
        public ActionResult Deletar(int id)
        {
            Palavras palavra = _banco.Palavras.Find(id);

            if (palavra == null)
                return StatusCode(404);

            palavra.Ativo = false;
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();
            
            return StatusCode(204);
        }
    }
}
