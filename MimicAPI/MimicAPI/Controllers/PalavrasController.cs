using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Helpers;
using MimicAPI.Models;
using MimicAPI.Models.DTO;
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
        private readonly IMapper _mapper;

        public PalavrasController(IPalavraRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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


        [HttpGet("{id}", Name = "Obter")]
        public ActionResult Obter(int id)
        {
            Palavras obj = _repository.Obter(id);

            if (obj == null)
                return StatusCode(404);

            PalavraDTO palavraDTO = _mapper.Map<Palavras, PalavraDTO>(obj);
            palavraDTO.Links = new List<LinkDTO>();

            palavraDTO.Links.Add(
                new LinkDTO("self", Url.Link("Obter", new { id = palavraDTO.Id }), "GET")
            );

            palavraDTO.Links.Add(
                new LinkDTO("Update", Url.Link("Update", new { id = palavraDTO.Id }), "PUT")
            );

            palavraDTO.Links.Add(
                new LinkDTO("Excluir", Url.Link("Excluir", new { id = palavraDTO.Id }), "DELETE")
            );

            return Ok(palavraDTO);
        }

        [Route("")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody] Palavras palavra)
        {
            _repository.Cadastrar(palavra);
            return Created($"/ApiBehaviorOptions/Palavras/{palavra.Id}", palavra);
        }


        [HttpPut("{id}", Name = "Update")]
        public ActionResult Atualizar(int id, [FromBody] Palavras palavra)
        {
            var obj = _repository.Obter(id);

            if (obj == null)
                return StatusCode(404);

            palavra.Id = id;
            _repository.Atualizar(palavra);

            return Ok();
        }

        [HttpDelete("{id}", Name = "Excluir")]
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
