using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class FichaUni : IFichaUni
    {
        public int IdFicha { get; set; }
        public int? IdCarreraUniversitaria { get; set; }
        public string NombreCarreraUniversitaria { get; set; }
        public int? IdInstitucionUniversitaria { get; set; }
        public string NombreInstitucionUniversitaria { get; set; }
        public int? IdSectorUniversitaria { get; set; }
        public string NombreSectorUniversitaria { get; set; }
        public int? IdEscuelaUniversitaria { get; set; }
        public decimal? PromedioUniversitaria { get; set; }
        public string OtraCarreraUniversitaria { get; set; }
        public string OtraInstitucionUniversitaria { get; set; }
        public string OtroSectorUniversitaria { get; set; }
        public int? IdDepartamentoEscUniv { get; set; }
        public int? IdLocalidadEscUniv { get; set; }
        public int? IdSubprograma { get; set; }
        public string Subprograma { get; set; }

    }
}
