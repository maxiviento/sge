using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class ReporteBecasAcademicas :IReporteBecasAcademicas
    {
        public ReporteBecasAcademicas()
        {
            BusquedaInstituciones = new ComboBox();
            BusquedaCarreras = new ComboBox();
            BusquedaProgramas = new ComboBox();
             BusquedaSectorProductivo = new ComboBox();
            ContenidoReporte = new List<IContenidoBecasAcademicas>();
            Estado = "0";
        }

        public IComboBox BusquedaInstituciones { get; set; }
        public IComboBox BusquedaCarreras { get; set; }
        public IComboBox BusquedaProgramas { get; set; }
        public IComboBox BusquedaSectorProductivo { get; set; }
        public IList<IContenidoBecasAcademicas> ContenidoReporte { get; set; }
        public IPager Pager { get; set; }
        public string Estado { get; set; }
    }

    public class ContenidoBecasAcademicas : IContenidoBecasAcademicas
    {
        public int IdInstitucion { get; set; }
        public string Institucion { get; set; }
        public int IdCarrera { get; set; }
        public string Carrera { get; set; }
        public int IdPrograma { get; set; }
        public string Programa { get; set; }
        public int IdSectorProductivo { get; set; }
        public string SectorProductivo { get; set; }
        public string Estado { get; set; }
        public int Cantidad { get; set; }
    }
}
