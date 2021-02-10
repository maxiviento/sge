using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IFichaConfVos
    {


        IFicha Ficha { get; set; }
        int IdFicha { get; set; }
        int? IdEmpresa { get; set; }
        int? IdSede { get; set; }
        int? IdSubprograma { get; set; }
        string Subprograma { get; set; }
        int? IdProyecto { get; set; }
        string AltaTemprana { get; set; }
        int? Modalidad { get; set; }
        DateTime? FechaInicioActividad { get; set; }
        DateTime? FechaFinActividad { get; set; }
        int? IdModalidadAfip { get; set; }
        string ModalidadAfip { get; set; }
        string AportesDeLaEmpresa { get; set; }

        int? IdNivelEscolaridad { get; set; }
        bool Cursando { get; set; }
        bool Finalizado { get; set; }
        bool DeseaTermNivel { get; set; }

        string ApellidoTutor { get; set; }
        string NombreTutor { get; set; }
        string NroDocumentoTutor { get; set; }
        string TelefonoTutor { get; set; }
        string EmailTutor { get; set; }
        string PuestoTutor { get; set; }

        string DescripcionEmpresa { get; set; }
        string DescripcionSede { get; set; }
        string CuitEmpresa { get; set; }
        IEmpresa Empresa { get; set; }
        ISede Sede { get; set; }
        IProyecto Proyecto { get; set; }
        DateTime? FechaNotificacion { get; set; }
        bool Notificado { get; set; }

        IHorarioFicha HorariosFicha { get; set; }


        int? Abandono_Escuela { get; set; }
        string Trabaja_Trabajo { get; set; }
        string cursa { get; set; } //Cursado 2014
        int? Ultimo_Cursado { get; set; }
        string pit { get; set; }
        string Centro_Adultos { get; set; }
        string Progresar { get; set; }
        int? Actividades { get; set; }
        string Benef_progre { get; set; }
        string Autoriza_Tutor { get; set; }
        string Apoderado { get; set; }

        string Apo_Cuil { get; set; }
        string Apo_Apellido { get; set; }
        string Apo_Nombre { get; set; }
        DateTime? Apo_Fer_Nac { get; set; }
        int? Apo_Tipo_Documento { get; set; }
        string Apo_Numero_Documento { get; set; }
        string Apo_Calle { get; set; }
        string Apo_Numero { get; set; }
        string Apo_Piso { get; set; }
        string Apo_Dpto { get; set; }
        string Apo_Monoblock { get; set; }
        string Apo_Parcela { get; set; }
        string Apo_Manzana { get; set; }
        string Apo_Barrio { get; set; }
        string Apo_Codigo_Postal { get; set; }
        int? Apo_Id_Localidad { get; set; }
        string Apo_Telefono { get; set; }
        string Apo_Tiene_Hijos { get; set; }
        int? Apo_Cantidad_Hijos { get; set; }
        string Apo_Sexo { get; set; }
        int? Apo_Estado_Civil { get; set; }

        int? Id_Usr_Sist { get; set; }
        DateTime Fec_Sist { get; set; }
        int? Id_Usr_Modif { get; set; }
        DateTime? Fec_Modif	{ get; set; }

        int? Id_Escuela { get; set; }
        IEscuela escuela { get; set; }
        ICurso curso { get; set; }
        // 16/04/2015 - nuevos campos
        int? ID_FACILITADOR { get; set; }
        int? ID_GESTOR { get; set; }
        int? ID_CURSO { get; set; }
        int? ID_SUBSIDIARIA { get; set; }
        //

        // 17/04/2015
        int? ID_ABANDONO_CUR { get; set; }
        short? TRABAJO_CONDICION { get; set; }
        string SENAF { get; set; }
        string PERTENECE_ONG { get; set; }
        int? ID_ONG { get; set; }
        string APO_CELULAR { get; set; }
        string APO_MAIL { get; set; }


        //06/05/2015 - actores
        IActor facilitador_monitor { get; set; }
        IActor gestor { get; set; }
        // 28/05/2015 - ONG
        IOng ong { get; set; }
    }
}
