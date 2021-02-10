using System.Collections.Generic;
using System.Data;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class FormularioAccionRepositorio : IFormularioAccionRepositorio
    {
        private readonly DataSGE _mdb;

        public FormularioAccionRepositorio()
        {
            _mdb = new DataSGE();
        }

        public IList<IFormularioAccion> GetAccionesDelFormulario(int idFormulario)
        {
            var formulario = _mdb.T_FORMULARIOS.FirstOrDefault(c => c.ID_FORMULARIO == idFormulario);

            return formulario.T_ACCIONES.Select(item => new FormularioAccion
                                                            {
                                                                Id_Accion = item.ID_ACCION, 
                                                                Id_Formulario = idFormulario,
                                                                Nombre_Accion = item.N_ACCION
                                                            }).Cast<IFormularioAccion>().ToList();
        }

        public void AddFormularioAccion(IFormularioAccion formularioAccion)
        {
            var formulario = _mdb.T_FORMULARIOS.FirstOrDefault(c => c.ID_FORMULARIO == formularioAccion.Id_Formulario);

            var accion = new T_ACCIONES
                             {
                                 ID_ACCION = (short)formularioAccion.Id_Accion,
                                 N_ACCION = formularioAccion.Nombre_Accion
                             };

            formulario.T_ACCIONES.Add(accion);

            _mdb.ObjectStateManager.ChangeObjectState(accion, EntityState.Unchanged);

            _mdb.SaveChanges();
        }

        public void DeleteFormularioAccion(IFormularioAccion formularioAccion)
        {
            if (formularioAccion.Id_Formulario != 0)
            {
                var formulario = _mdb.T_FORMULARIOS.FirstOrDefault(c => c.ID_FORMULARIO == formularioAccion.Id_Formulario);

                var accion = _mdb.T_ACCIONES.FirstOrDefault(c => c.ID_ACCION == formularioAccion.Id_Accion);

                formulario.T_ACCIONES.Remove(accion);

                _mdb.ObjectStateManager.ChangeObjectState(accion, EntityState.Unchanged);

                _mdb.SaveChanges();
            }
        }
    }
}