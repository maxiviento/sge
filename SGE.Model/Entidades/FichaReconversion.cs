using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class FichaReconversion : IFichaReconversion
    {
        public FichaReconversion()
        {
            Ficha = new Ficha();
            Empresa = new Empresa();
            Sede = new Sede();
            Proyecto = new Proyecto();
        }

        public int IdFicha { get; set; }
        public IFicha Ficha { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdSede { get; set; }
        public int? IdSubprograma { get; set; }
        public string Subprograma { get; set; }
        public int? IdProyecto { get; set; }
        public string AltaTemprana { get; set; }
        public int? Modalidad { get; set; }

        public DateTime? FechaInicioActividad { get; set; }
        public DateTime? FechaFinActividad { get; set; }
        public int? IdModalidadAfip { get; set; }
        public string ModalidadAfip { get; set; }
        public string AportesDeLaEmpresa { get; set; }
        public int? IdNivelEscolaridad { get; set; }
        public bool Cursando { get; set; }
        public bool Finalizado { get; set; }
        public bool DeseaTermNivel { get; set; }
        public string ApellidoTutor { get; set; }
        public string NombreTutor { get; set; }
        public string NroDocumentoTutor { get; set; }
        public string TelefonoTutor { get; set; }
        public string EmailTutor { get; set; }
        public string PuestoTutor { get; set; }
        public string DescripcionEmpresa { get; set; }
        public string DescripcionSede { get; set; }
        public string CuitEmpresa { get; set; }
        public IEmpresa Empresa { get; set; }
        public ISede Sede { get; set; }
        public IProyecto Proyecto { get; set; }

        public DateTime? FechaNotificacion { get; set; }
        public bool Notificado { get; set; }
        public IHorarioFicha HorariosFicha { get; set; }
    }
}
