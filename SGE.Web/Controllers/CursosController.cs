using System;
using System.Web.Mvc;
using System.Web.Routing;
using SGE.Model.Comun;
using SGE.Servicio.Servicio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.VistaInterfaces;
using SGE.Web.Controllers.Shared;
using SGE.Web.Infrastucture;
using SGE.Servicio.Vista.Shared;
using System.Text;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Entidades;
using System.Collections;

namespace SGE.Web.Controllers
{


    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMCursos)]
    public class CursosController : BaseController
    {
        private readonly IPersonaServicio _personaServicio;
        private readonly ILocalidadServicio _localidadservicio;
        private readonly ICursoServicio _CursoServicio;
        private readonly IActorServicio _actorServicio;
        private readonly IOngServicio _ongServicio;

        public CursosController() : base((int)Formularios.Cursos)
        {
            _personaServicio = new PersonaServicio();
            _localidadservicio = new LocalidadServicio();
            _CursoServicio = new CursoServicio();
            _actorServicio = new ActorServicio();
            _ongServicio = new OngServicio();
        }

        public ActionResult Index()
        {
            var vista = _CursoServicio.GetIndex();
            vista.BusquedaDescripcion = "";
            vista.BusquedaNroCurso = 0;
            return View(vista);
        }



        [HttpPost]
        public ActionResult BuscarCurso(CursosVista model)
        {
            int nroCurso = model.BusquedaNroCurso ?? 0;
            return View("Index", _CursoServicio.GetCursos(nroCurso ,model.BusquedaDescripcion));
        }

        public ActionResult IndexPager(Pager pager, CursosVista model)
        {
            ICursosVista vista = _CursoServicio.GetCursos(pager, model.Id, model.BusquedaDescripcion);
            vista.BusquedaDescripcion = model.BusquedaDescripcion;
            vista.BusquedaNroCurso = model.BusquedaNroCurso;
            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", TempData["Model"]);
        }



        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMCursos, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int IdCurso)
        {
            ICursoVista vista = _CursoServicio.GetCurso(IdCurso, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMCursos, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar()
        {
            ICursoVista vista = _CursoServicio.GetCurso(0, "Agregar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMCursos, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int IdCurso)
        {
            ICursoVista vista = _CursoServicio.GetCurso(IdCurso, "Modificar");
            return View("Formulario", vista);
        }


        [HttpPost]
        public ActionResult obtenerActor(string dni, int idRolActor)
        {
            IActorVista actor = _actorServicio.GetActorDNI(dni, idRolActor);

            var tablaretorno = new StringBuilder();
            string rolActor = string.Empty;

            switch (idRolActor)
            {
                case (int)Enums.Actor_Rol.GESTOR:
                    rolActor = "GESTOR";
                    break;

            }


            tablaretorno.Append(
                "<thead><tr><th style='height: 80px; width: 80px;'><div align='left'>DNI</div></th>");
            tablaretorno.Append("<th style='width: 200px;'><div align='left'>Apellido</div></th>");
            tablaretorno.Append("<th style='width: 200px;'><div align='left'>Nombre</div></th>");
            tablaretorno.Append("<th style='width: 150px;'><div align='left'>Rol</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Celular</div></th>");
            tablaretorno.Append("<th style='width: 150px;'><div align='left'>Mail</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var A in actor.Actores)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + A.DNI + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + A.APELLIDO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + A.NOMBRE + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + A.N_ACTOR_ROL + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + A.CELULAR + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + A.MAIL + "</td>");


                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionarActor(" +
                    A.ID_ACTOR + ",$" + A.DNI + "$,$" + A.APELLIDO + "$, $" + A.NOMBRE + "$, $" +
                    A.CELULAR + "$,$" + A.MAIL + "$, $" + A.N_BARRIO + "$, $" + A.N_LOCALIDAD + "$," + A.ID_ACTOR_ROL + ");'  /></td>");
            }

            tablaretorno.Append("</tr></tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        [HttpPost]
        public ActionResult obtenerONG(string cuit)
        {
            IOngsVista ong = new OngsVista();
             ong.ongs =   _ongServicio.getOngs(cuit);

            var tablaretorno = new StringBuilder();
            string rolActor = string.Empty;


            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 100px;'><div align='left'>Cuit</div></th>");
            tablaretorno.Append("<th style='width: 280px;'><div align='left'>Nombre</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Teléfono</div></th>");
            tablaretorno.Append("<th style='width: 150px;'><div align='left'>Mail</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var row in ong.ongs)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + row.CUIT + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.N_NOMBRE + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.TELEFONO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.MAIL_ONG + "</td>");
                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionaONG(" +
                    row.ID_ONG + ",$" + row.CUIT + "$,$" + row.N_NOMBRE + "$, $" + row.CELULAR + "$, $" +
                    row.TELEFONO + "$,$" + row.MAIL_ONG + "$, $" + row.RES_APELLIDO + "$, $" + row.RES_NOMBRE + "$, $"
                    + row.CON_APELLIDO + "$, $" + row.CON_NOMBRE + "$, $" +
                    row.CON_CELULAR + "$,$" + row.CON_N_LOCALIDAD + "$, $" +
                    row.CON_MAIL + "$);'  /></td>");
            }

            tablaretorno.Append("</tr></tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        [HttpPost]
        public ActionResult obtenerEscuela(string nombre)
        {
            var escuelas = _CursoServicio.TraerEscuelas(nombre);

            var tablaretorno = new StringBuilder();
            string rolActor = string.Empty;


            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 300px;'><div align='left'>Escuela</div></th>");
            tablaretorno.Append("<th style='width: 200px;'><div align='left'>Barrio</div></th>");
            tablaretorno.Append("<th style='width: 200px;'><div align='left'>Localidad</div></th>");
            //tablaretorno.Append("<th style='width: 150px;'><div align='left'>Localidad</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var row in escuelas)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + row.Nombre_Escuela + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.LocalidadEscuela.NombreLocalidad + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + (row.barrio.ID_BARRIO == 0 ? row.Barrio : row.barrio.N_BARRIO)  + "</td>");
                //tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.MAIL_ONG + "</td>");
                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionaEscuela(" +
                    row.Id_Escuela + ");'  /></td>");
            }

            tablaretorno.Append("</tr></tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        [HttpPost]
        public ActionResult TraerEscuela(int idEscuela)
        {
            var escuela = _CursoServicio.TraerEscuela(idEscuela);

            return Json(escuela);
        }
        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMCursos, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarCurso(CursoVista model)
        {
            switch (_CursoServicio.UpdateCurso(model))
            {
                case 0:
                    var vis = _CursoServicio.GetIndex();
                    vis.BusquedaDescripcion = "";
                    vis.BusquedaNroCurso = 0;
                    return View("Index", vis);
                case 1:
                    TempData["MensajeError"] = "----------------";
                    return View("Formulario", model);
                case 2:
                    TempData["MensajeError"] = "Seleccione una opción";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }


        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMCursos, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarCurso(CursoVista model)
        {
            //CursoVista modelo = model;
            switch (_CursoServicio.AddCurso(model))
            {
                case 0:
                    return View("Index", _CursoServicio.GetIndex());
                case 1:
                    TempData["MensajeError"] = "------------------------------";
                    return View("Formulario", _CursoServicio.GetCursoReload(model));
                case 2:
                    TempData["MensajeError"] = "Seleccione una opción";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }


        public ActionResult Cursos()
        {
            var vista = _CursoServicio.GetCursosAdm();
            vista.BusquedaDescripcion = "";
            vista.BusquedaNroCurso = 0;
            return View(vista);
        }

        [HttpPost]
        public ActionResult ListarCursos(CursosVista model)
        {
            int nroCurso = model.BusquedaNroCurso ?? 0;
            return View("Cursos", _CursoServicio.GetCursosAdm(nroCurso, model.BusquedaDescripcion));
        }

        public ActionResult IndexPagerAdm(Pager pager, CursosVista model)
        {
            ICursosVista vista = _CursoServicio.GetCursosAdm(pager, model.Id, model.BusquedaDescripcion);
            vista.BusquedaDescripcion = model.BusquedaDescripcion;
            vista.BusquedaNroCurso = model.BusquedaNroCurso;
            TempData["Model"] = vista;
            return RedirectToAction("PageRedirectAdm");
        }

        public ActionResult PageRedirectAdm()
        {
            return View("Cursos", TempData["Model"]);
        }


        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMCursos, Accion = Enums.Acciones.Modificar)]
        public ActionResult Asistencias(int IdCurso)
        {
            ICursoVista vista = _CursoServicio.GetCurso(IdCurso);
            return View("AsistenciaPeriodo", vista);
        }

        //public ActionResult CargarAsistencias(int IdCurso)
        //{
        //    ICursoVista vista = _CursoServicio.GetCurso(IdCurso);
        //    return View("CargarAsistencias", vista.fichas);
        //}

        [HttpPost]
        //[CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMCursos, Accion = Enums.Acciones.Agregar)]
        public ActionResult GuardarAsistencias(List<FichasCurso> model)
        {
            List<IFichasCurso> vista = new List<IFichasCurso>();
            foreach (var f in model)
            {
                IFichasCurso FIC = new FichasCurso
                  {

                      IdFicha = f.IdFicha,
                      tipoFicha = f.tipoFicha,
                      Cuil = f.Cuil,
                      Apellido = f.Apellido,
                      Nombre = f.Nombre,
                      TipoDocumento = f.TipoDocumento,
                      NumeroDocumento = f.NumeroDocumento,
                      MES = f.MES,
                      PORC_ASISTENCIA = f.PORC_ASISTENCIA,
                      id_curso = f.id_curso,
                  };

                vista.Add(FIC);
            
            }

            ICursoVista modelo;
            switch (_CursoServicio.addAsistencias(vista))
            {
                case 0:
                     //modelo = _CursoServicio.GetCurso((int)vista[0].id_curso);
                    return RedirectToAction("Asistencias", new { IdCurso = (int)vista[0].id_curso });
                    //return View("AsistenciaPeriodo", modelo);
                //case 1:
                //    TempData["MensajeError"] = "------------------------------";
                //    return View("Formulario", _CursoServicio.GetCursoReload(model));
                //case 2:
                //    TempData["MensajeError"] = "Seleccione una opción";
                //    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Cursos", _CursoServicio.GetIndex());
            }
        }


        public ActionResult ModificarAsistencias(int idCurso, string mesPeriodo)
        { 
        
            return View("FormularioAsistencias",_CursoServicio.TraerAsistenciasCurso(idCurso,mesPeriodo));
        
        }

        public ActionResult VerAsistencias(int idCurso, string mesPeriodo)
        {

            return View("VerAsistencias", _CursoServicio.TraerAsistenciasCurso(idCurso, mesPeriodo));

        }

        [HttpPost]
        //[CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMCursos, Accion = Enums.Acciones.Agregar)]
        public ActionResult UpdateAsistencias(List<FichasCurso> model)
        {
            List<IFichasCurso> vista = new List<IFichasCurso>();
            foreach (var f in model)
            {
                IFichasCurso FIC = new FichasCurso
                {

                    IdFicha = f.IdFicha,
                    tipoFicha = f.tipoFicha,
                    Cuil = f.Cuil,
                    Apellido = f.Apellido,
                    Nombre = f.Nombre,
                    TipoDocumento = f.TipoDocumento,
                    NumeroDocumento = f.NumeroDocumento,
                    MES = f.MES,
                    PORC_ASISTENCIA = f.PORC_ASISTENCIA,
                    id_curso = f.id_curso,
                    idAsistencia = f.idAsistencia
                };

                vista.Add(FIC);

            }

            //ICursoVista modelo;
            switch (_CursoServicio.UpdateAsistencias(vista))
            {
                case 0:
                    //modelo = _CursoServicio.GetCurso((int)vista[0].id_curso);
                    return RedirectToAction("Asistencias", new { IdCurso = (int)vista[0].id_curso });
                //return View("AsistenciaPeriodo", modelo);
                case 1:
                    TempData["MensajeError"] = "Error al actualizar las asistencias";
                    return RedirectToAction("Asistencias", new { IdCurso = (int)vista[0].id_curso });
                //case 2:
                //    TempData["MensajeError"] = "Seleccione una opción";
                //    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Cursos", _CursoServicio.GetIndex());
            }
        }

        public ActionResult VerInscriptos(int idCurso)
        {

            return View("VerInscriptos", _CursoServicio.GetFichasCurso(idCurso));

        }

       //[HttpPost]
        public ActionResult CargarAsistencias(/*List<FichasCurso> model, */int idCurso, string mesPeriodo)
        {

           //List<IFichasCurso> vista = new List<IFichasCurso>();
           //foreach(var item in model)
           //{
           //     vista.Add(item);
           //}
           //  model = null;
            return View("CargarAsistencias", _CursoServicio.GetFichasSinAsistencia( idCurso, mesPeriodo));

        }

        [HttpPost]
        //[CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMCursos, Accion = Enums.Acciones.Agregar)]
        public ActionResult insFichaSinAsistencias(List<FichasCurso> model)
        {
            List<IFichasCurso> vista = new List<IFichasCurso>();
            foreach (var f in model)
            {
                if (f.registrar == true)
                {
                    IFichasCurso FIC = new FichasCurso
                    {

                        IdFicha = f.IdFicha,
                        tipoFicha = f.tipoFicha,
                        Cuil = f.Cuil,
                        Apellido = f.Apellido,
                        Nombre = f.Nombre,
                        TipoDocumento = f.TipoDocumento,
                        NumeroDocumento = f.NumeroDocumento,
                        MES = f.MES,
                        PORC_ASISTENCIA = f.PORC_ASISTENCIA,
                        id_curso = f.id_curso,
                    };

                    vista.Add(FIC);
                }
            }

            switch (_CursoServicio.addAsistencias(vista))
            {
                case 0:
                    //modelo = _CursoServicio.GetCurso((int)vista[0].id_curso);
                    return RedirectToAction("ModificarAsistencias", new { idCurso = (int)vista[0].id_curso, mesPeriodo = vista[0].MES });
                //return View("AsistenciaPeriodo", modelo);
                //case 1:
                //    TempData["MensajeError"] = "------------------------------";
                //    return View("Formulario", _CursoServicio.GetCursoReload(model));
                //case 2:
                //    TempData["MensajeError"] = "Seleccione una opción";
                //    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Cursos", _CursoServicio.GetIndex());
            }
        }


    }
}
