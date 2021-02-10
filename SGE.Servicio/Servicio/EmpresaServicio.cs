using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;
using SGE.Servicio.Comun;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using System.Transactions;
using EfficientlyLazy.Crypto;
using System.Web;
using System.IO;
using NPOI.HSSF.UserModel;
using Excel;
using System.Globalization;

namespace SGE.Servicio.Servicio
{
    public class EmpresaServicio : BaseServicio, IEmpresaServicio
    {
        private readonly IEmpresaRepositorio _empresaRepositorio;
        private readonly ILocalidadRepositorio _localidadrepositorio;
        private readonly SedeRepositorio _sedeRepositorio;
        private readonly FichaRepositorio _fichaRepositorio;
        private readonly IAutenticacionServicio _aut;
        private readonly IConceptoRepositorio _conceptoRepositorio;

        public EmpresaServicio()
        {
            _empresaRepositorio = new EmpresaRepositorio();
            _aut = new AutenticacionServicio();
            _localidadrepositorio = new LocalidadRepositorio();
            _sedeRepositorio = new SedeRepositorio();
            _fichaRepositorio = new FichaRepositorio();
            _conceptoRepositorio = new ConceptoRepositorio();
        }

        public IEmpresasVista GetEmpresas()
        {
            IEmpresasVista vista = new EmpresasVista();

            var pager = new Pager(_empresaRepositorio.GetEmpresasCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexEmpresa", _aut.GetUrl("IndexPager", "Empresas"));

            vista.Pager = pager;

            vista.Empresas = _empresaRepositorio.GetEmpresas(pager.Skip, pager.PageSize);

            return vista;
        }

        public IEmpresasVista GetIndex()
        {
            IEmpresasVista vista = new EmpresasVista();

            var pager = new Pager(0,
                                 Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                 "FormIndexEmpresa", _aut.GetUrl("IndexPager", "Empresas"));

            vista.Pager = pager;

            return vista;
        }

        public IEmpresasVista GetEmpresas(string descripcion, string cuit)
        {
            IEmpresasVista vista = new EmpresasVista();

            var pager = new Pager(_empresaRepositorio.GetEmpresas(descripcion, cuit).Count,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexEmpresa", _aut.GetUrl("IndexPager", "Empresas"));

            vista.Pager = pager;

            vista.Empresas = _empresaRepositorio.GetEmpresas(descripcion, cuit, pager.Skip, pager.PageSize);

            return vista;
        }

        public IEmpresaVista GetEmpresa(int idEmpresa, string accion)
        {
            IEmpresaVista vista = new EmpresaVista { Accion = accion };

            CargarLocalidades(vista, _localidadrepositorio.GetLocalidades());
            CargarComboSiNo(vista);

            if (idEmpresa >= 0)
            {
                IEmpresa objreturn = _empresaRepositorio.GetEmpresa(idEmpresa);

                vista.Id = objreturn.IdEmpresa;
                vista.Descripcion = objreturn.NombreEmpresa;
                vista.Localidades.Selected = objreturn.IdLocalidad.ToString();

                string dom;
                switch (objreturn.DomicilioLaboralIdem)
                {
                    case "S":
                    case "Y":
                        dom = "1";
                        break;
                    case "N":
                        dom = "2";
                        break;
                    default:
                        dom = "0";
                        break;
                }

                vista.DomicilioLaboralIdem.Selected = dom;
                vista.Cuit = objreturn.Cuit;
                vista.CuitInicio = objreturn.Cuit;
                vista.CodigoActividad = objreturn.CodigoActividad;
                vista.CantidadEmpleados = Convert.ToInt16(objreturn.CantidadEmpleados);
                vista.Calle = objreturn.Calle;
                vista.Numero = objreturn.Numero;
                vista.Piso = objreturn.Piso;
                vista.Dpto = objreturn.Dpto;
                vista.CodigoPostal = objreturn.CodigoPostal;

                vista.IdUsuario = objreturn.IdUsuario;
                vista.NombreUsuario = objreturn.NombreUsuario;
                vista.ApellidoUsuario = objreturn.ApellidoUsuario;
                vista.Cuil = objreturn.Cuil;
                vista.Telefono = objreturn.Telefono;
                vista.Mail = objreturn.Mail;
                // 05/03/2013 DI CAMPLI LEANDRO
                vista.LoginUsuario = objreturn.LoginUsuario;
                //vista.Password = ss // DataHashing.Compute( //(Algorithm.SHA1, objreturn.PasswordUsuario);
                // 23/10/2016
                vista.celular = objreturn.celular;
                vista.mailEmp = objreturn.mailEmp;
                vista.verificada = objreturn.verificada;
                // 25/09/2018
                vista.EsCooperativa = objreturn.EsCooperativa;
                // 20/09/2019
                vista.CBU = objreturn.CBU;
                vista.CUENTA = objreturn.CUENTA;
                vista.TIPOCUENTA = objreturn.TIPOCUENTA;
                // 25/03/2020
                vista.FEC_ADHESION = objreturn.FEC_ADHESION;

                vista.Sedes = _sedeRepositorio.GetSedesByEmpresa(objreturn.IdEmpresa);
                vista.ListadoFichasPpp = _fichaRepositorio.GetFichasByEmpresa(objreturn.IdEmpresa);
                int cantBenefi = 0;
                int cantBenDisc = 0;
                foreach (Ficha f in vista.ListadoFichasPpp)
                {
                    if (f.BeneficiarioFicha.IdEstado == 2)
                    {
                        cantBenefi++;

                        if (f.EsDiscapacitado == true)
                        {
                            cantBenDisc++;
                        }
                    
                    }
                
                }

                vista.cantBenef = cantBenefi;
                vista.cantBenDisc = cantBenDisc;
                vista.Accion = accion;
                if (accion.Equals("Modificar") || (accion.Equals("Agregar")))
                {
                    vista.Localidades.Enabled = true;
                    vista.DomicilioLaboralIdem.Enabled = true;
                }
                else if (accion.Equals("Ver") || accion.Equals("Eliminar"))
                {
                    vista.Localidades.Enabled = false;
                    vista.DomicilioLaboralIdem.Enabled = false;
                }
            }

            return vista;
        }

        public IEmpresasVista GetEmpresas(Pager pPager, int id, string descripcion, string cuit)
        {
            IEmpresasVista vista = new EmpresasVista();

            var pager = new Pager(_empresaRepositorio.GetEmpresas(descripcion, cuit).Count,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexEmpresa", _aut.GetUrl("IndexPager", "Empresas"));

            if (pager.TotalCount != pPager.TotalCount)
            {
                pager.PageNumber = 1;
                pager.Skip = 0;
                vista.Pager = pager;
            }
            else
            {
                vista.Pager = pPager;
            }

            vista.Empresas = _empresaRepositorio.GetEmpresas(descripcion, cuit, vista.Pager.Skip, vista.Pager.PageSize);
            vista.DescripcionBusqueda = descripcion;
            vista.CuitBusqueda = cuit;

            return vista;
        }

        private static IEmpresa VistaToDatos(IEmpresaVista vista)
        {
            string dom; int? loc = null; Int16? cantEmp = null;
            string cuentabancor = "N";
            switch (vista.DomicilioLaboralIdem.Selected)
            {
                case "1":
                    dom = "S";
                    break;
                case "2":
                    dom = "N";
                    break;
                default:
                    dom = "";
                    break;
            }

            if (vista.Localidades.Selected != "0")
                loc = Convert.ToInt32(vista.Localidades.Selected);

            if (vista.CantidadEmpleados != null)
                cantEmp = Convert.ToInt16(vista.CantidadEmpleados);
            if (string.IsNullOrEmpty(vista.CBU))
            { cuentabancor = "N"; }
            else
            {
                if (vista.CBU.Substring(0, 3) == "020")
                {
                    cuentabancor = "S";
                
                }
            
            }
            IEmpresa emp = new Empresa
            {
                IdEmpresa = vista.Id,
                NombreEmpresa = vista.Descripcion,
                IdLocalidad = loc,
                Cuit = vista.Cuit,
                CodigoActividad = vista.CodigoActividad,
                DomicilioLaboralIdem = dom,
                CantidadEmpleados = cantEmp,
                Calle = vista.Calle,
                Numero = vista.Numero,
                Piso = vista.Piso,
                Dpto = vista.Dpto,
                CodigoPostal = vista.CodigoPostal,
                IdUsuario = vista.IdUsuario,
                celular = vista.celular,
                mailEmp = vista.mailEmp,
                verificada = vista.verificada,
                EsCooperativa = vista.EsCooperativa,
                CBU = vista.CBU,
                CUENTA = vista.CUENTA,
                TIPOCUENTA = vista.TIPOCUENTA,
                CUENTA_BANCOR = cuentabancor,
                FEC_ADHESION = vista.FEC_ADHESION,
            };

            return emp;
        }

        public int AddEmpresa(IEmpresaVista vista)
        {
            //Valida que el nuevo cuit no exista para otra empresa
            if (_empresaRepositorio.ExistsCuit(vista.Cuit))
            {
                CargarLocalidades(vista, _localidadrepositorio.GetLocalidades());
                CargarComboSiNo(vista);
                AddError("Ya existe una empresa con el mismo CUIT");
                return 0;
            }
            vista.Id = _empresaRepositorio.AddEmpresa(VistaToDatos(vista));
            vista.Accion = vista.Accion;
            //CargarLocalidades(vista, _localidadrepositorio.GetLocalidades());
            //CargarComboSiNo(vista);
            return vista.Id;
        }

        public int UpdateEmpresa(IEmpresaVista vista)
        {
            //Valida que el nuevo cuit no exista para otra empresa
            if (vista.CuitInicio != vista.Cuit)
            {
                if (_empresaRepositorio.ExistsCuit(vista.Cuit))
                {
                    CargarLocalidades(vista, _localidadrepositorio.GetLocalidades());
                    CargarComboSiNo(vista);
                    AddError("Ya existe una empresa con el mismo CUIT");
                    return 0;
                }
            }
            _empresaRepositorio.UpdateEmpresa(VistaToDatos(vista));

            return vista.Id;
        }

        public int DeleteEmpresa(IEmpresaVista vista)
        {
            //Si la empresa tiene fichas asociadas no se la puede eliminar
            if (_empresaRepositorio.EmpresaEnUso(vista.Id))
            {
                return 1;
            }

            _empresaRepositorio.DeleteEmpresa(VistaToDatos(vista));

            return 0;
        }

        //Validacion utilizada para eliminar una empresa
        public bool EmpresaEnUso(int idEmpresa)
        {
            return _empresaRepositorio.EmpresaEnUso(idEmpresa);
        }

        //Validacion utilizada para no duplicar un cuit 
        public bool ExistsCuit(string cuit)
        {
            return _empresaRepositorio.ExistsCuit(cuit);
        }

        private static void CargarLocalidades(IEmpresaVista vista, IEnumerable<ILocalidad> listaLocalidades)
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

        private static void CargarComboSiNo(IEmpresaVista vista)
        {
            vista.DomicilioLaboralIdem.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione un valor..." });
            vista.DomicilioLaboralIdem.Combo.Add(new ComboItem { Id = 1, Description = "Si" });
            vista.DomicilioLaboralIdem.Combo.Add(new ComboItem { Id = 2, Description = "No" });
        }

        private static IEmpresa VistaToDatosUsuariosEmpresa(IEmpresaVista vista)
        {
            IEmpresa emp = new Empresa
            {
                ApellidoUsuario = vista.ApellidoUsuario,
                NombreUsuario = vista.NombreUsuario,
                Cuil = vista.Cuil,
                LoginUsuario = vista.LoginUsuario ?? vista.Cuit,//vista.Cuit ?? vista.Cuil, // 18/04/2013 - DI CAMPLI LEANDRO - SE MODIFICA LA CREACION DEL LOGIN 
                PasswordUsuario = vista.Password,
                Telefono = vista.Telefono,
                Mail = vista.Mail,
                IdUsuario = vista.IdUsuario,
                IdEmpresa = vista.Id,
            };

            return emp;
        }

        public int AddUsuarioEmpresa(IEmpresaVista vista)
        {
            using (var transaction = new TransactionScope())
            {
                //Antes de asignar un usuario, se valida que la empresa exista. Si no existe, primero se inserta la empresa
                if (!_empresaRepositorio.ExistsEmpresa(vista.Cuit, vista.Descripcion))
                {
                    //Se da de alta la empresa
                    vista.Id = _empresaRepositorio.AddEmpresa(VistaToDatos(vista));
                }

                //Se asigna el usuario
                vista.Password = Convert.ToString(DateTime.Now.Month).PadLeft(2, '0') + Convert.ToString(DateTime.Now.Year);
                vista.Password = DataHashing.Compute(Algorithm.SHA1, vista.Password);
                vista.IdUsuario = _empresaRepositorio.AddUsuariosEmpresa(VistaToDatosUsuariosEmpresa(vista));


                CargarLocalidades(vista, _localidadrepositorio.GetLocalidades());
                CargarComboSiNo(vista);

                transaction.Complete();

                return 0;
            }
        }

        public int UpdateUsuarioEmpresa(IEmpresaVista vista)
        {
            _empresaRepositorio.UpdateUsuarioEmpresa(VistaToDatosUsuariosEmpresa(vista));

            return 0;
        }

        public bool ExistsEmpresa(string cuit, string descripcion)
        {
            return _empresaRepositorio.ExistsEmpresa(cuit, descripcion);
        }

        public bool EsNuevaEmpresa(int? idEmpresa)
        {
            return _empresaRepositorio.EsNuevaEmpresa(idEmpresa);
        }

        //Metodo para cargar listado de empresas, para seleccionar el destino de las fichas
        public IEmpresasVista CargarListaEmpresasDestino()
        {
            IEmpresasVista vista = new EmpresasVista();

            var pager = new Pager(_empresaRepositorio.GetEmpresasCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormDesvincularFichas", _aut.GetUrl("IndexPagerSelecccionarEmpresa", "Empresas"));

            vista.Pager = pager;

            vista.Empresas = _empresaRepositorio.GetEmpresas(pager.Skip, pager.PageSize);

            return vista;
        }

        public IEmpresasVista CargarListaEmpresasDestino(string descripcion, string cuit, int idempresaorigen)
        {
            IEmpresasVista vista = new EmpresasVista();

            var pager = new Pager(_empresaRepositorio.GetEmpresas(descripcion, cuit).Count,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormDesvincularFichas", _aut.GetUrl("IndexPagerSelecccionarEmpresa", "Empresas"));

            vista.Pager = pager;

            vista.ObjEmpresaOrigen = GetEmpresa(idempresaorigen, "");
            vista.IdEmpresaOrigen = vista.ObjEmpresaOrigen.Id;
            vista.Empresas = _empresaRepositorio.GetEmpresas(descripcion, cuit, pager.Skip, pager.PageSize);
            vista.Descripcion = descripcion;
            vista.Cuit = cuit;
            return vista;
        }

        public IEmpresasVista CargarListaEmpresasDestino(Pager pPager, int id, string descripcion, string cuit)
        {
            IEmpresasVista vista = new EmpresasVista();

            var pager = new Pager(_empresaRepositorio.GetEmpresas(descripcion, cuit).Count,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormDesvincularFichas", _aut.GetUrl("IndexPagerSelecccionarEmpresa", "Empresas"));

            if (pager.TotalCount != pPager.TotalCount)
            {
                pager.PageNumber = 1;
                pager.Skip = 0;
                vista.Pager = pager;
            }
            else
            {
                vista.Pager = pPager;
            }

            vista.Empresas = _empresaRepositorio.GetEmpresas(descripcion, cuit, vista.Pager.Skip, vista.Pager.PageSize);
            vista.Descripcion = descripcion;
            vista.Cuit = cuit;

            return vista;
        }

        public bool DesvincularFichas(int idOrigen, int idDestino)
        {
            if (idOrigen == idDestino)
            {
                AddError("El destino y el Origen no puede ser el mismo, verifique los datos");
                return false;
            }

            if (!_empresaRepositorio.UpdateFichaPpp(idOrigen, idDestino))
            {
                AddError("Surgio un error, no se pudo realizar el cambio, verifique los datos.");
                return false;
            }

            return true;
        }

        public IEmpresaVista BuscarFicha(IEmpresaVista vista)
        {
            vista.ListadoFichasPpp =
                vista.ListadoFichasPpp.OrderByDescending(c => c.NumeroDocumento == vista.BusquedaFicha).
                    ToList();

            vista.FichaEncontrada =
                (vista.ListadoFichasPpp.Where(c => c.NumeroDocumento == vista.BusquedaFicha).Count() >= 1
                     ? vista.BusquedaFicha
                     : "N");

            vista.TabSeleccionado = "2";
            return vista;
        }

        public IEmpresaVista GetEmpresa(string cuit, string accion)
        {
            IEmpresaVista vista = null;


            if (!string.IsNullOrEmpty(cuit))
            {
                IEmpresa objreturn = _empresaRepositorio.GetEmpresa(cuit);

                if (objreturn != null)
                {

                    vista = new EmpresaVista { Accion = accion };

                    CargarLocalidades(vista, _localidadrepositorio.GetLocalidades());
                    CargarComboSiNo(vista);


                    vista.Id = objreturn.IdEmpresa;
                    vista.Descripcion = objreturn.NombreEmpresa;
                    vista.Localidades.Selected = objreturn.IdLocalidad.ToString();

                    string dom;
                    switch (objreturn.DomicilioLaboralIdem)
                    {
                        case "S":
                        case "Y":
                            dom = "1";
                            vista.IdDomicilioLaboral = 1;
                            vista.NombreDomicilioLaboralSeleccionado = "Si";
                            break;
                        case "N":
                            dom = "2";
                            vista.IdDomicilioLaboral = 2;
                            vista.NombreDomicilioLaboralSeleccionado = "No";
                            break;
                        default:
                            dom = "0";
                            vista.IdDomicilioLaboral = 0;
                            vista.NombreDomicilioLaboralSeleccionado = "";
                            break;
                    }

                    vista.DomicilioLaboralIdem.Selected = dom;
                    vista.Cuit = objreturn.Cuit;
                    vista.CodigoActividad = objreturn.CodigoActividad;
                    vista.CantidadEmpleados = Convert.ToInt16(objreturn.CantidadEmpleados);
                    vista.Calle = objreturn.Calle;
                    vista.Numero = objreturn.Numero;
                    vista.Piso = objreturn.Piso;
                    vista.Dpto = objreturn.Dpto;
                    vista.CodigoPostal = objreturn.CodigoPostal;

                    vista.IdUsuario = objreturn.IdUsuario;
                    vista.NombreUsuario = objreturn.NombreUsuario;
                    vista.ApellidoUsuario = objreturn.ApellidoUsuario;
                    vista.Cuil = objreturn.Cuil;
                    vista.Telefono = objreturn.Telefono;
                    vista.Mail = objreturn.Mail;

                    vista.Accion = accion;
                    vista.Localidades.Enabled = false;
                    vista.DomicilioLaboralIdem.Enabled = false;
                    vista.IdLocalidad = objreturn.IdLocalidad ?? 0;
                    vista.NombreLocalidad = objreturn.NombreLocalidad;
                }
            }

            return vista;
        }

        public void BlanquearCuentaUsr(IEmpresaVista model)
        {

            model.Password = Convert.ToString(DateTime.Now.Month).PadLeft(2,'0') + Convert.ToString(DateTime.Now.Year);
            model.Password = DataHashing.Compute(Algorithm.SHA1, model.Password);
            _empresaRepositorio.BlanquearCuentaUsr(VistaToDatosUsuariosEmpresa(model));

        }

        public IList<IError> GetErrores()
        {
            return Errors;
        }
        // 16/01/2014 - DI CAMPLI LEANDRO - EXPORTAR LAS FICHAS DESDE EL FORMULARIO DE EMPRESA
        public byte[] ExportFichasEmpresa(IEmpresaVista model)
        {
            string path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateFichasEmpresa.xls");

            var fs =
                new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Fichas");
            
            int i = 0;

            foreach (var ficha in model.ListadoFichasPpp)
            {
                i++;

                var row = sheet.CreateRow(i);

                var celapellido = row.CreateCell(0);
                celapellido.SetCellValue(ficha.Apellido);

                var celnombre = row.CreateCell(1);
                celnombre.SetCellValue(ficha.Nombre);

                var celdni = row.CreateCell(2);
                celdni.SetCellValue(ficha.NumeroDocumento);


                var celestado = row.CreateCell(3);
                celestado.SetCellValue(ficha.NombreEstadoFicha);

                var celcondicion = row.CreateCell(4);
                celcondicion.SetCellValue(ficha.BeneficiarioFicha.NombreEstado ?? "");

                var celprograma = row.CreateCell(5);
                celprograma.SetCellValue(ficha.NombreTipoFicha);

                var celetapa = row.CreateCell(6);
                celetapa.SetCellValue(ficha.NombreEtapa);


            }

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }


        //02/06/2019 - RECUPERO CUPONES DEL BANCO
        public IRecuperoVista indexRecupero()
        {

            IRecuperoVista vista = new RecuperoVista();
            IList<IRecupero> lista = new List<IRecupero>();
            //var pager = new Pager(0, Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
            //                      "FormAltaMasivaBenef", _aut.GetUrl("IndexPager", "Fichas"));
            //vista.pager = pager;
            vista.Registros = lista;


            return vista;

        }

        public IRecuperoVista SubirArchivoRecupero(HttpPostedFileBase archivo, IRecuperoVista model)
        {


                if (archivo != null && archivo.ContentLength > 0)
            {
                var fileName = Path.GetFileName(archivo.FileName);
                var nombre = fileName;
                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Archivos/Importados"), nombre);
                archivo.SaveAs(path);

                model.ProcesarArchivo = true;
                model.Mensaje = "El archivo \"" + fileName + "\" fue subido al servidor correctamente. Confirme la acción de Procesar si lo cree conveniente.";
                model.ArchivoNombre = nombre;
            }
            else
            {
                 model.ProcesarArchivo = true;
                model.Mensaje = "El archivo no pudo ser subido al servidor. Vuelva a intentar.";
            }



                IList<IRecupero> lista = new List<IRecupero>();
            model.Registros = lista;

            return model;
        }

        public IRecuperoVista ProcesarArchivo(IRecuperoVista model)
        {
            string path = HttpContext.Current.Server.MapPath(@"\Archivos\Importados") + @"\" + model.ArchivoNombre;
            IList<IRecupero> lrecupero = new List<IRecupero>();
            int cantProcesados = 0;
            int cantError = 0;
            if (File.Exists(path))
            {
                using (var sr = new StreamReader(path))
                {
                    string nombreArchivo="";
                    DateTime fechaProceso = DateTime.Now;
                    DateTime fechaCobro = DateTime.Now;
                    DateTime fechaRendicion = DateTime.Now;
                    
                    while (sr.Peek() >= 0)
                    {
                        
                        var linea = sr.ReadLine();

                        if (linea != null)
                        {
                                if (linea.Substring(0, 1) == "1")
                                {
                                    nombreArchivo = linea.Substring(1, 10);
                                    fechaProceso = Convert.ToDateTime(linea.Substring(15, 2) + "/" + linea.Substring(13, 2) + "/" + linea.Substring(11, 2));
                                }
                                //if (linea.Substring(0, 1) == "3")
                                //{
                                //    nombreArchivo = linea.Substring(2, 11);
                                //    fechaProceso = Convert.ToDateTime(linea.Substring(16, 2) + "/" + linea.Substring(14, 2) + "/" + linea.Substring(12, 2));
                                //}
                          if (linea.Substring(0, 1) == "2")
                          {


                            IRecupero recupero = new Recupero();
                            try
                            {
                                    recupero.N_REGISTRO = linea;
                                    recupero.N_REG_CUPON = (linea.Substring(0,50)).Substring(1,49);
                                    recupero.CONVENIO =  linea.Substring(1, 4);
                                    recupero.GRPCONVENIO = linea.Substring(5, 2);
                                    recupero.ID_CUPON = Convert.ToInt32( linea.Substring(7, 16));
                                    //recupero.FEC_VENCIMIENTO = Convert.ToInt32(linea.Substring(7, 16));
                                    //recupero.IMPORTE = 4000;
                                    //recupero.PAGADO
                                    //FEC_MODIF
                                    recupero.NombreArchivo = nombreArchivo + ".dat";
                                    recupero.FEC_PROCESO = fechaProceso;
                                    recupero.FEC_COBRO = Convert.ToDateTime(linea.Substring(54, 2) + "/" + linea.Substring(52, 2) + "/" + linea.Substring(50, 2));
                                    recupero.FEC_RENDICION = Convert.ToDateTime(linea.Substring(60, 2) + "/" + linea.Substring(58, 2) + "/" + linea.Substring(56, 2));
                                    recupero.SUC_COBRO = linea.Substring(62, 4);
                                    recupero.ESTADO = "REGISTRADO";
                                    lrecupero.Add(recupero);
                                    cantProcesados++;
                               
                            }
                            catch(Exception EX)
                            {
                                recupero.N_REGISTRO = linea;
                                recupero.ESTADO = "ERROR AL REGISTRAR";
                                lrecupero.Add(recupero);
                                cantError++;
                            }
                          }
                        }




                    }

                    _empresaRepositorio.udpCuponRecupero(lrecupero);
                }
            }
            cantProcesados = lrecupero.Where(c=>c.ESTADO == "REGISTRADO").Count();
            cantError = lrecupero.Where(c => c.ESTADO != "REGISTRADO").Count();
            model.cantProcesados = cantProcesados;
            model.procesadosFail = cantError;
            model.Registros = lrecupero;

            return model;
        }

        public byte[] ExportRecupero(IRecuperoVista model)
        {

            var cupones = _empresaRepositorio.getRecupero(model.NombreArchivo);
            string path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateRecuperoCupon.xls");

            var fs =
                new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Cupones");
            int i = 0;

            foreach (var cupon in cupones)
            {
                i++;

                var row = sheet.CreateRow(i);
                var celIdCupon = row.CreateCell(0);
                celIdCupon.SetCellValue(cupon.ID_CUPON.ToString());

                var celIdEmp = row.CreateCell(1);
                celIdEmp.SetCellValue(cupon.ID_EMPRESA.ToString());

                var celnombre = row.CreateCell(2);
                celnombre.SetCellValue(cupon.N_EMPRESA);

                var celEstado = row.CreateCell(3);
                celEstado.SetCellValue(cupon.ESTADO);

                var celImporte = row.CreateCell(4);
                celImporte.SetCellValue(cupon.IMPORTE.ToString());

                var celPeriodo = row.CreateCell(5);
                celPeriodo.SetCellValue(cupon.PERIODO);

                var celArchivo = row.CreateCell(6);
                celArchivo.SetCellValue(cupon.NombreArchivo);

                var celFecProc = row.CreateCell(7);
                celFecProc.SetCellValue(cupon.FEC_PROCESO.HasValue ? cupon.FEC_PROCESO.ToString() : "");

                var celFecCobro = row.CreateCell(8);
                celFecCobro.SetCellValue(cupon.FEC_COBRO.HasValue ? cupon.FEC_COBRO.ToString() : "");

                var celSuc = row.CreateCell(9);
                celSuc.SetCellValue(cupon.SUC_COBRO);

                var celCodigoBarra = row.CreateCell(10);
                celCodigoBarra.SetCellValue(cupon.N_REG_CUPON);

            }

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }


        //02/09/2019 - RECUPERO DEBITO PARA EL BANCO
        public IRecuperoDebitoVista indexRecuperoDebito()
        {

            IRecuperoDebitoVista vista = new RecuperoDebitoVista();
            IList<IRecuperoDebito> lista = new List<IRecuperoDebito>();

            //IList<IConcepto> listaconceptos =
            //                      _conceptoRepositorio.GetConceptos(null, null, "").Where(
            //                          c => c.Año <= DateTime.Now.Year /*&& (c.Mes <= DateTime.Now.Month)*/).OrderByDescending(
            //                              c => c.Año).ThenByDescending(c => c.Mes).Take(13).
            //                          ToList();
            IList<IConcepto> listaconceptos =
                                _conceptoRepositorio.GetConceptos(null, null, "").Where(
                                    c => (c.Año <= DateTime.Now.Year /*&& (c.Mes <= DateTime.Now.Month)*/) || c.Año == (DateTime.Now.Year - 2)/*c.Año <= DateTime.Now.Year && (c.Mes <= DateTime.Now.Month)*/).OrderByDescending(
                                        c => c.Año).ThenByDescending(c => c.Mes).
                                    ToList();
            vista.Registros = lista;
            CargarConcepto(vista, listaconceptos);

            return vista;

        }

        private static void CargarConcepto(IRecuperoDebitoVista vista, IEnumerable<IConcepto> listaConceptos)
        {
            foreach (var listaconceptos in listaConceptos)
            {
                vista.Conceptos.Combo.Add(new ComboItem
                {
                    Id = listaconceptos.Id_Concepto,
                    Description =
                        listaconceptos.Año + " - " +
                        CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[
                            listaconceptos.Mes - 1].ToUpper()
                });

            }
        }

        public IRecuperoDebitoVista getRecuperosConcepto(IRecuperoDebitoVista model)
        {

            model.Registros = _empresaRepositorio.getRecuperosDebito(0,Convert.ToInt32(model.Conceptos.Selected)).ToList();
            return model;
        }




        public IRecuperoDebitoVista addLiqEmpresaDebito(IRecuperoDebitoVista model)
        {
            try
            {
                decimal montoesp = Convert.ToDecimal(ConfigurationManager.AppSettings["MontoCofinEspecial"]);
                IList<IRecuperoDebito> recuperos = _empresaRepositorio.getLiqEmpresaDebito(Convert.ToInt32(model.Conceptos.Selected)).ToList();
                string nomfile = string.Empty;
                if (recuperos.Count != 0)
                {
                    model.Mensaje = "Recuperos total" + recuperos.Count.ToString();
                    var fechasolicitud = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));


                    nomfile = "DEB" + Convert.ToString(DateTime.Now.Day) + Convert.ToString(DateTime.Now.Month) + Convert.ToString(DateTime.Now.Year) + Convert.ToString(DateTime.Now.Hour) + Convert.ToString(DateTime.Now.Minute) + Convert.ToString(DateTime.Now.Second) +
                                                           ".HAB";
                    int idrecupero = 0;
                    model.Mensaje = model.Mensaje  + "nomfile";
                    //IList<IAporteDebito> lAportes = _empresaRepositorio.getAporteDebito();
                    //++++++++++++++++++++++++Crear cabecera empresa y listar su detalle
                    var RecuperoEmp = from recuDeb in recuperos
                                      group recuDeb by
                                      new
                                      {
                                          recuDeb.ID_EMPRESA,
                                          recuDeb.N_EMPRESA,
                                          recuDeb.ID_CONCEPTO,
                                          recuDeb.ID_LIQUIDACION,
                                          recuDeb.COOPERATIVA,
                                          recuDeb.EMPLEADOS,
                                          recuDeb.CBU,
                                          recuDeb.CUENTA,
                                          recuDeb.CUENTA_BANCOR,
                                          recuDeb.CUIT,
                                          recuDeb.TIPO_CUENTA,

                                      } into grpEmpLiq


                                      select
                                                 new RecuperoDebEmpresa
                                                 {
                                                     ID_EMPRESA = grpEmpLiq.Key.ID_EMPRESA,
                                                     N_EMPRESA = grpEmpLiq.Key.N_EMPRESA,
                                                     ID_CONCEPTO = grpEmpLiq.Key.ID_CONCEPTO,
                                                     ID_LIQUIDACION = grpEmpLiq.Key.ID_LIQUIDACION,
                                                     COOPERATIVA = grpEmpLiq.Key.COOPERATIVA,
                                                     EMPLEADOS = grpEmpLiq.Key.EMPLEADOS,
                                                     CBU = grpEmpLiq.Key.CBU,
                                                     CUENTA = grpEmpLiq.Key.CUENTA,
                                                     CUENTA_BANCOR = grpEmpLiq.Key.CUENTA_BANCOR,
                                                     CUIT = grpEmpLiq.Key.CUIT,
                                                     TIPO_CUENTA = grpEmpLiq.Key.TIPO_CUENTA,
                                                 };
                    model.Mensaje = model.Mensaje + " agrupador";
                    var EmpresasREc = RecuperoEmp.ToList();
                    decimal montoTotal = 0;
                    decimal descuento = 0;
                    foreach (var recuemp in EmpresasREc)
                    {
                        
                        foreach (var benefliq in recuperos)
                        {
                            if (benefliq.ID_EMPRESA == recuemp.ID_EMPRESA)
                            {

                                DetalleRecuperoEmpresa d = new DetalleRecuperoEmpresa();
                                d.ID_BENEFICIARIO_CONCEP_LIQ = (int)benefliq.ID_BENEFICIARIO_CONCEP_LIQ;
                                //foreach (var aporte in lAportes)
                                //{ 
                                //    if(recuemp.EMPLEADOS <  
                                //}
                                //if (recuemp.EMPLEADOS >= 0 && recuemp.EMPLEADOS < 16) { d.MONTO_PARCIAL = 1500;}
                                //if (recuemp.EMPLEADOS >= 16 && recuemp.EMPLEADOS < 81) { d.MONTO_PARCIAL = 2500;}
                                //if (recuemp.EMPLEADOS >= 81 && recuemp.EMPLEADOS < 200000) { d.MONTO_PARCIAL = 3000; }
                                //d.MONTO_PARCIAL = benefliq.EMPLEADOS < 18 ?  1500 : 3000;
                                if (benefliq.CO_FINANCIAMIENTO == 1)
                                {
                                    d.MONTO_PARCIAL = benefliq.MONTO;
                                }
                                else
                                {
                                    d.MONTO_PARCIAL = montoesp;
                                }

                                montoTotal = montoTotal + d.MONTO_PARCIAL;
                                recuemp.detalle.Add(d);
                            }

                        }
                        //descuento = (montoTotal * 10 / 100);
                        recuemp.MONTO = montoTotal;//- descuento; // APLICAR EL 10% DE DESCUENTO
                        montoTotal = 0;
                    }
                    /////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    //model.Mensaje = model.Mensaje + "Fin foreach (var recuemp in EmpresasREc)";


                    //******************************ARMAR LOS REGISTROS***************************
                    CrearLineas(EmpresasREc, model.fechaDebito);
                    //******************************FIN ARMAR LOS REGISTROS***************************

                    //*************REGISTRAR*******************************************
                    idrecupero = _empresaRepositorio.AddRecupero(EmpresasREc, Convert.ToInt32(model.Conceptos.Selected), model.fechaDebito, nomfile);
                    //*************FIN REGISTRAR*******************************************
                    model.Mensaje = model.Mensaje + " idrecupero " + idrecupero.ToString();
                    //****************CONSULTAR Y DEVOLVER LAS CABECERAS DE LOS RECUPEROS DEL PERIODO**********************
                    model.Registros = _empresaRepositorio.getRecuperosDebito(idrecupero, 0);
                    //****************FIN CONSULTAR Y DEVOLVER LAS CABECERAS DE LOS RECUPEROS DEL PERIODO******************
                }
                else
                {
                    model.Registros = _empresaRepositorio.getRecuperosDebito(0, Convert.ToInt32(model.Conceptos.Selected));
                    model.Mensaje = "Recuperos total 0";
                }
                //IRecuperoDebitoVista model = new RecuperoDebitoVista();
                model.ArchivoNombre = nomfile;
                return model;
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.InnerException.ToString();
                return model;
            }
            finally
            {
                
            }
        }

        private void CrearLineas(List<RecuperoDebEmpresa> recuperoEmpresa, DateTime fechaDebito)
        {
            string TIPOCONVENIO = "";
            string SUCURSAL = "";
            string MONEDA = "";
            string SISTEMA = "";
            string CUENTA = "";
            string IMPORTE = "";
            string FECHA = "";
            string CONVENIO = "";
            string COMBPROBANTE = "";
            string CBU = "";
            string CUOTA = "";
            string USUARIO = "";
            

            FECHA = Convert.ToString(fechaDebito.Year) + Convert.ToString(fechaDebito.Month).PadLeft(2, '0') + Convert.ToString(fechaDebito.Day).PadLeft(2, '0');

            foreach (var emp in recuperoEmpresa)
            {
                TIPOCONVENIO = "003";

                //if (emp.CBU.Substring(0, 3) == "020")
                //{ emp.CUENTA_BANCOR = "S"; }
                //else { emp.CUENTA_BANCOR = "N"; }

                if (emp.CUENTA_BANCOR == "S")
                {
                    SUCURSAL = emp.CBU.Substring(3, 4).PadLeft(5, '0');
                    CUENTA = emp.CUENTA.Replace("/","").PadLeft(9, '0');
                }
                else
                {
                    SUCURSAL = "800".PadLeft(5, '0');
                    CUENTA = ("0").PadLeft(9, '0');//emp.CUENTA.Replace("/", "").PadLeft(9, '0');
                }

                MONEDA = "01";
                if (emp.TIPO_CUENTA == 146)
                { SISTEMA = "3"; }
                else { SISTEMA = "1"; }
                
                
                IMPORTE = Convert.ToString(emp.MONTO*100).Replace(",", "").Replace(".", "").PadLeft(18, '0');
                //FECHA = "20190901";
                CONVENIO = "03507"; //3507
                COMBPROBANTE = "000000";
                CBU = emp.CBU.PadLeft(22, '0');
                CUOTA = "00";
                USUARIO = emp.CUIT.Replace("-", "").PadRight(22, '0'); // "00000000000".PadLeft(22, '0');
                emp.N_REGISTRO = TIPOCONVENIO + SUCURSAL + MONEDA + SISTEMA + CUENTA + IMPORTE + FECHA + CONVENIO + COMBPROBANTE + CBU + CUOTA + USUARIO;
            
            }

            //return recuperoEmpresa;
        }

        public string GenerarArchivo(int idrecupero, string nomfile)
        {
            var emprecup = _empresaRepositorio.getRecuperosEmpresa(idrecupero);

            GenerarFile(emprecup, nomfile);

            return nomfile;
        }

        private void GenerarFile(IList<IRecuperoDebEmpresa> lista, string nomfile)
        {

            //var fechasolicitud = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));

            //string nomfile = string.Empty;
            //nomfile = "DEB" + "_" + Convert.ToString(DateTime.Now.Day) + Convert.ToString(DateTime.Now.Month) + Convert.ToString(DateTime.Now.Year) + Convert.ToString(DateTime.Now.Hour) + Convert.ToString(DateTime.Now.Minute) + Convert.ToString(DateTime.Now.Second) +
            //                                       ".deb";

            var file =
                HttpContext.Current.Server.MapPath("/Archivos/" + nomfile);

            var fiinfo = new FileInfo(file);

            if (fiinfo.Exists)
            {

            }
            else
            {
                StreamWriter sw1 = new StreamWriter(file);
                sw1.Close();

            }
            FileStream fs = new FileStream(file, FileMode.Truncate, FileAccess.ReadWrite);

            //using (fiinfo.CreateText())
            //{
            //}

            if (fs.CanWrite/*fiinfo.Exists*/)
            {

                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default))
                {

                    foreach (var item in lista)
                    {
                        


                        var text = item.N_REGISTRO;
                            sw.WriteLine(text);

                    }
                    sw.Close();
                    fs.Close();
                }
            }

            //return nomfile;

        }

        public IRecuperoDebitoVista SubirArchivoRecuperoDebito(HttpPostedFileBase archivo, IRecuperoDebitoVista model)
        {


            if (archivo != null && archivo.ContentLength > 0)
            {
                var fileName = Path.GetFileName(archivo.FileName);
                var nombre = fileName;
                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Archivos/Importados"), nombre);
                archivo.SaveAs(path);

                model.ProcesarArchivo = true;
                model.Mensaje = "El archivo \"" + fileName + "\" fue subido al servidor correctamente. Confirme la acción de Procesar si lo cree conveniente.";
                model.ArchivoNombre = nombre;
            }
            else
            {
                model.ProcesarArchivo = true;
                model.Mensaje = "El archivo no pudo ser subido al servidor. Vuelva a intentar.";
            }



            IList<IRecuperoDebito> lista = new List<IRecuperoDebito>();
            model.Registros = lista;

            return model;
        }

        public IRecuperoDebitoVista ProcesarArchivoDeb(IRecuperoDebitoVista model)
        {
            string path = HttpContext.Current.Server.MapPath(@"\Archivos\Importados") + @"\" + model.ArchivoNombre;
            IList<IRecuperoDebEmpresa> lrecupero = new List<IRecuperoDebEmpresa>();

            lrecupero = _empresaRepositorio.getRecuperosEmpresa(model.idrecuperodeb);
            int resultado = 0;
            int cantProcesados = 0;
            int cantError = 0;
            if (File.Exists(path))
            {
                using (var sr = new StreamReader(path))
                {

                    string registro = "";
                    string devolucion = "";

                    while (sr.Peek() >= 0)
                    {

                        var linea = sr.ReadLine();

                        if (linea != null)
                        {
                            registro = linea.Substring(81, 22);
                            devolucion = linea.Substring(103, 3);
                         var rec = lrecupero.FirstOrDefault(x => x.N_REGISTRO.Contains(registro));
                         rec.PROCESADO = "S";
                         rec.DEVOLUCION = devolucion;
                        }




                    }

                  resultado =  _empresaRepositorio.udpRecuperoDebito(lrecupero, model.idrecuperodeb);
                }
            }

            if (resultado != -1)
            {
                cantProcesados = lrecupero.Where(c => c.PROCESADO == "S").Count();
                cantError = lrecupero.Where(c => c.DEVOLUCION != "COB").Count();
                model.cantProcesados = cantProcesados;
                model.procesadosFail = cantError;
                model.Mensaje = "El archivo se procesó correctamente";
                //model.Registros = lrecupero;
            }
            else
            {
                model.Mensaje = "Se ha producido un error al procesar el archivo";
            }

            return model;
        }

    }
}
