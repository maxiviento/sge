using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IFichaReconversion
    {
        int IdFicha { get; set; }
        IFicha Ficha { get; set; }
        int? IdEmpresa { get; set; }
        int? IdSede { get; set; }
        int? IdSubprograma { get; set; }
        string Subprograma { get; set; }
        int? IdProyecto { get; set; }
        string AltaTemprana { get; set; }
        int? Modalidad { get; set; }
        DateTime? FechaInicioActividad { get; set; }
        DateTime? FechaFinActividad { get; set; }
        int? IdModalidadAfip { get; set; }
        string ModalidadAfip { get; set; }
        string AportesDeLaEmpresa { get; set; }

        int? IdNivelEscolaridad { get; set; }
        bool Cursando { get; set; }
        bool Finalizado { get; set; }
        bool DeseaTermNivel { get; set; }

        string ApellidoTutor { get; set; }
        string NombreTutor { get; set; }
        string NroDocumentoTutor { get; set; }
        string TelefonoTutor { get; set; }
        string EmailTutor { get; set; }
        string PuestoTutor { get; set; }

        string DescripcionEmpresa { get; set; }
        string DescripcionSede { get; set; }
        string CuitEmpresa { get; set; }
        IEmpresa Empresa { get; set; }
        ISede Sede { get; set; }
        IProyecto Proyecto { get; set; }
        DateTime? FechaNotificacion { get; set; }
        bool Notificado { get; set; }

        IHorarioFicha HorariosFicha { get; set; }
    }
}
