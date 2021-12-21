using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Helpers;
using MimicAPI.Models;
using MimicAPI.Repositories.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Controllers
{
    [Route("api/palavras")]
    public class PalavrasController : ControllerBase
    {
        private readonly IPalavraRepository _repository;

        public PalavrasController(IPalavraRepository repository)
        {
            _repository = repository;
        }

        [Route("")]
        [HttpGet]
        public ActionResult ObterTodas([FromQuery] PalavraUrlQuery query)
        {
            var item = _repository.ObterPalavras(query);

            if (query.NumPagina > item.Paginacao.TotalPaginas)
                return NotFound();

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(item.Paginacao));

            return Ok(item.ToList());
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult Obter(int id)
        {
            Palavras obj = _repository.Obter(id);

            if (obj == null)
                return StatusCode(404);

            return Ok(obj);
        }

        [Route("")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody] Palavras palavra)
        {
            _repository.Cadastrar(palavra);
            return Created($"/ApiBehaviorOptions/Palavras/{palavra.Id}", palavra);
        }

        [Route("{id}")]
        [HttpPut]
        public ActionResult Atualizar(int id, [FromBody] Palavras palavra)
        {
            var obj = _repository.Obter(id);

            if (obj == null)
                return StatusCode(404);

            palavra.Id = id;
            _repository.Atualizar(palavra);

            return Ok();
        }

        [Route("{id}")]
        [HttpDelete]
        public ActionResult Deletar(int id)
        {
            Palavras palavra = _repository.Obter(id);

            if (palavra == null)
                return StatusCode(404);

            _repository.Deletar(id);

            return StatusCode(204);
        }
    }
}
