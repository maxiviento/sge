using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class AccionVista : IAccionVista
    {
        public int Id { get; set; }
        public string Accion { get; set; }
        public bool Selected { get; set; }
    }
}
