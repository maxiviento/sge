using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Archivo : IArchivo
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public string Nombre { get; set; }
        public int CantidadRegistros { get; set; }
    }
}
