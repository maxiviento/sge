using System.Collections.Generic;
using System.Web;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface ILiquidacionServicio
    {
        ILiquidacionesVista GetLiquidacionesIndex();
        ILiquidacionesVista GetLiquidaciones();
        ILiquidacionesVista GetLiquidaciones(IPager pager);
        ILiquidacionesVista GetLiquidaciones(System.DateTime from, System.DateTime to, int nroResolucion, int estado, int programa);
        ILiquidacionesVista GetLiquidaciones(IPager pager, System.DateTime from, System.DateTime to, int nroResolucion, int estado, int programa);
        ILiquidacionVista GetLiquidacion(int idLiquidacion);
        ILiquidacionVista GetLiquidacion(int idLiquidacion, string accion);
        ILiquidacionVista GetLiquidacion();
        ILiquidacionVista GetLiquidacionRepagos();
        string GenerarFile(IList<IBeneficiarioLiquidacion> listaLiquidacionBeneficiario, System.DateTime fechaLiquidacion);

        int AgregarLiquidacion(ILiquidacionVista liquidacion);

        bool UpdateLiquidacion(ILiquidacionVista liquidacion);

        bool ChangeEstadoLiquidacion(ILiquidacionVista liquidacion);

        ILiquidacionVista BuscarBeneficiarioEnNomina(ILiquidacionVista vista);

        ILiquidacionVista QuitarBeneficiarioDeNomina(ILiquidacionVista vista, int idBeneficiario);

        ILiquidacionVista AgregarBeneficiarioDeNomina(ILiquidacionVista vista, int idBeneficiario);

        ILiquidacionVista CargarConceptoBeneficiarios(ILiquidacionVista vista);

        ILiquidacionVista CargarConceptoBeneficiariosRepago(ILiquidacionVista vista); 

        ILiquidacionVista VerConceptosBeneficiario(ILiquidacionVista vista, int idBeneficiario);

        ILiquidacionVista VerMontoLiquidarBeneficiario(ILiquidacionVista vista, int idBeneficiario, int importeAliquidar);

        ILiquidacionVista VolverFormBeneficiario(ILiquidacionVista vista);

        ILiquidacionVista VolverFormBeneficiarioDesdeImporte(ILiquidacionVista vista);

        ILiquidacionVista AgregarConceptoBeneficiario(ILiquidacionVista vista);

        ILiquidacionVista QuitarConceptoBeneficiario(ILiquidacionVista vista, int idBeneficiario, int idConcepto);

        string GenerarArchivo(ILiquidacionVista vista);

        IImportarLiquidacionVista SubirArchivo(HttpPostedFileBase archivo, int idliquidacion);

        IImportarLiquidacionVista ProcesarArchivo(IImportarLiquidacionVista model);

        ILiquidacionVista SubirArchivoRepago(HttpPostedFileBase archivo, ILiquidacionVista model);

        IImportarLiquidacionVista GetLiquidacionImportacion(int idLiquidacion);

        byte[] ExportLiquidacion(ILiquidacionVista model);
       
        //byte[] ReporteAnexo1Resolucion(int idLiquidacion, System.DateTime fechaLiquidacion);
        byte[] ReporteAnexo1Resolucion(ILiquidacionVista vista);

        ILiquidacionVista CargarBeneficiariosRepAnexoI(ILiquidacionVista vista);

    }
}
