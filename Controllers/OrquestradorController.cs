using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GerenciadorPresenca.Data;
using GerenciadorPresenca.Model;
using GerenciadorPresenca.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;  // ← ADICIONAR

namespace GerenciadorPresenca.Controllers
{
    [ApiController]
    [Route("gerenciadorPresenca")]
    public class OrquestradorController : Controller
    {
        private readonly IGerenciadorService _service;
        private readonly AppDbContext _context;  // ← ADICIONAR

        public OrquestradorController(IGerenciadorService service, AppDbContext context)  // ← ADICIONAR context aqui
        {           
            _service = service;
            _context = context;  // ← ADICIONAR
        }

        [HttpPost("confirmar")]
        public async Task<IActionResult> ConfirmarPresenca(Convidado convidado)
        {
            await _service.Confirmar(convidado);
            return Ok(new{message = "Confrimação feita com sucesso"});
        }

        [HttpGet("consultar")]
        public async Task<IActionResult> ConsultarConvidados()
        {
            return Ok(await _service.ListarConvidados());
        }
        
        [HttpPost("migrate")]
        public async Task<IActionResult> ApplyMigrations()
        {
            try
            {
                await _context.Database.MigrateAsync();
                return Ok(new { message = "Migrations aplicadas com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}