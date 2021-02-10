using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface ILiquidacionVista
    {
        int IdLiquidacion { get; set; }
        int IdEstadoLiquidacion { get; set; }
        System.DateTime FechaLiquidacion { get; set; }
        string Observacion { get; set; }
        int? NroResolucion { get; set; }
        string TxtNombre { get; set; }
        bool Generado { get; set; }
        IComboBox EstadosLiquidacion { get; set; }
        IComboBox Conceptos { get; set; }
        IList<IBeneficiarioLiquidacion> ListaBeneficiario { get; set; }
        string NumeroDocumento { get; set; }
        string Encontrado { get; set; }
        IBeneficiarioLiquidacion SeleccionadoBeneficiario { get; set; }
        int CountConceptos { get; set; }
        string SeleccionoConcepto { get; set; }
        string GenerarFile { get; set; }
        IComboBox Programas { get; set; }
        IComboBox Estados { get; set; }
        string Accion { get; set; }
        ILiquidacion Liquidacion { get; set; }

        IList<IBeneficiario> ListaBeneficiariosAnexo { get; set; }
        ILiquidacion BeneficiariosEnLiquidacion { get; set; }

        //Importacion Beneficiarios Repago
        bool Importado { get; set; }
        string Mensaje { get; set; }
        string Archivo { get; set; }
        bool Proceso { get; set; }


        //Importe Plan
        decimal ImportePlanEfe { get; set; }

        //filtro convenios
        IComboBox Convenios { get; set; }

    }
}
