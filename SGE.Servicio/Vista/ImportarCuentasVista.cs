using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class ImportarCuentasVista : IImportarCuentasVista
    {
        public ImportarCuentasVista()
        {
            ListaImportadoCorrectamente = new List<IBeneficiario>();
            ListaFallaImportacion = new List<IBeneficiario>();
        }

        public bool Importado { get; set; }
        public string Mensaje { get; set; }
        public string Archivo { get; set; }
        public IList<IBeneficiario> ListaImportadoCorrectamente { get; set; }
        public IList<IBeneficiario> ListaFallaImportacion { get; set; }
        public bool Proceso { get; set; }

    }
}
