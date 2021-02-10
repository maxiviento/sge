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
    public class EscuelaRepositorio : BaseRepositorio, IEscuelaRepositorio
    {
        private readonly DataSGE _mdb;

        public EscuelaRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IEscuela> QEscuela()
        {
            var a = (from e in _mdb.T_ESCUELAS
                     select
                         new Escuela
                             {
                                 Id_Escuela = e.ID_ESCUELA,
                                 Id_Localidad = e.ID_LOCALIDAD,
                                 Nombre_Escuela = e.N_ESCUELA,
                                 Cue = e.CUE,
                                 Anexo = e.ANEXO,
                                 Barrio = e.BARRIO,
                                 Id_Barrio = e.ID_BARRIO,
                                 CALLE = e.CALLE,
                                 NUMERO = e.NUMERO,
                                 ID_COORDINADOR = e.ID_COORDINADOR ?? 0,
                                 ID_DIRECTOR = e.ID_DIRECTOR ?? 0,
                                 ID_ENCARGADO = e.ID_ENCARGADO ?? 0,
                                 CUPO_CONFIAMOS = e.CUPO_CONFIAMOS ?? 0,
                                 TELEFONO = e.TELEFONO,
                                 regise = e.REGISE,
                             });

            return a;
        }
        
        public IList<IEscuela> GetEscuelas()
        {
            return QEscuela().OrderBy(o => o.Nombre_Escuela).ToList();
        }

        public IList<IEscuela> GetEscuelasByLocalidad(int? idLocalidad)
        {
            return QEscuela().OrderBy(o => o.Nombre_Escuela).Where(c => c.Id_Localidad == idLocalidad).ToList();
        }

        private IQueryable<IEscuela> QEscuelaCompleto()
        {
            var a = (from e in _mdb.T_ESCUELAS
                     from coo in _mdb.T_ACTORES.Where(coo=>coo.ID_ACTOR == e.ID_COORDINADOR && coo.ID_ACTOR_ROL == (int)Enums.Actor_Rol.COORDINADOR).DefaultIfEmpty()
                     from pcoo in _mdb.T_PERSONAS.Where(pcoo=>pcoo.ID_PERSONA == coo.ID_PERSONA).DefaultIfEmpty()
                     from lcoo in _mdb.T_LOCALIDADES.Where(lcoo=>lcoo.ID_LOCALIDAD == pcoo.ID_LOCALIDAD).DefaultIfEmpty()
                     from dir in _mdb.T_ACTORES.Where(dir => dir.ID_ACTOR == e.ID_DIRECTOR && dir.ID_ACTOR_ROL == (int)Enums.Actor_Rol.DIRECTOR).DefaultIfEmpty()
                     from pdir in _mdb.T_PERSONAS.Where(pdir => pdir.ID_PERSONA == dir.ID_PERSONA).DefaultIfEmpty()
                     from ldir in _mdb.T_LOCALIDADES.Where(ldir=>ldir.ID_LOCALIDAD == pdir.ID_LOCALIDAD).DefaultIfEmpty()
                     from enc in _mdb.T_ACTORES.Where(enc => enc.ID_ACTOR == e.ID_ENCARGADO && enc.ID_ACTOR_ROL == (int)Enums.Actor_Rol.ENCARGADO_ESCUELA).DefaultIfEmpty()
                     from penc in _mdb.T_PERSONAS.Where(penc => penc.ID_PERSONA == enc.ID_PERSONA).DefaultIfEmpty()
                     from lenc in _mdb.T_LOCALIDADES.Where(lenc => lenc.ID_LOCALIDAD == penc.ID_LOCALIDAD).DefaultIfEmpty()

                     select
                         new Escuela
                         {
                             Id_Escuela = e.ID_ESCUELA,
                             Id_Localidad = e.ID_LOCALIDAD,
                             Nombre_Escuela = e.N_ESCUELA,
                             Cue = e.CUE,
                             Anexo = e.ANEXO,
                             Barrio = e.BARRIO,
                             Id_Barrio = e.ID_BARRIO,
                             CALLE= e.CALLE,
                             NUMERO= e.NUMERO,
                             ID_COORDINADOR = e.ID_COORDINADOR ?? 0,
                             ID_DIRECTOR =e.ID_DIRECTOR ?? 0,
                             ID_ENCARGADO=e.ID_ENCARGADO  ?? 0,
                             CUPO_CONFIAMOS=e.CUPO_CONFIAMOS  ?? 0,
                             TELEFONO = e.TELEFONO,
                             COOR_APELLIDO=pcoo.APELLIDO,
                             COOR_NOMBRE=pcoo.NOMBRE,
                             COOR_DNI=pcoo.DNI,
                             COOR_CUIL=pcoo.CUIL,
                             COOR_CELULAR=pcoo.CELULAR,
                             COOR_TELEFONO=pcoo.TELEFONO,
                             COOR_MAIL=pcoo.MAIL,
                             COOR_ID_LOCALIDAD=pcoo.ID_LOCALIDAD ?? 0,
                             COOR_N_LOCALIDAD=lcoo.N_LOCALIDAD,
                             DIR_APELLIDO=pdir.APELLIDO,
                             DIR_NOMBRE = pdir.NOMBRE,
                             DIR_DNI = pdir.DNI,
                             DIR_CUIL = pdir.CUIL,
                             DIR_CELULAR = pdir.CELULAR,
                             DIR_TELEFONO = pdir.TELEFONO,
                             DIR_MAIL = pdir.MAIL,
                             DIR_ID_LOCALIDAD = pdir.ID_LOCALIDAD ?? 0,
                             DIR_N_LOCALIDAD = ldir.N_LOCALIDAD,
                             ENC_APELLIDO = penc.APELLIDO,
                             ENC_NOMBRE = penc.NOMBRE,
                             ENC_DNI = penc.DNI,
                             ENC_CUIL = penc.CUIL,
                             ENC_CELULAR = penc.CELULAR,
                             ENC_TELEFONO = penc.TELEFONO,
                             ENC_MAIL = penc.MAIL,
                             ENC_ID_LOCALIDAD = penc.ID_LOCALIDAD ?? 0,
                             ENC_N_LOCALIDAD = lenc.N_LOCALIDAD,
                             regise = e.REGISE,

                         });

            return a;
        }

        public IList<IEscuela> GetEscuelasCompleto()
        {
            return QEscuelaCompleto().OrderBy(o => o.Nombre_Escuela).ToList();
        }

        public IEscuela GetEscuelaCompleto(int idEscuela)
        {
            return QEscuelaCompleto().Where(x=>x.Id_Escuela == idEscuela).SingleOrDefault();
        }

        public IList<IEscuela> GetEscuelas(string nombre)
        {
            return QEscuelaSimple().Where(x => x.Nombre_Escuela.ToUpper().Contains(nombre.ToUpper()) || String.IsNullOrEmpty(nombre)).OrderBy(o => o.Nombre_Escuela).ToList();
        }

        private IQueryable<IEscuela> QEscuelaSimple()
        {
            var a = (from e in _mdb.T_ESCUELAS
                     from l in _mdb.T_LOCALIDADES.Where(l => l.ID_LOCALIDAD == e.ID_LOCALIDAD).DefaultIfEmpty()
                     from b in _mdb.T_BARRIOS.Where(b=>b.ID_BARRIO == e.ID_BARRIO).DefaultIfEmpty()
                     select
                         new Escuela
                         {
                             Id_Escuela = e.ID_ESCUELA,
                             Id_Localidad = e.ID_LOCALIDAD,
                             Nombre_Escuela = e.N_ESCUELA,
                             Cue = e.CUE,
                             Anexo = e.ANEXO,
                             Barrio = e.BARRIO,
                             CALLE = e.CALLE,
                             NUMERO = e.NUMERO,
                             ID_COORDINADOR = e.ID_COORDINADOR ?? 0,
                             ID_DIRECTOR = e.ID_DIRECTOR ?? 0,
                             ID_ENCARGADO = e.ID_ENCARGADO ?? 0,
                             CUPO_CONFIAMOS = e.CUPO_CONFIAMOS ?? 0,
                             TELEFONO = e.TELEFONO,
                             regise = e.REGISE,
                             LocalidadEscuela = 
                                                new Localidad 
                                                { 
                                                    IdLocalidad = e.ID_LOCALIDAD ?? 0,
                                                    NombreLocalidad = l.N_LOCALIDAD
                                                },
                            barrio = new Barrio
                                        {
                                            ID_BARRIO = e.ID_BARRIO ?? 0,
                                            N_BARRIO = b.N_BARRIO
                                        }

                         });

            var objectQuery = a as ObjectQuery;
            string consultaSql = objectQuery.ToTraceString();
            return a;
        }

        public int AddEscuela(IEscuela escuela)
        {
            var a = (from c in _mdb.T_ESCUELAS
                     select c.ID_ESCUELA);
            //AgregarDatos(persona);
            var model = new T_ESCUELAS
            {
                ID_ESCUELA = (int)a.Max() + 1,
                N_ESCUELA = escuela.Nombre_Escuela,
                CUE = escuela.Cue,
                ANEXO = escuela.Anexo == null ? 0 : (int)escuela.Anexo,
                BARRIO = escuela.Barrio,
                ID_LOCALIDAD = escuela.Id_Localidad == 0 ? null : escuela.Id_Localidad,
                CALLE = escuela.CALLE,
                NUMERO = escuela.NUMERO,
                ID_BARRIO = escuela.Id_Barrio == 0 ? null : escuela.Id_Barrio,
                ID_DIRECTOR = escuela.ID_DIRECTOR == 0 ? null : escuela.ID_DIRECTOR,
                ID_COORDINADOR = escuela.ID_COORDINADOR == 0 ? null : escuela.ID_COORDINADOR,
                ID_ENCARGADO = escuela.ID_ENCARGADO == 0 ? null : escuela.ID_ENCARGADO,
                TELEFONO = escuela.TELEFONO,
                CUPO_CONFIAMOS = escuela.CUPO_CONFIAMOS,
                REGISE = escuela.regise,
                //ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                //FEC_SIST = DateTime.Now,
                //ID_USR_MODIF = persona.id_usr_modif == 0 ? null : persona.id_usr_modif,
                //FEC_MODIF = persona.fec_modif
            };

            _mdb.T_ESCUELAS.AddObject(model);
            _mdb.SaveChanges();
            return model.ID_ESCUELA;
        }

        public void UpdateEscuela(IEscuela escuela)
        {
            if (escuela.Id_Escuela != 0)
            {
                var row = _mdb.T_ESCUELAS.SingleOrDefault(c => c.ID_ESCUELA == escuela.Id_Escuela);

                if (row != null)
                {
                row.N_ESCUELA = escuela.Nombre_Escuela;
                row.CUE = escuela.Cue;
                row.ANEXO = escuela.Anexo == null ? 0 : (int)escuela.Anexo;
                row.BARRIO = escuela.Barrio;
                row.ID_LOCALIDAD = escuela.Id_Localidad == 0 ? null : escuela.Id_Localidad;
                row.CALLE = escuela.CALLE;
                row.NUMERO = escuela.NUMERO;
                row.ID_BARRIO = escuela.Id_Barrio == 0 ? null : escuela.Id_Barrio;
                row.ID_DIRECTOR = escuela.ID_DIRECTOR == 0 ? null : escuela.ID_DIRECTOR;
                row.ID_COORDINADOR = escuela.ID_COORDINADOR == 0 ? null : escuela.ID_COORDINADOR;
                row.ID_ENCARGADO = escuela.ID_ENCARGADO == 0 ? null : escuela.ID_ENCARGADO;
                row.TELEFONO = escuela.TELEFONO;
                row.CUPO_CONFIAMOS = escuela.CUPO_CONFIAMOS;
                row.REGISE = escuela.regise;
                    //row.ID_USR_MODIF = GetUsuarioLoguer().Id_Usuario;
                    //row.FEC_MODIF = DateTime.Now;

                    _mdb.SaveChanges();
                }
            }
        }
    }
}
