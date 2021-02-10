using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class ControlDatosRepositorio : IControlDatosRepositorio
    {

        private readonly DataSGE _mdb;

        public ControlDatosRepositorio()
        {
            _mdb = new DataSGE();
        }

        public IList<IControlDatos> GetApoderadosDuplicadosActivos()
        {
            int[] listabeneficiarios =
                (from apo in _mdb.T_APODERADOS
                 where apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo
                 group apo by apo.ID_BENEFICIARIO
                     into g
                     where g.Count() > 1
                     select g.Key ?? 0).ToArray();


            var retorno = (from be in _mdb.T_BENEFICIARIOS
                           join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                           from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                           from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()

                           where
                               listabeneficiarios.Contains(be.ID_BENEFICIARIO) &&
                               be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo

                           select
                               new ControlDatos
                               {
                                   Id = be.ID_BENEFICIARIO,
                                   Descripcion = "Beneficiario con Apoderado duplicado Activo.",
                                   Identificador1 = be.T_FICHAS.NUMERO_DOCUMENTO,
                                   Identificador2 = be.T_FICHAS.APELLIDO,
                                   Identificador3 = be.T_FICHAS.NOMBRE,
                                   ModuloAccion = "Beneficiario",
                                   idEtapa = fi.ID_ETAPA,
                                   Etapa = eta.N_ETAPA,
                                   IdPrograma = (short)be.ID_PROGRAMA,
                                   N_Programa = pro.N_PROGRAMA

                               }).ToList().Cast<IControlDatos>().ToList();

            return retorno;

        }
        public IList<IControlDatos> GetApoderadosDuplicadosActivos(int idPrograma, int idEtapa)
        {
            int[] listabeneficiarios =
                (from apo in _mdb.T_APODERADOS
                 where apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo
                 group apo by apo.ID_BENEFICIARIO
                     into g
                     where g.Count() > 1
                     select g.Key ?? 0).ToArray();


            var retorno = (from be in _mdb.T_BENEFICIARIOS
                           join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                           from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                           from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()

                           where
                               listabeneficiarios.Contains(be.ID_BENEFICIARIO) &&
                               be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo
                               && (idPrograma == 0 ? be.ID_PROGRAMA > 0 : be.ID_PROGRAMA == idPrograma) 
                               && (fi.ID_ETAPA == idEtapa || idEtapa == 0)
                           select
                               new ControlDatos
                                   {
                                       Id = be.ID_BENEFICIARIO,
                                       Descripcion = "Beneficiario con Apoderado duplicado Activo.",
                                       Identificador1 = be.T_FICHAS.NUMERO_DOCUMENTO,
                                       Identificador2 = be.T_FICHAS.APELLIDO,
                                       Identificador3 = be.T_FICHAS.NOMBRE,
                                       ModuloAccion = "Beneficiario",
                                       idEtapa = fi.ID_ETAPA,
                                       Etapa = eta.N_ETAPA,
                                       IdPrograma = (short)be.ID_PROGRAMA,
                                       N_Programa = pro.N_PROGRAMA

                                   }).ToList().Cast<IControlDatos>().ToList();

            return retorno;

        }

        public IList<IControlDatos> GetApoderadosDuplicadosActivos(int idprograma)
        {
            int[] listabeneficiarios =
                (from apo in _mdb.T_APODERADOS
                 where
                     apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo &&
                     apo.T_BENEFICIARIOS.ID_PROGRAMA == idprograma
                 group apo by apo.ID_BENEFICIARIO
                     into g
                     where g.Count() > 1
                     select g.Key ?? 0).ToArray();


            var retorno = (from be in _mdb.T_BENEFICIARIOS
                           where
                               listabeneficiarios.Contains(be.ID_BENEFICIARIO) &&
                               be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo
                           select
                               new ControlDatos
                               {
                                   Id = be.ID_BENEFICIARIO,
                                   Descripcion = "Beneficiario con Apoderado duplicado Activo.",
                                   Identificador1 = be.T_FICHAS.NUMERO_DOCUMENTO,
                                   Identificador2 = be.T_FICHAS.APELLIDO,
                                   Identificador3 = be.T_FICHAS.NOMBRE,
                                   ModuloAccion = "Beneficiario"
                               }).ToList().Cast<IControlDatos>().ToList();


            return retorno;
        }

        public IList<IControlDatos> GetApoderadosDuplicadosActivos(int[] lista)
        {
            int[] listabeneficiarios =
                (from apo in _mdb.T_APODERADOS
                 where
                     apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo &&
                     lista.Contains(apo.T_BENEFICIARIOS.ID_BENEFICIARIO)
                 group apo by apo.ID_BENEFICIARIO
                     into g
                     where g.Count() > 1
                     select g.Key ?? 0).ToArray();


            var retorno = (from be in _mdb.T_BENEFICIARIOS
                           where
                               listabeneficiarios.Contains(be.ID_BENEFICIARIO) &&
                               be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo
                           select
                               new ControlDatos
                               {
                                   Id = be.ID_BENEFICIARIO,
                                   Descripcion = "Beneficiario con Apoderado duplicado Activo.",
                                   Identificador1 = be.T_FICHAS.NUMERO_DOCUMENTO,
                                   Identificador2 = be.T_FICHAS.APELLIDO,
                                   Identificador3 = be.T_FICHAS.NOMBRE,
                                   ModuloAccion = "Beneficiario"
                               }).ToList().Cast<IControlDatos>().ToList();


            return retorno;
        }

        public IList<IControlDatos> GetApoderadosDuplicadosActivos(int skip, int make, string s = "")
        {
            int[] listabeneficiarios =
               (from apo in _mdb.T_APODERADOS
                where apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo
                group apo by apo.ID_BENEFICIARIO
                    into g
                    where g.Count() > 1
                    select g.Key ?? 0).ToArray();


            var retorno = (from be in _mdb.T_BENEFICIARIOS
                           where
                               listabeneficiarios.Contains(be.ID_BENEFICIARIO) &&
                               be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo
                           select
                               new ControlDatos
                                   {
                                       Id = be.ID_BENEFICIARIO,
                                       Descripcion = "Beneficiario con Apoderado duplicado Activo.",
                                       Identificador1 = be.T_FICHAS.NUMERO_DOCUMENTO,
                                       Identificador2 = be.T_FICHAS.APELLIDO,
                                       Identificador3 = be.T_FICHAS.NOMBRE,
                                       ModuloAccion = "Beneficiario"
                                   }).ToList().Cast<IControlDatos>().Skip(skip).Take(make).ToList();


            return retorno;
        }

        public IList<IControlDatos> GetApoderadosDuplicadosActivos(int idprograma, int skip, int make)
        {
            int[] listabeneficiarios =
              (from apo in _mdb.T_APODERADOS
               where
                   apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo &&
                   apo.T_BENEFICIARIOS.ID_PROGRAMA == idprograma
               group apo by apo.ID_BENEFICIARIO
                   into g
                   where g.Count() > 1
                   select g.Key ?? 0).ToArray();


            var retorno = (from be in _mdb.T_BENEFICIARIOS

                           where
                               listabeneficiarios.Contains(be.ID_BENEFICIARIO) &&
                               be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo
                           select
                               new ControlDatos
                                   {
                                       Id = be.ID_BENEFICIARIO,
                                       Descripcion = "Beneficiario con Apoderado duplicado Activo.",
                                       Identificador1 = be.T_FICHAS.NUMERO_DOCUMENTO,
                                       Identificador2 = be.T_FICHAS.APELLIDO,
                                       Identificador3 = be.T_FICHAS.NOMBRE,
                                       ModuloAccion = "Beneficiario"
                                   }).ToList().Cast<IControlDatos>().Skip(skip).Take(make).ToList();


            return retorno;
        }

        public IList<IControlDatos> GetApoderadosDuplicadosActivos(int[] lista, int skip, int make)
        {
            int[] listabeneficiarios =
               (from apo in _mdb.T_APODERADOS
                where
                    apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo &&
                    lista.Contains(apo.T_BENEFICIARIOS.ID_BENEFICIARIO)
                group apo by apo.ID_BENEFICIARIO
                    into g
                    where g.Count() > 1
                    select g.Key ?? 0).ToArray();


            var retorno = (from be in _mdb.T_BENEFICIARIOS
                           where
                               listabeneficiarios.Contains(be.ID_BENEFICIARIO) &&
                               be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo
                           select
                               new ControlDatos
                                   {
                                       Id = be.ID_BENEFICIARIO,
                                       Descripcion = "Beneficiario con Apoderado duplicado Activo.",
                                       Identificador1 = be.T_FICHAS.NUMERO_DOCUMENTO,
                                       Identificador2 = be.T_FICHAS.APELLIDO,
                                       Identificador3 = be.T_FICHAS.NOMBRE,
                                       ModuloAccion = "Beneficiario"
                                   }).ToList().Cast<IControlDatos>().Skip(skip).Take(make).ToList();


            return retorno;
        }

        public IList<IControlDatos> GetCuentasenCero(int idPrograma, int idEtapa)
        {

            var beneficiariosinapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join cu in _mdb.T_CUENTAS_BANCO on be.ID_BENEFICIARIO equals cu.ID_BENEFICIARIO
                                      from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                      from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()

                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo && cu.NRO_CTA == 0
                                          && be.TIENE_APODERADO == "N"
                                          && (idPrograma == 0 ? be.ID_PROGRAMA > 0 : be.ID_PROGRAMA == idPrograma)
                                          && (fi.ID_ETAPA == idEtapa || idEtapa == 0)
                                      select
                                          new ControlDatos
                                              {
                                                  Id = be.ID_BENEFICIARIO,
                                                  Identificador1 = fi.NUMERO_DOCUMENTO,
                                                  Identificador2 = fi.APELLIDO,
                                                  Identificador3 = fi.NOMBRE,
                                                  ModuloAccion = "Beneficiarios",
                                                  Descripcion = "Beneficiario con Cuenta en Cero.",
                                                  idEtapa = fi.ID_ETAPA,
                                                  Etapa = eta.N_ETAPA,
                                                  IdPrograma = (short)be.ID_PROGRAMA,
                                                  N_Programa = pro.N_PROGRAMA
                                              }

                                     ).ToList().Cast<IControlDatos>().ToList();


            var beneficiarioconapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join apo in _mdb.T_APODERADOS on be.ID_BENEFICIARIO equals apo.ID_BENEFICIARIO
                                      from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                      from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()

                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                                          apo.NRO_CUENTA_BCO == 0 &&
                                          apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo
                                           && be.TIENE_APODERADO == "S"
                                           && (idPrograma == 0 ? be.ID_PROGRAMA > 0 : be.ID_PROGRAMA == idPrograma)
                                           && (fi.ID_ETAPA == idEtapa || idEtapa == 0)
                                      select
                                          new ControlDatos
                                              {
                                                  Id = be.ID_BENEFICIARIO,
                                                  Identificador1 = fi.NUMERO_DOCUMENTO,
                                                  Identificador2 = fi.APELLIDO,
                                                  Identificador3 = fi.NOMBRE,
                                                  ModuloAccion = "Beneficiarios",
                                                  Descripcion = "Beneficiario con Cuenta en Cero."
                                              }

                                     ).ToList().Cast<IControlDatos>().ToList();


            return beneficiariosinapo.Union(beneficiarioconapo).ToList();

        }

        public IList<IControlDatos> GetCuentasenCero(int skip, int take, string s = "")
        {
            var beneficiariosinapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join cu in _mdb.T_CUENTAS_BANCO on be.ID_BENEFICIARIO equals cu.ID_BENEFICIARIO
                                      from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                      from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo && cu.NRO_CTA == 0
                                          && be.TIENE_APODERADO == "N"
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Beneficiario con Cuenta en Cero.",
                                              idEtapa = fi.ID_ETAPA,
                                              Etapa = eta.N_ETAPA,
                                              IdPrograma = (short)be.ID_PROGRAMA,
                                              N_Programa = pro.N_PROGRAMA
                                          }

                                      ).ToList().Cast<IControlDatos>().ToList();


            var beneficiarioconapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join apo in _mdb.T_APODERADOS on be.ID_BENEFICIARIO equals apo.ID_BENEFICIARIO
                                      from eta in _mdb.T_ETAPAS.Where(eta=>eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                      from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                                          apo.NRO_CUENTA_BCO == 0 &&
                                          apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo
                                          && be.TIENE_APODERADO == "S"
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Beneficiario con Cuenta en Cero.",
                                              idEtapa = fi.ID_ETAPA,
                                              Etapa = eta.N_ETAPA,
                                              IdPrograma = (short)be.ID_PROGRAMA,
                                              N_Programa = pro.N_PROGRAMA
                                          }

                                     ).ToList().Cast<IControlDatos>().ToList();


            return beneficiariosinapo.Union(beneficiarioconapo).Skip(skip).Take(take).ToList();
        }

        public IList<IControlDatos> GetCuentasenCero(int idprograma)
        {
            var beneficiariosinapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join cu in _mdb.T_CUENTAS_BANCO on be.ID_BENEFICIARIO equals cu.ID_BENEFICIARIO
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo && cu.NRO_CTA == 0 &&
                                          be.ID_PROGRAMA == idprograma
                                          && be.TIENE_APODERADO == "N"
                                      select
                                          new ControlDatos
                                              {
                                                  Id = be.ID_BENEFICIARIO,
                                                  Identificador1 = fi.NUMERO_DOCUMENTO,
                                                  Identificador2 = fi.APELLIDO,
                                                  Identificador3 = fi.NOMBRE,
                                                  ModuloAccion = "Beneficiarios",
                                                  Descripcion = "Beneficiario con Cuenta en Cero."
                                              }

                                     ).ToList().Cast<IControlDatos>().ToList();


            var beneficiarioconapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join apo in _mdb.T_APODERADOS on be.ID_BENEFICIARIO equals apo.ID_BENEFICIARIO
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                                          apo.NRO_CUENTA_BCO == 0 &&
                                          apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo &&
                                          be.ID_PROGRAMA == idprograma
                                          && be.TIENE_APODERADO == "S"
                                      select
                                          new ControlDatos
                                              {
                                                  Id = be.ID_BENEFICIARIO,
                                                  Identificador1 = fi.NUMERO_DOCUMENTO,
                                                  Identificador2 = fi.APELLIDO,
                                                  Identificador3 = fi.NOMBRE,
                                                  ModuloAccion = "Beneficiarios",
                                                  Descripcion = "Beneficiario con Cuenta en Cero."
                                              }

                                     ).ToList().Cast<IControlDatos>().ToList();


            return beneficiariosinapo.Union(beneficiarioconapo).ToList();
        }

        public IList<IControlDatos> GetCuentasenCero(int idprograma, int skip, int take)
        {
            var beneficiariosinapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join cu in _mdb.T_CUENTAS_BANCO on be.ID_BENEFICIARIO equals cu.ID_BENEFICIARIO
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo && cu.NRO_CTA == 0 &&
                                          be.ID_PROGRAMA == idprograma
                                          && be.TIENE_APODERADO == "N"
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Beneficiario con Cuenta en Cero."
                                          }

                                    ).ToList().Cast<IControlDatos>().ToList();


            var beneficiarioconapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join apo in _mdb.T_APODERADOS on be.ID_BENEFICIARIO equals apo.ID_BENEFICIARIO
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                                          apo.NRO_CUENTA_BCO == 0 &&
                                          apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo &&
                                          be.ID_PROGRAMA == idprograma
                                          && be.TIENE_APODERADO == "S"
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Beneficiario con Cuenta en Cero."
                                          }

                                     ).ToList().Cast<IControlDatos>().ToList();


            return beneficiariosinapo.Union(beneficiarioconapo).Skip(skip).Take(take).ToList();
        }

        public IList<IControlDatos> GetCuentasenDuplicadas()
        {

            IList<IControlDatos> listareturn = new List<IControlDatos>();

            List<CuentasDuplicadas> listacuentasduplicadasapo =
               (from apo in _mdb.T_APODERADOS
                join be in _mdb.T_BENEFICIARIOS on apo.ID_BENEFICIARIO equals be.ID_BENEFICIARIO
                join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA

                where
                    apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo &&
                    be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                    fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario
                group apo by new { apo.NRO_CUENTA_BCO, apo.ID_SUCURSAL_BCO }
                    into g
                    where g.Count() > 1 && g.Key.NRO_CUENTA_BCO != null
                    select
                        new CuentasDuplicadas { NroCuenta = g.Key.NRO_CUENTA_BCO ?? 0, IdSucursal = g.Key.ID_SUCURSAL_BCO }).
                   ToList();


            foreach (var item in listacuentasduplicadasapo)
            {
                var beneficiario = (from be in _mdb.T_BENEFICIARIOS
                                    join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                    join apo in _mdb.T_APODERADOS on be.ID_BENEFICIARIO equals apo.ID_BENEFICIARIO
                                    from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                    from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()

                                    where apo.NRO_CUENTA_BCO == item.NroCuenta && apo.ID_SUCURSAL_BCO == item.IdSucursal
                                    select
                                        new ControlDatos
                                            {
                                                Id = be.ID_BENEFICIARIO,
                                                Identificador1 = fi.NUMERO_DOCUMENTO,
                                                Identificador2 = fi.APELLIDO,
                                                Identificador3 = fi.NOMBRE,
                                                Descripcion =
                                                    "El Apoderado de este Beneficiario, posee un Número de Cuenta y Sucursal igual a otro ya cargado.",
                                                ModuloAccion = "Beneficiario -> Apoderados",
                                                idEtapa = fi.ID_ETAPA,
                                                Etapa = eta.N_ETAPA,
                                                IdPrograma = (short)be.ID_PROGRAMA,
                                                N_Programa = pro.N_PROGRAMA

                                            }).ToList();



                foreach (var dato in beneficiario)
                {
                    listareturn.Add(dato);
                }

            }

            List<CuentasDuplicadas> listacuentasduplicadascue =
              (from cue in _mdb.T_CUENTAS_BANCO
               join be in _mdb.T_BENEFICIARIOS on cue.ID_BENEFICIARIO equals be.ID_BENEFICIARIO
               join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
               where
                   be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                   fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario
               group cue by new { cue.NRO_CTA, cue.ID_SUCURSAL }
                   into g
                   where g.Count() > 1 && g.Key.NRO_CTA != null
                   select
                       new CuentasDuplicadas { NroCuenta = g.Key.NRO_CTA ?? 0, IdSucursal = g.Key.ID_SUCURSAL }).
                  ToList();

            foreach (var item in listacuentasduplicadascue)
            {
                var beneficiario = (from be in _mdb.T_BENEFICIARIOS
                                    join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                    join cue in _mdb.T_CUENTAS_BANCO on be.ID_BENEFICIARIO equals cue.ID_BENEFICIARIO
                                    where cue.NRO_CTA == item.NroCuenta && cue.ID_SUCURSAL == item.IdSucursal
                                    select
                                        new ControlDatos
                                        {
                                            Id = be.ID_BENEFICIARIO,
                                            Identificador1 = fi.NUMERO_DOCUMENTO,
                                            Identificador2 = fi.APELLIDO,
                                            Identificador3 = fi.NOMBRE,
                                            Descripcion =
                                                "La Cuenta de este Beneficiario, posee un Número de Cuenta y Sucursal igual a otra ya cargada.",
                                            ModuloAccion = "Beneficiario"

                                        }).ToList();



                foreach (var dato in beneficiario)
                {
                    listareturn.Add(dato);
                }

            }


            return listareturn;
        }
        public IList<IControlDatos> GetCuentasenDuplicadas(int idPrograma, int idEtapa)
        {

            IList<IControlDatos> listareturn = new List<IControlDatos>();

            List<CuentasDuplicadas> listacuentasduplicadasapo =
               (from apo in _mdb.T_APODERADOS
                join be in _mdb.T_BENEFICIARIOS on apo.ID_BENEFICIARIO equals be.ID_BENEFICIARIO
                join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA

                where
                    apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo &&
                    be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                    fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario
                group apo by new { apo.NRO_CUENTA_BCO, apo.ID_SUCURSAL_BCO }
                    into g
                    where g.Count() > 1 && g.Key.NRO_CUENTA_BCO != null
                    select
                        new CuentasDuplicadas { NroCuenta = g.Key.NRO_CUENTA_BCO ?? 0, IdSucursal = g.Key.ID_SUCURSAL_BCO }).
                   ToList();


            foreach (var item in listacuentasduplicadasapo)
            {
                var beneficiario = (from be in _mdb.T_BENEFICIARIOS
                                    join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                    join apo in _mdb.T_APODERADOS on be.ID_BENEFICIARIO equals apo.ID_BENEFICIARIO
                                    from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                    from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()

                                    where apo.NRO_CUENTA_BCO == item.NroCuenta && apo.ID_SUCURSAL_BCO == item.IdSucursal
                                    && (idPrograma == 0 ? be.ID_PROGRAMA > 0 : be.ID_PROGRAMA == idPrograma)
                                    && (fi.ID_ETAPA == idEtapa || idEtapa == 0)
                                    select
                                        new ControlDatos
                                        {
                                            Id = be.ID_BENEFICIARIO,
                                            Identificador1 = fi.NUMERO_DOCUMENTO,
                                            Identificador2 = fi.APELLIDO,
                                            Identificador3 = fi.NOMBRE,
                                            Descripcion =
                                                "El Apoderado de este Beneficiario, posee un Número de Cuenta y Sucursal igual a otro ya cargado.",
                                            ModuloAccion = "Beneficiario -> Apoderados",
                                            idEtapa = fi.ID_ETAPA,
                                            Etapa = eta.N_ETAPA,
                                            IdPrograma = (short)be.ID_PROGRAMA,
                                            N_Programa = pro.N_PROGRAMA

                                        }).ToList();



                foreach (var dato in beneficiario)
                {
                    listareturn.Add(dato);
                }

            }

            List<CuentasDuplicadas> listacuentasduplicadascue =
              (from cue in _mdb.T_CUENTAS_BANCO
               join be in _mdb.T_BENEFICIARIOS on cue.ID_BENEFICIARIO equals be.ID_BENEFICIARIO
               join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
               where
                   be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                   fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario
                   && (idPrograma == 0 ? be.ID_PROGRAMA > 0 : be.ID_PROGRAMA == idPrograma)
                   && (fi.ID_ETAPA == idEtapa || idEtapa == 0)
               group cue by new { cue.NRO_CTA, cue.ID_SUCURSAL }
                   into g
                   where g.Count() > 1 && g.Key.NRO_CTA != null
                   select
                       new CuentasDuplicadas { NroCuenta = g.Key.NRO_CTA ?? 0, IdSucursal = g.Key.ID_SUCURSAL }).
                  ToList();

            foreach (var item in listacuentasduplicadascue)
            {
                var beneficiario = (from be in _mdb.T_BENEFICIARIOS
                                    join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                    join cue in _mdb.T_CUENTAS_BANCO on be.ID_BENEFICIARIO equals cue.ID_BENEFICIARIO
                                    where cue.NRO_CTA == item.NroCuenta && cue.ID_SUCURSAL == item.IdSucursal
                                    select
                                        new ControlDatos
                                        {
                                            Id = be.ID_BENEFICIARIO,
                                            Identificador1 = fi.NUMERO_DOCUMENTO,
                                            Identificador2 = fi.APELLIDO,
                                            Identificador3 = fi.NOMBRE,
                                            Descripcion =
                                                "La Cuenta de este Beneficiario, posee un Número de Cuenta y Sucursal igual a otra ya cargada.",
                                            ModuloAccion = "Beneficiario"

                                        }).ToList();



                foreach (var dato in beneficiario)
                {
                    listareturn.Add(dato);
                }

            }


            return listareturn;
        }
        public IList<IControlDatos> GetCuentasenDuplicadas(int skip, int take, string s = "")
        {

            IList<IControlDatos> listareturn = new List<IControlDatos>();

            List<CuentasDuplicadas> listacuentasduplicadasapo =
               (from apo in _mdb.T_APODERADOS
                join be in _mdb.T_BENEFICIARIOS on apo.ID_BENEFICIARIO equals be.ID_BENEFICIARIO
                join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                where
                    apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo &&
                    be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                    fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario
                group apo by new { apo.NRO_CUENTA_BCO, apo.ID_SUCURSAL_BCO }
                    into g
                    where g.Count() > 1 && g.Key.NRO_CUENTA_BCO != null
                    select
                        new CuentasDuplicadas { NroCuenta = g.Key.NRO_CUENTA_BCO ?? 0, IdSucursal = g.Key.ID_SUCURSAL_BCO }).
                   ToList();


            foreach (var item in listacuentasduplicadasapo)
            {
                var beneficiario = (from be in _mdb.T_BENEFICIARIOS
                                    join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                    join apo in _mdb.T_APODERADOS on be.ID_BENEFICIARIO equals apo.ID_BENEFICIARIO
                                    where apo.NRO_CUENTA_BCO == item.NroCuenta && apo.ID_SUCURSAL_BCO == item.IdSucursal
                                    select
                                        new ControlDatos
                                        {
                                            Id = be.ID_BENEFICIARIO,
                                            Identificador1 = fi.NUMERO_DOCUMENTO,
                                            Identificador2 = fi.APELLIDO,
                                            Identificador3 = fi.NOMBRE,
                                            Descripcion =
                                                "El Apoderado de este Beneficiario, posee un Número de Cuenta y Sucursal igual a otro ya cargado.",
                                            ModuloAccion = "Beneficiario -> Apoderados"

                                        }).ToList();



                foreach (var dato in beneficiario)
                {
                    listareturn.Add(dato);
                }

            }

            List<CuentasDuplicadas> listacuentasduplicadascue =
              (from cue in _mdb.T_CUENTAS_BANCO
               join be in _mdb.T_BENEFICIARIOS on cue.ID_BENEFICIARIO equals be.ID_BENEFICIARIO
               join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
               where
                   be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                   fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario
               group cue by new { cue.NRO_CTA, cue.ID_SUCURSAL }
                   into g
                   where g.Count() > 1 && g.Key.NRO_CTA != null
                   select
                       new CuentasDuplicadas { NroCuenta = g.Key.NRO_CTA ?? 0, IdSucursal = g.Key.ID_SUCURSAL }).
                  ToList();

            foreach (var item in listacuentasduplicadascue)
            {
                var beneficiario = (from be in _mdb.T_BENEFICIARIOS
                                    join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                    join cue in _mdb.T_CUENTAS_BANCO on be.ID_BENEFICIARIO equals cue.ID_BENEFICIARIO
                                    where cue.NRO_CTA == item.NroCuenta && cue.ID_SUCURSAL == item.IdSucursal
                                    select
                                        new ControlDatos
                                        {
                                            Id = be.ID_BENEFICIARIO,
                                            Identificador1 = fi.NUMERO_DOCUMENTO,
                                            Identificador2 = fi.APELLIDO,
                                            Identificador3 = fi.NOMBRE,
                                            Descripcion =
                                                "La cuenta de este beneficiario, posee un número de cuenta y sucursal igual a otra ya cargado.",
                                            ModuloAccion = "Beneficiario"

                                        }).ToList();



                foreach (var dato in beneficiario)
                {
                    listareturn.Add(dato);
                }

            }

            return listareturn.Skip(skip).Take(take).ToList();
        }

        public IList<IControlDatos> GetBeneficiariosMonedaIncorrecta()
        {
            short[] idmonedas =
                _mdb.T_TABLAS_BCO_CBA.Where(c => c.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Monedas).Select
                    (c => c.ID_TABLA_BCO_CBA).ToArray();

            var beneficiariosinapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join cu in _mdb.T_CUENTAS_BANCO on be.ID_BENEFICIARIO equals cu.ID_BENEFICIARIO
                                      from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                      from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()

                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                                          cu.ID_SUCURSAL != null && be.TIENE_APODERADO == "N" &&
                                          !idmonedas.Contains(cu.ID_MONEDA ?? 0)
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Beneficiario con moneda incorrecta",
                                              idEtapa = fi.ID_ETAPA,
                                              Etapa = eta.N_ETAPA,
                                              IdPrograma = (short)be.ID_PROGRAMA,
                                              N_Programa = pro.N_PROGRAMA
                                          }

                                     ).ToList().Cast<IControlDatos>().ToList();


            var beneficiarioconapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join apo in _mdb.T_APODERADOS on be.ID_BENEFICIARIO equals apo.ID_BENEFICIARIO
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                                          apo.ID_SUCURSAL_BCO != null && be.TIENE_APODERADO == "S" &&
                                          !idmonedas.Contains(apo.ID_MONEDA ?? 0)
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios -> Apoderado",
                                              Descripcion = "El Apoderado de este Beneficiario posee una moneda incorrecta."
                                          }

                                     ).ToList().Cast<IControlDatos>().ToList();



            return beneficiariosinapo.Union(beneficiarioconapo).ToList();
        }

        public IList<IControlDatos> GetBeneficiariosMonedaIncorrecta(int idPrograma, int idEtapa)
        {
            short[] idmonedas =
                _mdb.T_TABLAS_BCO_CBA.Where(c => c.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Monedas).Select
                    (c => c.ID_TABLA_BCO_CBA).ToArray();

            var beneficiariosinapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join cu in _mdb.T_CUENTAS_BANCO on be.ID_BENEFICIARIO equals cu.ID_BENEFICIARIO
                                      from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                      from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()

                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                                          cu.ID_SUCURSAL != null && be.TIENE_APODERADO == "N" &&
                                          !idmonedas.Contains(cu.ID_MONEDA ?? 0)
                                          && (idPrograma == 0 ? be.ID_PROGRAMA > 0 : be.ID_PROGRAMA == idPrograma)
                                          && (fi.ID_ETAPA == idEtapa || idEtapa == 0)
                                      select
                                          new ControlDatos
                                              {
                                                  Id = be.ID_BENEFICIARIO,
                                                  Identificador1 = fi.NUMERO_DOCUMENTO,
                                                  Identificador2 = fi.APELLIDO,
                                                  Identificador3 = fi.NOMBRE,
                                                  ModuloAccion = "Beneficiarios",
                                                  Descripcion = "Beneficiario con moneda incorrecta",
                                                  idEtapa = fi.ID_ETAPA,
                                                  Etapa = eta.N_ETAPA,
                                                  IdPrograma = (short)be.ID_PROGRAMA,
                                                  N_Programa = pro.N_PROGRAMA
                                              }

                                     ).ToList().Cast<IControlDatos>().ToList();


            var beneficiarioconapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join apo in _mdb.T_APODERADOS on be.ID_BENEFICIARIO equals apo.ID_BENEFICIARIO
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                                          apo.ID_SUCURSAL_BCO != null && be.TIENE_APODERADO == "S" &&
                                          !idmonedas.Contains(apo.ID_MONEDA ?? 0)
                                          && (idPrograma == 0 ? be.ID_PROGRAMA > 0 : be.ID_PROGRAMA == idPrograma)
                                          && (fi.ID_ETAPA == idEtapa || idEtapa == 0)
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios -> Apoderado",
                                              Descripcion = "El Apoderado de este Beneficiario posee una moneda incorrecta."
                                          }

                                     ).ToList().Cast<IControlDatos>().ToList();



            return beneficiariosinapo.Union(beneficiarioconapo).ToList();
        }

        public IList<IControlDatos> GetBeneficiariosMonedaIncorrecta(int skip, int take, string s = "")
        {
            short[] idmonedas =
                _mdb.T_TABLAS_BCO_CBA.Where(c => c.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Monedas).Select
                    (c => c.ID_TABLA_BCO_CBA).ToArray();

            var beneficiariosinapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join cu in _mdb.T_CUENTAS_BANCO on be.ID_BENEFICIARIO equals cu.ID_BENEFICIARIO
                                      from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                      from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()

                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                                          !idmonedas.Contains(cu.ID_MONEDA ?? 0)
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Beneficiario con moneda incorrecta.",
                                              idEtapa = fi.ID_ETAPA,
                                              Etapa = eta.N_ETAPA,
                                              IdPrograma = (short)be.ID_PROGRAMA,
                                              N_Programa = pro.N_PROGRAMA
                                          }

                                     ).ToList().Cast<IControlDatos>().ToList();


            var beneficiarioconapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join apo in _mdb.T_APODERADOS on be.ID_BENEFICIARIO equals apo.ID_BENEFICIARIO
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                                          !idmonedas.Contains(apo.ID_MONEDA ?? 0)
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios -> Apoderado",
                                              Descripcion = "El apoderado de este Beneficiario posee una moneda incorrecta"
                                          }

                                     ).ToList().Cast<IControlDatos>().ToList();



            return beneficiariosinapo.Union(beneficiarioconapo).Skip(skip).Take(take).ToList();
        }

        public IList<IControlDatos> GetDatosErroneos(int idPrograma, int idEtapa)
        {
            var cuentasencero = GetCuentasenCero(idPrograma, idEtapa);

            var apoderadosduplicadosactivos = GetApoderadosDuplicadosActivos(idPrograma, idEtapa);

            var cuentasduplicadas = GetCuentasenDuplicadas(idPrograma, idEtapa);

            var cuentasmonedaincorreta = GetBeneficiariosMonedaIncorrecta(idPrograma, idEtapa);

            var sinsucursal = GetBeneficiarioSinSucursalAsignada(idPrograma, idEtapa);

            var sinFechanotificacion = GetBeneficiarioSinFechaNotificacion(idPrograma, idEtapa);

            var sinFechainicio = GetBeneficiarioConAltaTempranaSinFechaInicio(idPrograma, idEtapa);

            var sincodigopostal = GetBeneficiarioSinCodigoPostaAsignado(idPrograma, idEtapa);

            var dniduplicado = GetBeneficiarioConDocumentoDuplicado(idPrograma, idEtapa);

            return
                cuentasencero.Union(
                    apoderadosduplicadosactivos.Union(
                        cuentasduplicadas.Union(
                            cuentasmonedaincorreta.Union(
                                sinsucursal.Union(sinFechanotificacion.Union(sinFechainicio.Union(sincodigopostal.Union(dniduplicado))))))))
                    .OrderBy(c => c.Descripcion).ThenBy(c => c.Identificador2)//.OrderBy(a=>a.Identificador2).
                    .ToList();
        }

        public IList<IControlDatos> GetDatosErroneos(int skip, int take, int idPrograma, int idEtapa)
        {
            var cuentasencero = GetCuentasenCero(idPrograma, idEtapa);

            var apoderadosduplicadosactivos = GetApoderadosDuplicadosActivos(idPrograma, idEtapa);
            var cuentasduplicadas = GetCuentasenDuplicadas(idPrograma, idEtapa);

            var cuentasmonedaincorreta = GetBeneficiariosMonedaIncorrecta(idPrograma, idEtapa);

            var sinsucursal = GetBeneficiarioSinSucursalAsignada(idPrograma, idEtapa);

            var sincodigopostal = GetBeneficiarioSinCodigoPostaAsignado(idPrograma, idEtapa);

            var dniduplicado = GetBeneficiarioConDocumentoDuplicado(idPrograma, idEtapa);

            return
                cuentasencero.Union(apoderadosduplicadosactivos.Union(cuentasduplicadas.Union(cuentasmonedaincorreta.Union(sinsucursal.Union(sincodigopostal.Union(dniduplicado)))))).
                    Skip(skip).Take(take).OrderBy(c=> c.Descripcion).ThenBy(c=>c.Identificador2)
                    //.OrderBy(a=> a.Identificador2)
                    .ToList();
        }

        public IList<IControlDatos> GetBeneficiarioSinSucursalAsignada(int idPrograma, int idEtapa)
        {
            var beneficiariosinapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join cu in _mdb.T_CUENTAS_BANCO on be.ID_BENEFICIARIO equals cu.ID_BENEFICIARIO
                                      from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                      from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()

                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo && (cu.ID_SUCURSAL == 0 || cu.ID_SUCURSAL == null)
                                          && be.TIENE_APODERADO == "N"
                                          && (idPrograma == 0 ? be.ID_PROGRAMA > 0 : be.ID_PROGRAMA == idPrograma)
                                          && (fi.ID_ETAPA == idEtapa || idEtapa == 0)
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Beneficiario sin Sucursal Asignada.",
                                              idEtapa = fi.ID_ETAPA,
                                              Etapa = eta.N_ETAPA,
                                              IdPrograma = (short)be.ID_PROGRAMA,
                                              N_Programa = pro.N_PROGRAMA
                                          }

                               ).ToList().Cast<IControlDatos>().ToList();


            var beneficiarioconapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join apo in _mdb.T_APODERADOS on be.ID_BENEFICIARIO equals apo.ID_BENEFICIARIO
                                      from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                      from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()

                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                                          (apo.ID_SUCURSAL_BCO == 0 || apo.ID_SUCURSAL_BCO == null) &&
                                          apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo
                                          && be.TIENE_APODERADO == "S"
                                          && (idPrograma == 0 ? be.ID_PROGRAMA > 0 : be.ID_PROGRAMA == idPrograma)
                                          && (fi.ID_ETAPA == idEtapa || idEtapa == 0)
                                      select
                                          new ControlDatos
                                              {
                                                  Id = be.ID_BENEFICIARIO,
                                                  Identificador1 = fi.NUMERO_DOCUMENTO,
                                                  Identificador2 = fi.APELLIDO,
                                                  Identificador3 = fi.NOMBRE,
                                                  ModuloAccion = "Beneficiarios",
                                                  Descripcion = "Apoderado sin Sucursal Asignada.",
                                                  idEtapa = fi.ID_ETAPA,
                                                  Etapa = eta.N_ETAPA,
                                                  IdPrograma = (short)be.ID_PROGRAMA,
                                                  N_Programa = pro.N_PROGRAMA
                                              }

                                     ).ToList().Cast<IControlDatos>().ToList();


            return beneficiariosinapo.Union(beneficiarioconapo).ToList();
        }

        public IList<IControlDatos> GetBeneficiarioSinSucursalAsignada(int skip, int take, string s = "")
        {
            var beneficiariosinapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join cu in _mdb.T_CUENTAS_BANCO on be.ID_BENEFICIARIO equals cu.ID_BENEFICIARIO
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo && (cu.ID_SUCURSAL == 0 || cu.ID_SUCURSAL == null)
                                          && be.TIENE_APODERADO == "N"
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Beneficiario sin Sucursal Asignada."
                                          }

                              ).ToList().Cast<IControlDatos>().ToList();


            var beneficiarioconapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join apo in _mdb.T_APODERADOS on be.ID_BENEFICIARIO equals apo.ID_BENEFICIARIO
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                                          (apo.ID_SUCURSAL_BCO == 0 || apo.ID_SUCURSAL_BCO == null) &&
                                          apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo
                                          && be.TIENE_APODERADO == "S"
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Apoderado sin Sucursal Asignada."
                                          }

                                     ).ToList().Cast<IControlDatos>().ToList();


            return beneficiariosinapo.Union(beneficiarioconapo).Skip(skip).Take(take).ToList();
        }

        public IList<IControlDatos> GetBeneficiarioSinSucursalAsignada(int idprograma)
        {
            var beneficiariosinapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join cu in _mdb.T_CUENTAS_BANCO on be.ID_BENEFICIARIO equals cu.ID_BENEFICIARIO
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo && (cu.ID_SUCURSAL == 0 || cu.ID_SUCURSAL == null)
                                          && be.ID_PROGRAMA == idprograma
                                          && be.TIENE_APODERADO == "N"
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Beneficiario sin Sucursal Asignada."
                                          }

                            ).ToList().Cast<IControlDatos>().ToList();


            var beneficiarioconapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join apo in _mdb.T_APODERADOS on be.ID_BENEFICIARIO equals apo.ID_BENEFICIARIO
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                                          (apo.ID_SUCURSAL_BCO == 0 || apo.ID_SUCURSAL_BCO == null) &&
                                          apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo
                                          && be.ID_PROGRAMA == idprograma
                                          && be.TIENE_APODERADO == "S"
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Apoderado sin Sucursal Asignada."
                                          }

                                     ).ToList().Cast<IControlDatos>().ToList();


            return beneficiariosinapo.Union(beneficiarioconapo).ToList();
        }

        public IList<IControlDatos> GetBeneficiarioSinSucursalAsignada(int idprograma, int skip, int take)
        {
            var beneficiariosinapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join cu in _mdb.T_CUENTAS_BANCO on be.ID_BENEFICIARIO equals cu.ID_BENEFICIARIO
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo && (cu.ID_SUCURSAL == 0 || cu.ID_SUCURSAL == null)
                                          && be.ID_PROGRAMA == idprograma
                                          && be.TIENE_APODERADO == "N"
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Beneficiario sin Sucursal Asignada."
                                          }

                           ).ToList().Cast<IControlDatos>().ToList();


            var beneficiarioconapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join apo in _mdb.T_APODERADOS on be.ID_BENEFICIARIO equals apo.ID_BENEFICIARIO
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                                          (apo.ID_SUCURSAL_BCO == 0 || apo.ID_SUCURSAL_BCO == null) &&
                                          apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo
                                          && be.ID_PROGRAMA == idprograma
                                          && be.TIENE_APODERADO == "S"
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Apoderado sin Sucursal Asignada."
                                          }

                                     ).ToList().Cast<IControlDatos>().ToList();


            return beneficiariosinapo.Union(beneficiarioconapo).ToList();
        }

        public IList<IControlDatos> GetBeneficiarioSinFechaNotificacion(int idPrograma, int idEtapa)
        {
            var beneficiarios = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                         from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                         from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()
                                      where
                                          fi.ID_ESTADO_FICHA == (int) Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int) Enums.EstadoBeneficiario.Activo
                                          && be.FEC_NOTIF == null
                                          && (idPrograma == 0 ? be.ID_PROGRAMA > 0 : be.ID_PROGRAMA == idPrograma)
                                          && (fi.ID_ETAPA == idEtapa || idEtapa == 0)
                                      select
                                          new ControlDatos
                                              {
                                                  Id = be.ID_BENEFICIARIO,
                                                  Identificador1 = fi.NUMERO_DOCUMENTO,
                                                  Identificador2 = fi.APELLIDO,
                                                  Identificador3 = fi.NOMBRE,
                                                  ModuloAccion = "Beneficiarios",
                                                  Descripcion = "Beneficiario sin fecha de notificación.",
                                                  idEtapa = fi.ID_ETAPA,
                                                  Etapa = eta.N_ETAPA,
                                                  IdPrograma = (short)be.ID_PROGRAMA,
                                                  N_Programa = pro.N_PROGRAMA
                                              }

                                     ).ToList().Cast<IControlDatos>().ToList();

            return beneficiarios;
        }

        public IList<IControlDatos> GetBeneficiarioSinFechaNotificacion(int skip, int take, string s = "")
        {
            var beneficiarios = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      where
                                          fi.ID_ESTADO_FICHA == (int) Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int) Enums.EstadoBeneficiario.Activo
                                          && be.FEC_NOTIF == null
                                      select
                                          new ControlDatos
                                              {
                                                  Id = be.ID_BENEFICIARIO,
                                                  Identificador1 = fi.NUMERO_DOCUMENTO,
                                                  Identificador2 = fi.APELLIDO,
                                                  Identificador3 = fi.NOMBRE,
                                                  ModuloAccion = "Beneficiarios",
                                                  Descripcion = "Beneficiario sin fecha de notificación."
                                              }

                                     ).ToList().Skip(skip).Take(take).Cast<IControlDatos>().ToList();

            return beneficiarios;
        }

        public IList<IControlDatos> GetBeneficiarioConAltaTempranaSinFechaInicio(int idPrograma, int idEtapa)
        {
            var beneficiariosPpp = (from be in _mdb.T_BENEFICIARIOS
                                    join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                    join fippp in _mdb.T_FICHA_PPP on fi.ID_FICHA equals fippp.ID_FICHA
                                    from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                    from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()
                                    where
                                        fi.ID_ESTADO_FICHA == (int) Enums.EstadoFicha.Beneficiario &&
                                        be.ID_ESTADO == (int) Enums.EstadoBeneficiario.Activo
                                        && fippp.MODALIDAD == (int) Enums.Modalidad.Cti && fippp.AT_FECHA_INICIO == null
                                        && (idPrograma == 0 ? be.ID_PROGRAMA > 0 : be.ID_PROGRAMA == idPrograma)
                                        && (fi.ID_ETAPA == idEtapa || idEtapa == 0)
                                    select
                                        new ControlDatos
                                            {
                                                Id = be.ID_BENEFICIARIO,
                                                Identificador1 = fi.NUMERO_DOCUMENTO,
                                                Identificador2 = fi.APELLIDO,
                                                Identificador3 = fi.NOMBRE,
                                                ModuloAccion = "Beneficiarios",
                                                Descripcion = "Beneficiario con alta temprana y sin fecha de inicio.",
                                                idEtapa = fi.ID_ETAPA,
                                                Etapa = eta.N_ETAPA,
                                                IdPrograma = (short)be.ID_PROGRAMA,
                                                N_Programa = pro.N_PROGRAMA
                                            }

                                   ).ToList().Cast<IControlDatos>().ToList();

            var beneficiariosVat = (from be in _mdb.T_BENEFICIARIOS
                                    join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                    join fivat in _mdb.T_FICHA_VAT on fi.ID_FICHA equals fivat.ID_FICHA
                                    from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                    from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()

                                    where
                                        fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                        be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo
                                        && fivat.MODALIDAD == (int)Enums.Modalidad.Cti && fivat.AT_FECHA_INICIO == null
                                        && (idPrograma == 0 ? be.ID_PROGRAMA > 0 : be.ID_PROGRAMA == idPrograma)
                                        && (fi.ID_ETAPA == idEtapa || idEtapa == 0)
                                    select
                                        new ControlDatos
                                        {
                                            Id = be.ID_BENEFICIARIO,
                                            Identificador1 = fi.NUMERO_DOCUMENTO,
                                            Identificador2 = fi.APELLIDO,
                                            Identificador3 = fi.NOMBRE,
                                            ModuloAccion = "Beneficiarios",
                                            Descripcion = "Beneficiario con alta temprana y sin fecha de inicio.",
                                            idEtapa = fi.ID_ETAPA,
                                            Etapa = eta.N_ETAPA,
                                            IdPrograma = (short)be.ID_PROGRAMA,
                                            N_Programa = pro.N_PROGRAMA
                                        }

                                   ).ToList().Cast<IControlDatos>().ToList();

            var beneficiariosPppp = (from be in _mdb.T_BENEFICIARIOS
                                    join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                     join fippp in _mdb.T_FICHA_VAT on fi.ID_FICHA equals fippp.ID_FICHA
                                     from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                     from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()

                                    where
                                        fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                        be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo
                                        && fippp.MODALIDAD == (int)Enums.Modalidad.Cti && fippp.AT_FECHA_INICIO == null
                                        && (idPrograma == 0 ? be.ID_PROGRAMA > 0 : be.ID_PROGRAMA == idPrograma)
                                        && (fi.ID_ETAPA == idEtapa || idEtapa == 0)
                                    select
                                        new ControlDatos
                                        {
                                            Id = be.ID_BENEFICIARIO,
                                            Identificador1 = fi.NUMERO_DOCUMENTO,
                                            Identificador2 = fi.APELLIDO,
                                            Identificador3 = fi.NOMBRE,
                                            ModuloAccion = "Beneficiarios",
                                            Descripcion = "Beneficiario con alta temprana y sin fecha de inicio.",
                                            idEtapa = fi.ID_ETAPA,
                                            Etapa = eta.N_ETAPA,
                                            IdPrograma = (short)be.ID_PROGRAMA,
                                            N_Programa = pro.N_PROGRAMA
                                        }

                                  ).ToList().Cast<IControlDatos>().ToList();

            return beneficiariosPpp.Union(beneficiariosVat.Union(beneficiariosPppp)).ToList();
        }

        public IList<IControlDatos> GetBeneficiarioConAltaTempranaSinFechaInicio(int skip, int take, string s = "")
        {
            var beneficiariosPpp = (from be in _mdb.T_BENEFICIARIOS
                                    join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                    join fippp in _mdb.T_FICHA_PPP on fi.ID_FICHA equals fippp.ID_FICHA
                                    where
                                        fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                        be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo
                                        && fippp.MODALIDAD == (int)Enums.Modalidad.Cti && fippp.AT_FECHA_INICIO == null
                                    select
                                        new ControlDatos
                                        {
                                            Id = be.ID_BENEFICIARIO,
                                            Identificador1 = fi.NUMERO_DOCUMENTO,
                                            Identificador2 = fi.APELLIDO,
                                            Identificador3 = fi.NOMBRE,
                                            ModuloAccion = "Beneficiarios",
                                            Descripcion = "Beneficiario con alta temprana y sin fecha de inicio."
                                        }

                                   ).ToList().Cast<IControlDatos>().ToList();

            var beneficiariosVat = (from be in _mdb.T_BENEFICIARIOS
                                    join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                    join fivat in _mdb.T_FICHA_VAT on fi.ID_FICHA equals fivat.ID_FICHA
                                    where
                                        fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                        be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo
                                        && fivat.MODALIDAD == (int)Enums.Modalidad.Cti && fivat.AT_FECHA_INICIO == null
                                    select
                                        new ControlDatos
                                        {
                                            Id = be.ID_BENEFICIARIO,
                                            Identificador1 = fi.NUMERO_DOCUMENTO,
                                            Identificador2 = fi.APELLIDO,
                                            Identificador3 = fi.NOMBRE,
                                            ModuloAccion = "Beneficiarios",
                                            Descripcion = "Beneficiario con alta temprana y sin fecha de inicio."
                                        }

                                   ).ToList().Cast<IControlDatos>().ToList();

            var beneficiariosPppp = (from be in _mdb.T_BENEFICIARIOS
                                     join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                     join fippp in _mdb.T_FICHA_VAT on fi.ID_FICHA equals fippp.ID_FICHA
                                     where
                                         fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                         be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo
                                         && fippp.MODALIDAD == (int)Enums.Modalidad.Cti && fippp.AT_FECHA_INICIO == null
                                     select
                                         new ControlDatos
                                         {
                                             Id = be.ID_BENEFICIARIO,
                                             Identificador1 = fi.NUMERO_DOCUMENTO,
                                             Identificador2 = fi.APELLIDO,
                                             Identificador3 = fi.NOMBRE,
                                             ModuloAccion = "Beneficiarios",
                                             Descripcion = "Beneficiario con alta temprana y sin fecha de inicio."
                                         }

                                  ).ToList().Cast<IControlDatos>().ToList();

            return beneficiariosPpp.Union(beneficiariosVat.Union(beneficiariosPppp)).Skip(skip).Take(take).ToList();
        }

        public IList<IControlDatos> GetBeneficiarioSinCodigoPostaAsignado(int idPrograma, int idEtapa)
        {
            var beneficiariosinapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                      from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo
                                          && (fi.CODIGO_POSTAL == "0" || fi.CODIGO_POSTAL == null)
                                          && be.TIENE_APODERADO == "N"
                                          && (idPrograma == 0 ? be.ID_PROGRAMA > 0 : be.ID_PROGRAMA == idPrograma)
                                          && (fi.ID_ETAPA == idEtapa || idEtapa == 0)
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Beneficiario sin Código Postal Asignado.",
                                              idEtapa = fi.ID_ETAPA,
                                              Etapa = eta.N_ETAPA,
                                              IdPrograma = (short)be.ID_PROGRAMA,
                                              N_Programa = pro.N_PROGRAMA
                                          }

                               ).ToList().Cast<IControlDatos>().ToList();


            var beneficiarioconapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join apo in _mdb.T_APODERADOS on be.ID_BENEFICIARIO equals apo.ID_BENEFICIARIO
                                      from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                                      from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()

                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                                          (apo.CODIGO_POSTAL == "0" || apo.CODIGO_POSTAL == null) &&
                                          apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo
                                          && be.TIENE_APODERADO == "S"
                                          && (idPrograma == 0 ? be.ID_PROGRAMA > 0 : be.ID_PROGRAMA == idPrograma)
                                          && (fi.ID_ETAPA == idEtapa || idEtapa == 0)
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Apoderado sin Código Postal Asignado.",
                                              idEtapa = fi.ID_ETAPA,
                                              Etapa = eta.N_ETAPA,
                                              IdPrograma = (short)be.ID_PROGRAMA,
                                              N_Programa = pro.N_PROGRAMA
                                          }

                                     ).ToList().Cast<IControlDatos>().ToList();


            return beneficiariosinapo.Union(beneficiarioconapo).ToList();
        }

        public IList<IControlDatos> GetBeneficiarioSinCodigoPostaAsignado(int skip, int take, string s = "")
        {
            var beneficiariosinapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo
                                          && (fi.CODIGO_POSTAL == "0" || fi.CODIGO_POSTAL == null)
                                          && be.TIENE_APODERADO == "N"
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Beneficiario sin Código Postal Asignado."
                                          }

                              ).ToList().Cast<IControlDatos>().ToList();


            var beneficiarioconapo = (from be in _mdb.T_BENEFICIARIOS
                                      join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                                      join apo in _mdb.T_APODERADOS on be.ID_BENEFICIARIO equals apo.ID_BENEFICIARIO
                                      where
                                          fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                                          be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                                          (apo.CODIGO_POSTAL == "0" || apo.CODIGO_POSTAL == null) &&
                                          apo.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo
                                          && be.TIENE_APODERADO == "S"
                                      select
                                          new ControlDatos
                                          {
                                              Id = be.ID_BENEFICIARIO,
                                              Identificador1 = fi.NUMERO_DOCUMENTO,
                                              Identificador2 = fi.APELLIDO,
                                              Identificador3 = fi.NOMBRE,
                                              ModuloAccion = "Beneficiarios",
                                              Descripcion = "Apoderado sin Código Postal Asignado."
                                          }

                                     ).ToList().Cast<IControlDatos>().ToList();


            return beneficiariosinapo.Union(beneficiarioconapo).Skip(skip).Take(take).ToList();
        }

        public IList<IControlDatos> GetBeneficiarioConDocumentoDuplicado(int idPrograma, int idEtapa)
        {
            string[] queryDupFiles =
                       (from be in _mdb.T_BENEFICIARIOS
                        join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                        where fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                              be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                              fi.NUMERO_DOCUMENTO != " "
                        group fi by fi.NUMERO_DOCUMENTO
                            into g
                            where g.Count() > 1
                            select g.Key).ToArray();


            var retorno = (from be in _mdb.T_BENEFICIARIOS
                           join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                           from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == fi.ID_ETAPA).DefaultIfEmpty()
                           from pro in _mdb.T_PROGRAMAS.Where(pro => pro.ID_PROGRAMA == be.ID_PROGRAMA).DefaultIfEmpty()

                           where fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                           be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                           queryDupFiles.Contains(fi.NUMERO_DOCUMENTO) && fi.NUMERO_DOCUMENTO != " "
                           && (idPrograma == 0 ? be.ID_PROGRAMA > 0 : be.ID_PROGRAMA == idPrograma)
                           && (fi.ID_ETAPA == idEtapa || idEtapa == 0)
                           select
                                         new ControlDatos
                                         {
                                             Id = be.ID_BENEFICIARIO,
                                             Identificador1 = fi.NUMERO_DOCUMENTO,
                                             Identificador2 = fi.APELLIDO,
                                             Identificador3 = fi.NOMBRE,
                                             ModuloAccion = "Beneficiarios",
                                             Descripcion = "Beneficiario con Nro de Documento duplicado.",
                                             idEtapa = fi.ID_ETAPA,
                                             Etapa = eta.N_ETAPA,
                                             IdPrograma = (short)be.ID_PROGRAMA,
                                             N_Programa = pro.N_PROGRAMA
                                         }

                              ).ToList().Cast<IControlDatos>().ToList();

            return retorno;
        }

        public IList<IControlDatos> GetBeneficiarioConDocumentoDuplicado(int skip, int take, string s = "")
        {
            string[] queryDupFiles =
                       (from be in _mdb.T_BENEFICIARIOS
                        join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                        where fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                              be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                              fi.NUMERO_DOCUMENTO != " "
                        group fi by fi.NUMERO_DOCUMENTO
                            into g
                            where g.Count() > 1
                            select g.Key).ToArray();


            var retorno = (from be in _mdb.T_BENEFICIARIOS
                           join fi in _mdb.T_FICHAS on be.ID_FICHA equals fi.ID_FICHA
                           where fi.ID_ESTADO_FICHA == (int)Enums.EstadoFicha.Beneficiario &&
                           be.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo &&
                           queryDupFiles.Contains(fi.NUMERO_DOCUMENTO) && fi.NUMERO_DOCUMENTO != " "
                           select
                                         new ControlDatos
                                         {
                                             Id = be.ID_BENEFICIARIO,
                                             Identificador1 = fi.NUMERO_DOCUMENTO,
                                             Identificador2 = fi.APELLIDO,
                                             Identificador3 = fi.NOMBRE,
                                             ModuloAccion = "Beneficiarios",
                                             Descripcion = "Beneficiario con Nro de Documento duplicado."
                                         }

                              ).ToList().Cast<IControlDatos>().ToList();

            return retorno.Skip(skip).Take(take).ToList();
        }
    }


    internal class CuentasDuplicadas
    {
        public int? NroCuenta { get; set; }
        public int? IdSucursal { get; set; }
    }
}
