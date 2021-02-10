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
    public class ApoderadoServicio : IApoderadoServicio
    {
        private readonly IApoderadoRepositorio _apoderadorepositorio;
        private readonly ILocalidadRepositorio _localidadrepositorio;
        private readonly IEstadoApoderadoRepositorio _estadoapoderadorepositorio;
        private readonly ITablaBcoCbaRepositorio _tablabcocbarepositorio;

        public ApoderadoServicio()
        {
            _localidadrepositorio = new LocalidadRepositorio();
            _apoderadorepositorio = new ApoderadoRepositorio();
            _estadoapoderadorepositorio = new EstadoApoderadoRepositorio();
            _tablabcocbarepositorio = new TablaBcoCbaRepositorio();
        }

        public IApoderadosVista GetApoderadosByBeneficiario(int idBeneficiario)
        {
            IApoderadosVista vista = new ApoderadosVista
                                         {
                                             Apoderados =
                                                 _apoderadorepositorio.GetApoderadosByBeneficiario(idBeneficiario)
                                         };

            return vista;
        }

        public IApoderadoVista GetApoderado(int idApoderado, string accion)
        {
            IApoderadoVista vista = new ApoderadoVista { Accion = accion };

            CargarLocalidades(vista, _localidadrepositorio.GetLocalidades());
            CargarEstadosApoderado(vista, _estadoapoderadorepositorio.GetEstadosApoderado());
            CargarSexo(vista);
            CargarTipoDocumento(vista);
            CargarMonedas(vista, _tablabcocbarepositorio.GetMonedas());
            CargarSistemas(vista, _tablabcocbarepositorio.GetSistemas());
            CargarSucursales(vista, _tablabcocbarepositorio.GetSucursales());

            if (idApoderado > 0)
            {
                IApoderado objreturn = _apoderadorepositorio.GetApoderado(idApoderado);

                vista.IdBeneficiario = objreturn.IdBeneficiario;
                vista.IdApoderado = objreturn.IdApoderado;
                vista.Apellido = objreturn.Apellido;
                vista.Nombre = objreturn.Nombre;
                vista.TipoDocumento.Selected = objreturn.TipoDocumento.ToString();
                vista.NumeroDocumento = objreturn.NumeroDocumento;
                vista.Cuil = objreturn.Cuil;
                vista.Sexo.Selected = objreturn.Sexo == "VARON" ? ((int)Enums.Sexo.Varon).ToString() : 
                    ((int)Enums.Sexo.Mujer).ToString();
                vista.Cbu = objreturn.Cbu;
                vista.UsuarioBanco = objreturn.UsuarioBanco;
                vista.FechaNacimiento = objreturn.FechaNacimiento;
                vista.Calle = objreturn.Calle;
                vista.Numero = objreturn.Numero;
                vista.Piso = objreturn.Piso;
                vista.Dpto = objreturn.Dpto;
                vista.Monoblock = objreturn.Monoblock;
                vista.Parcela = objreturn.Parcela;
                vista.Manzana = objreturn.Manzana;
                vista.EntreCalles = objreturn.EntreCalles;
                vista.Barrio = objreturn.Barrio;
                vista.CodigoPostal = objreturn.CodigoPostal;
                vista.TelefonoFijo = objreturn.TelefonoFijo;
                vista.TelefonoCelular = objreturn.TelefonoCelular;
                vista.Mail = objreturn.Mail;
                vista.FechaSolicitudCuenta = objreturn.FechaSolicitudCuenta;

                vista.Accion = accion;

                if (objreturn.IdSistema != null)
                {
                    vista.IdSistema = objreturn.IdSistema.Value;
                    vista.Sistemas.Selected = objreturn.IdSistema.ToString();
                }

                if (objreturn.NumeroCuentaBco != null) vista.NumeroCuentaBco = objreturn.NumeroCuentaBco.Value;

                if (objreturn.IdSucursal != null)
                {
                    vista.IdSucursal = objreturn.IdSucursal;
                    vista.Sucursales.Selected = objreturn.IdSucursal.ToString();
                }

                if (objreturn.IdMoneda != null && objreturn.IdMoneda != 0)
                {
                    vista.IdMoneda = objreturn.IdMoneda;
                    vista.Monedas.Selected = objreturn.IdMoneda.ToString();
                }
                

                if (objreturn.ApoderadoDesde != null)
                    vista.ApoderadoDesde = Convert.ToDateTime(objreturn.ApoderadoDesde.Value.ToShortDateString());

                if (objreturn.ApoderadoHasta != null)
                    vista.ApoderadoHasta = Convert.ToDateTime(objreturn.ApoderadoHasta.Value.ToShortDateString());

                if (objreturn.IdEstadoApoderado != null)
                {
                    vista.IdEstadoApoderado = objreturn.IdEstadoApoderado.Value;
                    vista.EstadosApoderados.Selected = objreturn.IdEstadoApoderado.ToString();
                }

                if (objreturn.IdLocalidad != null)
                {
                    vista.IdLocalidad = objreturn.IdLocalidad.Value;
                    vista.Localidades.Selected = objreturn.IdLocalidad.ToString();
                }

                if (accion.Equals("Modificar") || (accion.Equals("Agregar")))
                {
                    vista.Localidades.Enabled = true;
                    vista.EstadosApoderados.Enabled = true;
                    vista.Sexo.Enabled = true;
                    vista.TipoDocumento.Enabled = true;
                    vista.Monedas.Enabled = true;
                    vista.Sistemas.Enabled = true;
                    vista.Sistemas.ReadOnly = true;
                    vista.Sucursales.Enabled = true;
                }
                else if (accion.Equals("Ver") || accion.Equals("Eliminar") || accion.Equals("CambiarEstado"))
                {
                    vista.Localidades.Enabled = false;
                    if (accion.Equals("Ver") || accion.Equals("Eliminar"))
                    {
                        vista.EstadosApoderados.Enabled = false;
                    }
                    vista.Sexo.Enabled = false;
                    vista.TipoDocumento.Enabled = false;
                    vista.Monedas.Enabled = false;
                    vista.Sistemas.Enabled = false;
                    vista.Sucursales.Enabled = false;
                }
            }

            return vista;
        }

        private static IApoderado VistaToDatos(IApoderadoVista vista)
        {
            int? loc = null; Int16? estado = null;

            if (vista.Localidades.Selected != "0")
                loc = Convert.ToInt32(vista.Localidades.Selected);

            if (vista.EstadosApoderados.Selected != "0")
                estado = Convert.ToInt16(vista.EstadosApoderados.Selected);

            string sucursal = null;
            if (vista.Sucursales.Selected != "0")
                sucursal = vista.Sucursales.Selected;


            IApoderado apoderado = new Apoderado
                                 {
                                     IdApoderado = vista.IdApoderado,
                                     IdBeneficiario = vista.IdBeneficiario,
                                     Apellido = vista.Apellido,
                                     Nombre = vista.Nombre,
                                     TipoDocumento = Convert.ToInt32(vista.TipoDocumento.Selected),
                                     NumeroDocumento = vista.NumeroDocumento,
                                     Cuil = vista.Cuil,
                                     Sexo = vista.Sexo.Selected == ((int)Enums.Sexo.Varon).ToString() ? "VARON" : "MUJER",
                                     IdSistema = Constantes.IdSistemaBanco,
                                     NumeroCuentaBco = vista.NumeroCuentaBco,
                                     Cbu = vista.Cbu,
                                     UsuarioBanco = vista.UsuarioBanco,
                                     IdSucursal = Convert.ToInt16(sucursal),
                                     IdMoneda = Convert.ToInt16(vista.Monedas.Selected),
                                     ApoderadoDesde = vista.ApoderadoDesde,
                                     ApoderadoHasta = vista.ApoderadoHasta ,
                                     IdEstadoApoderado = estado ?? (int)Enums.EstadoApoderado.Activo,
                                     FechaNacimiento = vista.FechaNacimiento,
                                     Calle = vista.Calle,
                                     Numero = vista.Numero,
                                     Piso = vista.Piso,
                                     Dpto = vista.Dpto,
                                     Monoblock = vista.Monoblock,
                                     Parcela = vista.Parcela,
                                     Manzana = vista.Manzana,
                                     EntreCalles = vista.EntreCalles,
                                     Barrio = vista.Barrio,
                                     CodigoPostal = vista.CodigoPostal,
                                     IdLocalidad = loc,
                                     TelefonoFijo = vista.TelefonoFijo,
                                     TelefonoCelular = vista.TelefonoCelular,
                                     Mail = vista.Mail,
                                    

                                 };

            return apoderado;
        }

        public bool ExistsApoderado(string nroDocumento, int idBeneficiario)
        {
            //existe un apoderado 
            if (_apoderadorepositorio.ExistsApoderado(nroDocumento) == false)
                return false;
            
            if (_apoderadorepositorio.EsApoderadoDeOtroBeneficiario(nroDocumento, idBeneficiario))
            {
                //es apoderado de otro beneficiario con estado activo o reemplazado. en este caso preguntar si quiere anular el ant
                return true;
            }
            
            //es apoderado de otro beneficiario pero con estado suspendido
            return false;
        }

        public bool UpdateCuentaCbuByBeneficiario(int idBeneciario, string nroCuenta, string cbu, int idsucursal)
        {
            return _apoderadorepositorio.UpdateCuentaCbuByBeneficiario(idBeneciario, nroCuenta, cbu, idsucursal);
        }

        public IApoderadoVista GetApoderadoByBeneficiarioActivo(int idBeneficiario)
        {

            var apoderado = _apoderadorepositorio.GetApoderadoByBeneficiarioActivo(idBeneficiario);

            IApoderadoVista vista = new ApoderadoVista
                                        {
                                            Apellido = apoderado.Apellido,
                                            Nombre = apoderado.Nombre,
                                            Barrio = apoderado.Barrio,
                                            Calle = apoderado.Calle,
                                            Cbu = apoderado.Cbu,
                                            Cuil = apoderado.Cuil,
                                            CodigoPostal = apoderado.CodigoPostal,
                                            Dpto = apoderado.CodigoPostal,
                                            EntreCalles = apoderado.EntreCalles,
                                            IdApoderado = apoderado.IdApoderado,
                                            IdBeneficiario = apoderado.IdBeneficiario,
                                            IdEstadoApoderado = apoderado.IdEstadoApoderado,
                                            IdLocalidad = apoderado.IdLocalidad ?? 0,
                                            IdMoneda = apoderado.IdMoneda,
                                            IdSistema = apoderado.IdSistema,
                                            IdSucursal = apoderado.IdSucursal,
                                            FechaNacimiento = apoderado.FechaNacimiento,
                                            Mail = apoderado.Mail,
                                            Manzana = apoderado.Manzana,
                                            Monoblock = apoderado.Monoblock,
                                            Numero = apoderado.Numero,
                                            NumeroCuentaBco = apoderado.NumeroCuentaBco ?? 0,
                                            NumeroDocumento = apoderado.NumeroDocumento,
                                            Parcela = apoderado.Parcela,
                                            Piso = apoderado.Piso,
                                            TelefonoCelular = apoderado.TelefonoCelular,
                                            TelefonoFijo = apoderado.TelefonoFijo,
                                            UsuarioBanco = apoderado.UsuarioBanco
                                        };
            return vista;
        }

        public bool DeleteApoderado(IApoderadoVista apoderado)
        {
            return _apoderadorepositorio.DeleteApoderado(VistaToDatos(apoderado));
        }

        public bool CambiarEstado(IApoderadoVista apoderado)
        {

            if (apoderado.IdEstadoApoderado == (int)Enums.EstadoApoderado.Activo)
            {
                apoderado.ApoderadoHasta = null;
            }
            else
            {
                apoderado.ApoderadoHasta = DateTime.Now;
            }

            return _apoderadorepositorio.CambiarEstado(VistaToDatos(apoderado));
        }

        public bool LimpiarFechaSolicitud(IApoderadoVista apoderado)
        {
            apoderado.RealizoElCambio = _apoderadorepositorio.LimpiarFechaSolicitud(apoderado.IdApoderado);

            return apoderado.RealizoElCambio;
        }

        public bool EsApoderadoActivo(string nroDocumento)
        {
            return _apoderadorepositorio.EsApoderadoActivo(nroDocumento);
        }

        public int AddApoderado(IApoderadoVista vista)
        {

            if (vista.EstadosApoderados.Selected == ((int)Enums.EstadoApoderado.Activo).ToString())
            {
                //Busco los beneficiarios que tiene este apoderado con estado activo o reemplazado y 
                //les cambio el estado a SUSPENDIDO
                //_apoderadorepositorio.SuspenderBeneficiarios(vista.NumeroDocumento);
            }
            
            //cargo el apoderado para el beneficiario que se esta editando
            vista.IdApoderado = _apoderadorepositorio.AddApoderado(VistaToDatos(vista));
            vista.Accion = vista.Accion;

            CargarLocalidades(vista, _localidadrepositorio.GetLocalidades());
            CargarEstadosApoderado(vista, _estadoapoderadorepositorio.GetEstadosApoderado());
            CargarSexo(vista);
            CargarTipoDocumento(vista);
            CargarMonedas(vista, _tablabcocbarepositorio.GetMonedas());
            CargarSistemas(vista, _tablabcocbarepositorio.GetSistemas());
            CargarSucursales(vista, _tablabcocbarepositorio.GetSucursales());

            return 0;
        }

        public int UpdateApoderado(IApoderadoVista vista)
        {

            if ((Convert.ToInt32(vista.EstadosApoderados.Selected) == (int)Enums.EstadoApoderado.Reemplazado) || (Convert.ToInt32(vista.EstadosApoderados.Selected) == (int)Enums.EstadoApoderado.Suspendido))
            {
                vista.ApoderadoHasta = DateTime.Now;
            }
            else
            {
                vista.ApoderadoHasta = null;
            }

            _apoderadorepositorio.UpdateApoderado(VistaToDatos(vista));
            return 0;
        }
        #region Privates

        private static void CargarSexo(IApoderadoVista vista)
        {
            vista.Sexo.Combo.Add(new ComboItem { Id = (int)Enums.Sexo.Mujer, Description = "MUJER" });
            vista.Sexo.Combo.Add(new ComboItem { Id = (int)Enums.Sexo.Varon, Description = "VARON" });
        }

        private static void CargarMonedas(IApoderadoVista vista, IEnumerable<ITablaBcoCba> listaMonedas)
        {
            foreach (var lMoneda in listaMonedas)
            {
                vista.Monedas.Combo.Add(new ComboItem
                {
                    Id = Convert.ToInt32(lMoneda.IdTablaBcoCba),
                    Description = lMoneda.DescripcionBcoCba                   

                });

                if (vista.Monedas.Selected == null)
                {
                    vista.Monedas.Selected = lMoneda.IdTablaBcoCba.ToString();
                    vista.IdMoneda = lMoneda.IdTablaBcoCba;
                }

            }
        }

        private static void CargarSistemas(IApoderadoVista vista, IEnumerable<ITablaBcoCba> listaSistemas)
        {
            foreach (var lSistema in listaSistemas)
            {
                vista.Sistemas.Combo.Add(new ComboItem
                {
                    Id = Convert.ToInt32(lSistema.IdTablaBcoCba),
                    Description = lSistema.DescripcionBcoCba
                });
            }
        }

        private static void CargarSucursales(IApoderadoVista vista, IEnumerable<ITablaBcoCba> listaSucursales)
        {
            vista.Sucursales.Combo.Add(new ComboItem
            {
                Id = 0,
                Description = "0 - Ninguna"
            });

            foreach (var lSucursal in listaSucursales)
            {
                vista.Sucursales.Combo.Add(new ComboItem
                {
                    Id = Convert.ToInt32(lSucursal.IdTablaBcoCba),
                    Description = lSucursal.CodigoBcoCba + " - " + lSucursal.DescripcionBcoCba
                });
            }
        }

        private static void CargarLocalidades(IApoderadoVista vista, IEnumerable<ILocalidad> listaLocalidades)
        {
            vista.Localidades.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione una localidad..." });
            foreach (var lLocalidad in listaLocalidades)
            {
                vista.Localidades.Combo.Add(new ComboItem
                                                {
                                                    Id = lLocalidad.IdLocalidad,
                                                    Description = lLocalidad.NombreLocalidad
                                                });
            }
        }

        private static void CargarTipoDocumento(IApoderadoVista vista)
        {
            vista.TipoDocumento.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.Dni, Description = "DNI" });
            vista.TipoDocumento.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.Le, Description = "LE" });
            vista.TipoDocumento.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.Lc, Description = "LC" });
            vista.TipoDocumento.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.Ci, Description = "CI" });
            vista.TipoDocumento.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.NuncaTuvo, Description = "Nunca Tuvo" });
            vista.TipoDocumento.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.CedulaExtranjera, Description = "Cedula Extranjera" });
            vista.TipoDocumento.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.Otro, Description = "Otro" });
        }

        private static void CargarEstadosApoderado(IApoderadoVista vista, IEnumerable<IEstadoApoderado> listaEstadosApoderado)
        {
            //vista.EstadosApoderados.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione un estado..." });
            foreach (var lEstado in listaEstadosApoderado)
            {
                vista.EstadosApoderados.Combo.Add(new ComboItem
                                                      {
                                                          Id = lEstado.IdEstadoApoderado,
                                                          Description = lEstado.NombreEstadoApoderado
                                                      });
            }
        }
        #endregion
    }
}
