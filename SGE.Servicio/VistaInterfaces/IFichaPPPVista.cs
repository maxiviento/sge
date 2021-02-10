using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IFichaPppVista
    {
       int Id_Ficha { get; set; }
       int? Id_Nivel_Escolaridad { get; set; }
       bool Cursando { get; set; }
       bool Finalizado { get; set; }
       IComboBox Empresas { get; set; }
       int? Modalidad { get; set; }
       string Tareas { get; set; }
       bool Desea_Term_Nivel { get; set; }
       IComboBox Sedes { get; set; }
       string Accion { get; set; }
    }
}
