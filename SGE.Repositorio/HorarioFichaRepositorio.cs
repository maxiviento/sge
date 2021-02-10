using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class HorarioFichaRepositorio : BaseRepositorio, IHorarioFichaRepositorio
    {
        private readonly DataSGE _mdb;

        public HorarioFichaRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IHorarioFicha> QHorarioFicha()
        {
            var a =
                _mdb.T_FICHAS_EMP_HOR.Select(
                    c =>
                    new HorarioFicha
                        {
                            IdEmpresa = c.T_EMP_HORARIOS.ID_EMPRESA,
                            Corte = c.CORTE,
                            FechaSistema = c.FEC_SIST,
                            IdEmpresaHorario = c.ID_EMP_HORARIO,
                            IdFicha = c.ID_FICHA,
                            IdUsuarioSistema = c.ID_USR_SIST,
                            Observacion = c.OBSERVACION,
                            UsuarioSistema = c.T_USUARIOS.LOGIN,
                            DSemana = c.T_EMP_HORARIOS.ID_DIA
                        });

            return a;
        }

        public IList<IHorarioFicha> GetHorariosFicha(int idficha)
        {
            return QHorarioFicha().Where(c => c.IdFicha == idficha).ToList();
        }

        public IList<IHorarioFicha> GetHorariosFichaByDia(int idficha, string dia)
        {
            return QHorarioFicha().Where(c => c.IdFicha == idficha && c.DSemana == dia).ToList();
        }

        public int CountCortes(int idficha, string diasemana)
        {

            var a = (from c in _mdb.T_FICHAS_EMP_HOR
                     join teh in _mdb.T_EMP_HORARIOS on c.ID_EMP_HORARIO equals teh.ID_EMP_HORARIO
                     where teh.ID_DIA == diasemana && c.ID_FICHA == idficha
                     select c.CORTE).Distinct().Count();

            return a;
        }

        public bool AddHorarioFicha(IList<IHorarioFicha> lista, int idficha)
        {
            IComunDatos datos = new ComunDatos();

            

            AgregarDatos(datos);


            if (lista.Count > 0)
            {
                // 2403/2013 - DI CAMPLI LEANDRO -verificar si la lista de horarios nueva no es igual al de la base
                //de lo contrario no guardar nada
                IList<IHorarioFicha> listaOld;
                int intLista = (int)lista[0].IdFicha;
                listaOld = QHorarioFichaOld().Where(c => c.IdFicha == intLista).ToList();//GetHorariosFicha(lista[0].IdFicha);
                int udpHorario = 0;

                if (lista.Count() == listaOld.Count())
                {
                    foreach (var horarioFicha in lista)
                    {
                        if (udpHorario == 1)
                        {
                            break;
                        }

                        foreach (var horarioFichaOld in listaOld)
                        {

                            if (horarioFichaOld.IdEmpresaHorario == horarioFicha.IdEmpresaHorario &&
                                horarioFichaOld.IdFicha == horarioFicha.IdFicha &&
                                horarioFichaOld.Observacion == horarioFicha.Observacion &&
                                horarioFichaOld.Corte == horarioFicha.Corte)//(horarioFichaOld==horarioFicha/*horarioFichaOld.Equals(horarioFicha)*/)
                            {
                                udpHorario = 0;
                                break;
                            }
                            else
                            {
                                udpHorario = 1;

                            }
                        }


                    }
                }
                else { udpHorario = 1; }

                if (udpHorario == 0) { return false; }

                _mdb.ExecuteStoreCommand(
                    string.Format(
                        "delete from " + ConfigurationManager.AppSettings["EsquemaDB"] +
                        ".T_FICHAS_EMP_HOR where ID_FICHA = {0}",
                        lista[0].IdFicha));
            }
            else
            {
                _mdb.ExecuteStoreCommand(string.Format("delete from " + ConfigurationManager.AppSettings["EsquemaDB"] + ".T_FICHAS_EMP_HOR where ID_FICHA = {0}", idficha));
            }


            foreach (var horarioFicha in lista)
            {
                var fihonew = new T_FICHAS_EMP_HOR
                          {
                              ID_FICHA = horarioFicha.IdFicha,
                              CORTE = horarioFicha.Corte,
                              ID_EMP_HORARIO = horarioFicha.IdEmpresaHorario,
                              ID_USR_SIST = datos.IdUsuarioSistema,
                              OBSERVACION = horarioFicha.Observacion,
                              FEC_SIST = datos.FechaSistema
                          };

                _mdb.T_FICHAS_EMP_HOR.AddObject(fihonew);

            }

            _mdb.SaveChanges();

            return true;
        }


       public IList<IHorarioFicha> GetHorarioFichaByEmpresa(int idficha, int idempresa)
       {
            return QHorarioFicha().Where(c => c.IdFicha == idficha && c.IdEmpresa == idempresa)
                .OrderBy(o=> o.Corte)
                .ToList();

       }

       private IList<IHorarioFicha> QHorarioFichaOld()
       {
           //string x = "";
           var a =
               _mdb.T_FICHAS_EMP_HOR.Select(
                   c =>
                   new HorarioFicha
                   {
                       IdEmpresaHorario = c.ID_EMP_HORARIO,//IdEmpresaHorario = Convert.ToInt32(horarioFicha.Selected),
                       IdFicha = c.ID_FICHA,//IdFicha = ficha.IdFicha,                       
                       Observacion = c.OBSERVACION ?? "",//Observacion = ficha.ObsDomingo[posdomingo],
                       Corte = c.CORTE//Corte = cortedomingo
                   });

           IList<IHorarioFicha> listaHFadd = new List<IHorarioFicha>();
           foreach (IHorarioFicha f in a)
           {
               IHorarioFicha hf = new HorarioFicha
               {
                   IdEmpresaHorario = Convert.ToInt32(f.IdEmpresaHorario),
                   IdFicha = f.IdFicha,
                   Observacion = f.Observacion ?? "",
                   Corte = f.Corte
               };
               listaHFadd.Add(hf);
           }

           return listaHFadd;//a;
       }



       public string GetFecUltimaModif(int idFicha)
       { 
       
         var  fec = (from f in _mdb.T_FICHAS_EMP_HOR
                      where f.ID_FICHA ==  idFicha
                         select f.FEC_SIST).Max().ToString();
         if (fec != "" && fec != null)
         {
             return fec.Substring(0, 10);
         }
         else
         {
             return string.Empty;
         }
       
       }


    }
}
