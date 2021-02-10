using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;
using SGE.Model.Comun;
using System;
using System.Data.Objects;

namespace SGE.Repositorio
{
    public class OngRepositorio : BaseRepositorio, IOngRepositorio
    {
        private readonly DataSGE _mdb;

        public OngRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IOng> QOngCompleto()
        {
            var a = (from c in _mdb.T_ONG
                     from res in _mdb.T_ACTORES.Where(res => res.ID_ACTOR == c.ID_RESPONSABLE && res.ID_ACTOR_ROL == (int)Enums.Actor_Rol.RESPONSABLE).DefaultIfEmpty()
                     from pres in _mdb.T_PERSONAS.Where(pres => pres.ID_PERSONA == res.ID_PERSONA).DefaultIfEmpty()
                     from lres in _mdb.T_LOCALIDADES.Where(lres => lres.ID_LOCALIDAD == pres.ID_LOCALIDAD).DefaultIfEmpty()
                     from con in _mdb.T_ACTORES.Where(con => con.ID_ACTOR == c.ID_CONTACTO && con.ID_ACTOR_ROL == (int)Enums.Actor_Rol.RESPONSABLE).DefaultIfEmpty()
                     from pcon in _mdb.T_PERSONAS.Where(pcon => pcon.ID_PERSONA == con.ID_PERSONA).DefaultIfEmpty()
                     from lcon in _mdb.T_LOCALIDADES.Where(lcon => lcon.ID_LOCALIDAD == pcon.ID_LOCALIDAD).DefaultIfEmpty()


                     select
                         new Ong
                         {
                             ID_ONG = c.ID_ONG,
                             N_NOMBRE = c.N_NOMBRE,
                             ID_RESPONSABLE = c.ID_RESPONSABLE ?? 0,
                             ID_CONTACTO = c.ID_CONTACTO ?? 0,
                             CUIT = c.CUIT,
                             CELULAR = c.CELULAR,
                             TELEFONO = c.TELEFONO,
                             MAIL_ONG = c.MAIL_ONG,
                             FACTURACION = c.FACTURACION,
                             ID_USR_SIST = c.ID_USR_SIST ?? 0,
                             FEC_SIST = c.FEC_SIST,
                             ID_USR_MODIF = c.ID_USR_MODIF ?? 0,
                             FEC_MODIF = c.FEC_MODIF,

                             RES_APELLIDO = pres.APELLIDO,
                             RES_NOMBRE = pres.NOMBRE,
                             RES_DNI = pres.DNI,
                             RES_CUIL = pres.CUIL,
                             RES_CELULAR = pres.CELULAR,
                             RES_TELEFONO = pres.TELEFONO,
                             RES_MAIL = pres.MAIL,
                             RES_ID_LOCALIDAD = pres.ID_LOCALIDAD ?? 0,
                             RES_N_LOCALIDAD = lres.N_LOCALIDAD,

                             CON_APELLIDO = pcon.APELLIDO,
                             CON_NOMBRE = pcon.NOMBRE,
                             CON_DNI = pcon.DNI,
                             CON_CUIL = pcon.CUIL,
                             CON_CELULAR = pcon.CELULAR,
                             CON_TELEFONO = pcon.TELEFONO,
                             CON_MAIL = pcon.MAIL,
                             CON_ID_LOCALIDAD = pcon.ID_LOCALIDAD ?? 0,
                             CON_N_LOCALIDAD = lcon.N_LOCALIDAD,
                             

                         });
            var objectQuery = a as ObjectQuery;
            string consultaSql = objectQuery.ToTraceString();

            return a;
        }

        public List<IOng> GetOngs(string cuit)
        {
            return QOngCompleto().Where(c => c.CUIT.ToUpper().Contains(cuit.ToUpper()) || string.IsNullOrEmpty(cuit)).OrderBy(c => c.N_NOMBRE).ToList();
        
        }


        public IOng GetOng(int idOng)
        {
            return QOngCompleto().Where(c => c.ID_ONG == idOng).SingleOrDefault();

        }

        public List<IOng> GetOngsNom(string nombre)
        {
            return QOngCompleto().Where(c => c.N_NOMBRE.ToUpper().Contains(nombre.ToUpper()) || string.IsNullOrEmpty(nombre)).OrderBy(c => c.N_NOMBRE).ToList();

        }


        public List<IOng> GetOngsId(int id)
        {
            return QOngCompleto().Where(c => c.ID_ONG == id).ToList();

        }

        public int AddOng(IOng ong)
        {
            var a = (from c in _mdb.T_ONG
                     select c.ID_ONG);
            //AgregarDatos(persona);
            var model = new T_ONG
            {
                ID_ONG = ((int?)a.DefaultIfEmpty().Max() ?? 0) + 1,
                N_NOMBRE = (ong.N_NOMBRE ?? "").ToUpper().Trim(),
                CUIT = ong.CUIT,
                CELULAR = ong.CELULAR,
                TELEFONO = ong.TELEFONO,
                FACTURACION = ong.FACTURACION ?? "N",
                MAIL_ONG = ong.MAIL_ONG,
                ID_CONTACTO = ong.ID_CONTACTO == 0 ? null : ong.ID_CONTACTO,
                ID_RESPONSABLE = ong.ID_RESPONSABLE == 0 ? null : ong.ID_RESPONSABLE,

                ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                FEC_SIST = DateTime.Now,
                //ID_USR_MODIF = persona.id_usr_modif == 0 ? null : persona.id_usr_modif,
                //FEC_MODIF = persona.fec_modif
            };

            _mdb.T_ONG.AddObject(model);
            _mdb.SaveChanges();
            return model.ID_ONG;
        }

        public void UpdateOng(IOng ong)
        {
            if (ong.ID_ONG != 0)
            {
                var row = _mdb.T_ONG.SingleOrDefault(c => c.ID_ONG == ong.ID_ONG);

                if (row != null)
                {
                row.ID_ONG = ong.ID_ONG;
                row.N_NOMBRE = ong.N_NOMBRE;
                row.CUIT = ong.CUIT;
                row.CELULAR = ong.CELULAR;
                row.TELEFONO = ong.TELEFONO;
                row.FACTURACION = ong.FACTURACION ?? "N";
                row.MAIL_ONG = ong.MAIL_ONG;
                row.ID_CONTACTO = ong.ID_CONTACTO == 0 ? null : ong.ID_CONTACTO;
                row.ID_RESPONSABLE = ong.ID_RESPONSABLE == 0 ? null : ong.ID_RESPONSABLE;
                row.ID_USR_MODIF = GetUsuarioLoguer().Id_Usuario;
                row.FEC_MODIF = DateTime.Now;

                    _mdb.SaveChanges();
                }
            }
        }



    }
}
