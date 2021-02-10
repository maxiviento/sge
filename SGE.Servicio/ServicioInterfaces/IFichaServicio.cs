using System.Web;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IFichaServicio
    {
        IFichasVista GetFichas();
        IFichasVista GetIndex();
        IFichasVista GetFichas(IPager pager);
        IFichaVista GetFicha(int idFicha, string accion);

        IFichaVista GetFichaIndex();

        bool UpdateFicha(IFichaVista ficha);
        IFichasVista GetFichas(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, int idSubprograma, int TipoPrograma, string nrotramite);
        IFichasVista GetFichas(IPager pager, string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, int idSubprograma, int TipoPrograma, string nrotramite);
        IFichaVista GetFicha(IFichaVista ficha, string accion);
        IImportarExcelVista ImportarExcel(HttpPostedFileBase file);
        bool ProcesarExcel(IImportarExcelVista vista);
        bool ConvertirFicha(int idFicha);
        bool AddFicha(IFichaVista ficha);
        IFichaVista GetEmpresa(IFichaVista ficha);
        IFichasVista GetFichasForReportes(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha);
        byte[] ExportarFichas(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha);

        bool AgregarHorarioEmpresa(IFichaVista ficha);
        IFichaVista AgregarCicloEmpresa(IFichaVista ficha);
        // 18/02/2013 - DI CAMPLI LEANDRO - ALTA MASIVA DE BENEFICIARIOS ************
        IFichasVista SubirArchivoFichas(HttpPostedFileBase archivo, IFichasVista model);
        IFichasVista indexAltaMasiviaBenef();
        IFichasVista CargarMasivaFichasXLS(string ExcelNombre);
        int UpdateCargaMasiva(IFichasVista fichas);
        IFichasVista GetFichas(IFichasVista modelo);
        //**********************

        byte[] ExportarFichasPPP(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, int TipoPpp, int idSubprograma, string nrotramite);
        byte[] ExportarFichasTer(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, int idSubprograma);
        byte[] ExportarFichasUni(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, int idSubprograma);
        byte[] ExportarFichasProf(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa);
        byte[] ExportarFichasVat(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa);
        byte[] ExportarFichasReco_Prod(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, FichasVista vista);
        byte[] ExportarFichasEfec_Soc(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, FichasVista vista);
        byte[] ExportarFichasConfVos(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, FichasVista vista);
        //********08/01/2014 - GESTOR DE ESTADOS FICHAS *************
        IFichasVista indexGestorEstadosFichas();
        //***********************************************************


        void CargarLocalidadEscuela(ComboBox cbo, int idDepartamento);
        void CargarEscuela(ComboBox cbo, int idLocalidad);
        bool UpdateFichaEtapa(int idFicha, int idEtapa);
        IEscuela TraerEscuela(int idEscuela);
        IFichasVista indexReempadronarFichas();
        IFichasVista EmpadronarFichasXLS(string ExcelUrl);
        IFichasVista ReEmpadronarFichas(IFichasVista fichas);
        byte[] ExportEmpadronados(IFichasVista model);

    }
}
