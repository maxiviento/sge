using System;
using System.Configuration;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Servicio
{
    public class ConceptoServicio : IConceptoServicio
    {
        private readonly IConceptoRepositorio _conceptorepositorio;
        private readonly IAutenticacionServicio _aut;

        public ConceptoServicio()
        {
            _conceptorepositorio = new ConceptoRepositorio();
            _aut = new AutenticacionServicio();
        }

        public IConceptosVista GetConceptos()
        {
            IConceptosVista vista = new ConceptosVista();

            var pager = new Pager(_conceptorepositorio.GetCountConceptos(), Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormConceptos", _aut.GetUrl("IndexPager", "Conceptos"));

            vista.Pager = pager;

            vista.Conceptos = _conceptorepositorio.GetConceptos(pager.Skip, pager.PageSize);

            return vista;
        }

        public IConceptosVista GetIndex()
        {
            IConceptosVista vista = new ConceptosVista();

            var pager = new Pager(0, Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                "FormConceptos", _aut.GetUrl("IndexPager", "Conceptos"));

            vista.Pager = pager;

            return vista;
        }

        public IConceptosVista GetConceptos(IPager pager)
        {
            IConceptosVista vista = new ConceptosVista
                                        {
                                            Pager = pager,
                                            Conceptos = _conceptorepositorio.GetConceptos(pager.Skip, pager.PageSize)
                                        };

            return vista;
        }

        public IConceptoVista GetConcepto(int idConcepto, string accion)
        {
            IConceptoVista vista = new ConceptoVista();
            IConcepto objConcepto = _conceptorepositorio.GetConcepto(idConcepto);
                
            vista.IdConcepto = objConcepto.Id_Concepto;
            vista.Año = objConcepto.Año;
            vista.Mes = objConcepto.Mes;
            vista.Observacion = objConcepto.Observacion;
            vista.Accion = accion;

            return vista;
        }

        public IConceptoVista GetProximoConcepto()
        {
            IConceptoVista vista = new ConceptoVista();
            IConcepto objConcepto = _conceptorepositorio.GetProximoConcepto();
                
            vista.IdConcepto = objConcepto.Id_Concepto;
            vista.Año = objConcepto.Año;
            vista.Mes = objConcepto.Mes;
                
            return vista;
        }

        public IConceptosVista GetConceptos(short? año, short? mes, string observacion)
        {
            IConceptosVista vista = new ConceptosVista();

            var pager = new Pager(_conceptorepositorio.GetConceptos(año, mes, observacion).Count,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormConceptos", _aut.GetUrl("IndexPager", "Conceptos"));

            vista.Pager = pager;

            vista.Conceptos = _conceptorepositorio.GetConceptos(año, mes, observacion, pager.Skip, pager.PageSize);

            return vista;
        }

        public IConceptosVista GetConceptos(IPager pPager, short? año, short? mes, string observacion)
        {
            IConceptosVista vista = new ConceptosVista();

            var pager = new Pager(_conceptorepositorio.GetConceptos(año, mes, observacion).Count,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormConceptos", _aut.GetUrl("IndexPager", "Conceptos"));

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

            vista.Conceptos = _conceptorepositorio.GetConceptos(año, mes, observacion, vista.Pager.Skip, vista.Pager.PageSize);

            return vista;
        }

        public int AddConcepto(IConceptoVista modelo)
        {
            return _conceptorepositorio.AddConcepto(ConceptoVistaToDatos(modelo));
        }

        public bool UpdateConcepto(IConceptoVista modelo)
        {
            return _conceptorepositorio.UpdateConcepto(ConceptoVistaToDatos(modelo));
        }

        public IConceptosVista GetConceptosPagosBeneficiario(int idbeneficiario)
        {
            IConceptosVista vista = new ConceptosVista
                                        {
                                            Conceptos =
                                                _conceptorepositorio.GetConceptosPagosBeneficiario(idbeneficiario)
                                        };

            return vista;
        }

        private static IConcepto ConceptoVistaToDatos(IConceptoVista vista)
        {
            IConcepto concepto = new Concepto
            {
                Id_Concepto = vista.IdConcepto,
                Año = vista.Año ?? short.Parse(DateTime.Now.Year.ToString()),
                Mes = vista.Mes ?? short.Parse(DateTime.Now.Month.ToString()),
                Observacion = vista.Observacion
            };
            
            return concepto;
        }
    }
}
