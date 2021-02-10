using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IFichaVAT
    {
        int IdFicha { get; set; }
        IFicha ficha { get; set; }
        int? IdNivelEscolaridad { get; set; }
        bool Cursando { get; set; }
        bool Finalizado { get; set; }
        int? IdEmpresa { get; set; }
        int? Modalidad { get; set; }
        string Tareas { get; set; }
        int? IdSede { get; set; }
        string DescripcionEmpresa { get; set; }
        string DescripcionSede { get; set; }
        string CuitEmpresa { get; set; }
        IEmpresa Empresa { get; set; }
        ISede Sede { get; set; }
        string AltaTemprana { get; set; }
        DateTime? FechaInicioActividad { get; set; }
        DateTime? FechaFinActividad { get; set; }
        int? IdModalidadAFIP { get; set; }
        string ModalidadAFIP { get; set; }
        short? AniosAportes { get; set; }
        DateTime? FechaNotificacion { get; set; }
        bool Notificado { get; set; }
    }
}
