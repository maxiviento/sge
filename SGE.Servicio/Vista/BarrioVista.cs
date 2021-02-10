using System;
using System.ComponentModel.DataAnnotations;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Entidades;


namespace SGE.Servicio.Vista
{
    public class BarrioVista : IBarrioVista
    {
        public BarrioVista()
        {
            Localidades = new ComboBox();
            Seccionales = new ComboBox();
        }
        public int ID_BARRIO { get; set; }
        [Required(ErrorMessage = "Ingrese el Nombre del barrio")]
        public string N_BARRIO { get; set; }
        public int? ID_SECCIONAL { get; set; }
        public int? ID_LOCALIDAD { get; set; }
        public int? ID_USR_SIST { get; set; }
        public DateTime? FEC_SIST { get; set; }
        public int? ID_USR_MODIF { get; set; }
        public DateTime? FEC_MODIF { get; set; }
         
        public IComboBox Localidades { get; set; }
        public IComboBox Seccionales { get; set; }
        public string Accion { get; set; }
    }
}
