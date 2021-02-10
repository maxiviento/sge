using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class RelFamiliarRepositorio : BaseRepositorio, IRelFamiliarRepositorio
    {

        private readonly DataSGE _mdb;

        public RelFamiliarRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IRelacionFam> QrelFamiliar(int idficha)
        {
            var familiares = from f in _mdb.T_RELACIONES_FAM
                             from v in _mdb.T_VINCULOS.Where(v => v.ID_VINCULO == f.ID_VINCULO).DefaultIfEmpty()
                             from p in _mdb.T_PERSONAS.Where(p => p.ID_PERSONA == f.ID_PERSONA).DefaultIfEmpty()
                             where f.ID_FICHA == idficha
                             select new RelacionFam
                             {
                                 ID_FICHA = idficha,
                                 ID_VINCULO = v.ID_VINCULO,
                                 N_VINCULO = v.N_VINCULO,
                                 ID_PERSONA = f.ID_PERSONA,
                                 APELLIDO = p.APELLIDO,
                                 NOMBRE = p.NOMBRE,
                                 DNI = p.DNI

                             };

            return familiares;
        }

        public IList<IRelacionFam> GetFamiliares(int idficha)
        {

            return QrelFamiliar(idficha).ToList();
        }

        private IQueryable<IVinculo> QViculos()
        {
            var vinculos =   from v in _mdb.T_VINCULOS
                            
                             select new Vinculo
                             {

                                 ID_VINCULO = v.ID_VINCULO,
                                 N_VINCULO = v.N_VINCULO,


                             };

            return vinculos;
        }

        public IList<IVinculo> getVinculos()
        {
            return QViculos().ToList();
        }

        public int AddFamiliar(IRelacionFam familiar)
        {
           

            var familiarModel = new T_RELACIONES_FAM
            {
                ID_FICHA = (int)familiar.ID_FICHA,
                ID_PERSONA = (int)familiar.ID_PERSONA,
                ID_VINCULO = familiar.ID_VINCULO,
                ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                FEC_SIST = DateTime.Now,
            };

            _mdb.T_RELACIONES_FAM.AddObject(familiarModel);
            _mdb.SaveChanges();



            return 1;
        }

        public void DelFamiliar(int idficha, int idpersona)
        {
            //AgregarDatos(proyecto);

            var obj = _mdb.T_RELACIONES_FAM.SingleOrDefault(c => c.ID_FICHA == idficha && c.ID_PERSONA == idpersona);

            if (obj == null) return;

            //obj.ID_USR_SIST = proyecto.IdUsuarioSistema;

            //_mdb.SaveChanges();

            _mdb.DeleteObject(obj);

            _mdb.SaveChanges();
        }
    }
}
