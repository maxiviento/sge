using System.Collections.Generic;
using SGE.Servicio.VistaInterfaces.Shared;
using SGE.Model.Entidades.Interfaces;
using System;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IBarrioVista
    {
        int ID_BARRIO { get; set; }
        string N_BARRIO { get; set; }
        int? ID_SECCIONAL { get; set; }
        int? ID_LOCALIDAD { get; set; }
        int? ID_USR_SIST { get; set; }
        DateTime? FEC_SIST { get; set; }
        int? ID_USR_MODIF { get; set; }
        DateTime? FEC_MODIF { get; set; }

        IComboBox Localidades { get; set; }
        IComboBox Seccionales { get; set; }
        string Accion { get; set; }
    }
}
