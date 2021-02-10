using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Servicio.VistaInterfaces;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IOngServicio
    {
        IList<IOng> getOngs(string cuit);
        IOng GetOng(int idOng);
        IList<IOng> getOngsNom(string nombre);
        IOngsVista GetOngs(string NombreOng);
        IOngsVista GetIndex();
        IOngsVista GetOngs(IPager pPager, string NombreOng);
        IOngVista GetOng(int idOng, string accion);
        IOngVista GetOngReload(IOngVista vista);
        int AddOng(IOngVista vista);
        int UpdateOng(IOngVista vista);
        IList<IOng> GetOngsId(int id);

    }
}
