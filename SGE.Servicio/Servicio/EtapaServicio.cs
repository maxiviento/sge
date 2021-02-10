using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using NPOI.HSSF.UserModel;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;
using SGE.Servicio.Comun;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using IError = SGE.Servicio.Comun.IError;
using Color = iTextSharp.text.BaseColor;
using FontPdf = iTextSharp.text.Font;
using Rectangle = iTextSharp.text.Rectangle;

using System.Text;

namespace SGE.Servicio.Servicio
{
    public class EtapaServicio
    {
        private readonly EtapaRepositorio _etaparepositorio;
        private readonly ProgramaRepositorio _programarepositorio;
        private readonly FichaRepositorio _fichaRepositorio;

        public EtapaServicio()
        {
            _etaparepositorio = new EtapaRepositorio();
            _programarepositorio = new ProgramaRepositorio();
            _fichaRepositorio = new FichaRepositorio();
        
        }

        public EtapaVista getEtapasIndex()
        {
            EtapaVista vista = new EtapaVista();
            
            //vista.cantProcesados = 0;
            IList<IFicha> fichas = new List<IFicha>();
            vista.Fichas = fichas;
            var lista = _programarepositorio.GetProgramas();
            CargarPrograma(vista, lista);
            return vista;
        }

        public EtapasVista getEtapasAsociar()
        {
            EtapasVista vista = new EtapasVista();

            //vista.cantProcesados = 0;
            IList<IFicha> fichas = new List<IFicha>();
            vista.Fichas = fichas;
            var lista = _programarepositorio.GetProgramas();
            CargarSoloProgramas(vista, lista);
            return vista;
        }



        public EtapaVista getEtapas(string ejercicio, int idprograma)
        {

            EtapaVista vista = new EtapaVista();

            IList<IFicha> fichas = new List<IFicha>();
            vista.Fichas = fichas;

            vista.listEtapas = _etaparepositorio.getEtapas(ejercicio, idprograma);
            var lista = _programarepositorio.GetProgramas();
            CargarPrograma(vista, lista);
            vista.Programas.Selected = idprograma.ToString();
            CargarSoloEtpas(vista, vista.listEtapas);
            return vista;

        
        }


        public EtapasVista getEtapas(EtapasVista model)
        {

            //EtapaVista vista = new EtapaVista();

            IList<IFicha> fichas = new List<IFicha>();
            model.Fichas = fichas;
            int idprograma = Convert.ToInt32(model.Programas.Selected);
            model.listEtapas = _etaparepositorio.getEtapas(model.ejerc, Convert.ToInt32(model.Programas.Selected));
            var lista = _programarepositorio.GetProgramas();
            CargarSoloProgramas(model, lista);
            model.Programas.Selected = idprograma.ToString();
            CargarSoloEtpas(model, model.listEtapas);
            return model;


        }




        private void CargarPrograma(EtapaVista vista, IEnumerable<IPrograma> listaProgramas)
        {
            vista.Programas.Combo.Add(new ComboItem
            {
                Id = 0,
                Description = "TODOS"

            });


            foreach (var item in listaProgramas)
            {
                //if ((int)Enums.Programas.EfectoresSociales != item.IdPrograma)
                //{
                    vista.Programas.Combo.Add(new ComboItem
                    {
                        Id = item.IdPrograma,
                        Description = item.NombrePrograma

                    });
                //}
                //else
                //{
                //    if (base.AccesoPrograma(Enums.Formulario.AbmBeneficiario))
                //        vista.Programas.Combo.Add(new ComboItem
                //        {
                //            Id = item.IdPrograma,
                //            Description = item.NombrePrograma

                //        });
                //}
            }



        }


        private void CargarPrograma(EtapasVista vista, IEnumerable<IPrograma> listaProgramas)
        {
            vista.Programas.Combo.Add(new ComboItem
            {
                Id = 0,
                Description = "TODOS"

            });


            foreach (var item in listaProgramas)
            {
                //if ((int)Enums.Programas.EfectoresSociales != item.IdPrograma)
                //{
                vista.Programas.Combo.Add(new ComboItem
                {
                    Id = item.IdPrograma,
                    Description = item.NombrePrograma

                });
                //}
                //else
                //{
                //    if (base.AccesoPrograma(Enums.Formulario.AbmBeneficiario))
                //        vista.Programas.Combo.Add(new ComboItem
                //        {
                //            Id = item.IdPrograma,
                //            Description = item.NombrePrograma

                //        });
                //}
            }



        }


        private void CargarSoloProgramas(EtapaVista vista, IEnumerable<IPrograma> listaProgramas)
        {

            foreach (var item in listaProgramas)
            {
                //if ((int)Enums.Programas.EfectoresSociales != item.IdPrograma)
                //{
                vista.Programas.Combo.Add(new ComboItem
                {
                    Id = item.IdPrograma,
                    Description = item.NombrePrograma

                });
                //}
                //else
                //{
                //    if (base.AccesoPrograma(Enums.Formulario.AbmBeneficiario))
                //        vista.Programas.Combo.Add(new ComboItem
                //        {
                //            Id = item.IdPrograma,
                //            Description = item.NombrePrograma

                //        });
                //}
            }



        }



        private void CargarSoloProgramas(EtapasVista vista, IEnumerable<IPrograma> listaProgramas)
        {

            foreach (var item in listaProgramas)
            {
                //if ((int)Enums.Programas.EfectoresSociales != item.IdPrograma)
                //{
                vista.Programas.Combo.Add(new ComboItem
                {
                    Id = item.IdPrograma,
                    Description = item.NombrePrograma

                });
                //}
                //else
                //{
                //    if (base.AccesoPrograma(Enums.Formulario.AbmBeneficiario))
                //        vista.Programas.Combo.Add(new ComboItem
                //        {
                //            Id = item.IdPrograma,
                //            Description = item.NombrePrograma

                //        });
                //}
            }



        }




        public EtapaVista GetEtapa(int idEtapa, string accion)
        {
            EtapaVista vista = new EtapaVista();
            IEtapa etapa;
            vista.Accion = accion;

            IList<IFicha> fichas = new List<IFicha>();
            vista.Fichas = fichas;


            if (accion != "Agregar")
            {
                etapa = _etaparepositorio.getEtapa(idEtapa);

                vista.ID_ETAPA = etapa.ID_ETAPA;
                vista.N_ETAPA = etapa.N_ETAPA;
                vista.ID_PROGRAMA = etapa.ID_PROGRAMA;
                vista.MONTO_ETAPA = etapa.MONTO_ETAPA;
                vista.EJERCICIO = etapa.EJERCICIO;

                vista.FEC_INCIO = etapa.FEC_INCIO;
                vista.FEC_FIN = etapa.FEC_FIN;
                vista.ID_USR_SIST = etapa.ID_USR_SIST;
                vista.FEC_SIST = etapa.FEC_SIST;
                vista.ID_USR_MODIF = etapa.ID_USR_MODIF;
                vista.FEC_MODIF = etapa.FEC_MODIF;
                
                //if (vista.IdInstitucion != null)
                var lista = _programarepositorio.GetProgramas();
                CargarPrograma(vista, lista);
                
            }
            else
            {
                etapa = new Etapa();

                vista.N_ETAPA = etapa.N_ETAPA;
                vista.ID_PROGRAMA = 0;
                CargarSoloProgramas(vista, _programarepositorio.GetProgramas());
            }


            vista.Programas.Selected = vista.ID_PROGRAMA.ToString();


            if (accion == "Ver" || accion == "Eliminar")
            {
                vista.Programas.Enabled = false;
                
            }

            return vista;
        }


        public EtapaVista GetEtapa(EtapaVista vista)
        {

            IEtapa etapa;


            IList<IFicha> fichas = new List<IFicha>();
            vista.Fichas = fichas;
            etapa = _etaparepositorio.getEtapa(Convert.ToInt32(vista.etapas.Selected));

                vista.ID_ETAPA = etapa.ID_ETAPA;
                vista.N_ETAPA = etapa.N_ETAPA;
                vista.ID_PROGRAMA = etapa.ID_PROGRAMA;
                vista.MONTO_ETAPA = etapa.MONTO_ETAPA;
                vista.EJERCICIO = etapa.EJERCICIO;

                vista.FEC_INCIO = etapa.FEC_INCIO;
                vista.FEC_FIN = etapa.FEC_FIN;
                vista.ID_USR_SIST = etapa.ID_USR_SIST;
                vista.FEC_SIST = etapa.FEC_SIST;
                vista.ID_USR_MODIF = etapa.ID_USR_MODIF;
                vista.FEC_MODIF = etapa.FEC_MODIF;

                //if (vista.IdInstitucion != null)
                vista.listEtapas = _etaparepositorio.getEtapas(vista.ejerc, Convert.ToInt32(vista.programaSel));
                vista.Programas = new ComboBox();
                var lista = _programarepositorio.GetProgramas();
                CargarSoloProgramas(vista, lista);
                CargarSoloEtpas(vista, vista.listEtapas);


            vista.Programas.Selected = vista.ID_PROGRAMA.ToString();


            return vista;
        }



        public EtapasVista GetEtapa(EtapasVista vista)
        {

            IEtapa etapa;


            IList<IFicha> fichas = new List<IFicha>();
            vista.Fichas = fichas;
            if (vista.etapas.Selected != null)
            {
                etapa = _etaparepositorio.getEtapa(Convert.ToInt32(vista.etapas.Selected));
            }
            else 
            {
                etapa = _etaparepositorio.getEtapa(vista.ID_ETAPA);
            }
            vista.ID_ETAPA = etapa.ID_ETAPA;
            vista.N_ETAPA = etapa.N_ETAPA;
            vista.ID_PROGRAMA = etapa.ID_PROGRAMA;
            vista.MONTO_ETAPA = etapa.MONTO_ETAPA;
            vista.EJERCICIO = etapa.EJERCICIO;

            vista.FEC_INCIO = etapa.FEC_INCIO;
            vista.FEC_FIN = etapa.FEC_FIN;
            vista.ID_USR_SIST = etapa.ID_USR_SIST;
            vista.FEC_SIST = etapa.FEC_SIST;
            vista.ID_USR_MODIF = etapa.ID_USR_MODIF;
            vista.FEC_MODIF = etapa.FEC_MODIF;

            //if (vista.IdInstitucion != null)
            vista.listEtapas = _etaparepositorio.getEtapas(vista.ejerc, Convert.ToInt32(vista.programaSel));
            vista.Programas = new ComboBox();
            var lista = _programarepositorio.GetProgramas();
            CargarSoloProgramas(vista, lista);
            CargarSoloEtpas(vista, vista.listEtapas);


            vista.Programas.Selected = vista.ID_PROGRAMA.ToString();


            return vista;
        }


        public int AddEtapa(EtapaVista vista)
        {
            try
            {
                IEtapa etapa = new Etapa();
                etapa.ID_ETAPA = vista.ID_ETAPA;
                etapa.ID_PROGRAMA =Convert.ToInt32(vista.Programas.Selected);
                etapa.EJERCICIO = vista.EJERCICIO;
                etapa.N_ETAPA = vista.N_ETAPA;
                etapa.FEC_INCIO = vista.FEC_INCIO;
                etapa.FEC_FIN = vista.FEC_FIN;
                etapa.MONTO_ETAPA = vista.MONTO_ETAPA;
                
                _etaparepositorio.addEtapa(etapa);
                return 0;
            }
            catch(Exception ex)
            {
                return -1;
            }
            
        
        }

        public int udpEtapa(EtapaVista vista)
        {
            int resu = 0;
            Etapa etapa = new Etapa();
            etapa.ID_ETAPA = vista.ID_ETAPA;
            etapa.ID_PROGRAMA = vista.ID_PROGRAMA;
            etapa.N_ETAPA = vista.N_ETAPA;
            etapa.EJERCICIO = vista.EJERCICIO;
            etapa.FEC_INCIO = vista.FEC_INCIO;
            etapa.FEC_FIN = vista.FEC_FIN;
            etapa.MONTO_ETAPA = vista.MONTO_ETAPA;

            resu = _etaparepositorio.udpEtapa(etapa);

            return resu;
            

        }


        private void CargarSoloEtpas(EtapaVista vista, IEnumerable<IEtapa> listaEtapas)
        {

            foreach (var item in listaEtapas)
            {
                //if ((int)Enums.Programas.EfectoresSociales != item.IdPrograma)
                //{
                vista.etapas.Combo.Add(new ComboItem
                {
                    Id = item.ID_ETAPA,
                    Description = item.N_ETAPA

                });

            }

        }

        private void CargarSoloEtpas(EtapasVista vista, IEnumerable<IEtapa> listaEtapas)
        {

            foreach (var item in listaEtapas)
            {
                //if ((int)Enums.Programas.EfectoresSociales != item.IdPrograma)
                //{
                vista.etapas.Combo.Add(new ComboItem
                {
                    Id = item.ID_ETAPA,
                    Description = item.N_ETAPA

                });

            }

        }


        public EtapasVista GetEtapasAsignar(EtapasVista vista)
        {
                IEtapa etapa= _etaparepositorio.getEtapa(Convert.ToInt32(vista.etapas.Combo.FirstOrDefault().Id));

    
                vista.N_ETAPA = etapa.N_ETAPA;
                vista.ID_PROGRAMA = etapa.ID_PROGRAMA;
                vista.EJERCICIO = etapa.EJERCICIO;
                vista.FEC_INCIO = etapa.FEC_INCIO;
                vista.FEC_FIN = etapa.FEC_FIN;
                vista.MONTO_ETAPA = etapa.MONTO_ETAPA;


            return vista;
        }



        public EtapasVista SubirArchivoFichas(HttpPostedFileBase archivo, EtapasVista model)
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
                            //model.Importado = true;
                            model.ProcesarExcel = true;
                            //model.Mensaje = "El archivo \"" + fileName + "\" fue subido al servidor correctamente. Confirme la acción de Procesar si lo cree conveniente.";
                            model.ExcelNombre = nombre;
                        }
                        else
                        {
                            //model.Importado = false;
                            model.ProcesarExcel = false;
                            //model.Mensaje = "El archivo \"" + fileName + "\" fue subido al servidor correctamente. Pero el Archivo no tiene el Formato correcto.";
                            model.ExcelNombre = null;
                            File.Delete(path);
                        }
                    }
                }
            }
            else
            {
                model.ProcesarExcel = true;
                // model.Mensaje = "El archivo no pudo ser subido al servidor. Vuelva a intentar.";
            }
            IList<IFicha> lista = new List<IFicha>();
            model.Fichas = lista;

            return model;
        }



        public IFichasVista CargarFichasEtapaXLS(string ExcelUrl)
        {

            IFichasVista vista = new FichasVista();
            //********************15/02/2013 - DI CAMPLI LEANDRO -******************************
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

            //var pager = new Pager(0, Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
            //                      "FormIndexFichas", _aut.GetUrl("IndexPager", "Fichas"));
            //vista.pager = pager;
            vista.ProcesarExcel = true;
            vista.Fichas = _fichaRepositorio.GetFichas(idBeneficiariosExport);

            return vista;
        }


        public int udpEtapaFichas(EtapasVista model)
        {
            int resu = 0;

           resu = _etaparepositorio.udpEtapaFichas(model.Fichas, model.ID_ETAPA);




           return resu;
        }

        public EtapasVista getFichas(EtapasVista model)
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


            model.Fichas = _fichaRepositorio.GetFichas(idBeneficiariosExport);
            return model;
        }



        public ComboBox getEtapasCombo(int idprograma, string ejercicio)
        {

            IList<IEtapa> etapas;
            ComboBox combo = new ComboBox();
            etapas = _etaparepositorio.getEtapas(ejercicio, idprograma);
            var lista = _programarepositorio.GetProgramas();
            foreach (var item in etapas)
            {
                //if ((int)Enums.Programas.EfectoresSociales != item.IdPrograma)
                //{
                combo.Combo.Add(new ComboItem
                {
                    Id = item.ID_ETAPA,
                    Description = item.N_ETAPA

                });

            }

            return combo;
        }



        public IList<IEtapa> getEtapasPrograma(int idprograma)
        {

            IList<IEtapa> etapas;

            etapas = _etaparepositorio.getEtapas("", idprograma).OrderByDescending(c => c.ID_ETAPA).Take((int)10).ToList();
            var lista = _programarepositorio.GetProgramas();


            return etapas;
        }

    }
}
