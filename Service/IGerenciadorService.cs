using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GerenciadorPresenca.Model;

namespace GerenciadorPresenca.Service
{
    public interface IGerenciadorService
    {
        Task<Convidado> Confirmar(Convidado convidado);
        Task<List<Convidado>> ListarConvidados();
    }
}