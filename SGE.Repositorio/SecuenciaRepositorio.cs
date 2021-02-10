using System.Configuration;
using System.Linq;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public static class SecuenciaRepositorio
    {
        public static int GetId()
        {
            var mdb= new DataSGE();

            var esquemaDb = ConfigurationManager.AppSettings["EsquemaDB"];

            var queryString = "SELECT " + esquemaDb + ".HIBERNATE_SEQUENCE.nextval from dual";

            var contactQuery = mdb.ExecuteStoreQuery<decimal>(queryString).FirstOrDefault();

            return (int)contactQuery;
        }
    }
}
