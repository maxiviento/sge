using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class FormularioRepositorio : IFormularioRepositorio
    {
         private readonly DataSGE _mdb;

         public FormularioRepositorio()
         {
            _mdb = new DataSGE();
         }

         private IQueryable<IFormulario> QFormulario()
         {
             var a = (from c in _mdb.T_FORMULARIOS
                      select
                          new Formulario
                          {
                              Id_Formulario = c.ID_FORMULARIO,
                              Nombre_Formulario = c.N_FORMULARIO,
                          });
             return a;
         }

         public IList<IFormulario> GetFormularios()
         {
             return QFormulario().OrderBy(c => c.Nombre_Formulario).ToList();
         }
    }
}
