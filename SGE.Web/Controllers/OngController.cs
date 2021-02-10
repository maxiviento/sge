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
   [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMOng)]
    public class OngController : BaseController
    {
        private readonly IOngServicio _ongServicio;
        //private readonly ILocalidadServicio _localidadservicio;
        private readonly IActorServicio _actorServicio;

        public OngController() : base((int)Formularios.Ongs)
        {
            _ongServicio = new OngServicio();
            //_localidadservicio = new LocalidadServicio();
            _actorServicio = new ActorServicio();

        }


        public ActionResult Index()
        {
            var vista = _ongServicio.GetIndex();
            vista.DescripcionBusca = "";
            return View(vista);
        }

        public ActionResult IndexPager(Pager pager, OngsVista model)
        {
            IOngsVista vista = _ongServicio.GetOngs(pager, model.DescripcionBusca);
            vista.DescripcionBusca = model.DescripcionBusca;

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", TempData["Model"]);
        }


        [HttpPost]
        public ActionResult BuscarOng(OngsVista model)
        {

            return View("Index", _ongServicio.GetOngs(model.DescripcionBusca));
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMOng, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int IdOng)
        {
            IOngVista vista = _ongServicio.GetOng(IdOng, "Ver");
            return View("Formulario", vista);
        }


        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMCursos, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar()
        {
            IOngVista vista = _ongServicio.GetOng(0, "Agregar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMCursos, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int IdOng)
        {
            IOngVista vista = _ongServicio.GetOng(IdOng, "Modificar");
            return View("Formulario", vista);
        }


        [HttpPost]
        public ActionResult obtenerActor(string dni, int idRolActor,string rubro)
        {
            IActorVista actor = _actorServicio.GetActorDNI(dni, idRolActor);

            var tablaretorno = new StringBuilder();
            string rolActor = string.Empty;



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
                    A.CELULAR + "$,$" + A.MAIL + "$, $" + A.N_BARRIO + "$, $" + A.N_LOCALIDAD + "$, $" + rubro + "$," + A.ID_ACTOR_ROL + ");'  /></td>");
            }

            tablaretorno.Append("</tr></tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }



        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMOng, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarOng(OngVista model)
        {
            switch (_ongServicio.UpdateOng(model))
            {
                case 0:
                    var vis = _ongServicio.GetIndex();
                    vis.DescripcionBusca = "";
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
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMOng, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarOng(OngVista model)
        {
            //EscuelaVista modelo = model;
            switch (_ongServicio.AddOng(model))
            {
                case 0:
                    return View("Index", _ongServicio.GetIndex());
                case 1:
                    TempData["MensajeError"] = "------------------------------";
                    return View("Formulario", _ongServicio.GetOngReload(model));
                case 2:
                    TempData["MensajeError"] = "Seleccione una opción";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }
    }
}
