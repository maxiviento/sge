using System.ComponentModel.DataAnnotations;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class FichaPppVista : IFichaPppVista
    {
        public FichaPppVista()
        {
            Empresas = new ComboBox();
            Sedes = new ComboBox();
        }

        [Key]
        [Display(Name = "Id:")]
        public int Id_Ficha { get; set; }

        [Display(Name = "*Nivel de Escolaridad")]
        public int? Id_Nivel_Escolaridad { get; set; }

        [Display(Name = "*Cursando")]
        public bool Cursando { get; set; }

        [Display(Name = "*Finalizado")]
        public bool Finalizado { get; set; }

        [Display(Name = "*Empresas")]
        public IComboBox Empresas { get; set; }

        [Display(Name = "*Modalidad")]
        public int? Modalidad{ get; set; }

        [Display(Name = "*Tareas")]
        public string Tareas { get; set; }

        [Display(Name = "*Desea Terminar Nivel")]
        public bool Desea_Term_Nivel { get; set; }

        [Display(Name = "*Sedes")]
        public IComboBox Sedes { get; set; }

        public string Accion { get; set; }

    }
}
