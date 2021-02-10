using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;
using System;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IBeneficiariosVista
    {
        IList<IBeneficiario> ListaBeneficiarios { get; set; }
        IPager Pager { get; set; }
        string FileDownload { get; set; }
        string FileNombre { get; set; }
        string ApellidoBusqueda { get; set; }
        string NombreBusqueda { get; set; }
        string CuilBusqueda { get; set; }
        string NumeroDocumentoBusqueda { get; set; }
        string ConCuenta { get; set; }
        string ConApoderado { get; set; }
        string ConModalidad { get; set; }
        string ConDiscapacidad { get; set; }
        string ConAltaTemprana { get; set; }
        IComboBox Programas { get; set; }
        IComboBox EstadosBeneficiarios { get; set; }
        string ApellidoNombreApoderadoBusqueda { get; set; }
        bool ErrorDatos { get; set; }
        int[] idBeneficiarios { get; set; } // 26/02/2013 - DI CAMPLI LEANDRO - SE AÑADE ARRAY PARA CAPTURAR LOS IDBENEFICIARIO PARA OPTIMIZAR LAS CONSULTAS.

        // 21/05/2013 - DI CAMPLI LEANDRO - SE AÑADE EL FILTRO DE ETAPAS PARA BENEFICIARIOS
        IComboBox etapas { get; set; }
        int filtroEtapa { get; set; }
        int programaSel { get; set; }
        string filtroEjerc { get; set; }
        string filtroNombre { get; set; }
        IComboBox EtapaProgramas { get; set; }
        string ejerc { get; set; }

        // 17/06/2013 - DI CAMPLI LEANDRO - PERMITE VERIFICAR SI SE ESTÁ BUSCANDO POR ETAPAS
        string BuscarPorEtapa { get; set; }

        DateTime FechaInicioBeneficio { get; set; } // 19/06/2013 - DI CAMPLI LEANDRO - FECHA INICIO PARA FILTRO
        DateTime FechaBenefHasta { get; set; } // 24/06/2013 - DI CAMPLI LEANDRO - FECHA INICIO PARA FILTRO HASTA

        int idSubprograma { get; set; }
        string ProgPorRol { get; set; }

        // 26/11/2016
        IComboBox ComboTiposPpp { get; set; }
        string idFormulario { get; set; }
        IComboBox ComboSubprogramas { get; set; }
        //25/02/2018
        string nrotramite_busqueda { get; set; }

    }
}
