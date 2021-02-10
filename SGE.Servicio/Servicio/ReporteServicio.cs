using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using NPOI.HSSF.UserModel;
using SGE.Model.Comun;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Comun;
using SGE.Model.Entidades;
using System.Globalization;
using SGE.Repositorio.Modelo;

namespace SGE.Servicio.Servicio
{
    public class ReporteServicio : BaseServicio, IReporteServicio
    {
        private readonly ISucursalCoberturaRepositorio _sucursalCoberturaRepositorio;
        private readonly ILocalidadRepositorio _localidadRepositorio;
        private readonly IDepartamentoRepositorio _departamentoRepositorio;
        private readonly IProgramaRepositorio _programaRepositorio;
        private readonly IAutenticacionServicio _aut;
        private readonly IFichaRepositorio _fichaRepositorio;
        private readonly IBeneficiarioRepositorio _beneficiariorepositorio;
        private readonly IConceptoRepositorio _conceptorepositorio;
        private readonly IInstitucionRepositorio _institucionRepositorio;
        private readonly ICarreraRepositorio _carreraRepositorio;
        private readonly IAsistenciasRepositorio _asistenciaRepositorio;

        private readonly IEstadoBeneficiarioRepositorio _estadobeneficiariorepositorio; // 08/04/2013 - DI CAMPLI LEANDRO - NUEVO FILTRO PARA EL REPORTE
        private readonly EtapaRepositorio _etaparepositorio;

        public ReporteServicio()
        {
            _sucursalCoberturaRepositorio = new SucursalCoberturaRepositorio();
            _localidadRepositorio = new LocalidadRepositorio();
            _departamentoRepositorio = new DepartamentoRepositorio();
            _aut = new AutenticacionServicio();
            _programaRepositorio = new ProgramaRepositorio();
            _fichaRepositorio = new FichaRepositorio();
            _beneficiariorepositorio = new BeneficiarioRepositorio();
            _conceptorepositorio = new ConceptoRepositorio();
            _institucionRepositorio = new InstitucionRepositorio();
            _carreraRepositorio = new CarreraRepositorio();
            _estadobeneficiariorepositorio = new EstadoBeneficiarioRepositorio();
            _asistenciaRepositorio = new AsistenciasRepositorio();
        }

        public IReporteDepLocProgEsta IndexRepDepLocProgEst()
        {
            IReporteDepLocProgEsta vista = new ReporteDepLocProgEsta();

            var pager =
                new Pager(0,
                          Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                          "FormIndexRDepLocProEst", _aut.GetUrl("IndexPagerRDepLocProEst", "Reportes"));



            var listaprogramas = _programaRepositorio.GetProgramas();

            var listadepartamentos = _departamentoRepositorio.GetDepartamentos();

            var listlocalidades = _localidadRepositorio.GetLocalidades();

            vista.Pager = pager;

            CargarPrograma(vista, listaprogramas);

            CargarDepartamentos(vista, listadepartamentos);

            CargarLocalidad(vista, listlocalidades);

            // 08/04/2013 - DI CAMPLI LEANDRO - NUEVO FILTRO PARA EL REPORTE
            CargarEstadosBeneficiario(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 1, Description = "PRIMER PASO" });
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 2, Description = "APRENDIZ" });
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 3, Description = "XMÍ" });

            return vista;

        }

        public IReporteDepLocProgEsta GetReporteDepLocProgEstaChangeDep(IReporteDepLocProgEsta vista)
        {


            var pager =
                new Pager(0,
                          Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                          "FormIndexRDepLocProEst", _aut.GetUrl("IndexPagerRDepLocProEst", "Reportes"));


            var listaprogramas = _programaRepositorio.GetProgramas();

            var listadepartamentos = _departamentoRepositorio.GetDepartamentos();

            var listlocalidades =
                _localidadRepositorio.GetLocalidadesByDepto(Convert.ToInt32(vista.BusquedaDepartamentos.Selected));

            vista.Pager = pager;

            CargarPrograma(vista, listaprogramas);

            CargarDepartamentos(vista, listadepartamentos);

            CargarLocalidad(vista, listlocalidades);

            // 08/04/2013 - DI CAMPLI LEANDRO - NUEVO FILTRO PARA EL REPORTE
            CargarEstadosBeneficiario(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());

            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 1, Description = "PRIMER PASO" });
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 2, Description = "APRENDIZ" });
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 3, Description = "XMÍ" });

            return vista;
        }

        public IReporteDepLocProgEsta GetReporteDepLocProgEsta(IReporteDepLocProgEsta vista)
        {

            IList<IFicha> fichas =
                _fichaRepositorio.GetFichasRDepLocProgEst(Convert.ToInt32(vista.BusquedaDepartamentos.Selected),
                                                          Convert.ToInt32(vista.BusquedaProgramas.Selected),
                                                          Convert.ToInt32(vista.BusquedaLocalidades.Selected),
                                                          vista.Estado, Convert.ToInt32(vista.EstadoBenef.Selected ?? "0"));

            int tipoprog = 0;
            tipoprog = Convert.ToInt32(vista.ComboTiposPpp.Selected);
            if (Convert.ToInt32(vista.BusquedaProgramas.Selected) == 3)
            {
                if (tipoprog > 0)
                {
                    fichas = fichas.Where(c => c.TipoPrograma == tipoprog).ToList();
                }

            }

            IList<IContenidoDepLocProgEsta> agrupado = (from t in fichas
                                                        group t by
                                                            new { t.IdDepartamento, t.NombreDepartamento, t.IdLocalidad, t.Localidad,t.TipoFicha, t.NombreTipoFicha }
                                                            into grp
                                                            select new ContenidoDepLocProgEsta
                                                                       {
                                                                           idDepartamento = (grp.Key.IdDepartamento) ??
                                                                               0,
                                                                           Departamento =
                                                                               (grp.Key.NombreDepartamento) ??
                                                                               "Sin Dept.".ToString(),
                                                                           idLocalidad =(grp.Key.IdLocalidad) ??
                                                                               0,
                                                                           Localidad = 
                                                                               grp.Key.Localidad ?? "Sin Loc.".ToString(),
                                                                           idPrograma = (grp.Key.TipoFicha ?? 0),
                                                                           Programa =
                                                                               grp.Key.NombreTipoFicha ??
                                                                               "Sin Prog.".ToString(),
                                                                           Cantidad = grp.Count()
                                                                       }).ToList().Cast<IContenidoDepLocProgEsta>().ToList();


            vista.ContenidoReporte =
                agrupado.OrderBy(c => c.Departamento).ThenBy(c => c.Localidad).ThenBy(c => c.Programa).ToList();


            var listaprogramas = _programaRepositorio.GetProgramas();

            var listadepartamentos = _departamentoRepositorio.GetDepartamentos();

            IList<ILocalidad> listlocalidades = Convert.ToInt32(vista.BusquedaDepartamentos.Selected) != 0 ? _localidadRepositorio.GetLocalidadesByDepto(Convert.ToInt32(vista.BusquedaDepartamentos.Selected)) : _localidadRepositorio.GetLocalidades();

            CargarPrograma(vista, listaprogramas);

            CargarDepartamentos(vista, listadepartamentos);

            CargarLocalidad(vista, listlocalidades);


            // 08/04/2013 - DI CAMPLI LEANDRO - NUEVO FILTRO PARA EL REPORTE
            CargarEstadosBeneficiario(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());

            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 1, Description = "PRIMER PASO" });
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 2, Description = "APRENDIZ" });
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 3, Description = "XMÍ" });

            return vista;

        }

        public byte[] ExportReporteCantidades(IReporteDepLocProgEsta vista)
        {
            IList<IFicha> fichas =
               _fichaRepositorio.GetFichasRDepLocProgEst(Convert.ToInt32(vista.BusquedaDepartamentos.Selected),
                                                         Convert.ToInt32(vista.BusquedaProgramas.Selected),
                                                         Convert.ToInt32(vista.BusquedaLocalidades.Selected),
                                                         vista.Estado, Convert.ToInt32(vista.EstadoBenef.Selected ?? "0"));


            int tipoprog = 0;
            tipoprog = Convert.ToInt32(vista.ComboTiposPpp.Selected);
            if (Convert.ToInt32(vista.BusquedaProgramas.Selected) == 3)
            {
                if (tipoprog > 0)
                {
                    fichas = fichas.Where(c => c.TipoPrograma == tipoprog).ToList();
                }

            }


            IList<IContenidoDepLocProgEsta> agrupado = (from t in fichas
                                                        group t by
                                                            new { t.IdDepartamento, t.NombreDepartamento, t.IdLocalidad, t.Localidad, t.TipoFicha, t.NombreTipoFicha }
                                                            into grp
                                                            select new ContenidoDepLocProgEsta
                                                            {
                                                                idDepartamento = (grp.Key.IdDepartamento) ??
                                                                    0,
                                                                Departamento =
                                                                    (grp.Key.NombreDepartamento) ??
                                                                    "Sin Dept.".ToString(),
                                                                idLocalidad = (grp.Key.IdLocalidad) ??
                                                                    0,
                                                                Localidad =
                                                                    grp.Key.Localidad ?? "Sin Loc.".ToString(),
                                                                idPrograma = (grp.Key.TipoFicha ?? 0),
                                                                Programa =
                                                                    grp.Key.NombreTipoFicha ??
                                                                    "Sin Prog.".ToString(),
                                                                Cantidad = grp.Count()
                                                            }).ToList().Cast<IContenidoDepLocProgEsta>().ToArray();


            vista.ContenidoReporte =
                agrupado.OrderBy(c => c.Departamento).ThenBy(c => c.Localidad).ThenBy(c => c.Programa).ToList();


            string path = "";

            path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateDepLocProgEst_Cantidad.xls");

            var fs =new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            int i = 1;
            int cantidadTotal = 0;

            foreach (var item in agrupado)
            {
                i++;

                var row = sheet.CreateRow(i);

                var cellDepartamento = row.CreateCell(0); cellDepartamento.SetCellValue(item.Departamento);
                var cellLocalidad = row.CreateCell(1); cellLocalidad.SetCellValue(item.Localidad);
                var cellPrograma = row.CreateCell(2); cellPrograma.SetCellValue(item.Programa);
                var cellCantidad = row.CreateCell(3); cellCantidad.SetCellValue(item.Cantidad);
                cantidadTotal = cantidadTotal + item.Cantidad;
            }
            var rowtotal = sheet.CreateRow(i+1);
            var cellCantidadTotal = rowtotal.CreateCell(3); cellCantidadTotal.SetCellValue(cantidadTotal);

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();

        }

        //export para el detalle
        public byte[] ExportReporteDetalle(int idDepartamento, int idPrograma, int idLocalidad, string estado, int estadoBenef, int tipoprograma)
        {
            IList<IFicha> listaparaExcel =
                _fichaRepositorio.GetFichasRDepLocProgEstDetalle(idDepartamento,
                                                                 idPrograma,
                                                                 idLocalidad,
                                                                 estado, estadoBenef);

            int tipoprog = 0;
            tipoprog = Convert.ToInt32(tipoprograma);
            if (Convert.ToInt32(idPrograma) == 3)
            {
                if (tipoprog > 0)
                {
                    listaparaExcel = listaparaExcel.Where(c => c.TipoPrograma == tipoprog).ToList();
                }

            }

            string path = "";
            if (idPrograma == (int) Enums.Programas.Universitaria || idPrograma == (int) Enums.Programas.Terciaria)
            {
                path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosBecas.xls");
            }
            else
            {
                path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosDetalle.xls");
            }
        
            var fs =
                new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

                int i = 1;

                switch (Convert.ToInt32(idPrograma))
                {
                    case (int) Enums.Programas.Terciaria:
                        #region case (int)Enums.TipoFicha.Terciaria
                        foreach (var beneficiario in listaparaExcel)
                        {
                            i++;
                            var row = sheet.CreateRow(i);
                            //Ficha
                            var cellidficha = row.CreateCell(0);cellidficha.SetCellValue(beneficiario.IdFicha);
                            var cellApellido = row.CreateCell(1);cellApellido.SetCellValue(beneficiario.Apellido);
                            var cellNombre = row.CreateCell(2);cellNombre.SetCellValue(beneficiario.Nombre);
                            var cellDni = row.CreateCell(3);cellDni.SetCellValue(beneficiario.NumeroDocumento);
                            var cellidcarrera = row.CreateCell(4);cellidcarrera.SetCellValue(beneficiario.CarreraFicha.IdCarrera == null? String.Empty: beneficiario.CarreraFicha.IdCarrera.ToString());
                            var cellncarrera = row.CreateCell(5);cellncarrera.SetCellValue(beneficiario.CarreraFicha.NombreCarrera);
                            var cellidinstitucion = row.CreateCell(6);cellidinstitucion.SetCellValue(beneficiario.CarreraFicha.IdInstitucion == null? String.Empty: beneficiario.CarreraFicha.InstitucionCarrera.IdInstitucion.ToString());
                            var cellninstitucion = row.CreateCell(7);cellninstitucion.SetCellValue(beneficiario.CarreraFicha.InstitucionCarrera.NombreInstitucion);

                            //Beneficiario
                            var cellIdestadoben = row.CreateCell(8); cellIdestadoben.SetCellValue(beneficiario.BeneficiarioFicha.IdEstado == null ? String.Empty : beneficiario.BeneficiarioFicha.IdEstado.ToString());
                            var cellEstadoben = row.CreateCell(9); cellEstadoben.SetCellValue(beneficiario.BeneficiarioFicha.NombreEstado == null ? "NO ES BENEFICIARIO" : beneficiario.BeneficiarioFicha.NombreEstado.ToString());

                            int cantConceptosPagados = _conceptorepositorio.GetConceptosByBeneficiario(beneficiario.BeneficiarioFicha.IdBeneficiarioExport ?? 0).Where(c=> c.Año== DateTime.Now.Year).Count();
                            var cellPago = row.CreateCell(10); 
                            if (cantConceptosPagados !=0)
                                cellPago.SetCellValue(cantConceptosPagados + "/" + "12");

                        }
                        #endregion
                        break;
                    case (int)Enums.Programas.Universitaria:
                        #region case (int)Enums.TipoFicha.Universitaria
                        foreach (var beneficiario in listaparaExcel)
                        {
                            i++;
                            var row = sheet.CreateRow(i);
                            //Ficha
                            var cellidficha = row.CreateCell(0);
                            cellidficha.SetCellValue(beneficiario.IdFicha);
                            var cellApellido = row.CreateCell(1);
                            cellApellido.SetCellValue(beneficiario.Apellido);
                            var cellNombre = row.CreateCell(2);
                            cellNombre.SetCellValue(beneficiario.Nombre);
                            var cellDni = row.CreateCell(3);
                            cellDni.SetCellValue(beneficiario.NumeroDocumento);
                            var cellidcarrera = row.CreateCell(4);
                            cellidcarrera.SetCellValue(beneficiario.IdCarreraUniversitaria == null
                                                           ? String.Empty
                                                           : beneficiario.CarreraFicha.IdCarrera.ToString());
                            var cellncarrera = row.CreateCell(5);cellncarrera.SetCellValue(beneficiario.CarreraFicha.NombreCarrera);
                            var cellidinstitucion = row.CreateCell(6);cellidinstitucion.SetCellValue(beneficiario.CarreraFicha.InstitucionCarrera.IdInstitucion== null? String.Empty: beneficiario.CarreraFicha.InstitucionCarrera.IdInstitucion.ToString());
                            var cellninstitucion = row.CreateCell(7);cellninstitucion.SetCellValue(beneficiario.CarreraFicha.InstitucionCarrera.NombreInstitucion);

                            //Beneficiario
                            var cellIdestadoben = row.CreateCell(8); cellIdestadoben.SetCellValue(beneficiario.BeneficiarioFicha.IdEstado == null ? String.Empty : beneficiario.BeneficiarioFicha.IdEstado.ToString());
                            var cellEstadoben = row.CreateCell(9); cellEstadoben.SetCellValue(beneficiario.BeneficiarioFicha.NombreEstado == null ? "NO ES BENEFICIARIO" : beneficiario.BeneficiarioFicha.NombreEstado.ToString());

                            int cantConceptosPagados = _conceptorepositorio.GetConceptosByBeneficiario(beneficiario.BeneficiarioFicha.IdBeneficiarioExport ?? 0).Where(c => c.Año == DateTime.Now.Year).Count();
                            var cellPago = row.CreateCell(10);
                            if (cantConceptosPagados != 0)
                                cellPago.SetCellValue(cantConceptosPagados + "/" + "12");
                        }
                        #endregion
                        break;
                    case (int)Enums.Programas.Ppp:
                        #region case (int)Enums.TipoFicha.Ppp
                        foreach (var beneficiario in listaparaExcel)
                        {
                            i++;
                            var row = sheet.CreateRow(i);
                            //Ficha
                            var cellidficha = row.CreateCell(0);cellidficha.SetCellValue(beneficiario.IdFicha);
                            var cellApellido = row.CreateCell(1);cellApellido.SetCellValue(beneficiario.Apellido);
                            var cellNombre = row.CreateCell(2);cellNombre.SetCellValue(beneficiario.Nombre);
                            var cellDni = row.CreateCell(3);cellDni.SetCellValue(beneficiario.NumeroDocumento);
                            var cellrazonSocial = row.CreateCell(4); cellrazonSocial.SetCellValue(beneficiario.FichaPpp.IdEmpresa == null ? String.Empty : beneficiario.FichaPpp.Empresa.NombreEmpresa.ToString());
                            var cellCuit = row.CreateCell(5); cellCuit.SetCellValue(beneficiario.FichaPpp.IdEmpresa == null ? String.Empty : beneficiario.FichaPpp.Empresa.Cuit.ToString());
           
                            //Beneficiario
                            var cellIdestadoben = row.CreateCell(6); cellIdestadoben.SetCellValue(beneficiario.BeneficiarioFicha.IdEstado == null ? String.Empty : beneficiario.BeneficiarioFicha.IdEstado.ToString());
                            var cellEstadoben = row.CreateCell(7); cellEstadoben.SetCellValue(beneficiario.BeneficiarioFicha.NombreEstado == null ? "NO ES BENEFICIARIO" : beneficiario.BeneficiarioFicha.NombreEstado.ToString());

                            int cantConceptosPagados = _conceptorepositorio.GetConceptosByBeneficiario(beneficiario.BeneficiarioFicha.IdBeneficiarioExport ?? 0).Where(c => c.Año == DateTime.Now.Year).Count();
                            var cellPago = row.CreateCell(8);
                            if (cantConceptosPagados != 0)
                                cellPago.SetCellValue(cantConceptosPagados + "/" + "12");
                        }
                        #endregion
                        break;
                    case (int)Enums.Programas.Vat:
                        #region case (int)Enums.TipoFicha.Vat
                        foreach (var beneficiario in listaparaExcel)
                        {
                            i++;
                            var row = sheet.CreateRow(i);
                            //Ficha
                            var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.IdFicha);
                            var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Apellido);
                            var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Nombre);
                            var cellDni = row.CreateCell(3); cellDni.SetCellValue(beneficiario.NumeroDocumento);
                            var cellrazonSocial = row.CreateCell(4); cellrazonSocial.SetCellValue(beneficiario.FichaVat.Empresa.NombreEmpresa == null ? String.Empty : beneficiario.FichaVat.Empresa.NombreEmpresa.ToString());
                            var cellCuit = row.CreateCell(5); cellCuit.SetCellValue(beneficiario.FichaVat.Empresa.Cuit == null ? String.Empty : beneficiario.FichaVat.Empresa.Cuit.ToString());
           
                            //Beneficiario
                            var cellIdestadoben = row.CreateCell(6); cellIdestadoben.SetCellValue(beneficiario.BeneficiarioFicha.IdEstado == null ? String.Empty : beneficiario.BeneficiarioFicha.IdEstado.ToString());
                            var cellEstadoben = row.CreateCell(7); cellEstadoben.SetCellValue(beneficiario.BeneficiarioFicha.NombreEstado == null ? String.Empty : beneficiario.BeneficiarioFicha.NombreEstado.ToString());

                            int cantConceptosPagados = _conceptorepositorio.GetConceptosByBeneficiario(beneficiario.BeneficiarioFicha.IdBeneficiarioExport ?? 0).Where(c => c.Año == DateTime.Now.Year).Count();
                            var cellPago = row.CreateCell(8);
                            if (cantConceptosPagados != 0)
                                cellPago.SetCellValue(cantConceptosPagados + "/" + "12");
                        }
                        #endregion
                        break;
                    case (int)Enums.Programas.PppProf:
                        #region case (int)Enums.TipoFicha.PppProf
                        foreach (var beneficiario in listaparaExcel)
                        {
                            i++;
                            var row = sheet.CreateRow(i);
                            //Ficha
                            var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.IdFicha);
                            var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Apellido);
                            var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Nombre);
                            var cellDni = row.CreateCell(3); cellDni.SetCellValue(beneficiario.NumeroDocumento);
                            var cellrazonSocial = row.CreateCell(4); cellrazonSocial.SetCellValue(beneficiario.FichaPppp.Empresa.NombreEmpresa == null ? String.Empty : beneficiario.FichaPppp.Empresa.NombreEmpresa.ToString());
                            var cellCuit = row.CreateCell(5); cellCuit.SetCellValue(beneficiario.FichaPppp.Empresa.Cuit == null ? String.Empty : beneficiario.FichaPppp.Empresa.Cuit.ToString());
           
                            //Beneficiario
                            var cellIdestadoben = row.CreateCell(6); cellIdestadoben.SetCellValue(beneficiario.BeneficiarioFicha.IdEstado == null ? String.Empty : beneficiario.BeneficiarioFicha.IdEstado.ToString());
                            var cellEstadoben = row.CreateCell(7); cellEstadoben.SetCellValue(beneficiario.BeneficiarioFicha.NombreEstado == null ? String.Empty : beneficiario.BeneficiarioFicha.NombreEstado.ToString());

                            int cantConceptosPagados = _conceptorepositorio.GetConceptosByBeneficiario(beneficiario.BeneficiarioFicha.IdBeneficiarioExport ?? 0).Where(c => c.Año == DateTime.Now.Year).Count();
                            var cellPago = row.CreateCell(8);
                            if (cantConceptosPagados != 0)
                                cellPago.SetCellValue(cantConceptosPagados + "/" + "12");
                        }
                        #endregion
                        break;
                    case (int)Enums.Programas.ReconversionProductiva:
                        #region case (int)Enums.TipoFicha.ReconversioProductiva
                        foreach (var beneficiario in listaparaExcel)
                        {
                            i++;
                            var row = sheet.CreateRow(i);
                            //Ficha
                            var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.IdFicha);
                            var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Apellido);
                            var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Nombre);
                            var cellDni = row.CreateCell(3); cellDni.SetCellValue(beneficiario.NumeroDocumento);
                            var cellrazonSocial = row.CreateCell(4); cellrazonSocial.SetCellValue(beneficiario.FichaReconversion.Empresa.NombreEmpresa == null ? String.Empty : beneficiario.FichaReconversion.Empresa.NombreEmpresa.ToString());
                            var cellCuit = row.CreateCell(5); cellCuit.SetCellValue(beneficiario.FichaReconversion.Empresa.Cuit == null ? String.Empty : beneficiario.FichaReconversion.Empresa.Cuit.ToString());
           
                            //Beneficiario
                            var cellIdestadoben = row.CreateCell(6); cellIdestadoben.SetCellValue(beneficiario.BeneficiarioFicha.IdEstado == null ? String.Empty : beneficiario.BeneficiarioFicha.IdEstado.ToString());
                            var cellEstadoben = row.CreateCell(7); cellEstadoben.SetCellValue(beneficiario.BeneficiarioFicha.NombreEstado == null ? "NO ES BENEFICIARIO" : beneficiario.BeneficiarioFicha.NombreEstado.ToString());

                            int cantConceptosPagados = _conceptorepositorio.GetConceptosByBeneficiario(beneficiario.BeneficiarioFicha.IdBeneficiarioExport ?? 0).Where(c => c.Año == DateTime.Now.Year).Count();
                            var cellPago = row.CreateCell(8);
                            if (cantConceptosPagados != 0)
                                cellPago.SetCellValue(cantConceptosPagados + "/" + "12");
                        }
                        #endregion
                        break;
                    case (int)Enums.Programas.EfectoresSociales:
                        #region case (int)Enums.TipoFicha.EfectoresSociales
                        foreach (var beneficiario in listaparaExcel)
                        {
                            i++;
                            var row = sheet.CreateRow(i);
                            //Ficha
                            var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.IdFicha);
                            var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Apellido);
                            var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Nombre);
                            var cellDni = row.CreateCell(3); cellDni.SetCellValue(beneficiario.NumeroDocumento);
                            var cellrazonSocial = row.CreateCell(4); cellrazonSocial.SetCellValue(beneficiario.FichaEfectores.Empresa.NombreEmpresa == null ? String.Empty : beneficiario.FichaEfectores.Empresa.NombreEmpresa.ToString());
                            var cellCuit = row.CreateCell(5); cellCuit.SetCellValue(beneficiario.FichaEfectores.Empresa.Cuit == null ? String.Empty : beneficiario.FichaEfectores.Empresa.Cuit.ToString());
           
                            //Beneficiario
                            var cellIdestadoben = row.CreateCell(6); cellIdestadoben.SetCellValue(beneficiario.BeneficiarioFicha.IdEstado == null ? String.Empty : beneficiario.BeneficiarioFicha.IdEstado.ToString());
                            var cellEstadoben = row.CreateCell(7); cellEstadoben.SetCellValue(beneficiario.BeneficiarioFicha.NombreEstado == null ? String.Empty : beneficiario.BeneficiarioFicha.NombreEstado.ToString());
                            int cantConceptosPagados = _conceptorepositorio.GetConceptosByBeneficiario(beneficiario.BeneficiarioFicha.IdBeneficiarioExport ?? 0).Where(c => c.Año == DateTime.Now.Year).Count();
                           
                            var cellPago = row.CreateCell(8);
                            if (cantConceptosPagados != 0)
                                cellPago.SetCellValue(cantConceptosPagados + "/" + "12");
                        }
                        #endregion
                        break;
                }


            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        private static void CargarDepartamentos(IReporteDepLocProgEsta vista, IEnumerable<IDepartamento> listaDepartamentos)
        {
         
            vista.BusquedaDepartamentos.Combo.Add(new ComboItem { Id = -1, Description = "Seleccione un Departamento..." });
            foreach (var lDepartamento in listaDepartamentos)
            {
                vista.BusquedaDepartamentos.Combo.Add(new ComboItem
                {
                    Id = lDepartamento.IdDepartamento,
                    Description = lDepartamento.NombreDepartamento
                });
            }
        }

        private static void CargarLocalidad(IReporteDepLocProgEsta vista, IEnumerable<ILocalidad> listaLocalidades)
        {
            vista.BusquedaLocalidades.Combo.Add(new ComboItem { Id = -1, Description = "Seleccione una localidad..." });
            foreach (var lLocalidad in listaLocalidades)
            {
                vista.BusquedaLocalidades.Combo.Add(new ComboItem
                {
                    Id = lLocalidad.IdLocalidad,
                    Description = lLocalidad.NombreLocalidad
                });
            }
        }

        private void CargarPrograma(IReporteDepLocProgEsta vista, IEnumerable<IPrograma> listaProgramas)
        {
            vista.BusquedaProgramas.Combo.Add(new ComboItem
            {
                Id = 0,
                Description = "TODOS"

            });
            foreach (var item in listaProgramas)
            {
                if ((int)Enums.Programas.EfectoresSociales != item.IdPrograma)
                {
                    vista.BusquedaProgramas.Combo.Add(new ComboItem
                    {
                        Id = item.IdPrograma,
                        Description = item.NombrePrograma

                    });


                }
                else
                {
                        if (base.AccesoPrograma(Enums.Formulario.AbmBeneficiario))
                            vista.BusquedaProgramas.Combo.Add(new ComboItem
                            {
                                Id = item.IdPrograma,
                                Description = item.NombrePrograma

                            });
                    
                }

            }



        }

        // 08/04/2013 - DI CAMPLI LEANDRO - AÑADIR NUEVO FILTRO POR ESTADO DE BENEFICIARIO
        private static void CargarEstadosBeneficiario(IReporteDepLocProgEsta vista, IEnumerable<IEstadoBeneficiario> listaEstado)
        {
            vista.EstadoBenef.Combo.Add(new ComboItem
            {
                Id = 0,
                Description = "TODOS"

            });
            foreach (var lEstado in listaEstado)
            {

                vista.EstadoBenef.Combo.Add(new ComboItem { Id = lEstado.Id_Estado_Beneficiario, Description = lEstado.Estado_Beneficiario });
            }
        }


        public IAsistenciaRptVista GetIndexAsistenciasRpt()
        {
            IAsistenciaRptVista vista = new AsistenciaRptVista();

            IList<IAsistenciaRpt> lista = new List<IAsistenciaRpt>();

            var pager = new Pager(0, Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormAsistenciaRpt", _aut.GetUrl("IndexPagerAsistenciaRpt", "Reportes"));

            vista.Pager = pager;

            vista.ListaAsistencias = lista;

            Periodo pp = new Periodo();
            DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
            vista.cboperiodos.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione el Período" });

                for (int i = 1; i < 13; i++)
                {
                    pp.idperiodo = i;
                    pp.N_Periodo = "Período de " + formatoFecha.GetMonthName(i);


                    vista.cboperiodos.Combo.Add(new ComboItem { Id = pp.idperiodo, Description = pp.N_Periodo });
                }


            

           //Cargar combo estados beneficiario
                CargarEstadosBeneficiarioforIndex(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());

                var lstProgramas = _programaRepositorio.GetProgramas();

                // 10/06/2013 - DI CAMPLI LEANDRO - CARGAN LOS PROGRAMAS PARA LAS ETAPAS
                CargarProgramaEtapa(vista, lstProgramas);

            return vista;
        }


        public IAsistenciaRptVista GetAsistenciasRpt(string mesPeriodo, int idCurso, int idEscuela, int idEstadoBenf, int idEtapa)
        {
            IAsistenciaRptVista vista = new AsistenciaRptVista();


            IList<IAsistenciaRpt> lAsistenciasRpt;

                lAsistenciasRpt = _asistenciaRepositorio.GetAsistenciasRpt(mesPeriodo, idCurso, idEscuela, idEstadoBenf, idEtapa).ToList();



            int cantreg = 0;

            cantreg = lAsistenciasRpt.Count();

            var pager = new Pager(cantreg,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormAsistenciaRpt", _aut.GetUrl("IndexPagerAsistenciaRpt", "Reportes"));

            vista.Pager = pager;

            vista.ListaAsistencias = lAsistenciasRpt.Skip(pager.Skip).Take(pager.PageSize).ToList();
            //Cargar estados beneficiarios
            //CargarEstadosFichas(vista, _estadoFichaRepositorio.GetEstadosFicha());
            Periodo pp = new Periodo();
            DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
            vista.cboperiodos.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione el Período" });

            for (int i = 1; i < 13; i++)
            {
                pp.idperiodo = i;
                pp.N_Periodo = "Período de " + formatoFecha.GetMonthName(i);


                vista.cboperiodos.Combo.Add(new ComboItem { Id = pp.idperiodo, Description = pp.N_Periodo });
            }
            vista.cboperiodos.Selected = mesPeriodo;
            //Cargar combo estados beneficiario
            CargarEstadosBeneficiarioforIndex(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());
            vista.EstadosBeneficiarios.Selected = Convert.ToString(idEstadoBenf);
            var lstProgramas = _programaRepositorio.GetProgramas();
            CargarProgramaEtapa(vista, lstProgramas);

            return vista;
        }

        public IAsistenciaRptVista GetAsistenciasRpt(IPager pPager, string mesPeriodo, int idCurso, int idEscuela, int idEstadoBenf, int idEtapa)
        {
            IAsistenciaRptVista vista = new AsistenciaRptVista();


            int cantreg = 0;


            vista.ListaAsistencias = _asistenciaRepositorio.GetAsistenciasRpt(mesPeriodo, idCurso, idEscuela, idEstadoBenf, idEtapa);
            cantreg = vista.ListaAsistencias.Count();


            var pager = new Pager(cantreg,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormAsistenciaRpt", _aut.GetUrl("IndexPagerAsistenciaRpt", "Reportes"));

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



            vista.ListaAsistencias = vista.ListaAsistencias.Skip(vista.Pager.Skip).Take(vista.Pager.PageSize).ToList();
            vista.ID_CURSO = idCurso;
            //vista.ID_ESCUELA = idEscuela;
            //vista.nombre_busqueda = nombre;
            //vista. = apellido;

            //debo cargar los estados de beneficiario
            //CargarEstadosFichas(vista, _estadoFichaRepositorio.GetEstadosFicha());

           

            //vista.ComboTipoFicha.Selected = tipoficha.ToString()
            Periodo pp = new Periodo();
            DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
            vista.cboperiodos.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione el Período" });

            for (int i = 1; i < 13; i++)
            {
                pp.idperiodo = i;
                pp.N_Periodo = "Período de " + formatoFecha.GetMonthName(i);


                vista.cboperiodos.Combo.Add(new ComboItem { Id = pp.idperiodo, Description = pp.N_Periodo });
            }

            vista.cboperiodos.Selected = mesPeriodo;
            //Cargar combo estados beneficiario
            CargarEstadosBeneficiarioforIndex(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());
            vista.EstadosBeneficiarios.Selected = Convert.ToString(idEstadoBenf);
            var lstProgramas = _programaRepositorio.GetProgramas();
            CargarProgramaEtapa(vista, lstProgramas);

            return vista;
        }

        private static void CargarEstadosBeneficiarioforIndex(IAsistenciaRptVista vista, IEnumerable<IEstadoBeneficiario> listaEstado)
        {
            vista.EstadosBeneficiarios.Combo.Add(new ComboItem
            {
                Id = 0,
                Description = "TODOS"

            });
            foreach (var lEstado in listaEstado)
            {

                vista.EstadosBeneficiarios.Combo.Add(new ComboItem { Id = lEstado.Id_Estado_Beneficiario, Description = lEstado.Estado_Beneficiario });
            }
        }

        public byte[] ExportarAsistencias(string mesPeriodo, int idCurso, int idEscuela, int idEstadoBenf, int idEtapa)
        {
            var path = String.Empty;


            path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateAsistencias.xls");

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Asistencias");

            List<IAsistenciaRpt> listaparaExcel = null;

            listaparaExcel = _asistenciaRepositorio.GetAsistenciasRpt(mesPeriodo, idCurso, idEscuela, idEstadoBenf, idEtapa).ToList();


            var i = 0;


                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.ID_FICHA);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.APELLIDO);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.NOMBRE);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.CUIL);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.NUMERO_DOCUMENTO);
                        var cellidEstadoFicha = row.CreateCell(5); cellidEstadoFicha.SetCellValue(beneficiario.ID_EST_FIC == null ?
                            String.Empty : beneficiario.ID_EST_FIC.ToString());
                        var cellN_ESTADO_FICHA = row.CreateCell(6); cellN_ESTADO_FICHA.SetCellValue(beneficiario.N_ESTADO_FICHA);
                        var cellID_BENEFICIARIO = row.CreateCell(7); cellID_BENEFICIARIO.SetCellValue(beneficiario.ID_BENEFICIARIO == null ?
                            String.Empty : beneficiario.ID_BENEFICIARIO.ToString());
                        var cellBEN_ID_ESTADO = row.CreateCell(8); cellBEN_ID_ESTADO.SetCellValue(beneficiario.BEN_ID_ESTADO == null ?
                            String.Empty : beneficiario.BEN_ID_ESTADO.ToString());
                        var cellBEN_N_ESTADO = row.CreateCell(9); cellBEN_N_ESTADO.SetCellValue(beneficiario.BEN_N_ESTADO);
                        var cellBEN_FEC_INICIO = row.CreateCell(10); cellBEN_FEC_INICIO.SetCellValue(beneficiario.BEN_FEC_INICIO == null ? String.Empty
                            : beneficiario.BEN_FEC_INICIO.Value.ToShortDateString());
                        var cellBEN_FEC_BAJA = row.CreateCell(11); cellBEN_FEC_BAJA.SetCellValue(beneficiario.BEN_FEC_BAJA == null ? String.Empty
                            : beneficiario.BEN_FEC_BAJA.Value.ToShortDateString());
                        var cellESC_ID_ESCUELA = row.CreateCell(12); cellESC_ID_ESCUELA.SetCellValue(beneficiario.ESC_ID_ESCUELA == null ?
                            String.Empty : beneficiario.ESC_ID_ESCUELA.ToString());
                        var cellN_ESCUELA = row.CreateCell(13); cellN_ESCUELA.SetCellValue(beneficiario.N_ESCUELA);
                        var cellESC_CALLE = row.CreateCell(14); cellESC_CALLE.SetCellValue(beneficiario.ESC_CALLE);
                        var cellESC_NUMERO = row.CreateCell(15); cellESC_NUMERO.SetCellValue(beneficiario.ESC_NUMERO);
                        var cellESC_BARRIO = row.CreateCell(16); cellESC_BARRIO.SetCellValue(beneficiario.ESC_BARRIO);
                        var cellESC_ID_LOCALIDAD = row.CreateCell(17); cellESC_ID_LOCALIDAD.SetCellValue(beneficiario.ESC_ID_LOCALIDAD == null ?
                            String.Empty : beneficiario.ESC_ID_LOCALIDAD.ToString());
                        var cellESC_N_LOCALIDAD = row.CreateCell(18); cellESC_N_LOCALIDAD.SetCellValue(beneficiario.ESC_N_LOCALIDAD);
                        var cellCOO_APE = row.CreateCell(19); cellCOO_APE.SetCellValue(beneficiario.COO_APE);
                        var cellCOO_NOM = row.CreateCell(20); cellCOO_NOM.SetCellValue(beneficiario.COO_NOM);
                        var cellCOO_DNI = row.CreateCell(21); cellCOO_DNI.SetCellValue(beneficiario.COO_DNI);
                        var cellENC_APE = row.CreateCell(22); cellENC_APE.SetCellValue(beneficiario.ENC_APE);
                        var cellENC_NOM = row.CreateCell(23); cellENC_NOM.SetCellValue(beneficiario.ENC_NOM);
                        var cellENC_DNI = row.CreateCell(24); cellENC_DNI.SetCellValue(beneficiario.ENC_DNI);

                        var cellCUR_ID_CURSO = row.CreateCell(25); cellCUR_ID_CURSO.SetCellValue(beneficiario.CUR_ID_CURSO == null ?
                            String.Empty : beneficiario.CUR_ID_CURSO.ToString());
                        var cellN_CURSO = row.CreateCell(26); cellN_CURSO.SetCellValue(beneficiario.N_CURSO);
                        var cellNRO_COND_CURSO = row.CreateCell(27); cellNRO_COND_CURSO.SetCellValue(beneficiario.NRO_COND_CURSO == null ?
                            String.Empty : beneficiario.NRO_COND_CURSO.ToString());
                        var cellPERIODO = row.CreateCell(28); cellPERIODO.SetCellValue(beneficiario.PERIODO);
                        var cellPORC_ASISTENCIA = row.CreateCell(29); cellPORC_ASISTENCIA.SetCellValue(beneficiario.PORC_ASISTENCIA == null ?
                            String.Empty : beneficiario.PORC_ASISTENCIA.ToString());
                        var cellID_ETAPA = row.CreateCell(30); cellID_ETAPA.SetCellValue(beneficiario.ID_ETAPA == null ?
                            String.Empty : beneficiario.ID_ETAPA.ToString());



            }


            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        private void CargarProgramaEtapa(IAsistenciaRptVista vista, IEnumerable<IPrograma> listaProgramas)
        {
            vista.EtapaProgramas.Combo.Add(new ComboItem
            {
                Id = 0,
                Description = "TODOS"

            });


            foreach (var item in listaProgramas)
            {
                if ((int)Enums.Programas.EfectoresSociales != item.IdPrograma)
                {
                    vista.EtapaProgramas.Combo.Add(new ComboItem
                    {
                        Id = item.IdPrograma,
                        Description = item.NombrePrograma

                    });
                }
                else
                {
                    if (base.AccesoPrograma(Enums.Formulario.AbmBeneficiario))
                        vista.EtapaProgramas.Combo.Add(new ComboItem
                        {
                            Id = item.IdPrograma,
                            Description = item.NombrePrograma

                        });
                }
            }



        }
    }

}
