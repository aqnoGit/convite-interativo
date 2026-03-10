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
using Microsoft.EntityFrameworkCore;

namespace GerenciadorPresenca.Controllers
{
    [ApiController]
    [Route("gerenciadorPresenca")]
    public class OrquestradorController : Controller
    {
        private readonly IGerenciadorService _service;
        private readonly AppDbContext _context;

        public OrquestradorController(IGerenciadorService service, AppDbContext context)
        {           
            _service = service;
            _context = context;
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

        [HttpGet("test-connection")]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                var connString = _context.Database.GetConnectionString();
                
                if (string.IsNullOrEmpty(connString))
                {
                    return BadRequest(new { error = "Connection string está vazia!" });
                }
                
                // Testar se consegue conectar no banco
                var canConnect = await _context.Database.CanConnectAsync();
                
                return Ok(new { 
                    message = "Teste completo!",
                    connectionStringLength = connString.Length,
                    canConnect = canConnect,
                    preview = connString.Substring(0, Math.Min(50, connString.Length)) + "..."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { 
                    error = ex.Message,
                    type = ex.GetType().Name,
                    stackTrace = ex.StackTrace
                });
            }
        } 
    }
}