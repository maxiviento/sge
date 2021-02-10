using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using iTextSharp.text;
using System.Globalization;


namespace SGE.Servicio.Servicio
{
    public class CursoServicio : ICursoServicio
    {
        private readonly ICursoRepositorio _cursoRepositorio;
        private readonly IAutenticacionServicio _autenticacion;
        private readonly IOngRepositorio _ongRepositorio;
        private readonly IEscuelaRepositorio _escuelaRepositorio;
        private readonly IAsistenciasRepositorio _asistenciasRepositorio;

        public CursoServicio()
        {
            _cursoRepositorio = new CursoRepositorio();
            _autenticacion = new AutenticacionServicio();
            _ongRepositorio = new OngRepositorio();
            _escuelaRepositorio = new EscuelaRepositorio();
            _asistenciasRepositorio = new AsistenciasRepositorio();
        }

        public ICursoVista GetCursosEscuela(string NombreCurso)
        {

            ICursoVista cursoVista = new CursoVista();
            IList<ICurso> cursos;
            cursos = _cursoRepositorio.GetCursosEscuela(NombreCurso);
            cursoVista.cursos = cursos;

            return cursoVista;
        }

        public ICursoVista GetCursos(string NombreCurso)
        {

            ICursoVista cursoVista = new CursoVista();
            IList<ICurso> cursos;
            cursos = _cursoRepositorio.GetCursos(NombreCurso);
            cursoVista.cursos = cursos;

            return cursoVista;
        }

        public ICursoVista GetCursos(int NroCurso)
        {

            ICursoVista cursoVista = new CursoVista();
            IList<ICurso> cursos;
            cursos = _cursoRepositorio.GetCursos(NroCurso);
            cursoVista.cursos = cursos;

            return cursoVista;
        }

        public ICursosVista GetCursos(int NroCurso, string NombreCurso)
        {
            int cantreg = 0;
            ICursosVista vista = new CursosVista();
            //IList<ICurso> cursos;

            vista.cursos = _cursoRepositorio.getCursos(NroCurso, NombreCurso);
            

            
            cantreg = vista.cursos.Count();
            var pager = new Pager(cantreg,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexCurso", _autenticacion.GetUrl("IndexPager", "Cursos"));

            vista.Pager = pager;

            vista.cursos = vista.cursos.Skip(vista.Pager.Skip).Take(vista.Pager.PageSize).ToList();

            return vista;
        }

        public ICursosVista GetIndex()
        {
            ICursosVista vista = new CursosVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexCurso", _autenticacion.GetUrl("IndexPager", "Cursos"));

            vista.Pager = pager;


            return vista;
        }

        public ICursosVista GetCursos(IPager pPager, int NroCurso, string NombreCurso)
        {

            ICursosVista vista = new CursosVista();
            //IList<ICurso> cursos;
            int cantreg = 0;
            
            vista.cursos = _cursoRepositorio.getCursos(NroCurso, NombreCurso);
            cantreg = vista.cursos.Count();
            var pager = new Pager(cantreg,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexCurso", _autenticacion.GetUrl("IndexPager", "Cursos"));

            if (pager.TotalCount != pPager.TotalCount)
            {
                pager.PageNumber = 1;
                pager.Skip = 0;
                vista.Pager = pager;
            }
            else
            {
                vista.Pager = pPager;
            }



            vista.cursos = vista.cursos.Skip(vista.Pager.Skip).Take(vista.Pager.PageSize).ToList();
            
            

            return vista;
        }

        public ICursoVista GetCurso(int idCurso, string accion)
        {

            ICursoVista vista = new CursoVista();
            ICurso c;
            vista.Accion = accion;
            



            if (accion != "Agregar")
            {
                c = _cursoRepositorio.getCurso(idCurso);

                                vista.ID_CURSO = c.ID_CURSO;
                                vista.N_CURSO = c.N_CURSO;
                                vista.ID_DOCENTE = c.ID_DOCENTE ?? 0;
                                vista.ID_TUTOR = c.ID_TUTOR ?? 0;
                                vista.ID_ONG = c.ID_ONG ?? 0;
                                vista.ID_ESCUELA = c.ID_ESCUELA ?? 0;
                                vista.F_INICIO = c.F_INICIO;
                                vista.F_FIN = c.F_FIN;
                                vista.DURACION = c.DURACION;
                                vista.EXPEDIENTE_LEG = c.EXPEDIENTE_LEG;
                                vista.ID_USR_SIST = c.ID_USR_SIST;
                                vista.FEC_SIST = c.FEC_SIST;
                                vista.ID_USR_MODIF = c.ID_USR_MODIF;
                                vista.FEC_MODIF = c.FEC_MODIF;
                                vista.DOC_APELLIDO = c.DOC_APELLIDO;
                                vista.DOC_NOMBRE = c.DOC_NOMBRE;
                                vista.DOC_DNI = c.DOC_DNI;
                                vista.DOC_CUIL = c.DOC_CUIL;
                                vista.DOC_CELULAR = c.DOC_CELULAR;
                                vista.DOC_TELEFONO = c.DOC_TELEFONO;
                                vista.DOC_MAIL = c.DOC_MAIL;
                                vista.DOC_ID_LOCALIDAD = c.DOC_ID_LOCALIDAD ?? 0;
                                vista.DOC_N_LOCALIDAD = c.DOC_N_LOCALIDAD;
                                vista.TUT_APELLIDO = c.TUT_APELLIDO;
                                vista.TUT_NOMBRE = c.TUT_NOMBRE;
                                vista.TUT_DNI = c.TUT_DNI;
                                vista.TUT_CUIL = c.TUT_CUIL;
                                vista.TUT_CELULAR = c.TUT_CELULAR;
                                vista.TUT_TELEFONO = c.TUT_TELEFONO;
                                vista.TUT_MAIL = c.TUT_MAIL;
                                vista.TUT_ID_LOCALIDAD = c.TUT_ID_LOCALIDAD ?? 0;
                                vista.TUT_N_LOCALIDAD = c.TUT_N_LOCALIDAD;
                                vista.NRO_COND_CURSO = c.NRO_COND_CURSO;

                                //if (vista.ID_ONG != 0)
                                //{
                                //    IOng ong = new Ong();
                                //    ong = _ongRepositorio.GetOng((int)vista.ID_ONG);

                                //    vista.ONG_N_NOMBRE = ong.N_NOMBRE;
                                //    vista.ONG_CUIT =  ong.CUIT;
                                //    vista.ONG_CELULAR =  ong.CELULAR;
                                //    vista.ONG_TELEFONO =  ong.TELEFONO;
                                //    vista.ONG_MAIL =  ong.MAIL_ONG;
                                //    vista.ONG_ID_RESPONSABLE =  ong.ID_RESPONSABLE;
                                //    vista.ONG_ID_CONTACTO =  ong.ID_CONTACTO;
                                //    vista.ONG_FACTURACION =  ong.FACTURACION;
                                //    vista.RES_APELLIDO =  ong.RES_APELLIDO;
                                //    vista.RES_NOMBRE =  ong.RES_NOMBRE;
                                //    vista.RES_DNI =  ong.RES_DNI;
                                //    vista.RES_CUIL =  ong.RES_CUIL;
                                //    vista.RES_CELULAR =  ong.RES_CELULAR;
                                //    vista.RES_TELEFONO =  ong.RES_TELEFONO;
                                //    vista.RES_MAIL =  ong.RES_MAIL;
                                //    vista.RES_ID_LOCALIDAD =  ong.RES_ID_LOCALIDAD;
                                //    vista.RES_N_LOCALIDAD =  ong.RES_N_LOCALIDAD;
                                //    vista.CON_APELLIDO =  ong.CON_APELLIDO;
                                //    vista.CON_NOMBRE =  ong.CON_NOMBRE;
                                //    vista.CON_DNI =  ong.CON_DNI;
                                //    vista.CON_CUIL =  ong.CON_CUIL;
                                //    vista.CON_CELULAR =  ong.CON_CELULAR;
                                //    vista.CON_TELEFONO =  ong.CON_TELEFONO;
                                //    vista.CON_MAIL =  ong.CON_MAIL;
                                //    vista.CON_ID_LOCALIDAD =  ong.CON_ID_LOCALIDAD;
                                //    vista.CON_N_LOCALIDAD =  ong.CON_N_LOCALIDAD;
                                //}

                                if (vista.ID_ESCUELA != 0)
                                {
                                    IEscuela escuela;
                                    vista.ID_ESCUELA = c.ID_ESCUELA;
                                    escuela = _escuelaRepositorio.GetEscuelaCompleto((int)vista.ID_ESCUELA);//_escuelaRepositorio.GetEscuelasCompleto().Where(x => x.Id_Escuela == (int)vista.ID_ESCUELA).SingleOrDefault();
                                    vista.N_ESCUELA=escuela.Nombre_Escuela;
                                    vista.Cue= escuela.Cue;
                                    vista.Anexo= escuela.Anexo;
                                    vista.Barrio= escuela.Barrio;
                                    vista.CALLE= escuela.CALLE;
                                    vista.NUMERO= escuela.NUMERO;
                                    vista.CUPO_CONFIAMOS= escuela.CUPO_CONFIAMOS;
                                    vista.TELEFONO= escuela.TELEFONO;
                                    vista.COOR_APELLIDO= escuela.COOR_APELLIDO;
                                    vista.COOR_NOMBRE= escuela.COOR_NOMBRE;
                                    vista.COOR_DNI= escuela.COOR_DNI;
                                    vista.COOR_CUIL= escuela.COOR_CUIL;
                                    vista.COOR_CELULAR= escuela.COOR_CELULAR;
                                    vista.COOR_TELEFONO= escuela.COOR_TELEFONO;
                                    vista.COOR_MAIL= escuela.COOR_MAIL;
                                    vista.COOR_ID_LOCALIDAD= escuela.COOR_ID_LOCALIDAD;
                                    vista.COOR_N_LOCALIDAD= escuela.COOR_N_LOCALIDAD;
                                    vista.DIR_APELLIDO= escuela.DIR_APELLIDO;
                                    vista.DIR_NOMBRE= escuela.DIR_NOMBRE;
                                    vista.DIR_DNI= escuela.DIR_DNI;
                                    vista.DIR_CUIL= escuela.DIR_CUIL;
                                    vista.DIR_CELULAR= escuela.DIR_CELULAR;
                                    vista.DIR_TELEFONO= escuela.DIR_TELEFONO;
                                    vista.DIR_MAIL= escuela.DIR_MAIL;
                                    vista.ENC_APELLIDO= escuela.ENC_APELLIDO;
                                    vista.ENC_NOMBRE= escuela.ENC_NOMBRE;
                                    vista.ENC_DNI= escuela.ENC_DNI;
                                    vista.ENC_CUIL= escuela.ENC_CUIL;
                                    vista.ENC_CELULAR= escuela.ENC_CELULAR;
                                    vista.ENC_TELEFONO= escuela.ENC_TELEFONO;
                                    vista.ENC_MAIL= escuela.ENC_MAIL;
                                    vista.ENC_ID_LOCALIDAD = escuela.ENC_ID_LOCALIDAD;
                                    vista.ENC_N_LOCALIDAD=escuela.ENC_N_LOCALIDAD;
                                }

                //if (accion != "ModificarActorRol")
                //{
                // CargarActoresRol(vista, _personaRepositorio.GetActoresRol());
                //}
            }
            else
            {
                c = new Curso();

                vista.N_CURSO = c.DOC_NOMBRE;
                vista.ID_DOCENTE = 0;
                vista.ID_CURSO = 0;
                vista.ID_ONG = 0;
                vista.ID_TUTOR = 0;


            }



            if (accion == "Ver" || accion == "Eliminar")
            {

            }

            return vista;
        }

        public int AddCurso(ICursoVista vista)
        {


            vista.ID_CURSO = _cursoRepositorio.AddCurso(VistaToDatos(vista));

            return 0;
        }

        private static ICurso VistaToDatos(ICursoVista vista)
        {
            ICurso curso = new Curso
            {       
                    ID_CURSO =vista.ID_CURSO,
                    NRO_COND_CURSO = vista.NRO_COND_CURSO,
                    N_CURSO = vista.N_CURSO,
                    ID_DOCENTE = vista.ID_DOCENTE ?? 0,
                    ID_TUTOR = vista.ID_TUTOR ?? 0,
                    ID_ONG = vista.ID_ONG ?? 0,
                    ID_ESCUELA = vista.ID_ESCUELA ?? 0,
                    F_INICIO = vista.F_INICIO,
                    F_FIN = vista.F_FIN,
                    DURACION = vista.DURACION,
                    EXPEDIENTE_LEG = vista.EXPEDIENTE_LEG,
                    MESES = vista.MESES,

            };

            return curso;
        }

        public int UpdateCurso(ICursoVista vista)
        {

            _cursoRepositorio.UpdateCurso(VistaToDatos(vista));

            return 0;

        }

        public ICursoVista GetCursoReload(ICursoVista vista)
        {
            ICurso c;

                c = _cursoRepositorio.getCurso(vista.ID_CURSO);

                vista.ID_CURSO = c.ID_CURSO;
                vista.N_CURSO = c.N_CURSO;
                vista.ID_DOCENTE = c.ID_DOCENTE ?? 0;
                vista.ID_TUTOR = c.ID_TUTOR ?? 0;
                vista.ID_ONG = c.ID_ONG ?? 0;
                vista.F_INICIO = c.F_INICIO;
                vista.F_FIN = c.F_FIN;
                vista.DURACION = c.DURACION;
                vista.EXPEDIENTE_LEG = c.EXPEDIENTE_LEG;
                vista.ID_USR_SIST = c.ID_USR_SIST;
                vista.FEC_SIST = c.FEC_SIST;
                vista.ID_USR_MODIF = c.ID_USR_MODIF;
                vista.FEC_MODIF = c.FEC_MODIF;
                vista.DOC_APELLIDO = c.DOC_APELLIDO;
                vista.DOC_NOMBRE = c.DOC_NOMBRE;
                vista.DOC_DNI = c.DOC_DNI;
                vista.DOC_CUIL = c.DOC_CUIL;
                vista.DOC_CELULAR = c.DOC_CELULAR;
                vista.DOC_TELEFONO = c.DOC_TELEFONO;
                vista.DOC_MAIL = c.DOC_MAIL;
                vista.DOC_ID_LOCALIDAD = c.DOC_ID_LOCALIDAD ?? 0;
                vista.DOC_N_LOCALIDAD = c.DOC_N_LOCALIDAD;
                vista.TUT_APELLIDO = c.TUT_APELLIDO;
                vista.TUT_NOMBRE = c.TUT_NOMBRE;
                vista.TUT_DNI = c.TUT_DNI;
                vista.TUT_CUIL = c.TUT_CUIL;
                vista.TUT_CELULAR = c.TUT_CELULAR;
                vista.TUT_TELEFONO = c.TUT_TELEFONO;
                vista.TUT_MAIL = c.TUT_MAIL;
                vista.TUT_ID_LOCALIDAD = c.TUT_ID_LOCALIDAD ?? 0;
                vista.TUT_N_LOCALIDAD = c.TUT_N_LOCALIDAD;
                vista.NRO_COND_CURSO = c.NRO_COND_CURSO;



                //if (vista.ID_ONG != 0)
                //{
                //    IOng ong = new Ong();
                //    ong = _ongRepositorio.GetOng((int)vista.ID_ONG);

                //    vista.ONG_N_NOMBRE = ong.N_NOMBRE;
                //    vista.ONG_CUIT = ong.CUIT;
                //    vista.ONG_CELULAR = ong.CELULAR;
                //    vista.ONG_TELEFONO = ong.TELEFONO;
                //    vista.ONG_MAIL = ong.MAIL_ONG;
                //    vista.ONG_ID_RESPONSABLE = ong.ID_RESPONSABLE;
                //    vista.ONG_ID_CONTACTO = ong.ID_CONTACTO;
                //    vista.ONG_FACTURACION = ong.FACTURACION;
                //    vista.RES_APELLIDO = ong.RES_APELLIDO;
                //    vista.RES_NOMBRE = ong.RES_NOMBRE;
                //    vista.RES_DNI = ong.RES_DNI;
                //    vista.RES_CUIL = ong.RES_CUIL;
                //    vista.RES_CELULAR = ong.RES_CELULAR;
                //    vista.RES_TELEFONO = ong.RES_TELEFONO;
                //    vista.RES_MAIL = ong.RES_MAIL;
                //    vista.RES_ID_LOCALIDAD = ong.RES_ID_LOCALIDAD;
                //    vista.RES_N_LOCALIDAD = ong.RES_N_LOCALIDAD;
                //    vista.CON_APELLIDO = ong.CON_APELLIDO;
                //    vista.CON_NOMBRE = ong.CON_NOMBRE;
                //    vista.CON_DNI = ong.CON_DNI;
                //    vista.CON_CUIL = ong.CON_CUIL;
                //    vista.CON_CELULAR = ong.CON_CELULAR;
                //    vista.CON_TELEFONO = ong.CON_TELEFONO;
                //    vista.CON_MAIL = ong.CON_MAIL;
                //    vista.CON_ID_LOCALIDAD = ong.CON_ID_LOCALIDAD;
                //    vista.CON_N_LOCALIDAD = ong.CON_N_LOCALIDAD;
                //}
                if (vista.ID_ESCUELA != 0)
                {
                    IEscuela escuela;
                    vista.ID_ESCUELA = c.ID_ESCUELA;
                    escuela = _escuelaRepositorio.GetEscuelaCompleto((int)vista.ID_ESCUELA);//_escuelaRepositorio.GetEscuelasCompleto().Where(x => x.Id_Escuela == (int)vista.ID_ESCUELA).SingleOrDefault();
                    vista.N_ESCUELA = escuela.Nombre_Escuela;
                    vista.Cue = escuela.Cue;
                    vista.Anexo = escuela.Anexo;
                    vista.Barrio = escuela.Barrio;
                    vista.CALLE = escuela.CALLE;
                    vista.NUMERO = escuela.NUMERO;
                    vista.CUPO_CONFIAMOS = escuela.CUPO_CONFIAMOS;
                    vista.TELEFONO = escuela.TELEFONO;
                    vista.COOR_APELLIDO = escuela.COOR_APELLIDO;
                    vista.COOR_NOMBRE = escuela.COOR_NOMBRE;
                    vista.COOR_DNI = escuela.COOR_DNI;
                    vista.COOR_CUIL = escuela.COOR_CUIL;
                    vista.COOR_CELULAR = escuela.COOR_CELULAR;
                    vista.COOR_TELEFONO = escuela.COOR_TELEFONO;
                    vista.COOR_MAIL = escuela.COOR_MAIL;
                    vista.COOR_ID_LOCALIDAD = escuela.COOR_ID_LOCALIDAD;
                    vista.COOR_N_LOCALIDAD = escuela.COOR_N_LOCALIDAD;
                    vista.DIR_APELLIDO = escuela.DIR_APELLIDO;
                    vista.DIR_NOMBRE = escuela.DIR_NOMBRE;
                    vista.DIR_DNI = escuela.DIR_DNI;
                    vista.DIR_CUIL = escuela.DIR_CUIL;
                    vista.DIR_CELULAR = escuela.DIR_CELULAR;
                    vista.DIR_TELEFONO = escuela.DIR_TELEFONO;
                    vista.DIR_MAIL = escuela.DIR_MAIL;
                    vista.ENC_APELLIDO = escuela.ENC_APELLIDO;
                    vista.ENC_NOMBRE = escuela.ENC_NOMBRE;
                    vista.ENC_DNI = escuela.ENC_DNI;
                    vista.ENC_CUIL = escuela.ENC_CUIL;
                    vista.ENC_CELULAR = escuela.ENC_CELULAR;
                    vista.ENC_TELEFONO = escuela.ENC_TELEFONO;
                    vista.ENC_MAIL = escuela.ENC_MAIL;
                    vista.ENC_ID_LOCALIDAD = escuela.ENC_ID_LOCALIDAD;
                    vista.ENC_N_LOCALIDAD = escuela.ENC_N_LOCALIDAD;
                }



            return vista;
        }
      
        public IList<IEscuela> TraerEscuelas(string nombre)
        {
            try
            {

                return _escuelaRepositorio.GetEscuelas(nombre);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEscuela TraerEscuela(int idEscuela)
        {
            try
            {

                return _escuelaRepositorio.GetEscuelaCompleto(idEscuela);

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //**********************************************
        public ICursosVista GetCursosAdm(int NroCurso, string NombreCurso)
        {
            int cantreg = 0;
            ICursosVista vista = new CursosVista();
            //IList<ICurso> cursos;

            vista.cursos = _cursoRepositorio.getCursos(NroCurso, NombreCurso);



            cantreg = vista.cursos.Count();
            var pager = new Pager(cantreg,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "ListIndexCurso", _autenticacion.GetUrl("IndexPagerAdm", "Cursos"));

            vista.Pager = pager;

            vista.cursos = vista.cursos.Skip(vista.Pager.Skip).Take(vista.Pager.PageSize).ToList();

            return vista;
        }

        public ICursosVista GetCursosAdm()
        {
            ICursosVista vista = new CursosVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "ListIndexCurso", _autenticacion.GetUrl("IndexPagerAdm", "Cursos"));

            vista.Pager = pager;


            return vista;
        }

        public ICursosVista GetCursosAdm(IPager pPager, int NroCurso, string NombreCurso)
        {

            ICursosVista vista = new CursosVista();
            //IList<ICurso> cursos;
            int cantreg = 0;

            vista.cursos = _cursoRepositorio.getCursos(NroCurso, NombreCurso);
            cantreg = vista.cursos.Count();
            var pager = new Pager(cantreg,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "ListIndexCurso", _autenticacion.GetUrl("IndexPagerAdm", "Cursos"));

            if (pager.TotalCount != pPager.TotalCount)
            {
                pager.PageNumber = 1;
                pager.Skip = 0;
                vista.Pager = pager;
            }
            else
            {
                vista.Pager = pPager;
            }



            vista.cursos = vista.cursos.Skip(vista.Pager.Skip).Take(vista.Pager.PageSize).ToList();



            return vista;
        }

        //**********************************************

        public ICursoVista GetCurso(int idCurso)
        {

            ICursoVista vista = new CursoVista();
            ICurso c;

                c = _cursoRepositorio.getCurso(idCurso);

                vista.ID_CURSO = c.ID_CURSO;
                vista.N_CURSO = c.N_CURSO;
                vista.ID_DOCENTE = c.ID_DOCENTE ?? 0;
                vista.ID_TUTOR = c.ID_TUTOR ?? 0;
                vista.ID_ONG = c.ID_ONG ?? 0;
                vista.ID_ESCUELA = c.ID_ESCUELA ?? 0;
                vista.F_INICIO = c.F_INICIO;
                vista.F_FIN = c.F_FIN;
                vista.DURACION = c.DURACION;
                vista.EXPEDIENTE_LEG = c.EXPEDIENTE_LEG;
                vista.ID_USR_SIST = c.ID_USR_SIST;
                vista.FEC_SIST = c.FEC_SIST;
                vista.ID_USR_MODIF = c.ID_USR_MODIF;
                vista.FEC_MODIF = c.FEC_MODIF;
                vista.DOC_APELLIDO = c.DOC_APELLIDO;
                vista.DOC_NOMBRE = c.DOC_NOMBRE;
                vista.DOC_DNI = c.DOC_DNI;
                vista.DOC_CUIL = c.DOC_CUIL;
                vista.DOC_CELULAR = c.DOC_CELULAR;
                vista.DOC_TELEFONO = c.DOC_TELEFONO;
                vista.DOC_MAIL = c.DOC_MAIL;
                vista.DOC_ID_LOCALIDAD = c.DOC_ID_LOCALIDAD ?? 0;
                vista.DOC_N_LOCALIDAD = c.DOC_N_LOCALIDAD;
                vista.TUT_APELLIDO = c.TUT_APELLIDO;
                vista.TUT_NOMBRE = c.TUT_NOMBRE;
                vista.TUT_DNI = c.TUT_DNI;
                vista.TUT_CUIL = c.TUT_CUIL;
                vista.TUT_CELULAR = c.TUT_CELULAR;
                vista.TUT_TELEFONO = c.TUT_TELEFONO;
                vista.TUT_MAIL = c.TUT_MAIL;
                vista.TUT_ID_LOCALIDAD = c.TUT_ID_LOCALIDAD ?? 0;
                vista.TUT_N_LOCALIDAD = c.TUT_N_LOCALIDAD;
                vista.NRO_COND_CURSO = c.NRO_COND_CURSO;

                vista.asistencias = _asistenciasRepositorio.GetAsistencias(idCurso);
                vista.fichas = _cursoRepositorio.GetFichasCurso(idCurso).ToList();
                DateTimeFormatInfo formatoFec = CultureInfo.CurrentCulture.DateTimeFormat;
                List<Periodo> lp = new List<Periodo>();
                lp = (from v in vista.asistencias
                      where (v.MES != null || v.MES != "")
                      group v by new { v.MES } into grouping
                                                 select new
                                                     Periodo
                                                 {
                                                     idperiodo = Convert.ToInt32(grouping.Key.MES == "" ? "0" : grouping.Key.MES),
                                                     N_Periodo = "Período de " + formatoFec.GetMonthName(Convert.ToInt32(grouping.Key.MES == "" ? "0" : grouping.Key.MES))
                                                 }).OrderBy(x => x.idperiodo).ToList();


                vista.periodos = new List<IPeriodo>();
                foreach (var p in lp)
                {
                    vista.periodos.Add(new Periodo {idperiodo=p.idperiodo,N_Periodo=p.N_Periodo });
                
                }

            Periodo pp = new Periodo();
            DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
            vista.cboperiodos.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione el Período" });
            string existe = "N";
            if(lp != null && lp.Count > 0)
            {                 
                    for(int i=1; i<13; i++)
                    {
                        pp.idperiodo = i;
                        pp.N_Periodo = "Período de " + formatoFecha.GetMonthName(i);
                        existe = "N";
                        foreach (var item in lp)
                        { 

                            if(item.idperiodo == pp.idperiodo)
                            {
                                existe="S";
                            }
                        }
                        if (existe == "N")
                        {
                            vista.cboperiodos.Combo.Add(new ComboItem { Id = pp.idperiodo, Description = pp.N_Periodo });
                        }

                    }
            
            }
            else
            {
 
                for(int i=1; i<13; i++)
                {
                    pp.idperiodo = i;
                    pp.N_Periodo = "Período de " + formatoFecha.GetMonthName(i);
                     

                    vista.cboperiodos.Combo.Add( new ComboItem {Id= pp.idperiodo,Description= pp.N_Periodo} );
                }


            }

            vista.totalFichas = vista.fichas.Count();
            return vista;
        }

        public IList<IFichasCurso> GetFichasCurso(int idcurso)
        {
            IList<IFichasCurso> fichas = _cursoRepositorio.GetFichasCurso(idcurso).ToList();
            fichas = fichas.OrderBy(x => x.Apellido).ThenBy(x => x.Nombre).ToList();

            return fichas;
        }

        public int addAsistencias(List<IFichasCurso> fichasCurso)
        {
            List<IAsistencia> asistencias = new List<IAsistencia>();

            foreach(var ficha in fichasCurso)
            {
                IAsistencia a = new Asistencia
                {
                ID_CURSO = ficha.id_curso ?? 0,
                //ID_ESCUELA = ficha.ID_ESCUELA ?? 0, //Esto es para asistencias de escuelas -- Se registraran en otra interface?
                ID_FICHA = ficha.IdFicha,
                MES = ficha.MES,
                //PERIODO = ficha.PERIODO,
                PORC_ASISTENCIA = ficha.PORC_ASISTENCIA,
                
                };
                asistencias.Add(a);
            }

            return _asistenciasRepositorio.AddAsistencias(asistencias);
        
        }

        private static IAsistencia VistaToDatos(Asistencia vista)
        {
            IAsistencia asistencia = new Asistencia
            {
                ID_ASISTENCIA = vista.ID_ASISTENCIA,
                ID_CURSO = vista.ID_CURSO ?? 0,
                ID_ESCUELA = vista.ID_ESCUELA ?? 0,
                ID_FICHA = vista.ID_FICHA ?? 0,
                MES = vista.MES,
                PERIODO = vista.PERIODO,
                PORC_ASISTENCIA = vista.PORC_ASISTENCIA ?? 0,

            };

            return asistencia;
        }

        public  IList<IFichasCurso>  TraerAsistenciasCurso(int idCurso, string mesPeriodo)
        { 
        
            IList<IAsistencia> asistencias;
            IList<IFichasCurso> fichasCurso = new List<IFichasCurso>();
            asistencias = _asistenciasRepositorio.GetAsistenciasPeriodo(idCurso, mesPeriodo);

            asistencias = asistencias.OrderBy(c => c.Apellido.ToLower().Trim()).ThenBy(o => o.Nombre.Trim().ToUpper()).ToList();

            foreach(var item in asistencias)
            {

                fichasCurso.Add(new FichasCurso
                                 {
                                     idAsistencia = item.ID_ASISTENCIA,
                                     IdFicha = item.ID_FICHA ?? 0,
                                     id_curso = item.ID_CURSO,
                                     Apellido = item.Apellido,
                                     Nombre = item.Nombre,
                                     NumeroDocumento = item.NumeroDocumento,
                                     TipoDocumento = item.TipoDocumento,
                                     tipoFicha = item.tipoFicha,
                                     Cuil = item.Cuil,
                                     PORC_ASISTENCIA = item.PORC_ASISTENCIA,
                                     MES = item.MES,
                                     
                                 });
                
            }

            return fichasCurso;
        
        }

        public int UpdateAsistencias(List<IFichasCurso> fichasCurso)
        {
            List<IAsistencia> asistencias = new List<IAsistencia>();

            foreach (var ficha in fichasCurso)
            {
                IAsistencia a = new Asistencia
                {
                    ID_ASISTENCIA = ficha.idAsistencia,
                    ID_CURSO = ficha.id_curso ?? 0,
                    //ID_ESCUELA = ficha.ID_ESCUELA ?? 0, //Esto es para asistencias de escuelas -- Se registraran en otra interface?
                    ID_FICHA = ficha.IdFicha,
                    MES = ficha.MES,
                    //PERIODO = ficha.PERIODO,
                    PORC_ASISTENCIA = ficha.PORC_ASISTENCIA,

                };
                asistencias.Add(a);
            }

            return _asistenciasRepositorio.UpdateAsistencias(asistencias);

        }

        public IList<IFichasCurso> TraerAsistenciasCurso(List<IFichasCurso> model ,int idCurso, string mesPeriodo)
        {


            IList<IFichasCurso> fichasCurso = new List<IFichasCurso>();

            //AGREGAR LAS FICHAS QUE NO ESTAN REGISTRADAS EN ESE PERIODO
            var fc = _cursoRepositorio.GetFichasCurso(idCurso).ToList();
            fc = fc.OrderBy(c => c.Apellido.ToLower().Trim()).ThenBy(o => o.Nombre.Trim().ToUpper()).ToList();
            foreach (var item in fc)
            {
                var f = model.Where(x => x.IdFicha == item.IdFicha).Select(x => x.IdFicha).SingleOrDefault();
                if (f != item.IdFicha)
                {
                    fichasCurso.Add(new FichasCurso
                    {
                        idAsistencia = 0,
                        IdFicha = item.IdFicha,
                        id_curso = item.id_curso,
                        Apellido = item.Apellido,
                        Nombre = item.Nombre,
                        NumeroDocumento = item.NumeroDocumento,
                        TipoDocumento = item.TipoDocumento,
                        tipoFicha = item.tipoFicha,
                        Cuil = item.Cuil,
                        PORC_ASISTENCIA = item.PORC_ASISTENCIA ?? 0,
                        MES = mesPeriodo,

                    });
                }

            }

            return fichasCurso;

        }


        public IList<IFichasCurso> GetFichasSinAsistencia(int idCurso, string mesPeriodo)
        {

            IList<IAsistencia> asistencias;
            IList<IFichasCurso> fichasCurso = new List<IFichasCurso>();
            asistencias = _asistenciasRepositorio.GetAsistenciasPeriodo(idCurso, mesPeriodo);

            asistencias = asistencias.OrderBy(c => c.Apellido.ToLower().Trim()).ThenBy(o => o.Nombre.Trim().ToUpper()).ToList();

            
                //AGREGAR LAS FICHAS QUE NO ESTAN REGISTRADAS EN ESE PERIODO
                var fc = _cursoRepositorio.GetFichasCurso(idCurso).ToList();
                fc = fc.OrderBy(c => c.Apellido.ToLower().Trim()).ThenBy(o => o.Nombre.Trim().ToUpper()).ToList();
                foreach (var item in fc)
                {
                    var f = asistencias.Where(x => x.ID_FICHA == item.IdFicha).Select(x => x.ID_FICHA).SingleOrDefault();
                    if (f != item.IdFicha)
                    {
                        fichasCurso.Add(new FichasCurso
                        {
                            idAsistencia = 0,
                            IdFicha = item.IdFicha,
                            id_curso = item.id_curso,
                            Apellido = item.Apellido,
                            Nombre = item.Nombre,
                            NumeroDocumento = item.NumeroDocumento,
                            TipoDocumento = item.TipoDocumento,
                            tipoFicha = item.tipoFicha,
                            Cuil = item.Cuil,
                            PORC_ASISTENCIA = item.PORC_ASISTENCIA ?? 0,
                            MES = mesPeriodo,

                        });
                    }

                }

            

            return fichasCurso;

        }

    }
}
