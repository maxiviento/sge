using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface ILiquidacionesVista
    {
        IList<ILiquidacion> Liquidaciones { get; set; }
        IPager Pager { get; set; }
        DateTime From { get; set; }
        DateTime To { get; set; }
        int? NroResolución { get; set; }
        string Programa { get; set; }
        int CantBenef { get; set; }
        string Usuario { get; set; }
        IComboBox Estados { get; set; }
        IComboBox Programas { get; set; }
        //filtro convenios
        IComboBox Convenios { get; set; }
    }
}
