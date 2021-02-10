using System;
using System.ComponentModel.DataAnnotations;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Entidades;
using SGE.Servicio.Comun;

namespace SGE.Servicio.Vista
{
    public class OngVista : IOngVista
    {

        public int ID_ONG { get; set; }
        [Required(ErrorMessage = "Ingrese el Nombre de la ONG")]
        public string N_NOMBRE { get; set; }
        [StringLength(13, ErrorMessage = "CUIL supera los 13 caracteres.")]
        public string CUIT { get; set; }
        public string CELULAR { get; set; }
        public string TELEFONO { get; set; }
        [ValidateEmail]
        public string MAIL_ONG { get; set; }
        public int? ID_RESPONSABLE { get; set; }
        public int? ID_CONTACTO { get; set; }
        public string FACTURACION { get; set; }
        public int? ID_USR_SIST { get; set; }
        public DateTime? FEC_SIST { get; set; }
        public int? ID_USR_MODIF { get; set; }
        public DateTime? FEC_MODIF { get; set; }
        public string RES_APELLIDO { get; set; }
        public string RES_NOMBRE { get; set; }
        public string RES_DNI { get; set; }
        public string RES_CUIL { get; set; }
        public string RES_CELULAR { get; set; }
        public string RES_TELEFONO { get; set; }
        public string RES_MAIL { get; set; }
        public int? RES_ID_LOCALIDAD { get; set; }
        public string RES_N_LOCALIDAD { get; set; }
        public string CON_APELLIDO { get; set; }
        public string CON_NOMBRE { get; set; }
        public string CON_DNI { get; set; }
        public string CON_CUIL { get; set; }
        public string CON_CELULAR { get; set; }
        public string CON_TELEFONO { get; set; }
        public string CON_MAIL { get; set; }
        public int? CON_ID_LOCALIDAD { get; set; }
        public string CON_N_LOCALIDAD { get; set; }

        public string Accion { get; set; }
    }
}
