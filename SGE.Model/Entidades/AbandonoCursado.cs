using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class AbandonoCursado : IAbandonoCursado
    {
        public int ID_ABANDONO_CUR { get; set; }
        public string PERIODO { get; set; }
    }
}
