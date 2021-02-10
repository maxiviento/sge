using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class ImportarLiquidacionVista : IImportarLiquidacionVista
    {
        public ImportarLiquidacionVista()
        {
            ListaBeneficiarios = new List<IBeneficiario>();
            ListaApoderados = new List<IApoderado>();
        }

        public IList<IBeneficiario> ListaBeneficiarios { get; set; }
        public IList<IApoderado> ListaApoderados { get; set; }
        public ILiquidacion Liquidacion { get; set; }
        public bool Importado { get; set; }
        public string Mensaje { get; set; }
        public string Archivo { get; set; }
        public bool Proceso { get; set; }
        public int IdLiquidacion { get; set; }
    }
}
