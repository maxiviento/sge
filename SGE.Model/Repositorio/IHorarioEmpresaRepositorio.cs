using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IHorarioEmpresaRepositorio
    {
        IList<IHorarioEmpresa> GetHorariosEmpresa(int? idempresa);
        IList<IHorarioEmpresa> GetHorariosEmpresa(int? idempresa, string iddia);
        bool AddHoarioEmpresa(IHorarioEmpresa horarioempresa);
        //IList<IHorarioEmpresa> GetHorarioEmpresaForFicha(int? idempresa, string iddia);
        IList<IHorarioEmpresa> GetHorarioEmpresaForFicha(int? idempresaHorario);
    }
}
