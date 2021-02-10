using System;
using System.Collections.Generic;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IFichaPPP
    {
        int IdFicha { get; set; }
        IFicha Ficha { get; set; }
        int? IdNivelEscolaridad { get; set; }
        bool Cursando { get; set; }
        bool Finalizado { get; set; }
        int? IdEmpresa { get; set; }
        int? Modalidad { get; set; }
        string Tareas { get; set; }
        bool DeseaTermNivel { get; set; }
        int? IdSede{ get; set; }
        string DescripcionEmpresa { get; set; }
        string DescripcionSede { get; set; }
        string CuitEmpresa { get; set; }
        IEmpresa Empresa { get; set; }
        ISede Sede { get; set; }
        string AltaTemprana { get; set; }
        DateTime? FechaInicioActividad { get; set; }
        DateTime? FechaFinActividad { get; set; }
        int? IdModalidadAfip { get; set; }
        string ModalidadAfip { get; set; }
        DateTime? FechaNotificacion { get; set; }
        bool Notificado { get; set; }
        //IHorarioFicha HorariosFicha { get; set; }
        IEnumerable<IHorarioFicha> HorariosFicha { get; set; }
        // 27/07/2016
        int? IdSubprograma { get; set; }
        string Subprograma { get; set; }
        decimal monto { get; set; }

        // 16/08/2016
        int? Id_Escuela { get; set; }
        string N_ESCUELA { get; set; }
        string CueEsc { get; set; }
        string CALLEEsc { get; set; }
        string NUMEROEsc { get; set; }
        string TELEFONOEsc { get; set; }
        string RegiseEsc { get; set; }
        //IEscuela escuelaPpp { get; set; }
        int? ID_ONG { get; set; }
        IOng ong { get; set; }
        string ppp_aprendiz { get; set; }
        int? ID_CURSO { get; set; }
        ICurso cursoPpp { get; set; }
        // 22/11/2016
        int? TIPO_PPP { get; set; }

        // 04/04/2017
        string sede_rotativa { get; set; }
        string horario_rotativo { get; set; }

        // 14/07/2018
        string n_descripcion_t { get; set; }
        string n_cursado_ins { get; set; }
        DateTime? egreso { get; set; }
        string cod_uso_interno { get; set; }
        string carga_horaria { get; set; }
        string ch_cual { get; set; }
        int? id_carrera { get; set; }
        string CONSTANCIA_EGRESO { get; set; }
        string MATRICULA { get; set; }
        string CONSTANCIA_CURSO { get; set; }
        // 23/09/2018
        int? co_financiamiento { get; set; }
        // 25/09/2018
        string SENAF { get; set; }
    }
}
