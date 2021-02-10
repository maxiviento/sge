using System;
using System.ComponentModel.DataAnnotations;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Entidades;

namespace SGE.Servicio.Vista
{
    public class PersonaVista : IPersonaVista
    {

        public PersonaVista()
        {
            Localidades = new ComboBox();
            barrios = new ComboBox();
            ActorRoles = new ComboBox();
            listActorRol = new List<IActorVista>();
            departamentos = new ComboBox();

        }
        [Key]
        [Display(Name = "Id:")]
        public int id_persona { get; set; }
        [StringLength(100, ErrorMessage = "Apellido supera los 100 caracteres.")]
        [Required(ErrorMessage = "Ingrese el Apellido")]
        public string apellido { get; set; }
        [Display(Name = "*Nombre")]
        [StringLength(100, ErrorMessage = "Nombre supera los 100 caracteres.")]
        [Required(ErrorMessage = "Ingrese el Nombre")]
        public string nombre { get; set; }
        [Range(0, 9999999999999999999, ErrorMessage = "Valor incorrecto para el Número de Documento.")]
        [Required(ErrorMessage = "Ingrese el Número de Documento.")]
        public string dni { get; set; }
        [Display(Name = "*CUIL")]
        [StringLength(13, ErrorMessage = "CUIL supera los 13 caracteres.")]
        //[Required(ErrorMessage = "Ingrese el CUIL.")]
        public string cuil { get; set; }
        public string celular { get; set; }
        public string telefono { get; set; }
        public string mail { get; set; }
        public int? id_barrio { get; set; }
        public int? id_localidad { get; set; }
        public string empleado { get; set; }
        public int? id_usr_sist { get; set; }
        public DateTime? fec_sist { get; set; }
        public int? id_usr_modif { get; set; }
        public DateTime? fec_modif { get; set; }
        
        public string Accion { get; set; }
        public string AccionLlamada { get; set; }
        public IComboBox Localidades { get; set; }
        public IComboBox barrios { get; set; }
        public IComboBox ActorRoles { get; set; }
        public IComboBox departamentos { get; set; }
        public IList<IActorVista> listActorRol { get; set; }
        public IActorVista newActor { get; set; }
        
    }
}
