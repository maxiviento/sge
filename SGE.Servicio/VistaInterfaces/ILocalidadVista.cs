using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface ILocalidadVista
    {
        int Id { get; set; }
        string Nombre { get; set; }
        int IdDepartamento { get; set; }
        string NombreDepartamento { get; set; }
        IComboBox NombreDepartamentoCombo { get; set; }

        string Accion { get; set; }
        string BusquedaAnterior { get; set; }

        int IdSucursal {get; set;}
        string NombreSucursal { get; set; }
    }
}
