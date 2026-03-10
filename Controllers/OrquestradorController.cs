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

        [HttpGet("test-db-details")]
        public async Task<IActionResult> TestDatabaseDetails()
        {
            try
            {
                var connString = _context.Database.GetConnectionString();
                
                // Tentar conectar e pegar mais detalhes
                var canConnect = false;
                string errorDetail = "";
                
                try
                {
                    canConnect = await _context.Database.CanConnectAsync();
                }
                catch (Exception dbEx)
                {
                    errorDetail = dbEx.Message + " | InnerException: " + dbEx.InnerException?.Message;
                }
                
                return Ok(new { 
                    connectionString = connString?.Replace(":bp9PrQi6cNCQNDlnrWe8GABL7r3cd1hs", ":****"),
                    canConnect = canConnect,
                    errorIfAny = errorDetail
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { 
                    error = ex.Message,
                    innerError = ex.InnerException?.Message
                });
            }
        }
    }
}