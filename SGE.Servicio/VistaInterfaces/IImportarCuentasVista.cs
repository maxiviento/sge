using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IImportarCuentasVista
    {
        bool Importado { get; set; }
        string Mensaje { get; set; }
        string Archivo { get; set; }
        IList<IBeneficiario> ListaImportadoCorrectamente { get; set; }
        IList<IBeneficiario> ListaFallaImportacion { get; set; }
        bool Proceso { get; set; }
    }
}
