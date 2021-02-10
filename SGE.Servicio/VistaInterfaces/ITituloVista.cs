using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface ITituloVista
    {
        int Id { get; set; }
        string Descripcion { get; set; }
        int IdUniversidad { get; set; }
        IComboBox Universidades { get; set; }
        string Accion { get; set; }
    }
}
