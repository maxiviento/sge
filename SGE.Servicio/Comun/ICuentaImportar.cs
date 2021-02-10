using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Servicio.Comun
{
    public interface ICuentaImportar
    {
        string Sucursal {get;set;}
        string NombreSucursal { get; set; }
        string NroCuenta {get;set;}
        string Apellido {get;set;}
        string SegundoApellido {get;set;}
        string Nombre {get;set;}
        string SegundoNombre {get;set;}
        string NumeroDocumento { get; set; }
        string Cbu {get;set;}
        bool Importado { get; set; }
        bool EsApoderado { get; set; }

    }
}
