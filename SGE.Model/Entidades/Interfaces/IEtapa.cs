using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IEtapa
    {

         int ID_ETAPA {get;set;}
         int ID_PROGRAMA { get; set; }
         string N_ETAPA {get;set;}
         decimal? MONTO_ETAPA  {get;set;}
         string EJERCICIO   {get;set;}
         DateTime FEC_INCIO  {get;set;}
         DateTime? FEC_FIN  {get;set;}
         int? ID_USR_SIST { get; set; }
         DateTime? FEC_SIST  {get;set;}
         int? ID_USR_MODIF { get; set; }
         DateTime? FEC_MODIF { get; set; }

         string N_PROGRAMA { get; set; }
    }
}
