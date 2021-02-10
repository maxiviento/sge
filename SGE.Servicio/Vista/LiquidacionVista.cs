using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class LiquidacionVista : ILiquidacionVista
    {
        public LiquidacionVista()
        {
            EstadosLiquidacion = new ComboBox();
            Conceptos = new ComboBox();
            Programas = new ComboBox();
            Estados = new ComboBox();
            Encontrado = "N";
            SeleccionadoBeneficiario = new BeneficiarioLiquidacion();
            GenerarFile = "N";
            Accion = "Alta";
            ListaBeneficiario = new List<IBeneficiarioLiquidacion>();
            ListaBeneficiariosAnexo= new List<IBeneficiario>();
            Liquidacion = new Liquidacion();
            Convenios = new ComboBox();
           
        }

        public int IdLiquidacion { get; set; }
        public int IdEstadoLiquidacion { get; set; }
        public DateTime FechaLiquidacion { get; set; }
        public string Observacion { get; set; }
        [Range (0, 9999999999999999999, ErrorMessage="Valor Incorrecto de resolución")]
        public int? NroResolucion { get; set; }
        public string TxtNombre { get; set; }
        public bool Generado { get; set; }
        public IComboBox EstadosLiquidacion { get; set; }
        public IComboBox Conceptos { get; set; }
        public IList<IBeneficiarioLiquidacion> ListaBeneficiario { get; set; }
        public string NumeroDocumento { get; set; }
        public string Encontrado { get; set; }
        public IBeneficiarioLiquidacion SeleccionadoBeneficiario { get; set; }
        public int CountConceptos { get; set; }
        public string SeleccionoConcepto { get; set; }
        public string GenerarFile { get; set; }
        public IComboBox Programas { get; set; }
        public IComboBox Estados { get; set; }
        public string Accion { get; set; }
        public ILiquidacion Liquidacion { get; set; }

        public IList<IBeneficiario> ListaBeneficiariosAnexo { get; set; }

        public ILiquidacion BeneficiariosEnLiquidacion { get; set; }

        //Importacion Beneficiarios Repago
        public bool Importado { get; set; }
        public string Mensaje { get; set; }
        public string Archivo { get; set; }
        public bool Proceso { get; set; }

        public decimal ImportePlanEfe { get; set; }

        //filtro convenios
        public IComboBox Convenios { get; set; }
       
    }
}
