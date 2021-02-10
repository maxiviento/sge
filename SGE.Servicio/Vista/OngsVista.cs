using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using System.ComponentModel.DataAnnotations;
using System;

namespace SGE.Servicio.Vista
{
    public class OngsVista : IOngsVista
    {
        public IList<IOng> ongs { get; set; }
        public IPager Pager { get; set; }
        public int Id { get; set; }
        public string DescripcionBusca { get; set; }
        public int idNivel { get; set; }
    }
}
