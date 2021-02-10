using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;
using System;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IGrupoFamiliarVista
    {
        int idficha { get; set; }
        int idPersona { get; set; }
        string numero_documento { get; set; }
        string Nombres { get; set; }
        string Apellido { get; set; }
        int idVinculo { get; set; }
        string vinculo { get; set; }
        IComboBox vinculos { get; set; }
        IList<IRelacionFam> Familiares { get; set; }

        string Accion { get; set; }
        string AccionLlamada { get; set; }
    }
}
