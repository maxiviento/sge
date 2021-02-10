
using SGE.Model.Entidades.Interfaces;
using System.Collections.Generic;
namespace SGE.Servicio.VistaInterfaces
{
    public interface IInstitucionVista
    {
        int Id { get; set; }
        string Descripcion { get; set; }
        string Accion { get; set; }
        IList<ISector> Sectores { get; set; } // 07/03/2013 - DI CAMPLI LEANDRO - SE AÑADE ASIGNACION DE SECTORES A INSTITUCIONES
        string restultados { get; set; } // 08/03/2013 - DI CAMPLI LEANDRO 
    }
}
