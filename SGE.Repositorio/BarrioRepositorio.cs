using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;
using SGE.Model.Comun;
using System;

namespace SGE.Repositorio
{
    public class BarrioRepositorio: BaseRepositorio,IBarrioRepositorio
    {
        private readonly DataSGE _mdb;

        public BarrioRepositorio()
        {
            _mdb = new DataSGE();
        }

        public IList<IBarrio> GetBarrios()
        {
            return Qbarrios().ToList();
        }

        public IList<IBarrio> GetBarrios(string nombre)
        {
            return Qbarrios().Where(x=>x.N_BARRIO.ToUpper().Contains(nombre.ToUpper()) || String.IsNullOrEmpty(nombre)).ToList();
        }

        private IQueryable<IBarrio> Qbarrios()
        {
            var a = (from b in _mdb.T_BARRIOS
                        from l in _mdb.T_LOCALIDADES.Where(l=>l.ID_LOCALIDAD == b.ID_LOCALIDAD).DefaultIfEmpty()
                        from s in _mdb.T_SECCIONALES.Where(s => s.ID_SECCIONAL == b.ID_SECCIONAL).DefaultIfEmpty()
                     select new Barrio
                     {
                         ID_BARRIO = b.ID_BARRIO,
                         N_BARRIO = b.N_BARRIO,
                         ID_LOCALIDAD = b.ID_LOCALIDAD,
                         ID_SECCIONAL = b.ID_SECCIONAL,
                         ID_USR_SIST = b.ID_USR_SIST,
                         FEC_SIST = b.FEC_SIST,
                         ID_USR_MODIF= b.ID_USR_MODIF,
                         FEC_MODIF = b.FEC_MODIF,
                         N_LOCALIDAD = l.N_LOCALIDAD,
                         N_SECCIONAL = s.N_SECCIONAL
                     });

            return a;

        }

        public IBarrio GetBarrio(int idBarrio)
        {

            return Qbarrios().Where(C => C.ID_BARRIO == idBarrio).SingleOrDefault();        
        }

        public int AddBarrio(IBarrio barrio)
        {
            var a = (from c in _mdb.T_BARRIOS
                     select c.ID_BARRIO);
            //AgregarDatos(persona);
            var model = new T_BARRIOS
            {
                ID_BARRIO = (int)a.Max() + 1,
                N_BARRIO = barrio.N_BARRIO,
                ID_LOCALIDAD = barrio.ID_LOCALIDAD == 0 ? null : barrio.ID_LOCALIDAD,
                ID_SECCIONAL = barrio.ID_SECCIONAL == 0 ? null : barrio.ID_SECCIONAL,
                ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                FEC_SIST = DateTime.Now,
                //ID_USR_MODIF = persona.id_usr_modif == 0 ? null : persona.id_usr_modif,
                //FEC_MODIF = persona.fec_modif
            };

            _mdb.T_BARRIOS.AddObject(model);
            _mdb.SaveChanges();
            return model.ID_BARRIO;
        }


        public void UpdateBarrio(IBarrio barrio)
        {
            if (barrio.ID_BARRIO != 0)
            {
                var row = _mdb.T_BARRIOS.SingleOrDefault(c => c.ID_BARRIO == barrio.ID_BARRIO);

                if (row != null)
                {
                    row.N_BARRIO = barrio.N_BARRIO;
                    row.ID_LOCALIDAD = barrio.ID_LOCALIDAD == 0 ? null : barrio.ID_LOCALIDAD;
                    row.ID_SECCIONAL = barrio.ID_SECCIONAL == 0 ? null : barrio.ID_SECCIONAL;
                    row.ID_USR_MODIF = GetUsuarioLoguer().Id_Usuario;
                    row.FEC_MODIF = DateTime.Now;

                    _mdb.SaveChanges();
                }
            }
        }


        private IQueryable<ISeccional> QSeccionales()
        {
            var a = (from s in  _mdb.T_SECCIONALES
                     select new Seccional
                     {
                         ID_SECCIONAL = s.ID_SECCIONAL,
                         N_SECCIONAL = s.N_SECCIONAL,
                         ID_USR_SIST = s.ID_USR_SIST,
                         FEC_SIST = s.FEC_SIST,
                         ID_USR_MODIF = s.ID_USR_MODIF,
                         FEC_MODIF = s.FEC_MODIF,
                     });

            return a;

        }

        public IList<ISeccional> GetSeccionales()
        {

            return QSeccionales().OrderBy(x => x.N_SECCIONAL).ToList();
        }


        public IList<IBarrio> GetBarriosLocalidad(int idLocalidad)
        {
            return Qbarrios().Where(x => x.ID_LOCALIDAD == idLocalidad).ToList();
        }
    }
}
