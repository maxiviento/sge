using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Repositorio.Modelo;
using SGE.Model.Repositorio;

namespace SGE.Repositorio
{
    public class ProyectoRepositorio : BaseRepositorio, IProyectoRepositorio
    {
        private readonly DataSGE _mdb;

        public ProyectoRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IProyecto> QProyecto()
        {
            var a = (from i in _mdb.T_PROYECTOS
                     select
                         new Proyecto()
                         {
                             IdProyecto = i.ID_PROYECTO,
                             NombreProyecto = i.N_PROYECTO.ToUpper().Trim(),
                             CantidadAcapacitar = i.CANT_CAPACITAR,
                             FechaInicioProyecto = i.FEC_INICIO,
                             FechaFinProyecto = i.FEC_FIN,
                             MesesDuracionProyecto = i.MESES_DURAC,
                             IdUsuarioSistema = i.ID_USR_SIST,
                             UsuarioSistema = i.T_USUARIOS.LOGIN,
                             FechaSistema = i.FEC_SIST
                         });
            return a;
        }

        public IProyecto GetProyecto(int id)
        {
            return QProyecto().Where(c => c.IdProyecto == id).SingleOrDefault();
        }

        public int AddProyecto(IProyecto proyecto)
        {
            AgregarDatos(proyecto);

            var proyModel = new T_PROYECTOS()
            {
                ID_PROYECTO = SecuenciaRepositorio.GetId(),
                N_PROYECTO = (proyecto.NombreProyecto?? "").ToUpper().Trim(),
                CANT_CAPACITAR = proyecto.CantidadAcapacitar,
                FEC_INICIO = proyecto.FechaInicioProyecto,
                FEC_FIN = proyecto.FechaFinProyecto,
                MESES_DURAC = proyecto.MesesDuracionProyecto,
                ID_USR_SIST = proyecto.IdUsuarioSistema,
                FEC_SIST = proyecto.FechaSistema
            };

            _mdb.T_PROYECTOS.AddObject(proyModel);
            _mdb.SaveChanges();

            return proyModel.ID_PROYECTO;
        }

        public void UpdateProyecto(IProyecto proyecto)
        {
            AgregarDatos(proyecto);

            var obj = _mdb.T_PROYECTOS.SingleOrDefault(c => c.ID_PROYECTO == proyecto.IdProyecto);

            if (obj == null) return;

            obj.N_PROYECTO = proyecto.NombreProyecto;
            obj.CANT_CAPACITAR = proyecto.CantidadAcapacitar;
            obj.FEC_INICIO = proyecto.FechaInicioProyecto;
            obj.FEC_FIN = proyecto.FechaFinProyecto;
            obj.MESES_DURAC = proyecto.MesesDuracionProyecto;
            obj.ID_USR_SIST = proyecto.IdUsuarioSistema;

            _mdb.SaveChanges();
        }

        public void DeleteProyecto(IProyecto proyecto)
        {
            AgregarDatos(proyecto);
            
            var obj = _mdb.T_PROYECTOS.SingleOrDefault(c => c.ID_PROYECTO == proyecto.IdProyecto);

            if (obj == null) return;

            obj.ID_USR_SIST = proyecto.IdUsuarioSistema;

            _mdb.SaveChanges();

            _mdb.DeleteObject(obj);

            _mdb.SaveChanges();
        }


        public IList<IProyecto> GetProyectos(string NombreProyecto)
        {
            return QProyecto().Where(c => ((c.NombreProyecto.ToLower().Contains(NombreProyecto.ToLower())) || String.IsNullOrEmpty(NombreProyecto))).OrderBy(c => c.NombreProyecto).ToList();
        }
    }
}
