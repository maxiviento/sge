using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;
using System;

namespace SGE.Servicio.Vista
{
    public class BeneficiariosVista : IBeneficiariosVista
    {
        public BeneficiariosVista()
        {
            ConCuenta = "T";
            ConApoderado = "T";
            ConModalidad = "T";
            ConAltaTemprana = "T";
            ConDiscapacidad = "T";
            Programas = new ComboBox();
            EstadosBeneficiarios = new ComboBox();
            ErrorDatos = false;

            etapas = new ComboBox();
            EtapaProgramas = new ComboBox();
            ComboSubprogramas = new ComboBox();
            ComboTiposPpp = new ComboBox();

        }

        public IList<IBeneficiario> ListaBeneficiarios { get; set; }
        public IPager Pager { get; set; }
        public string FileDownload { get; set; }
        public string FileNombre { get; set; }
        public string ApellidoBusqueda { get; set; }
        public string NombreBusqueda { get; set; }
        public string CuilBusqueda { get; set; }
        public string NumeroDocumentoBusqueda { get; set; }
        public string ConCuenta { get; set; }
        public string ConAltaTemprana { get; set; }
        public IComboBox Programas { get; set; }
        public IComboBox EstadosBeneficiarios { get; set; }
        public string ApellidoNombreApoderadoBusqueda { get; set; }
        public bool ErrorDatos { get; set; }
        public string ConApoderado { get; set; }
        public string ConModalidad { get; set; }
        public string ConDiscapacidad { get; set; }
        public int[] idBeneficiarios { get; set; } // 26/02/2013 - DI CAMPLI LEANDRO - VER IBENEFICIARIOSVISTA


        // 21/05/2013 - DI CAMPLI LEANDRO - SE AÑADE EL FILTRO DE ETAPAS PARA BENEFICIARIOS
        public IComboBox etapas { get; set; }
        public int filtroEtapa { get; set; }
        public int programaSel { get; set; }
        public string filtroEjerc { get; set; }
        public string filtroNombre { get; set; }
        public IComboBox EtapaProgramas { get; set; }
        public string ejerc { get; set; }

        // 17/06/2013 - DI CAMPLI LEANDRO - PERMITE VERIFICAR SI SE ESTÁ BUSCANDO POR ETAPAS
        public string BuscarPorEtapa { get; set; }

        public DateTime FechaInicioBeneficio { get; set; } // 19/06/2013 - DI CAMPLI LEANDRO - FECHA INICIO PARA FILTRO
        public DateTime FechaBenefHasta { get; set; } // 24/06/2013 - DI CAMPLI LEANDRO - FECHA INICIO PARA FILTRO HASTA

        public IComboBox ComboSubprogramas { get; set; }
        public int idSubprograma { get; set; }
        public string ProgPorRol { get; set; }

        // 26/11/2016
        public IComboBox ComboTiposPpp { get; set; }
        public string idFormulario { get; set; }

        //25/02/2018
        public string nrotramite_busqueda { get; set; }
    }
}
