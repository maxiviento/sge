using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class SubprogramaRol : ISubprogramaRol
    {
        public int Id_Rol { get; set; }
        public string Nombre_Rol { get; set; }
        public int Id_Estado_Rol { get; set; }
        public int IdSubprograma { get; set; }
        public int IdPrograma { get; set; }
        public string NombreSubprograma { get; set; }
        public string Descripcion { get; set; }
        public decimal monto { get; set; }
    }
}
