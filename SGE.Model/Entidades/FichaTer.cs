using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class FichaTer : IFichaTer
    {
        public int IdFicha { get; set; }
        public int? IdCarreraTerciaria { get; set; }
        public string NombreCarreraTerciaria { get; set; }
        public int? IdInstitucionTerciaria { get; set; }
        public string NombreInstitucionTerciaria { get; set; }
        public int? IdSectorTerciaria { get; set; }
        public string NombreSectorTerciaria { get; set; }
        public int? IdEscuelaTerciaria { get; set; }
        public decimal? PromedioTerciaria { get; set; }
        public string OtraCarreraTerciaria { get; set; }
        public string OtraInstitucionTerciaria { get; set; }
        public string OtroSectorTerciaria { get; set; }
        public int? IdDepartamentoEscTerc { get; set; }
        public int? IdLocalidadEscTerc { get; set; }
        public int? IdSubprograma { get; set; }
        public string Subprograma { get; set; }

    }
}
