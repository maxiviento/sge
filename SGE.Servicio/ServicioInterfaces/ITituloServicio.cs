using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface ITituloServicio
    {
        ITitulosVista GetTitulos();
        ITitulosVista GetTitulos(string descripcion);
        ITituloVista GetTitulo(int id, string accion);
        ITitulosVista GetTitulos(Pager pager, int id, string descripcion);
        int AddTitulo(ITituloVista titulo);
        int UpdateTitulo(ITituloVista titulo);
        int DeleteTitulo(ITituloVista titulo);

    }
}
