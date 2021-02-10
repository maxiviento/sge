using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IOng
    {
        int ID_ONG { get; set; }
        string N_NOMBRE { get; set; }
        string CUIT { get; set; }
        string CELULAR { get; set; }
        string TELEFONO { get; set; }
        string MAIL_ONG { get; set; }
        int? ID_RESPONSABLE { get; set; }
        int? ID_CONTACTO { get; set; }
        string FACTURACION { get; set; }
        int? ID_USR_SIST { get; set; }
        DateTime? FEC_SIST { get; set; }
        int? ID_USR_MODIF { get; set; }
        DateTime? FEC_MODIF { get; set; }
        string RES_APELLIDO { get; set; }
        string RES_NOMBRE { get; set; }
        string RES_DNI { get; set; }
        string RES_CUIL { get; set; }
        string RES_CELULAR { get; set; }
        string RES_TELEFONO { get; set; }
        string RES_MAIL { get; set; }
        int? RES_ID_LOCALIDAD { get; set; }
        string RES_N_LOCALIDAD { get; set; }
        string CON_APELLIDO { get; set; }
        string CON_NOMBRE { get; set; }
        string CON_DNI { get; set; }
        string CON_CUIL { get; set; }
        string CON_CELULAR { get; set; }
        string CON_TELEFONO { get; set; }
        string CON_MAIL { get; set; }
        int? CON_ID_LOCALIDAD { get; set; }
        string CON_N_LOCALIDAD { get; set; }
    }
}
