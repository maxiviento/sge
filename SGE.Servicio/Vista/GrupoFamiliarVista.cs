using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Comun;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class GrupoFamiliarVista : IGrupoFamiliarVista
    {
        public GrupoFamiliarVista()
        {
            vinculos = new ComboBox();
        
        }

        public int idficha { get; set; }
        public int idPersona { get; set; }
        public string numero_documento { get; set; }
        public string Nombres { get; set; }
        public string Apellido { get; set; }
        public int idVinculo { get; set; }
        public string vinculo { get; set; }
        public IComboBox vinculos { get; set; }
        public IList<IRelacionFam> Familiares { get; set; }

        public string Accion { get; set; }
        public string AccionLlamada { get; set; }
    }
}
