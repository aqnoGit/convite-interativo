using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GerenciadorPresenca.Data;
using GerenciadorPresenca.Model;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorPresenca.Service
{
    public class GerenciadorPresencaService : IGerenciadorService
    {
        private readonly AppDbContext _appDbContext;

        public GerenciadorPresencaService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Convidado> Confirmar(Convidado convidado)
        {
            _appDbContext.Convidados.Add(convidado);
            await _appDbContext.SaveChangesAsync();
            return convidado;
        }

        public async Task<List<Convidado>> ListarConvidados()
        {
            return await _appDbContext.Convidados.ToListAsync();
        }

    }
}