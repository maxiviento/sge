using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class ControlDatosVista : IControlDatosVista
    {
        public ControlDatosVista()
        {
            Programas = new ComboBox();
            etapas = new ComboBox();
            EtapaProgramas = new ComboBox();
        
        }
        public IList<IControlDatos> ListaControlDatos { get; set; }
        public IPager Pager { get; set; }


        // 02/06/2014 - DI CAMPLI LEANDRO - SE AÑADE EL FILTRO DE ETAPAS PARA BENEFICIARIOS
        public IComboBox etapas { get; set; }
        public int filtroEtapa { get; set; }
        public int programaSel { get; set; }
        public string filtroEjerc { get; set; }
        public string filtroNombre { get; set; }
        public IComboBox EtapaProgramas { get; set; }
        public string ejerc { get; set; }
        public string BuscarPorEtapa { get; set; }

        public IComboBox Programas { get; set; }
    }
}
