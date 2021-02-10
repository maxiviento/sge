using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SGE.Servicio.Servicio;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Comun;
using SGE.Web.Reportes.Datasource;


namespace SGE.Web.Reportes
{
    public class FichaModel
    {

        //public virtual List<FichaReporte> LstFichas()
        //{
        //    FichaServicio _fichaServicio = new FichaServicio();

        //    var fichas = from c in (_fichaServicio.GetFichas2().Take(3))

        //                 select new FichaReporte

        //                 {
        //                     IdFicha = c.IdFicha,
        //                     Nombre = c.Nombre,
        //                     Apellido = c.Apellido,
        //                     NumeroDocumento = c.NumeroDocumento,
        //                     TipoFicha = c.TipoFicha,
        //                     Cuil = c.Cuil,

        //                 };

        //    return fichas.ToList();

        //}
        public virtual List<FichaReporte> LstFichas()
        {
            FichaServicio _fichaServicio = new FichaServicio();

            var fichas = from c in (_fichaServicio.GetFichaNotificacion())

                         select new FichaReporte

                         {
                             IdFicha = c.IdFicha,
                             Nombre = c.Nombre,
                             Apellido = c.Apellido,
                             NumeroDocumento = c.NumeroDocumento,
                             Cuil = c.Cuil,
                             CuitEmpresa = (c.TipoFicha == (int)Enums.TipoFicha.Ppp ? c.FichaPpp.Empresa.Cuit : (c.TipoFicha == (int)Enums.TipoFicha.PppProf ? c.FichaPppp.Empresa.Cuit : "0")),
                             DescripcionEmpresa = (c.TipoFicha == (int)Enums.TipoFicha.Ppp ? c.FichaPpp.Empresa.NombreEmpresa
                                                    : (c.TipoFicha == (int)Enums.TipoFicha.PppProf
                                                                    ? c.FichaPppp.Empresa.NombreEmpresa : ""))
                         };

            return fichas.ToList();

        }

        public virtual List<FichaReporte> LstFichasRpt(int[] idFicha)
        {
            FichaServicio _fichaServicio = new FichaServicio();

            var fichas = from c in (_fichaServicio.GetFichaNotificacion(idFicha))

                         select new FichaReporte

                         {
                             IdFicha = c.IdFicha,
                             Nombre = c.Nombre,
                             Apellido = c.Apellido,
                             NumeroDocumento = c.NumeroDocumento,
                             Cuil = c.Cuil,
                             CuitEmpresa = (c.TipoFicha == (int)Enums.TipoFicha.Ppp ? c.FichaPpp.Empresa.Cuit : (c.TipoFicha == (int)Enums.TipoFicha.PppProf ? c.FichaPppp.Empresa.Cuit : "0")),
                             DescripcionEmpresa = (c.TipoFicha == (int)Enums.TipoFicha.Ppp ? c.FichaPpp.Empresa.NombreEmpresa
                                                    : (c.TipoFicha == (int)Enums.TipoFicha.PppProf
                                                                    ? c.FichaPppp.Empresa.NombreEmpresa : ""))
                         };

            return fichas.ToList();

        }
    }
}