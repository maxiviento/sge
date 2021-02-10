using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Model.Comun;
using System.Web;
using System.IO;
using Excel;
using System.Data;


namespace SGE.Servicio.Servicio
{
    public class SubprogramaServicio : BeneficiarioServicio, ISubprogramaServicio
    {
        private readonly ISubprogramaRepositorio _subprogramaRepositorio;
        private readonly IAutenticacionServicio _autenticacion;
        private readonly IRolFormularioAccionRepositorio _rolformularioaccionrepositorio;
        private readonly IUsuarioRolRepositorio _usuarioRolRepositorio;
        private readonly FichaRepositorio _fichaRepositorio;

        public SubprogramaServicio()
        { 
            _subprogramaRepositorio=new SubprogramaRepositorio();
            _autenticacion = new AutenticacionServicio();
            _rolformularioaccionrepositorio = new RolFormularioAccionRepositorio();
            _usuarioRolRepositorio = new UsuarioRolRepositorio();
            _fichaRepositorio = new FichaRepositorio();
        }

        public ISubprogramasVista GetSubprogramas()
        {
            ISubprogramasVista vista = new SubprogramasVista();

            var pager = new Pager(_subprogramaRepositorio.GetSubprogramasCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexSubprograma", _autenticacion.GetUrl("IndexPager", "Subprogramas"));

            vista.Pager = pager;

            vista.Subprogramas = _subprogramaRepositorio.GetSubprogramas(pager.Skip, pager.PageSize);

            return vista;
        }

        public ISubprogramasVista GetIndex()
        {
            ISubprogramasVista vista = new SubprogramasVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexSubprograma", _autenticacion.GetUrl("IndexPager", "Subprogramas"));

            vista.Pager = pager;

            return vista;
        }

        public ISubprogramasVista GetSubprogramas(string descripcion)
        {
            ISubprogramasVista vista = new SubprogramasVista();

            var pager = new Pager(_subprogramaRepositorio.GetSubprogramas(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexSubprograma", _autenticacion.GetUrl("IndexPager", "Subprogramas"));

            vista.Pager = pager;

            vista.Subprogramas = _subprogramaRepositorio.GetSubprogramas(descripcion, pager.Skip, pager.PageSize);

            return vista;
        }

        public ISubprogramaVista GetSubprograma(int idsubprograma, string accion)
        {
            ISubprogramaVista vista = new SubprogramaVista() { Accion = accion };

            if (idsubprograma > 0)
            {
                ISubprograma row = _subprogramaRepositorio.GetSubprograma(idsubprograma);

                vista.Id = row.IdSubprograma;
                vista.Nombre = row.NombreSubprograma;
                vista.Descripcion = row.Descripcion;
                vista.monto = row.monto;
                vista.IdPrograma = row.IdPrograma;
                vista.Programa = row.Programa;
            }

            return vista;
        }

        public ISubprogramasVista GetSubprogramas(Pager pPager, int id, string descripcion)
        {
            ISubprogramasVista vista = new SubprogramasVista();

            var pager = new Pager(_subprogramaRepositorio.GetSubprogramas(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexSubprograma", _autenticacion.GetUrl("IndexPager", "Subprogramas"));

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

            vista.Subprogramas = _subprogramaRepositorio.GetSubprogramas(descripcion, vista.Pager.Skip, vista.Pager.PageSize);
            vista.DescripcionBusca = descripcion;

            return vista;
        }

        private static ISubprograma VistaToDatos(ISubprogramaVista vista)
        {
            ISubprograma programa = new Subprograma
            {
                IdSubprograma = vista.Id,
                NombreSubprograma = vista.Nombre,
                Descripcion = vista.Descripcion,
                monto = vista.monto,
                IdPrograma = vista.IdPrograma
            };
            return programa;
        }

        public int AddSubprograma(ISubprogramaVista vista)
        {
            if (_subprogramaRepositorio.GetSubprogramas(vista.Nombre).Count() > 0)
            {
                return 1;
            }

            vista.Id =_subprogramaRepositorio.AddSubprograma(VistaToDatos(vista));

            return 0;
        }
        
        public int UpdateSubprograma(ISubprogramaVista vista)
        {
            _subprogramaRepositorio.UpdateSubprograma(VistaToDatos(vista));

            return 0;
        }



        public List<Subprograma> GetSubprogramasRol(int idPrograma)
        {
            //**************CARGAR EL COMBO PROGRAMAS***********************************
            // 28/08/2014 - controlar aca si tiene el rol activado para condicionar los programas y subprogramas.
            var accesoprograma =
                                  _rolformularioaccionrepositorio.GetFormulariosDelUsuario(base.GetUsuarioLoguer().Id_Usuario).Where(
                                      c =>
                                      c.Id_Accion == (int)Enums.Acciones.ProgramasPorRol &&
                                      c.Id_Formulario == (int)Enums.Formulario.AbmFicha).Count() > 0
                                      ? true
                                      : false;
            var usuarioRoles = _usuarioRolRepositorio.GetRolesDelUsuario(base.GetUsuarioLoguer().Id_Usuario);


            if (accesoprograma) //verifica si debe traer todos los programas asociados
            {
                List<Subprograma> lsubprogr= new List<Subprograma>();
                lsubprogr.Add(new Subprograma { IdSubprograma = 0, NombreSubprograma = "Todos" });

                var lsubprogramas = (from rs in _subprogramaRepositorio.GetSubprogramasRol()
                                    join u in usuarioRoles on rs.Id_Rol equals u.IdRol
                                    where rs.IdPrograma == idPrograma
                                    group rs by new { rs.IdSubprograma, rs.NombreSubprograma } into grouping
                                 select new
                                     Subprograma
                                 {
                                     IdSubprograma = grouping.Key.IdSubprograma,
                                     NombreSubprograma = grouping.Key.NombreSubprograma
                                 }).ToList();


                foreach (var item in lsubprogramas)
                {

                    lsubprogr.Add(new Subprograma { IdSubprograma = item.IdSubprograma, NombreSubprograma = item.NombreSubprograma });

                }


                return lsubprogr;
            }
            else
            {
                var lsub = _subprogramaRepositorio.GetSubprogr();

                return lsub;
            }
            //*****************************************************************************************

        }

        public IList<ISubprograma> GetSubprogramasN(string nombre)
        {
          return  _subprogramaRepositorio.GetSubprogramas(nombre);
        
        }

        public List<Subprograma> GetSubprogramas(int idPrograma)
        {
            var lsub = _subprogramaRepositorio.GetSubprogramas(idPrograma);
            List<Subprograma> lsubprogr = new List<Subprograma>();
            lsubprogr.Add(new Subprograma { IdSubprograma = 0, NombreSubprograma = "Todos" });


            foreach (var item in lsub)
            {

                lsubprogr.Add(new Subprograma { IdSubprograma = item.IdSubprograma, NombreSubprograma = item.NombreSubprograma });

            }


            return lsubprogr;
            
        }

        public SubprogramaVista SubirArchivoFichas(HttpPostedFileBase archivo, SubprogramaVista model)
        {
            if (archivo != null && archivo.ContentLength > 0)
            {
                var fileName = Path.GetFileName(archivo.FileName);
                var nombre = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day +
                             DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";

                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Archivos/Importados"), nombre);
                archivo.SaveAs(path);

                //Verifíco el Formato del Archivo
                var fileInfo = new FileInfo(path);

                IExcelDataReader excelReader = null;

                FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);

                string file = fileInfo.Name;

                if (file.Split('.')[1].Equals("xls"))
                {
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (file.Split('.')[1].Equals("xlsx"))
                {
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }

                if (excelReader != null) excelReader.IsFirstRowAsColumnNames = true;
                var dsExcel = new DataSet();
                if (excelReader != null)
                {
                    dsExcel = excelReader.AsDataSet();
                    excelReader.Close();
                }
                if (dsExcel != null)
                {
                    if (dsExcel.Tables[0].Rows.Count > 0)
                    {
                        if (dsExcel.Tables[0].Columns.Contains("ID_FICHA"))
                        {
                            model.ProcesarExcel = true;
                            model.ExcelNombre = nombre;
                        }
                        else
                        {
                            model.ProcesarExcel = false;
                            model.ExcelNombre = null;
                            File.Delete(path);
                        }
                    }
                }
            }
            else
            {
                model.ProcesarExcel = true;
                
            }
            IList<IFicha> lista = new List<IFicha>();
            model.Fichas = lista;

            return model;
        }

        public IFichasVista CargarFichasSubprogXLS(string ExcelUrl, int idprograma)
        {

            IFichasVista vista = new FichasVista();
            vista.ExcelNombre = ExcelUrl;
            List<int> idBeneficiariosExport = new List<int>();
            string path = HttpContext.Current.Server.MapPath(@"\Archivos\Importados") + @"\" + ExcelUrl;
            vista.ExcelUrl = path;
            var fileInfo = new FileInfo(path);
            IExcelDataReader excelReader = null;
            FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
            string file = fileInfo.Name;

            if (file.Split('.')[1].Equals("xls"))
            {
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (file.Split('.')[1].Equals("xlsx"))
            {
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }

            if (excelReader != null) excelReader.IsFirstRowAsColumnNames = true;
            var dsExcel = new DataSet();
            if (excelReader != null)
            {
                dsExcel = excelReader.AsDataSet();
                excelReader.Close();
            }
            if (dsExcel != null)
            {

                if (dsExcel.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow row in dsExcel.Tables[0].Rows)
                    {
                        if (row["ID_FICHA"] != null)
                        {
                            idBeneficiariosExport.Add(Convert.ToInt32(row["ID_FICHA"].ToString()));
                        }

                    }
                }
            }

            vista.ProcesarExcel = true;
            vista.Fichas = _fichaRepositorio.GetFichas(idBeneficiariosExport, idprograma);

            return vista;
        }

        public int udpSubprogramaFichas(SubprogramaVista model)
        {
            int resu = 0;

            resu = _subprogramaRepositorio.udpSubprogramaFichas(model.Fichas, model.idsubprograma);




            return resu;
        }

        public SubprogramaVista getFichas(SubprogramaVista model)
        {
            List<int> idBeneficiariosExport = new List<int>();
            if (model != null)
            {

                if (model.Fichas.Count > 0)
                {

                    foreach (IFicha row in model.Fichas)
                    {
                        idBeneficiariosExport.Add(Convert.ToInt32(row.IdFicha));

                    }
                }
            }


            model.Fichas = _fichaRepositorio.GetFichas(idBeneficiariosExport, model.tipoficha);
            return model;
        }
    }
}

