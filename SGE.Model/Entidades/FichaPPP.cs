using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class FichaPPP :IFichaPPP
    {
        public FichaPPP()
        {
            Ficha = new Ficha();
            Empresa = new Empresa();
            Sede = new Sede();
            //escuelaPpp = new Escuela();
            ong = new Ong();
            cursoPpp = new Curso();
        }

        public int IdFicha { get; set; }
        public int? IdNivelEscolaridad { get; set; }
        public bool Cursando { get; set; }
        public bool Finalizado { get; set; }
        public int? IdEmpresa { get; set; }
        public int? Modalidad { get; set; }
        public string Tareas { get; set; }
        public bool DeseaTermNivel { get; set; }
        public int? IdSede { get; set; }
        public string DescripcionEmpresa { get; set; }
        public string DescripcionSede { get; set; }
        public string CuitEmpresa { get; set; }
        public IEmpresa Empresa { get; set; }
        public ISede Sede { get; set; }
        public string AltaTemprana { get; set; }
        public DateTime? FechaInicioActividad { get; set; }
        public DateTime? FechaFinActividad { get; set; }
        public int? IdModalidadAfip { get; set; }
        public string ModalidadAfip { get; set; }
        public DateTime? FechaNotificacion { get; set; }
        public bool Notificado { get; set; }

        //public IHorarioFicha HorariosFicha { get; set; }
        public IEnumerable<IHorarioFicha> HorariosFicha { get; set; }

        public IFicha Ficha { get; set; }

        // 27/07/2016
        public int? IdSubprograma { get; set; }
        public string Subprograma { get; set; }
        public decimal monto { get; set; }

        // 16/08/2016
        public int? Id_Escuela { get; set; }
        public string N_ESCUELA { get; set; }
        public string CueEsc { get; set; }
        public string CALLEEsc { get; set; }
        public string NUMEROEsc { get; set; }
        public string TELEFONOEsc { get; set; }
        public string RegiseEsc { get; set; }
        //public IEscuela escuelaPpp { get; set; }
        public int? ID_ONG { get; set; }
        public IOng ong { get; set; }
        public string ppp_aprendiz { get; set; }
        public int? ID_CURSO { get; set; }
        public ICurso cursoPpp { get; set; }
        // 22/11/2016
        public int? TIPO_PPP { get; set; }

        // 04/04/2017
        public string sede_rotativa { get; set; }
        public string horario_rotativo { get; set; }


        // 14/07/2018
        public string n_descripcion_t { get; set; }
        public string n_cursado_ins { get; set; }
        public DateTime? egreso { get; set; }
        public string cod_uso_interno { get; set; }
        public string carga_horaria { get; set; }
        public string ch_cual { get; set; }
        public int? id_carrera { get; set; }
        public string CONSTANCIA_EGRESO { get; set; }
        public string MATRICULA { get; set; }
        public string CONSTANCIA_CURSO { get; set; }

        // 23/09/2018
        public int? co_financiamiento { get; set; }

        // 25/09/2018
        public string SENAF { get; set; }
    }
}
