namespace SGE.Model.Repositorio
{
    public interface IInicializarTablasRepositorio
    {
        int InsertarFormularios();
        int InsertarAcciones();
        int InsertarFormularioAccion();
        int InsertarEstados();
        int InsertarEstadoBeneficiarioLiquidacion();
        int InsertarEstadosInscriptos();
        int InsertarTipoFicha();
        int InsertarProgramas();
    }
}
