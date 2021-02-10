using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IControlDatosRepositorio
    {
        IList<IControlDatos> GetApoderadosDuplicadosActivos();
        IList<IControlDatos> GetApoderadosDuplicadosActivos(int idPrograma, int idEtapa);
        IList<IControlDatos> GetApoderadosDuplicadosActivos(int idprograma);
        IList<IControlDatos> GetApoderadosDuplicadosActivos(int[] listabeneficiarios);
        IList<IControlDatos> GetApoderadosDuplicadosActivos(int skip, int make, string s = "");
        IList<IControlDatos> GetApoderadosDuplicadosActivos(int idprograma, int skip, int make);
        IList<IControlDatos> GetApoderadosDuplicadosActivos(int[] listabeneficiarios, int skip, int make);

        IList<IControlDatos> GetCuentasenCero(int idPrograma, int idEtapa);
        IList<IControlDatos> GetCuentasenCero(int skip, int take, string s = "");
        IList<IControlDatos> GetCuentasenCero(int idprograma);
        IList<IControlDatos> GetCuentasenCero(int idprograma, int skip, int take);

        IList<IControlDatos> GetCuentasenDuplicadas();
        IList<IControlDatos> GetCuentasenDuplicadas(int idPrograma, int idEtapa);
        IList<IControlDatos> GetCuentasenDuplicadas(int skip, int take, string s = "");

        IList<IControlDatos> GetBeneficiariosMonedaIncorrecta();
        IList<IControlDatos> GetBeneficiariosMonedaIncorrecta(int idPrograma, int idEtapa);
        IList<IControlDatos> GetBeneficiariosMonedaIncorrecta(int skip, int take, string s = "");

        IList<IControlDatos> GetDatosErroneos(int idPrograma, int idEtapa);
        IList<IControlDatos> GetDatosErroneos(int skip, int take, int idPrograma, int idEtapa);


        IList<IControlDatos> GetBeneficiarioSinSucursalAsignada(int idPrograma, int idEtapa);
        IList<IControlDatos> GetBeneficiarioSinSucursalAsignada(int skip, int take, string s = "");
        IList<IControlDatos> GetBeneficiarioSinSucursalAsignada(int idprograma);
        IList<IControlDatos> GetBeneficiarioSinSucursalAsignada(int idprograma, int skip, int take);

        IList<IControlDatos> GetBeneficiarioSinFechaNotificacion(int idPrograma, int idEtapa);
        IList<IControlDatos> GetBeneficiarioSinFechaNotificacion(int skip, int take, string s = "");

        IList<IControlDatos> GetBeneficiarioConAltaTempranaSinFechaInicio(int idPrograma, int idEtapa);
        IList<IControlDatos> GetBeneficiarioConAltaTempranaSinFechaInicio(int skip, int take, string s = "");

        IList<IControlDatos> GetBeneficiarioSinCodigoPostaAsignado(int idPrograma, int idEtapa);
        IList<IControlDatos> GetBeneficiarioSinCodigoPostaAsignado(int skip, int take, string s = "");

        IList<IControlDatos> GetBeneficiarioConDocumentoDuplicado(int idPrograma, int idEtapa);
        IList<IControlDatos> GetBeneficiarioConDocumentoDuplicado(int skip, int take, string s = "");
    }
}
