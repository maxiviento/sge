using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IEscuela
    {
        int Id_Escuela { get; set; }
        int? Id_Localidad { get; set; }
        string Nombre_Escuela { get; set; }
        string Cue { get; set; }
        int? Anexo { get; set; }
        string Barrio { get; set; }
        ILocalidad LocalidadEscuela {get;set;}
        IBarrio barrio { get; set; }
        //14/04/2015 - Nuevos campos para escuela
         string CALLE { get; set; }
         string NUMERO { get; set; }
         int? ID_COORDINADOR { get; set; }
         int? ID_DIRECTOR { get; set; }
         int? ID_ENCARGADO { get; set; }
         int? CUPO_CONFIAMOS { get; set; }
         string TELEFONO { get; set; }
         //25/09/2016 - Nuevo campo para escuela
         string regise { get; set; }

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

         int? Id_Barrio { get; set; }


    }
}
