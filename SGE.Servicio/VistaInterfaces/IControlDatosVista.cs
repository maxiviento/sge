using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IControlDatosVista
    {
        IList<IControlDatos> ListaControlDatos { get; set; }
        IPager Pager { get; set; }

        // 02/06/2014 - DI CAMPLI LEANDRO - SE AÑADE EL FILTRO DE ETAPAS PARA BENEFICIARIOS
        IComboBox etapas { get; set; }
        int filtroEtapa { get; set; }
        int programaSel { get; set; }
        string filtroEjerc { get; set; }
        string filtroNombre { get; set; }
        IComboBox EtapaProgramas { get; set; }
        string ejerc { get; set; }
        string BuscarPorEtapa { get; set; }

        IComboBox Programas { get; set; }
    }
}
