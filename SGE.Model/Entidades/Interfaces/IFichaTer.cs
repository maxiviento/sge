using System;
using System.Collections.Generic;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IFichaTer
    {
        int IdFicha { get; set; }     
        int? IdCarreraTerciaria { get; set; }
        string NombreCarreraTerciaria { get; set; }      
        int? IdInstitucionTerciaria { get; set; }
        string NombreInstitucionTerciaria { get; set; }        
        int? IdSectorTerciaria { get; set; }
        string NombreSectorTerciaria { get; set; }        
        int? IdEscuelaTerciaria { get; set; }
        decimal? PromedioTerciaria { get; set; }
        string OtraCarreraTerciaria { get; set; }
        string OtraInstitucionTerciaria { get; set; }
        string OtroSectorTerciaria { get; set; }
        int? IdDepartamentoEscTerc { get; set; }
        int? IdLocalidadEscTerc { get; set; }
        int? IdSubprograma { get; set; }
        string Subprograma { get; set; }

    }
}
