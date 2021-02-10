using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class TituloVista :ITituloVista
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int IdUniversidad { get; set; }
        public IComboBox Universidades { get; set; }
        public string Accion { get; set; }
    }
}
