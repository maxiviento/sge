using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface ISucursalCoberturaVista
    {
        int IdTablaBcoCba { get; set; }
        int IdSucursal { get; set; }
        IComboBox Sucursales { get; set; }
        int IdLocalidad { get; set; }
        IComboBox Localidades { get; set; }
        string Accion { get; set; }
    }
}
