using SGE.Model.Repositorio;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Servicio
{
    public class InicializarTablasServicio : IInicializarTablasServicio
    {
        private readonly IInicializarTablasRepositorio _inicializarTablasRepositorio;

        public InicializarTablasServicio()
        {
            _inicializarTablasRepositorio = new InicializarTablasRepositorio();
        }

        public int Insertar(IInicializarTablasVista vista)
        {
            const string log1 = "Se han insertado ";
            const string log2 = " registros en la tabla ";

            if (vista.Inserciones!=null)
            {
                vista.Inserciones.Add(log1 + _inicializarTablasRepositorio.InsertarFormularios() + log2 + "T_FORMULARIOS.");
                vista.Inserciones.Add(log1 + _inicializarTablasRepositorio.InsertarAcciones() + log2 + "T_ACCIONES.");
                vista.Inserciones.Add(log1 + _inicializarTablasRepositorio.InsertarFormularioAccion() + log2 + "T_FORMULARIO_ACCION.");
                vista.Inserciones.Add(log1 + _inicializarTablasRepositorio.InsertarEstados() + log2 + "T_ESTADOS.");
                vista.Inserciones.Add(log1 + _inicializarTablasRepositorio.InsertarEstadoBeneficiarioLiquidacion() + log2 + "T_ESTADOS_BEN_CON_LIQ.");
                vista.Inserciones.Add(log1 + _inicializarTablasRepositorio.InsertarEstadosInscriptos() + log2 + "T_ESTADOS_FICHA.");
                vista.Inserciones.Add(log1 + _inicializarTablasRepositorio.InsertarTipoFicha() + log2 + "T_TIPO_FICHA.");
                vista.Inserciones.Add(log1 + _inicializarTablasRepositorio.InsertarProgramas() + log2 + "T_PROGRAMAS.");
            }
            
            return 0;
        }
    }
}
