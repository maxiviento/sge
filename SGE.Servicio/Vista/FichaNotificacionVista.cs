using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Servicio.Vista
{
    class FichaNotificacionVista
    {

        //datos de la ficha
        public int IdFicha { get; set; }
        public int TipoFicha { get; set; }
        public string Cuil { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimineto { get; set; }
        public int TipoDocumento { get; set; }
        //public ComboBox TipoDocumentoCombo { get; set; }
        public string strTipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Piso { get; set; }
        //public ComboBox Localidades { get; set; }
        public string strLocalidad { get; set; }
        //public int IdDepartamento { get; set; }
        //public ComboBox Departamentos { get; set; }

        //DATOS DE LA EMPRESA
        //PPP
        public int? IdEmpresa { get; set; }
        public int? IdEmpresaInicial { get; set; }
        public string CuitEmpresa { get; set; }
        public string DescripcionEmpresa { get; set; }
        public string EmpresaLocalidad { get; set; }
        public string EmpresaCodigoActividad { get; set; }
        public string EmpresaDomicilioLaboralIdem { get; set; }
        public short? EmpresaCantidadEmpleados { get; set; }
        public string EmpresaCalle { get; set; }
        public string EmpresaNumero { get; set; }
        public string EmpresaPiso { get; set; }
        public string EmpresaDpto { get; set; }
        public string EmpresaCodigoPostal { get; set; }
        public string Tareas { get; set; }
        public string LugarLlamada { get; set; }
        public int IdLlamada { get; set; }

    }
}
