using System.Collections.Generic;
using SGE.Servicio.VistaInterfaces.Shared;
using SGE.Model.Entidades.Interfaces;
using System;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IEscuelaVista
    {
        int Id_Escuela { get; set; }
        int? Id_Localidad { get; set; }
        string Nombre_Escuela { get; set; }
        string Cue { get; set; }
        int? Anexo { get; set; }
        string Barrio { get; set; }
        //public ILocalidad LocalidadEscuela { get; set; }
        //public IBarrio barrio { get; set; }
        string CALLE { get; set; }
        string NUMERO { get; set; }
        int? ID_COORDINADOR { get; set; }
        int? ID_DIRECTOR { get; set; }
        int? ID_ENCARGADO { get; set; }
        int? CUPO_CONFIAMOS { get; set; }
        string TELEFONO { get; set; }
        string COOR_APELLIDO { get; set; }
        string COOR_NOMBRE { get; set; }
        string COOR_DNI { get; set; }
        string COOR_CUIL { get; set; }
        string COOR_CELULAR { get; set; }
        string COOR_TELEFONO { get; set; }
        string COOR_MAIL { get; set; }
        int? COOR_ID_LOCALIDAD { get; set; }
        string COOR_N_LOCALIDAD { get; set; }

        string DIR_APELLIDO { get; set; }
        string DIR_NOMBRE { get; set; }
        string DIR_DNI { get; set; }
        string DIR_CUIL { get; set; }
        string DIR_CELULAR { get; set; }
        string DIR_TELEFONO { get; set; }
        string DIR_MAIL { get; set; }
        int? DIR_ID_LOCALIDAD { get; set; }
        string DIR_N_LOCALIDAD { get; set; }
        string ENC_APELLIDO { get; set; }
        string ENC_NOMBRE { get; set; }
        string ENC_DNI { get; set; }
        string ENC_CUIL { get; set; }
        string ENC_CELULAR { get; set; }
        string ENC_TELEFONO { get; set; }
        string ENC_MAIL { get; set; }
        int? ENC_ID_LOCALIDAD { get; set; }
        string ENC_N_LOCALIDAD { get; set; }
        IComboBox Localidades { get; set; }
        IComboBox barrios { get; set; }
        IComboBox departamentos { get; set; }
        int? Id_Barrio { get; set; }

        IList<IEscuela> escuelas { get; set; }
        string Accion { get; set; }

        //25/09/2016 - Nuevo campo para escuela
        string regise { get; set; }
    }
}
