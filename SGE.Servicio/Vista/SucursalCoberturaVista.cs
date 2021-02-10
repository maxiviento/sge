using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Comun;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class SucursalCoberturaVista : ISucursalCoberturaVista
    {
        public SucursalCoberturaVista()
        {
            Sucursales = new ComboBox();
            Localidades = new ComboBox();
        }

        public int IdTablaBcoCba { get; set; }
        public int IdSucursal { get; set; }
        public IComboBox Sucursales { get; set; }
        public int IdLocalidad { get; set; }
        public IComboBox Localidades { get; set; }
        public string Accion { get; set; }
    }
}
