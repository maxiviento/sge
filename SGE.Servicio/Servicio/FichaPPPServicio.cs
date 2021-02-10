using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using System.Transactions;
using EfficientlyLazy.Crypto;

namespace SGE.Servicio.Servicio
{
    public class FichaPPPServicio: IFichaPPPServicio
    {
        private readonly FichaPPPRepositorio _fichapppRepositorio;
        private readonly EmpresaRepositorio _empresaRepositorio;
        private readonly SedeRepositorio _sedeRepositorio;
        private readonly IAutenticacionServicio _aut;

        public FichaPPPServicio()
        {
            _fichapppRepositorio = new FichaPPPRepositorio();
            _empresaRepositorio = new EmpresaRepositorio();
            _sedeRepositorio = new SedeRepositorio();
            _aut = new AutenticacionServicio();
        }

        public IFichasPPPVista GetFichasPPP()
        {
            try
            {
                IFichasPPPVista vista = new FichasPPPVista();

                var pager = new Pager(_fichapppRepositorio.GetCountFichasPPP(),
                                      Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                      "FormIndexFichasPPP", _aut.GetUrl("IndexPager", "FichaPPP"));
                
                vista.Pager = pager;

                vista.FichasPPP = _fichapppRepositorio.GetFichasPPP(pager.Skip, pager.PageSize);              
                CargarEmpresas(vista, _empresaRepositorio.GetEmpresas());

                return vista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CargarEmpresas(IFichasPPPVista vista, IList<IEmpresa> listaEmpresas)
        {
            vista.Busqueda_Empresas.Combo.Add(new ComboItem() { Id = 0, Description = "Seleccione una empresa..." });
            foreach (var lEmpresa in listaEmpresas)
            {
                vista.Busqueda_Empresas.Combo.Add(new ComboItem()
                                                      {
                                                          Id = lEmpresa.IdEmpresa,
                                                          Description = lEmpresa.NombreEmpresa
                                                              
                                                      });
            }
        }

        public IFichasPPPVista GetFichasPPP(int id_empresa)
        {
            try
            {
                IFichasPPPVista vista = new FichasPPPVista();

                var pager = new Pager(_fichapppRepositorio.GetFichasByEmpresa(id_empresa).Count,
                                      Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                       "FormIndexFichasPPP", _aut.GetUrl("IndexPager", "FichaPPP"));

                vista.Pager = pager;

                vista.FichasPPP = _fichapppRepositorio.GetFichasPPP(id_empresa, pager.Skip, pager.PageSize);

                CargarEmpresas(vista, _empresaRepositorio.GetEmpresas());

                vista.Busqueda_Empresas.Selected = id_empresa.ToString();

                return vista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IFichaPPPVista GetFichaPPP(int id_fichappp, string accion)
        {
            try
            {
                IFichaPPPVista vista =  new FichaPPPVista();
                vista.Accion = accion;

                if (id_fichappp > 0)
                {
                    IFichaPPP objreturn = _fichapppRepositorio.GetFichaPPP(id_fichappp);

                    vista.Id_Ficha = objreturn.Id_Ficha;
                    vista.Id_Nivel_Escolaridad = objreturn.Id_Nivel_Escolaridad;
                    vista.Cursando=objreturn.Cursando;
                    vista.Finalizado=objreturn.Finalizado;
                    CargarEmpresas(vista, _empresaRepositorio.GetEmpresas());
                    vista.Empresas.Selected = objreturn.Id_Empresa.ToString();
                    vista.Modalidad = objreturn.Modalidad;
                    vista.Tareas = objreturn.Tareas;
                    vista.Desea_Term_Nivel = objreturn.Desea_Term_Nivel;
                    CargarSedes(vista, _sedeRepositorio.GetSedes());
                    vista.Sedes.Selected = objreturn.Id_Sede.ToString();

                    vista.Accion = accion;
                    if (accion.Equals("Modificar") || (accion.Equals("Agregar")))
                    {
                        vista.Empresas.Enabled = true;
                        vista.Sedes.Enabled = true;
                    }
                    else if (accion.Equals("Ver") || accion.Equals("Eliminar"))
                    {
                        vista.Empresas.Enabled = false;
                        vista.Sedes.Enabled = false;
                    }
                }

                return vista;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public IFichasPPPVista GetFichasPPP(Pager pPager, int Id, int id_empresa)
        {

            try
            {
                IFichasPPPVista vista = new FichasPPPVista();

                var pager = new Pager(_fichapppRepositorio.GetFichasByEmpresa(id_empresa).Count,
                                      Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                      "FormIndexFichasPPP", _aut.GetUrl("IndexPager", "FichaPPP"));

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

                vista.FichasPPP = _fichapppRepositorio.GetFichasPPP(id_empresa, vista.Pager.Skip, vista.Pager.PageSize);

                CargarEmpresas(vista, _empresaRepositorio.GetEmpresas());

                vista.Busqueda_Empresas.Selected = id_empresa.ToString();

               

                return vista;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CargarEmpresas(IFichaPPPVista vista, IList<IEmpresa> listaEmpresas)
        {
            vista.Empresas.Combo.Add(new ComboItem() { Id = 0, Description = "Seleccione una empresa..." });
            foreach (var lEmpresa in listaEmpresas)
            {
                vista.Empresas.Combo.Add(new ComboItem()
                {
                    Id = lEmpresa.IdEmpresa,
                    Description = lEmpresa.NombreEmpresa.Trim()
                });
            }
        }

        private void CargarSedes(IFichaPPPVista vista, IList<ISede> listaSedes)
        {
            vista.Sedes.Combo.Add(new ComboItem() { Id = 0, Description = "Seleccione una sede..." });
            foreach (var lSede in listaSedes)
            {
                vista.Sedes.Combo.Add(new ComboItem()
                {
                    Id = lSede.IdSede,
                    Description = lSede.NombreSede
                });
            }
        }
    }
}
