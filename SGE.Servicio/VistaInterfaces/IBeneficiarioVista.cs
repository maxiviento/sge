using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IBeneficiarioVista : IFichaVista
    {
        int IdBeneficiario { get; set; }
        int IdPrograma { get; set; }
        int IdEstado { get; set; }
        string Email { get; set; }
        ComboBox Estados { get; set; }
        string Programa { get; set; }

        bool TieneApoderadoActivo { get; set; }
        bool TieneApoderado { get; set; }
        IList<IApoderado> ListaApoderados { get; set; }

        int? NumeroCuenta { get; set; }
        string Cbu { get; set; }
        string Sucursal { get; set; }
        DateTime? FechaSolicitudCuenta { get; set; }
        bool ClearFechaSolicitud { get; set; }
        IComboBox Sucursales { get; set; }
        bool SeGuardo { get; set; }
        IComboBox Monedas { get; set; }
        IList<IConcepto> ListaConceptosPago { get; set; }

        DateTime? FechaNotificacion { get; set; }
        bool Notificado { get; set; }


        //SE AÑADEN EN LA TABLA T_BENEFICIARIOS LA FECHA DE INICIO Y BAJA DEL BENEFICIO PARA REALIZAR
        //LA LIQUIDACION Y DEJAR DE DEPENDER DE LA FECHA DE NOTIFICACION

        DateTime? FechaInicioBeneficio { get; set; }
        DateTime? FechaBajaBeneficio { get; set; }
       
    }
}
