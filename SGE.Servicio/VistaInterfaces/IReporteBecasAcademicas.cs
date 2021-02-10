using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IReporteBecasAcademicas
    {
        IComboBox BusquedaInstituciones { get; set; }
        IComboBox BusquedaCarreras { get; set; }
        IComboBox BusquedaProgramas { get; set; }
        IComboBox BusquedaSectorProductivo { get; set; }
        IList<IContenidoBecasAcademicas> ContenidoReporte { get; set; }
        IPager Pager { get; set; }
        string Estado { get; set; }
    }

    public interface IContenidoBecasAcademicas
    {
        int IdInstitucion { get; set; }
        string Institucion { get; set; }
        int IdCarrera { get; set; }
        string Carrera { get; set; }
        int IdPrograma { get; set; }
        string Programa { get; set; }
        int IdSectorProductivo { get; set; }
        string SectorProductivo { get; set; }
        string Estado { get; set; }
        int Cantidad { get; set; }
    }
}
