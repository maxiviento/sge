using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;
using System.Data.Common;

namespace SGE.Repositorio
{
    public class EtapaRepositorio : BaseRepositorio
    {

                private readonly DataSGE _mdb;

                public EtapaRepositorio()
                {
                    _mdb = new DataSGE();
                }

                public IQueryable<IEtapa> QEtapas()
                { 
                    var etapas = from e in  _mdb.T_ETAPAS
                                                select new Etapa
                        {
                                    ID_ETAPA = e.ID_ETAPA,
                                    ID_PROGRAMA = e.ID_PROGRAMA,
                                    N_ETAPA = e.N_ETAPA,
                                    MONTO_ETAPA = e.MONTO_ETAPA ?? 0,
                                    EJERCICIO = e.EJERCICIO,
                                    FEC_INCIO = e.FEC_INCIO,
                                    FEC_FIN = e.FEC_FIN,
                                    ID_USR_SIST = e.ID_USR_SIST,
                                    FEC_SIST = e.FEC_SIST,
                                    ID_USR_MODIF = e.ID_USR_MODIF,
                                    FEC_MODIF = e.FEC_MODIF,
                                    N_PROGRAMA = e.T_PROGRAMAS.N_PROGRAMA
                        
                        };
                    
                    return etapas;
                }

                public IList<IEtapa> getEtapas(string ejercicio, int idprograma)
                {
                    ejercicio = ejercicio == "" ? null : ejercicio;

                    var etapas = QEtapas().Where(x => (x.EJERCICIO == ejercicio || ejercicio == null) && (x.ID_PROGRAMA == idprograma || idprograma==0)).ToList();
                    return etapas;
                
                }

                public IEtapa getEtapa(int idetapa)
                {


                    var etapa = QEtapas().Where(x => x.ID_ETAPA == idetapa).FirstOrDefault();
                    return etapa;

                }

        public int addEtapa(IEtapa etapa)
        {
            int idEtapa = 0;
                        var a = (from c in _mdb.T_ETAPAS
                     select c.ID_ETAPA);
                        if (a.Count()>0)
                        {
                            idEtapa = a.Max();
                        }

                        T_ETAPAS Etapa = new T_ETAPAS
                        {
                            ID_ETAPA =idEtapa + 1,
                            N_ETAPA = etapa.N_ETAPA,
                            ID_PROGRAMA = (short)etapa.ID_PROGRAMA,
                            EJERCICIO = etapa.EJERCICIO,
                            FEC_INCIO = etapa.FEC_INCIO,
                            FEC_FIN = etapa.FEC_FIN,
                            MONTO_ETAPA = etapa.MONTO_ETAPA,
                            ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                            FEC_SIST = DateTime.Now

                        };

                        _mdb.T_ETAPAS.AddObject(Etapa);
                        _mdb.SaveChanges();

            return etapa.ID_ETAPA;
        }


        public int udpEtapa(Etapa vista)
        {
            if (vista.ID_ETAPA != 0)
            {
                
                var etapa = _mdb.T_ETAPAS.Where(x => x.ID_ETAPA == vista.ID_ETAPA).SingleOrDefault();

                if (etapa != null)
                {
                    etapa.N_ETAPA = vista.N_ETAPA;
                    etapa.ID_PROGRAMA = (short)vista.ID_PROGRAMA;
                    etapa.EJERCICIO = vista.EJERCICIO;
                    etapa.FEC_INCIO = vista.FEC_INCIO;
                    etapa.FEC_FIN = vista.FEC_FIN;
                    etapa.MONTO_ETAPA = vista.MONTO_ETAPA;
                    etapa.ID_USR_MODIF = GetUsuarioLoguer().Id_Usuario;
                    etapa.FEC_MODIF = DateTime.Now;

                    _mdb.SaveChanges();
                    return 0;
                }else { return 2; }
            }
            return 1;
        }


        //public void DeleteCarrera(IEtapa vista)
        //{
        //    //var a = (from f in _mdb.T_FICHAS.Where(x=>x.ID_ETAPA == vista.ID_ETAPA).Count();
        //    //if(a>0){return 1;}
            
        //    var row = _mdb.T_ETAPAS.SingleOrDefault(c => c.ID_ETAPA == vista.ID_ETAPA);

        //    if (row.ID_ETAPA == 0) return;

        //    _mdb.DeleteObject(row);
        //    _mdb.SaveChanges();
        //}


        public int udpEtapaFichas(IList<IFicha> fichas, int idEtapa)
        {
            int cant = 0;
            try
            {
                foreach (Ficha item in fichas)
                {
                    IComunDatos comun = new ComunDatos();

                    AgregarDatos(comun);

                    var f = _mdb.T_FICHAS.FirstOrDefault(c => c.ID_FICHA == item.IdFicha);

                    f.ID_ETAPA = idEtapa;
                    f.ID_USR_MODIF = comun.IdUsuarioSistema;
                    f.FEC_MODIF = comun.FechaSistema;

                    _mdb.SaveChanges();

                    cant++;

                }
                return cant;
            }
            catch (Exception ex)
            {
                return cant;
            }
            finally
            {
                
            }

        }

    }

}
