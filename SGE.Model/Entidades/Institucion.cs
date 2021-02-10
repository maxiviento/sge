using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Institucion : IInstitucion
    {
        public Institucion()
        {
            
        }

        public int IdInstitucion { get; set; }
        public string NombreInstitucion { get; set; }
        public IList<ICarrera> Carreras { get; set; }
       
    }
}
