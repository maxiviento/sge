using System;
using System.Collections.Generic;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Servicio
{
   public class ProyectoServicio : IProyectoServicio
    {
       private readonly IProyectoRepositorio _proyectoRepositorio;

       public ProyectoServicio()
       {
           _proyectoRepositorio = new ProyectoRepositorio();
       }
       public IProyectoVista GetProyectos(string NombreProyecto)
        {

            IProyectoVista proyectoVista = new ProyectoVista();
           IList<IProyecto> proyectos;
           proyectos = _proyectoRepositorio.GetProyectos(NombreProyecto);
           proyectoVista.Proyectos = proyectos;

           return proyectoVista;
        }
    }
}
