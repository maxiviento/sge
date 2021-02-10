using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class FichaConfVos : IFichaConfVos
    {
        
        public FichaConfVos()
        {
            Ficha = new Ficha();
            Empresa = new Empresa();
            Sede = new Sede();
            Proyecto = new Proyecto();
            escuela = new Escuela();
            curso = new Curso();
            facilitador_monitor = new Actor();
            gestor = new Actor();
            ong = new Ong();
        }

        public IFicha Ficha { get; set; }
        public int IdFicha { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdSede { get; set; }
        public int? IdSubprograma { get; set; }
        public string Subprograma { get; set; }
        public int? IdProyecto { get; set; }
        public string AltaTemprana { get; set; }
        public int? Modalidad { get; set; }
        public DateTime? FechaInicioActividad { get; set; }
        public DateTime? FechaFinActividad { get; set; }
        public int? IdModalidadAfip { get; set; }
        public string ModalidadAfip { get; set; }
        public string AportesDeLaEmpresa { get; set; }

        public int? IdNivelEscolaridad { get; set; }
        public bool Cursando { get; set; }
        public bool Finalizado { get; set; }
        public bool DeseaTermNivel { get; set; }

        public string ApellidoTutor { get; set; }
        public string NombreTutor { get; set; }
        public string NroDocumentoTutor { get; set; }
        public string TelefonoTutor { get; set; }
        public string EmailTutor { get; set; }
        public string PuestoTutor { get; set; }

        public string DescripcionEmpresa { get; set; }
        public string DescripcionSede { get; set; }
        public string CuitEmpresa { get; set; }
        public IEmpresa Empresa { get; set; }
        public ISede Sede { get; set; }
        public IProyecto Proyecto { get; set; }
        public DateTime? FechaNotificacion { get; set; }
        public bool Notificado { get; set; }

        public IHorarioFicha HorariosFicha { get; set; }


        public int? Abandono_Escuela { get; set; }
        public string Trabaja_Trabajo { get; set; }
        public string cursa { get; set; } //Cursado 2014
        public int? Ultimo_Cursado { get; set; }
        public string pit { get; set; }
        public string Centro_Adultos { get; set; }
        public string Progresar { get; set; }
        public int? Actividades { get; set; }
        public string Benef_progre { get; set; }
        public string Autoriza_Tutor { get; set; }
        public string Apoderado { get; set; }
         
        public string Apo_Cuil { get; set; }
        public string Apo_Apellido { get; set; }
        public string Apo_Nombre { get; set; }
        public DateTime? Apo_Fer_Nac { get; set; }
        public int? Apo_Tipo_Documento { get; set; }
        public string Apo_Numero_Documento { get; set; }
        public string Apo_Calle { get; set; }
        public string Apo_Numero { get; set; }
        public string Apo_Piso { get; set; }
        public string Apo_Dpto { get; set; }
        public string Apo_Monoblock { get; set; }
        public string Apo_Parcela { get; set; }
        public string Apo_Manzana { get; set; }
        public string Apo_Barrio { get; set; }
        public string Apo_Codigo_Postal { get; set; }
        public int? Apo_Id_Localidad { get; set; }
        public string Apo_Telefono { get; set; }
        public string Apo_Tiene_Hijos { get; set; }
        public int? Apo_Cantidad_Hijos { get; set; }
        public string Apo_Sexo { get; set; }
        public int? Apo_Estado_Civil { get; set; }
         
        public int? Id_Usr_Sist { get; set; }
        public DateTime Fec_Sist { get; set; }
        public int? Id_Usr_Modif { get; set; }
        public DateTime? Fec_Modif { get; set; }

        //Información de la Escuela a la que asiste:
        public int? Id_Escuela { get; set; }
        public string ESC_NOMBRE { get; set; }


        public IEscuela escuela { get; set; }
        //14/04/2015 - Nuevos campos para escuela
        public string ESC_CALLE { get; set; }
        public string ESC_NUMERO { get; set; }
        public int? ID_COORDINADOR { get; set; }
        public int? ID_DIRECTOR { get; set; }
        public int? ID_ENCARGADO { get; set; }
        public int? CUPO_CONFIAMOS { get; set; }
        public string TELEFONO { get; set; }
        public string COOR_APELLIDO { get; set; }
        public string COOR_NOMBRE { get; set; }
        public string COOR_DNI { get; set; }
        public string COOR_CUIL { get; set; }
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
        //public int? DIR_ID_LOCALIDAD { get; set; }
        //public string DIR_N_LOCALIDAD { get; set; }



        //

        //Curso

        public ICurso curso { get; set; }
        //


        // 16/04/2015 - nuevos campos
        public int? ID_FACILITADOR { get; set; }
        public int? ID_GESTOR { get; set; }
        public int? ID_CURSO { get; set; }
        public int? ID_SUBSIDIARIA { get; set; }
        //

        // 17/04/2015
        public int? ID_ABANDONO_CUR { get; set; }
        public short? TRABAJO_CONDICION { get; set; }
        public string SENAF { get; set; }
        public string PERTENECE_ONG { get; set; }
        public int? ID_ONG { get; set; }
        public string APO_CELULAR { get; set; }
        public string APO_MAIL { get; set; }

        //06/05/2015 - actores
        public IActor facilitador_monitor { get; set; }
        public IActor gestor { get; set; }
        // 28/05/2015 - ONG
        public IOng ong { get; set; }

    }
}
