using System;
using SGE.Model.Entidades.Interfaces;
namespace SGE.Model.Entidades
{
    public class RelacionFam : IRelacionFam
    {

        public int? ID_FICHA { get; set; }
        public int? ID_PERSONA { get; set; }
        public string APELLIDO { get; set; }
        public string NOMBRE { get; set; }
        public string DNI { get; set; }
        public int? ID_VINCULO { get; set; }
        public string N_VINCULO { get; set; }

    }
}
