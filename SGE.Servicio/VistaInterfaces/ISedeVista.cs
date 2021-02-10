using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface ISedeVista
    {
        int Id { get; set; }
        string Descripcion { get; set; }
        IComboBox Empresas { get; set; }
        ComboBox Localidades { get; set; }
        string Calle { get; set; }
        string Numero { get; set; }
        string Piso { get; set; }
        string Dpto { get; set; }
        string CodigoPostal { get; set; }
        string ApellidoContacto { get; set; }
        string NombreContacto { get; set; }
        string Telefono { get; set; }
        string Fax { get; set; }
        string Email { get; set; }         
        string Accion { get; set; }
        int IdEmpresa { get; set; }
    }
}
