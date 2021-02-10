using System;
using System.Collections.Generic;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IFichaUni
    {
        int IdFicha { get; set; }  
        int? IdCarreraUniversitaria { get; set; }
        string NombreCarreraUniversitaria { get; set; }
        int? IdInstitucionUniversitaria { get; set; }
        string NombreInstitucionUniversitaria { get; set; }
        int? IdSectorUniversitaria { get; set; }
        string NombreSectorUniversitaria { get; set; }
        int? IdEscuelaUniversitaria { get; set; }
        decimal? PromedioUniversitaria { get; set; }
        string OtraCarreraUniversitaria { get; set; }
        string OtraInstitucionUniversitaria { get; set; }
        string OtroSectorUniversitaria { get; set; }
        int? IdDepartamentoEscUniv { get; set; }
        int? IdLocalidadEscUniv { get; set; }
        int? IdSubprograma { get; set; }
        string Subprograma { get; set; }

    }
}
