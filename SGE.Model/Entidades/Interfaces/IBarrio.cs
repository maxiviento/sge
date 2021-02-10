using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IBarrio
    {
          int ID_BARRIO {get; set;}
          string N_BARRIO {get; set;}
          int? ID_SECCIONAL {get; set;}
          int? ID_LOCALIDAD {get; set;}
          int? ID_USR_SIST {get; set;}
          DateTime? FEC_SIST {get; set;}
          int? ID_USR_MODIF {get; set;}
          DateTime? FEC_MODIF {get; set;}

          string N_LOCALIDAD { get; set; }
          string N_SECCIONAL { get; set; }
    }
}
