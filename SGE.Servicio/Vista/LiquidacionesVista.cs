using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;


namespace SGE.Servicio.Vista
{
    public class LiquidacionesVista : ILiquidacionesVista
    {
        public LiquidacionesVista()
        {
            To = DateTime.Now.AddDays(-15);
            Estados = new ComboBox();
            Programas = new ComboBox();
            Convenios = new ComboBox();

        }

        public IList<ILiquidacion> Liquidaciones { get; set; }
        public IPager Pager { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int? NroResolución { get; set; }
        public string Programa { get; set; }
        public int CantBenef { get; set; }
        public string Usuario { get; set; }
        public IComboBox Estados { get; set; }
        public IComboBox Programas { get; set; }
        //filtro convenios
        public IComboBox Convenios { get; set; }
    }
}
