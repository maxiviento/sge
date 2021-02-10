using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Escuela : IEscuela
    {
        public Escuela()
        {
            LocalidadEscuela = new Localidad();
            barrio = new Barrio();
        }

        public int Id_Escuela { get; set; }
        public int? Id_Localidad { get; set; }
        public string Nombre_Escuela { get; set; }
        public string Cue { get; set; }
        public int? Anexo { get; set; }
        public string Barrio { get; set; }
        public ILocalidad LocalidadEscuela { get; set; }
        public IBarrio barrio { get; set; }
        //14/04/2015 - Nuevos campos para escuela
        public string CALLE { get; set; }
        public string NUMERO { get; set; }
        public int? ID_COORDINADOR { get; set; }
        public int? ID_DIRECTOR { get; set; }
        public int? ID_ENCARGADO { get; set; }
        public int? CUPO_CONFIAMOS { get; set; }
        public string TELEFONO { get; set; }
        //25/09/2016 - Nuevo campo para escuela
        public string regise { get; set; }

        public string COOR_APELLIDO  { get; set; }
        public string COOR_NOMBRE { get; set; }
        public string COOR_DNI { get; set; }
        public string COOR_CUIL  { get; set; }
        public string COOR_CELULAR { get; set; }
        public string COOR_TELEFONO { get; set; }
        public string COOR_MAIL { get; set; }
        public int? COOR_ID_LOCALIDAD { get; set; }
        public string COOR_N_LOCALIDAD { get; set; }

        public string DIR_APELLIDO { get; set; }
        public string DIR_NOMBRE { get; set; }
        public string DIR_DNI { get; set; }
        public string DIR_CUIL { get; set; }
        public string DIR_CELULAR { get; set; }
        public string DIR_TELEFONO { get; set; }
        public string DIR_MAIL { get; set; }
        public int? DIR_ID_LOCALIDAD { get; set; }
        public string DIR_N_LOCALIDAD { get; set; }


        public string ENC_APELLIDO { get; set; }
        public string ENC_NOMBRE { get; set; }
        public string ENC_DNI { get; set; }
        public string ENC_CUIL { get; set; }
        public string ENC_CELULAR { get; set; }
        public string ENC_TELEFONO { get; set; }
        public string ENC_MAIL { get; set; }
        public int? ENC_ID_LOCALIDAD { get; set; }
        public string ENC_N_LOCALIDAD { get; set; }
        
        //public int? DIR_ID_LOCALIDAD { get; set; }
        //public string DIR_N_LOCALIDAD { get; set; }
        public int? Id_Barrio { get; set; }

    }
}
