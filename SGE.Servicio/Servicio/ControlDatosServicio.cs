using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Model.Comun;
using SGE.Servicio.Comun;
using SGE.Model.Entidades;
using System.IO;
using System.Web;
using NPOI.HSSF.UserModel;

namespace SGE.Servicio.Servicio
{
    public class ControlDatosServicio :  BaseServicio, IControlDatosServicio
    {
        private readonly IControlDatosRepositorio _controldatosrepositorio;
        private readonly IAutenticacionServicio _aut;
        private readonly IProgramaRepositorio _programarepositorio;

        public ControlDatosServicio()
        {
            _controldatosrepositorio = new ControlDatosRepositorio();
            _aut = new AutenticacionServicio();
            _programarepositorio = new ProgramaRepositorio();
        }

        public IControlDatosVista GetControlDatos(int idPrograma, int idEtapa)
        {
            IControlDatosVista vista = new ControlDatosVista();
            var lista = _programarepositorio.GetProgramas();

            IList<IControlDatos> listadatos = _controldatosrepositorio.GetDatosErroneos(idPrograma, idEtapa);


            var pager = new Pager(listadatos.Count,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexControlDatos", _aut.GetUrl("IndexPager", "ControlDatos"));

            vista.Pager = pager;

            vista.ListaControlDatos = listadatos.Skip(pager.Skip).Take(pager.PageSize).ToList();

            CargarPrograma(vista, lista);
            CargarProgramaEtapa(vista, lista);

            return vista;
        }

        public IControlDatosVista GetControlDatos(IPager pager, int idPrograma, int idEtapa)
        {

            IControlDatosVista vista = new ControlDatosVista();

            var lista = _programarepositorio.GetProgramas();


            IList<IControlDatos> listadatos = _controldatosrepositorio.GetDatosErroneos(idPrograma, idEtapa);


            var searchpager = new Pager(listadatos.Count,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexControlDatos", _aut.GetUrl("IndexPager", "ControlDatos"));



            if (searchpager.TotalCount != pager.TotalCount)
            {
                pager.PageNumber = 1;
                pager.Skip = 0;
                vista.Pager = pager;
            }
            else
            {
                vista.Pager = pager;
            }

            vista.ListaControlDatos = listadatos.Skip(pager.Skip).Take(pager.PageSize).ToList();

            CargarPrograma(vista, lista);
            CargarProgramaEtapa(vista, lista);
            
            return vista;
        }


        public IControlDatosVista GetControlDatosIndex()
        {
            IControlDatosVista vista = new ControlDatosVista();
            var lista = _programarepositorio.GetProgramas();

            //IList<IControlDatos> listadatos =


            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexControlDatos", _aut.GetUrl("IndexPager", "ControlDatos"));

            vista.Pager = pager;

            vista.ListaControlDatos = new List<IControlDatos>();//listadatos.Skip(pager.Skip).Take(pager.PageSize).ToList();

            CargarPrograma(vista, lista);
            CargarProgramaEtapa(vista, lista);

            return vista;
        }


        private void CargarPrograma(IControlDatosVista vista, IEnumerable<IPrograma> listaProgramas)
        {
            vista.Programas.Combo.Add(new ComboItem
            {
                Id = 0,
                Description = "TODOS"

            });


            foreach (var item in listaProgramas)
            {
                if ((int)Enums.Programas.EfectoresSociales != item.IdPrograma)
                {
                    vista.Programas.Combo.Add(new ComboItem
                    {
                        Id = item.IdPrograma,
                        Description = item.NombrePrograma

                    });
                }
                else
                {
                    if (base.AccesoPrograma(Enums.Formulario.AbmBeneficiario))
                        vista.Programas.Combo.Add(new ComboItem
                        {
                            Id = item.IdPrograma,
                            Description = item.NombrePrograma

                        });
                }
            }



        }


        private void CargarProgramaEtapa(IControlDatosVista vista, IEnumerable<IPrograma> listaProgramas)
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



        public byte[] ExportControlDatos(IControlDatosVista model)
        {
            string path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateControlDatos.xls");

            var fs =
                new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Liquidacion");
            IList<IControlDatos> listadatos = _controldatosrepositorio.GetDatosErroneos(Convert.ToInt32(model.Programas.Selected), Convert.ToInt32(
                                                                              model.etapas.Selected));
            model.ListaControlDatos = listadatos;

            int i = 0;

            foreach (var reg in model.ListaControlDatos)
            {
                i++;

                var row = sheet.CreateRow(i);

                var celDocu = row.CreateCell(0);
                celDocu.SetCellValue(reg.Identificador1);

                var celApe = row.CreateCell(1);
                celApe.SetCellValue(reg.Identificador2);

                var celnombre = row.CreateCell(2);
                celnombre.SetCellValue(reg.Identificador3);

                var celPrograma = row.CreateCell(3);
                celPrograma.SetCellValue(reg.N_Programa);

                var celEtapa = row.CreateCell(4);
                celEtapa.SetCellValue(reg.Etapa);

                var celDescripcion = row.CreateCell(5);
                celDescripcion.SetCellValue(reg.Descripcion);

            }

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

    }
}
