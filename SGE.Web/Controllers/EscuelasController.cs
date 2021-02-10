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

namespace SGE.Web.Controllers
{
   [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMEscuelas)]
    public class EscuelasController : BaseController
    {
       private readonly IEscuelaServicio _escuelaServicio;
        private readonly ILocalidadServicio _localidadservicio;
        private readonly IActorServicio _actorServicio;

        public EscuelasController() : base((int)Formularios.Escuelas)
        {
            _escuelaServicio = new EscuelaServicio();
            _localidadservicio = new LocalidadServicio();
            _actorServicio = new ActorServicio();

        }


        public ActionResult Index()
        {
            var vista = _escuelaServicio.GetIndex();
            vista.BusquedaDescripcion = "";
            return View(vista);

        }

        [HttpPost]
        public ActionResult BuscarEscuela(EscuelasVista model)
        {

            return View("Index", _escuelaServicio.GetEscuelas(model.BusquedaDescripcion));
        }

        public ActionResult IndexPager(Pager pager, EscuelasVista model)
        {
            IEscuelasVista vista = _escuelaServicio.GetEscuelas(pager,  model.BusquedaDescripcion);
            vista.BusquedaDescripcion = model.BusquedaDescripcion;
            
            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", TempData["Model"]);
        }



        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMEscuelas, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int IdEscuela)
        {
            IEscuelaVista vista = _escuelaServicio.GetEscuela(IdEscuela, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMEscuelas, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar()
        {
            IEscuelaVista vista = _escuelaServicio.GetEscuela(0, "Agregar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMEscuelas, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int IdEscuela)
        {
            IEscuelaVista vista = _escuelaServicio.GetEscuela(IdEscuela, "Modificar");
            return View("Formulario", vista);
        }


       
        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMEscuelas, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarEscuela(EscuelaVista model)
        {
            switch (_escuelaServicio.UpdateEscuela(model))
            {
                case 0:
                    var vis = _escuelaServicio.GetIndex();
                    vis.BusquedaDescripcion = "";
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
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMEscuelas, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarEscuela(EscuelaVista model)
        {
            //EscuelaVista modelo = model;
            switch (_escuelaServicio.AddEscuela(model))
            {
                case 0:
                    return View("Index", _escuelaServicio.GetIndex());
                case 1:
                    TempData["MensajeError"] = "------------------------------";
                    return View("Formulario", _escuelaServicio.GetEscuelaReload(model));
                case 2:
                    TempData["MensajeError"] = "Seleccione una opción";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        public ActionResult BuscarBarrios(int idlocalidad)
        {
            var barrios = _escuelaServicio.cboCargarBarrio(Convert.ToInt32(idlocalidad))
                            ;

            return Json(barrios);
        }

        [HttpPost]
        public ActionResult CargarLocalidades(int idDep)
        {
            var localidad = _escuelaServicio.cboCargarLocalidad(Convert.ToInt32(idDep))
                            ;

            return Json(localidad);
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



    }
}
