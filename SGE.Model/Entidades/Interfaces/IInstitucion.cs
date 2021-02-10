using System.Collections.Generic;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IInstitucion
    {
        int IdInstitucion { get; set; }
        string NombreInstitucion { get; set; }
        IList<ICarrera> Carreras { get; set; }
        

    }
}
