using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface ICarreraVista
    {
        int Id { get; set; }
        string Nombre { get; set; }
        int? IdNivel { get; set; }
        int? IdInstitucion { get; set; }
        int? IdSector { get; set; }

        IComboBox NombreNivel { get; set; }
        IComboBox NombreInstitucion { get; set; }
        IComboBox NombreSector { get; set; }

        string Accion { get; set; }
        string BusquedaAnterior { get; set; }
    }
}
