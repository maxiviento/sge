using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class ReporteDepLocProgEsta : IReporteDepLocProgEsta
    {
        public ReporteDepLocProgEsta()
        {
            BusquedaLocalidades = new ComboBox();
            BusquedaDepartamentos = new ComboBox();
            BusquedaProgramas = new ComboBox();
            ContenidoReporte = new List<IContenidoDepLocProgEsta>();
            Estado = "0";
            EstadoBenef = new ComboBox();
            ComboTiposPpp = new ComboBox();

        }

        public IComboBox BusquedaLocalidades { get; set; }
        public IComboBox BusquedaDepartamentos { get; set; }
        public IComboBox BusquedaProgramas { get; set; }
        public IList<IContenidoDepLocProgEsta> ContenidoReporte { get; set; }
        public IPager Pager { get; set; }
        public string Estado { get; set; }
        public IComboBox EstadoBenef { get; set;} // 08/04/2013 - DI CAMPLI LEANDRO - NUEVO FILTRO PARA EL REPORTE
        public IComboBox ComboTiposPpp { get; set; }
    }

    public class ContenidoDepLocProgEsta : IContenidoDepLocProgEsta
    {
        public int idDepartamento { get; set; }
        public string Departamento { get; set; }
        public int idLocalidad { get; set; }
        public string Localidad { get; set; }
        public int idPrograma { get; set; }
        public string Programa { get; set; }
        public string Estado { get; set; }
        public int Cantidad { get; set; }
        public int idEstadoBenef { get; set; } // 08/04/2013 - DI CAMPLI LEANDRO - NUEVO FILTRO PARA EL REPORTE
    }
}
