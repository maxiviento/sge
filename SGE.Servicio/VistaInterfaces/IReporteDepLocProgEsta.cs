using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IReporteDepLocProgEsta
    {
        IComboBox BusquedaLocalidades { get; set; }
        IComboBox BusquedaDepartamentos { get; set; }
        IComboBox BusquedaProgramas { get; set; }
        IList<IContenidoDepLocProgEsta> ContenidoReporte { get; set; }
        IPager Pager { get; set; }
        string Estado { get; set; }
        IComboBox EstadoBenef { get; set;} // 08/04/2013 - DI CAMPLI LEANDRO - NUEVO FILTRO PARA EL REPORTE
        IComboBox ComboTiposPpp { get; set; }

    }

    public interface IContenidoDepLocProgEsta
    {
        int idDepartamento { get; set; }
        string Departamento { get; set; }
        int idLocalidad { get; set; }
        string Localidad { get; set; }
        int idPrograma { get; set; }
        string Programa { get; set; }
        string Estado { get; set; }
        int Cantidad { get; set; }
        int idEstadoBenef { get; set; } // 08/04/2013 - DI CAMPLI LEANDRO - NUEVO FILTRO PARA EL REPORTE
    }
}
