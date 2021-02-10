﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class DetalleRecuDeb
    {
        public int ID_DETALLE_REC_DEB { get; set; }
        public int ID_RECUPERO_DEB { get; set; }
        public int ID_BENEFICIARIO_CONCEP_LIQ { get; set; }
        public int ID_USR_SIST { get; set; }
        public decimal MONTO { get; set; }
        public DateTime? FEC_SIST { get; set; }
        public int ID_USR_MODIF { get; set; }
        public DateTime? FEC_MODIF { get; set; }
         
        public int TIPO_PPP { get; set; }
    }
}
