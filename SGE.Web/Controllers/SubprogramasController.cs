using System.Web.Mvc;
using SGE.Model.Comun;
using SGE.Servicio.Servicio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Web.Controllers.Shared;
using SGE.Web.Infrastucture;
using SGE.Model.Entidades;
using System.Text;
using SGE.Model.Entidades.Interfaces;
using System.Collections.Generic;
using System.Web;

namespace SGE.Web.Controllers
{
    [CustomAuthorize(Formulario = Enums.Formulario.AbmSubprogramas)]
    public class SubprogramasController : BaseController
    {
        //
        // GET: /Subprogramas/
        private readonly ISubprogramaServicio _subprogramaServicio;
        private readonly IProgramaServicio _programaServicio;

        public SubprogramasController()
            : base((int)Formularios.SubProgramas)
        {
            _subprogramaServicio = new  SubprogramaServicio();
            _programaServicio = new ProgramaServicio();
        }

        public ActionResult Index()
        {
            var vista = _subprogramaServicio.GetIndex();
            vista.DescripcionBusca = "";
            return View(vista);
        }

        [HttpPost]
        public ActionResult BuscarSubprograma(SubprogramasVista model)
        {
            return View("Index", _subprogramaServicio.GetSubprogramas(model.DescripcionBusca));
        }

        public ActionResult IndexPager(Pager pager, SubprogramasVista model)
        {
            ISubprogramasVista vista = _subprogramaServicio.GetSubprogramas(pager, model.Id, model.DescripcionBusca);

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", TempData["Model"]);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSubprogramas, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar(int? idSubprograma)
        {
            ISubprogramaVista vista = _subprogramaServicio.GetSubprograma(-1,"Agregar");
            if (idSubprograma != null) vista.Id = idSubprograma.Value;

            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSubprogramas, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idSubprograma)
        {
            ISubprogramaVista vista = _subprogramaServicio.GetSubprograma(idSubprograma, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSubprogramas, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idSubprograma)
        {
            ISubprogramaVista vista = _subprogramaServicio.GetSubprograma(idSubprograma, "Modificar");
            return View("Formulario", vista);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSubprogramas, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarSubprograma(SubprogramaVista model)
        {
            switch (_subprogramaServicio.AddSubprograma(model))
            {
                case 0:
                    return View("Index", _subprogramaServicio.GetSubprogramas());
                case 1:
                    TempData["MensajeError"] = "Ya existe un Subprograma con ese nombre";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSubprogramas, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarSubprograma(SubprogramaVista model)
        {
            switch (_subprogramaServicio.UpdateSubprograma(model))
            {
                case 0:
                    var vista = _subprogramaServicio.GetSubprogramas();
                    vista.DescripcionBusca = "";
                    return View("Index", vista);
                case 1:
                    TempData["MensajeError"] = "Ya existe un Subprograma con ese nombre";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }


        [HttpPost]
        public ActionResult SubprogramasRoles(int idprograma)
        {
            var subprogramas = _subprogramaServicio.GetSubprogramasRol(idprograma);

            return Json(subprogramas);
        }

        [HttpPost]
        public ActionResult SubprogramasProg(int idprograma)
        {
            var subprogramas = _subprogramaServicio.GetSubprogramas(idprograma);

            return Json(subprogramas);
        }

        [HttpPost]
        public ActionResult Programas(string programa)
        {
            var programas = _programaServicio.GetProgramas(programa);


            var tablaretorno = new StringBuilder();


            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 100px;'><div align='left'>ID</div></th>");
            tablaretorno.Append("<th style='width: 300px;'><div align='left'>Programa</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");


            foreach (var row in programas.Programas)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + row.IdPrograma + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.NombrePrograma + "</td>");
                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionarPrograma(" +
                    row.IdPrograma + ",$" + row.NombrePrograma + "$);'  /></td>");
            }

            tablaretorno.Append("</tr></tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        //06/03/2016 - DI CAMPLI LEANDRO - ASOCIAS SUBPROGRAMAS PARA FICHAS
        public ActionResult AsociarSubprograma()
        {

            //IFichaVista vista = _fichaservicio.GetFichas();
            SubprogramaVista model = new SubprogramaVista();
            IList<IFicha> fichas = new List<IFicha>();
            model.Fichas = fichas;
            model.paso = "PASO 1";
            model.tipoficha = 3;
            return View(model);

        }

        [HttpPost]
        public ActionResult obtenerSubProg(string nombre)
        {
            ISubprogramasVista subP = new SubprogramasVista();
            subP.Subprogramas = _subprogramaServicio.GetSubprogramasN(nombre);

            var tablaretorno = new StringBuilder();



            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 100px;'><div align='left'>ID</div></th>");
            tablaretorno.Append("<th style='width: 280px;'><div align='left'>Nombre</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Descripción</div></th>");
            tablaretorno.Append("<th style='width: 150px;'><div align='left'>Monto</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var row in subP.Subprogramas)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + row.IdSubprograma + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.NombreSubprograma + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.Descripcion + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.monto + "</td>");
                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18' height='18' alt='' onclick='SeleccionaSubP(" +
                    row.IdSubprograma + ",$" + row.NombreSubprograma + "$);'  /></td></tr>");
            }

            tablaretorno.Append("</tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }


        //06/03/2016 - DI CAMPLI LEANDRO - ASOCIAS SUBPROGRAMAS PARA FICHAS
        public ActionResult AsociarSubprogramaPaso2(SubprogramaVista model)
        {
            IList<IFicha> fichas = new List<IFicha>();
            model.Fichas = fichas;
            model.paso = "PASO 2";
            return View("AsociarSubprograma", model);

        }

        //06/03/2016 - DI CAMPLI LEANDRO - ASOCIAS SUBPROGRAMAS PARA FICHAS
        public ActionResult AsociarSubprogramaPaso3(SubprogramaVista model)
        {
            IList<IFicha> fichas = new List<IFicha>();
            model.Fichas = fichas;
            model.paso = "PASO 3";
            model.subprograma = model.Selsubprograma;
            return View("AsociarSubprograma", model);

        }

        [HttpPost]
        public ActionResult SubirArchivoFichas(HttpPostedFileBase archivoFichas, SubprogramaVista model)
        {

            SubprogramaVista vista = _subprogramaServicio.SubirArchivoFichas(archivoFichas, model);
            model.paso = "PASO 3";
            model.subprograma = model.Selsubprograma;
            model.ProcesarExcel = vista.ProcesarExcel;
            model.ExcelNombre = vista.ExcelNombre;
            return View("AsociarSubprograma", model);

        }

        public ActionResult CargarFichasSubprogXLS(string ExcelNombre, int idSubprograma, string subprograma, int idprograma)
        {
            SubprogramaVista model = new SubprogramaVista();



            model.paso = "PASO 3";


            IFichasVista vista = _subprogramaServicio.CargarFichasSubprogXLS(ExcelNombre, idprograma);

            model.Fichas = vista.Fichas;
            model.ProcesarExcel = vista.ProcesarExcel;
            model.ExcelNombre = ExcelNombre;
            model.subprograma = subprograma;
            model.Selsubprograma = subprograma;
            model.tipoficha = idprograma;
            model.idsubprograma = idSubprograma;
            return View("AsociarSubprograma", model);
        }

        public ActionResult udpSubprogramaFichas(SubprogramaVista model)
        {
            model = (SubprogramaVista)TempData["SubprogramaVista"];
            int cant = 0;
            cant = _subprogramaServicio.udpSubprogramaFichas(model);
            model.cantProcesados = cant;
            model = _subprogramaServicio.getFichas(model);
            model.subprograma = model.Selsubprograma;
            model.ProcesarExcel = model.ProcesarExcel;
            model.ExcelNombre = model.ExcelNombre;
            model.paso = "PASO 3";

            return View("AsociarSubprograma", model);
        }

    }
}
