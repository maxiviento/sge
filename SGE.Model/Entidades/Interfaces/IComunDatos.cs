using System;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IComunDatos
    {
        //System.DateTime FechaAlta { get; set; }
        //System.DateTime FechaModificacion { get; set; }
        //int IdUsuarioModificador { get; set; }
        //int IdUsuarioAlta { get; set; }

        DateTime? FechaSistema { get; set; }
        int? IdUsuarioSistema { get; set; }
        string UsuarioSistema { get; set; }

    }
}
