using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IFichaRepositorio
    {
        IList<IFicha> GetFichas();
        IFicha GetFicha(int idFicha);
        int GetCountFichas();
        IList<IFicha> GetFichas(int skip, int take);
        bool UpdateFicha(IFicha ficha);
        bool UpdateEstadoFicha(int idFicha, int idEstadoFicha);
        IList<IFicha> GetFichas(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, string nrotramite, int tipoprograma = 0);
        IList<IFicha> GetFichas(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int skip, int take, int idEtapa);
        IList<IFicha> GetFichas(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int skip, int take, int excluirtipoficha, int idEtapa);
        bool ConvertirFicha(int idFicha);
        IList<IFicha> GetFichasByEmpresa(int? idEmpresa);
        IList<IFicha> GetFichasByEmpresa(int? idEmpresa, int skip, int take);
        IFichaPPP GetFichaPpp(int idficha);
        IList<IFicha> GetFichaByNumeroDocumento(string numeroDocumento);
        bool UpdateFichaSimple(IFicha ficha);

        int AddFicha(IFicha ficha);

        IFichaPPP GetFormFichaPpp();
        IFichaVAT GetFichaVat(int idficha);
        IFichaPPPP GetFichaPppp(int idficha);

        IList<IFicha> GetFichasCompleto(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha);
        IList<IFicha> GetFichasRDepLocProgEst(int? iddepartamento, int? idprograma, int? idlocalidad, string estado, int EstadoBenef);
        IList<IFicha> GetFichasRDepLocProgEstDetalle(int? iddepartamento, int? idprograma, int? idlocalidad, string estado, int estadoBenef);

        IFichaReconversion GetFichaReconversion(int idficha);
        IFichaReconversion GetFormFichaReconversion();
        
        IFichaEfectoresSociales GetFichaEfectores(int idficha);
        IFichaEfectoresSociales GetFormFichaEfectores();

        IList<IFicha> GetFichasRBecasAcademicas(int? idSector, int? idInstitucion, int? idCarrera, int? idPrograma, string estado);
        List<IFicha> GetFichasNotificacion(int[] idFichas);// 20/02/2013 - DI CAMPLI LEANDRO - IMPRIMIR NOTIFICACIONES
        List<IFicha> GetFichasNotificacion();// 20/02/2013 - DI CAMPLI LEANDRO - IMPRIMIR NOTIFICACIONES
        IList<IFicha> GetFichas(List<int> idBeneficiariosExport); // 27/06/2013 - DI CAMPLI LEANDRO - FILTRO DIRECTO A IQUERYBLE
        IFichaConfVos GetFichaConfVos(int idficha); // 09/04/2014 - DI CAMPLI LEANDRO
        //T_FICHAS_CONF_VOS getApoConfVos(int idFicha); // 10/04/2014 - DI CAMPLI LEANDRO
        IFichaConfVos GetFormFichaConfVos();
        bool UpdateFichaEtapa(int idFicha, int idEtapa);
        IFichaTer GetFichaTer(int idficha);
        IFichaUni GetFichaUni(int idficha);
        IList<IFicha> GetFichas(List<int> idBeneficiariosExport, int idPrograma);
        int ReEmpadronarFicha(int IdFicha);
    }
}
