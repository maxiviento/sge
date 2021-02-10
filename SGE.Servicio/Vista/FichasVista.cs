using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;
using System;

namespace SGE.Servicio.Vista
{
    public class FichasVista : IFichasVista
    {
        public FichasVista()
        {
            ComboTipoFicha = new ComboBox();
            ComboEstadoFicha = new ComboBox();
            ProcesarExcel = false;
            EtapaProgramas = new ComboBox();
            etapas = new ComboBox();

            ComboSubprogramas = new ComboBox();
            ComboTiposPpp = new ComboBox();
        }

        public IList<IFicha> Fichas { get; set; }
        public IPager pager { get; set; }
        public string cuil_busqueda { get; set; }
        public string nro_doc_busqueda { get; set; }
        public string nombre_busqueda { get; set; }
        public string apellido_busqueda { get; set; }
        public int tipoficha { get; set; }
        public int estadoficha { get; set; }
        public IComboBox ComboTipoFicha { get; set; }
        public IComboBox ComboEstadoFicha { get; set; }
        public string ExcelNombre { get; set; }
        public string ExcelUrl { get; set; }
        public bool ProcesarExcel { get; set; }
        public int cantProcesados { get; set; } // 21/02/2013 - DI CAMPLI LEANDRO - CARGA MASIVA DE BENEFICIARIOS


        // 10/06/2013 - DI CAMPLI LEANDRO - SE AÑADE EL FILTRO DE ETAPAS PARA BENEFICIARIOS
        public IComboBox etapas { get; set; }
        public int filtroEtapa { get; set; }
        public int programaSel { get; set; }
        public string filtroEjerc { get; set; }
        public string filtroNombre { get; set; }
        public IComboBox EtapaProgramas { get; set; }
        public string ejerc { get; set; }

        // 17/06/2013 - DI CAMPLI LEANDRO - PERMITE VERIFICAR SI SE ESTÁ BUSCANDO POR ETAPAS
        public string BuscarPorEtapa { get; set; }

        //08/01/2014 gestor
        public string idEstadoFicha { get; set; }

        // 28/04/2014 - Di Campli Leandro - Fecha inicio beneficio en carga masivas de beneficiarios
        public DateTime FechaInicioBeneficiario { get; set; }

        // 22/08/2014 - DI CAMPLI LEANDRO - SE AGREGAN FILTROS PARA SUBPROGRAMAS
        public IComboBox ComboSubprogramas { get; set; }

        public int idSubprograma { get; set; }
        public string ProgPorRol { get; set; }

        // 25/11/2016
        public IComboBox ComboTiposPpp { get; set; }
        public string idFormulario { get; set; }

        // 08/01/2018
        public int Empadronados { get; set; }
        public int EmpadronadosFail { get; set; }

        //25/02/2018
        public int nrocaja { get; set; }
        public int tipoppp { get; set; }
        public string nrotramite_busqueda { get; set; }
    }
}
 