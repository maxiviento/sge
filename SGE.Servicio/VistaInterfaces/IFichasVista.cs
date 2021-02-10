using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;
using System;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IFichasVista
    {
        IList<IFicha> Fichas { get; set; }
        IPager pager { get; set; }
        string cuil_busqueda { get; set; }
        string nro_doc_busqueda { get; set; }
        string nombre_busqueda { get; set; }
        string apellido_busqueda { get; set; }
        int tipoficha { get; set; }
        int estadoficha { get; set; }
        IComboBox ComboTipoFicha { get; set; }
        IComboBox ComboEstadoFicha { get; set; }
        string ExcelNombre { get; set; }
        string ExcelUrl { get; set; }
        bool ProcesarExcel { get; set; }
        int cantProcesados { get; set; } // 21/02/2013 - DI CAMPLI LEANDRO - CARGA MASIVA DE BENEFICIARIOS


        // 10/06/2013 - DI CAMPLI LEANDRO - SE AÑADE EL FILTRO DE ETAPAS PARA BENEFICIARIOS
        IComboBox etapas { get; set; }
        int filtroEtapa { get; set; }
        int programaSel { get; set; }
        string filtroEjerc { get; set; }
        string filtroNombre { get; set; }
        IComboBox EtapaProgramas { get; set; }
        string ejerc { get; set; }

        // 17/06/2013 - DI CAMPLI LEANDRO - PERMITE VERIFICAR SI SE ESTÁ BUSCANDO POR ETAPAS
        string BuscarPorEtapa { get; set; }

        //08/01/2014 gestor
        string idEstadoFicha { get; set; }
        // 28/04/2014
        DateTime FechaInicioBeneficiario { get; set; }

        int idSubprograma { get; set; }
        string ProgPorRol { get; set; }

        // 25/11/2016
        IComboBox ComboTiposPpp { get; set; }
        string idFormulario { get; set; }

        // 08/01/2018
        int Empadronados { get; set; }
        int EmpadronadosFail { get; set; }

        //25/02/2018
        int nrocaja { get; set; }
        int tipoppp { get; set; }
        string nrotramite_busqueda { get; set; }
    }
}
