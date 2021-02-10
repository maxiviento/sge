using System;
using System.Linq;

namespace SGE.Importador
{
    public static class EFSequence
    {
        public static Int32 GetNextValue()
        {
            //Modelo.GE mdb = new Modelo.GE();
            Datos.GEContainer mdb = new Datos.GEContainer();

            string queryString = @"SELECT GESTION_EMPLEOS.HIBERNATE_SEQUENCE.nextval from dual";

            decimal valor = mdb.ExecuteStoreQuery<decimal>(queryString).FirstOrDefault();

            return Convert.ToInt32(valor);
        }
    }
}
