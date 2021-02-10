using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Titulo : ITitulo
    {
        public Int16 IdTitulo { get; set; }
        public Int16? IdUniversidad { get; set; }
        public string Descripcion { get; set; }
    }
}
