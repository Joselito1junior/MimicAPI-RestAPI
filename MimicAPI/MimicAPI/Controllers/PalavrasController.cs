﻿using Microsoft.AspNetCore.Mvc;
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
        public ActionResult ObterTodas()
        {
            return Ok(_banco.Palavras);
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
            return Ok();
        }

        [Route("{id}")]
        [HttpPut]
        public ActionResult Atualizar(int id, [FromBody] Palavras palavra)
        {
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

            palavra.Ativo = false;
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();
            
            return Ok();
        }
    }
}
