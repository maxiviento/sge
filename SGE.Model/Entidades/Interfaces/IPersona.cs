using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IPersona
    {
        int id_persona { get; set; }
        string apellido           { get; set; }
        string nombre             { get; set; }
        string dni                { get; set; }
        string cuil               { get; set; }
        string celular            { get; set; }
        string telefono           { get; set; }
        string mail               { get; set; }
        int? id_barrio { get; set; }
        int? id_localidad { get; set; }
        string empleado           { get; set; }
        int? id_usr_sist { get; set; }
        DateTime? fec_sist { get; set; }
        int? id_usr_modif { get; set; }
        DateTime? fec_modif { get; set; }
    }
}
