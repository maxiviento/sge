using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
    public class EscuelaServicio : IEscuelaServicio
    {
        private readonly IActorRepositorio _ActoresRepositorio;
        private readonly IAutenticacionServicio _autenticacion;
        private readonly IEscuelaRepositorio _escuelaRepositorio;
        private readonly ILocalidadRepositorio _localidadRepositorio;
        private readonly IDepartamentoRepositorio _departamentoRepositorio;

        public EscuelaServicio()
        {
            _autenticacion = new AutenticacionServicio();
            _escuelaRepositorio = new EscuelaRepositorio();
            _localidadRepositorio = new LocalidadRepositorio();
            _departamentoRepositorio = new DepartamentoRepositorio();
        }

        public IEscuelasVista GetEscuelas(string NombreEscuela)
        {
            int cantreg = 0;
            IEscuelasVista vista = new EscuelasVista();

            vista.escuelas = _escuelaRepositorio.GetEscuelas(NombreEscuela); ;

            cantreg = vista.escuelas.Count();
            var pager = new Pager(cantreg,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexEscuela", _autenticacion.GetUrl("IndexPager", "Escuelas"));

            vista.Pager = pager;

            vista.escuelas = vista.escuelas.Skip(vista.Pager.Skip).Take(vista.Pager.PageSize).ToList();

            return vista;

        }


        public IEscuelasVista GetIndex()
        {
            IEscuelasVista vista = new EscuelasVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexEscuela", _autenticacion.GetUrl("IndexPager", "Escuelas"));

            vista.Pager = pager;


            return vista;
        }

        public IEscuelasVista GetEscuelas(IPager pPager, string NombreEscuela)
        {

            IEscuelasVista vista = new EscuelasVista();
            //IList<IEscuela> Escuelas;
            int cantreg = 0;

            vista.escuelas = _escuelaRepositorio.GetEscuelas(NombreEscuela);
            cantreg = vista.escuelas.Count();
            var pager = new Pager(cantreg,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexEscuela", _autenticacion.GetUrl("IndexPager", "Escuelas"));

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



            vista.escuelas = vista.escuelas.Skip(vista.Pager.Skip).Take(vista.Pager.PageSize).ToList();



            return vista;
        }

        public IEscuelaVista GetEscuela(int idEscuela, string accion)
        {

            IEscuelaVista vista = new EscuelaVista();
            IEscuela c;
            vista.Accion = accion;
            CargarDepartamento(vista, _departamentoRepositorio.GetDepartamentos());

            if (accion != "Agregar")
            {
                c = _escuelaRepositorio.GetEscuelaCompleto(idEscuela);

                    IEscuela escuela;
                    vista.Id_Escuela = c.Id_Escuela;
                    escuela = _escuelaRepositorio.GetEscuelaCompleto((int)vista.Id_Escuela);
                    vista.Nombre_Escuela = escuela.Nombre_Escuela;
                    vista.Cue = escuela.Cue;
                    vista.Anexo = escuela.Anexo;
                    vista.Barrio = escuela.Barrio;
                    vista.Id_Barrio = escuela.Id_Barrio;
                    vista.CALLE = escuela.CALLE;
                    vista.NUMERO = escuela.NUMERO;
                    vista.CUPO_CONFIAMOS = escuela.CUPO_CONFIAMOS;
                    vista.TELEFONO = escuela.TELEFONO;
                    vista.ID_COORDINADOR = escuela.ID_COORDINADOR;
                    vista.ID_DIRECTOR = escuela.ID_DIRECTOR;
                    vista.ID_ENCARGADO = escuela.ID_ENCARGADO;
                    vista.COOR_APELLIDO = escuela.COOR_APELLIDO;
                    vista.COOR_NOMBRE = escuela.COOR_NOMBRE;
                    vista.COOR_DNI = escuela.COOR_DNI;
                    vista.COOR_CUIL = escuela.COOR_CUIL;
                    vista.COOR_CELULAR = escuela.COOR_CELULAR;
                    vista.COOR_TELEFONO = escuela.COOR_TELEFONO;
                    vista.COOR_MAIL = escuela.COOR_MAIL;
                    vista.COOR_ID_LOCALIDAD = escuela.COOR_ID_LOCALIDAD;
                    vista.COOR_N_LOCALIDAD = escuela.COOR_N_LOCALIDAD;
                    vista.DIR_APELLIDO = escuela.DIR_APELLIDO;
                    vista.DIR_NOMBRE = escuela.DIR_NOMBRE;
                    vista.DIR_DNI = escuela.DIR_DNI;
                    vista.DIR_CUIL = escuela.DIR_CUIL;
                    vista.DIR_CELULAR = escuela.DIR_CELULAR;
                    vista.DIR_TELEFONO = escuela.DIR_TELEFONO;
                    vista.DIR_MAIL = escuela.DIR_MAIL;
                    vista.DIR_ID_LOCALIDAD = escuela.DIR_ID_LOCALIDAD;
                    vista.DIR_N_LOCALIDAD = escuela.DIR_N_LOCALIDAD;
                    vista.ENC_APELLIDO = escuela.ENC_APELLIDO;
                    vista.ENC_NOMBRE = escuela.ENC_NOMBRE;
                    vista.ENC_DNI = escuela.ENC_DNI;
                    vista.ENC_CUIL = escuela.ENC_CUIL;
                    vista.ENC_CELULAR = escuela.ENC_CELULAR;
                    vista.ENC_TELEFONO = escuela.ENC_TELEFONO;
                    vista.ENC_MAIL = escuela.ENC_MAIL;
                    vista.ENC_ID_LOCALIDAD = escuela.ENC_ID_LOCALIDAD;
                    vista.ENC_N_LOCALIDAD = escuela.ENC_N_LOCALIDAD;


                    int idlocalidad = Convert.ToInt32(escuela.Id_Localidad ?? 0);
                    var d = _localidadRepositorio.GetLocalidad(idlocalidad);
                    string idDep = d.IdDepartamento == null ? "0" : d.IdDepartamento.ToString();
                    vista.departamentos.Selected = idDep;
                    CargarLocalidad(vista, _localidadRepositorio.GetLocalidadesByDepto(idDep == null ? 0 : Convert.ToInt32(idDep)));
                    vista.Localidades.Selected = escuela.Id_Localidad.ToString();
                    CargarBarrio(vista, (from b in _localidadRepositorio.GetBarrios().Where(x => x.ID_LOCALIDAD == idlocalidad).DefaultIfEmpty()
                                         select b).ToList());
                    vista.barrios.Selected = escuela.Id_Barrio.ToString();

            }
            else
            {
                c = new Escuela();

                vista.Nombre_Escuela = c.Nombre_Escuela;
                vista.Id_Localidad = 0;
                vista.Id_Barrio = 0;
                vista.Id_Escuela = 0;
                vista.ID_COORDINADOR = 0;
                vista.ID_ENCARGADO = 0;
                vista.ID_COORDINADOR = 0;

                CargarLocalidad(vista, _localidadRepositorio.GetLocalidades());
                CargarBarrio(vista, _localidadRepositorio.GetBarrios());

            }



            if (accion == "Ver" || accion == "Eliminar")
            {
                vista.departamentos.Enabled = false;
                vista.Localidades.Enabled = false;
                vista.barrios.Enabled = false;
            }

            return vista;
        }

        public int AddEscuela(IEscuelaVista vista)
        {


            vista.Id_Escuela = _escuelaRepositorio.AddEscuela(VistaToDatos(vista));

            return 0;
        }

        private static IEscuela VistaToDatos(IEscuelaVista vista)
        {
            IEscuela escuela = new Escuela
            {
                Id_Escuela = vista.Id_Escuela,
                Nombre_Escuela = vista.Nombre_Escuela,
                Cue = vista.Cue,
                Anexo = vista.Anexo == null ? 0 : (int)vista.Anexo,
                Barrio = vista.Barrio,
                Id_Barrio = vista.barrios.Selected == "0" ? 0 : Convert.ToInt32(vista.barrios.Selected),
                Id_Localidad = vista.Localidades.Selected == "0" ? 0 : Convert.ToInt32(vista.Localidades.Selected),
                CALLE = vista.CALLE,
                NUMERO = vista.NUMERO,
                ID_DIRECTOR = vista.ID_DIRECTOR == 0 ? null : vista.ID_DIRECTOR,
                ID_COORDINADOR = vista.ID_COORDINADOR == 0 ? null : vista.ID_COORDINADOR,
                ID_ENCARGADO = vista.ID_ENCARGADO == 0 ? null : vista.ID_ENCARGADO,
                TELEFONO = vista.TELEFONO,
                CUPO_CONFIAMOS = vista.CUPO_CONFIAMOS,
                regise = vista.regise,

            };

            return escuela;
        }

        public int UpdateEscuela(IEscuelaVista vista)
        {

            _escuelaRepositorio.UpdateEscuela(VistaToDatos(vista));

            return 0;

        }

        public IEscuelaVista GetEscuelaReload(IEscuelaVista vista)
        {
            IEscuela c;
            string loc = vista.Localidades.Selected;
            string dd = vista.departamentos.Selected;
            string bb = vista.barrios.Selected;

            if (vista.Accion != "Agregar")
            {
                //c = _escuelaRepositorio.GetEscuelaCompleto(vista.Id_Escuela);

                //IEscuela escuela;
                //vista.Id_Escuela = c.Id_Escuela;
                //escuela = _escuelaRepositorio.GetEscuelasCompleto().Where(x => x.Id_Escuela == (int)vista.Id_Escuela).SingleOrDefault();
                //vista.Nombre_Escuela = escuela.Nombre_Escuela;
                //vista.Cue = escuela.Cue;
                //vista.Anexo = escuela.Anexo;
                //vista.Barrio = escuela.Barrio;
                //vista.CALLE = escuela.CALLE;
                //vista.NUMERO = escuela.NUMERO;
                //vista.CUPO_CONFIAMOS = escuela.CUPO_CONFIAMOS;
                //vista.TELEFONO = escuela.TELEFONO;
                //vista.ID_COORDINADOR = escuela.ID_COORDINADOR;
                //vista.ID_DIRECTOR = escuela.ID_DIRECTOR;
                //vista.ID_ENCARGADO = escuela.ID_ENCARGADO;
                //vista.COOR_APELLIDO = escuela.COOR_APELLIDO;
                //vista.COOR_NOMBRE = escuela.COOR_NOMBRE;
                //vista.COOR_DNI = escuela.COOR_DNI;
                //vista.COOR_CUIL = escuela.COOR_CUIL;
                //vista.COOR_CELULAR = escuela.COOR_CELULAR;
                //vista.COOR_TELEFONO = escuela.COOR_TELEFONO;
                //vista.COOR_MAIL = escuela.COOR_MAIL;
                //vista.COOR_ID_LOCALIDAD = escuela.COOR_ID_LOCALIDAD;
                //vista.COOR_N_LOCALIDAD = escuela.COOR_N_LOCALIDAD;
                //vista.DIR_APELLIDO = escuela.DIR_APELLIDO;
                //vista.DIR_NOMBRE = escuela.DIR_NOMBRE;
                //vista.DIR_DNI = escuela.DIR_DNI;
                //vista.DIR_CUIL = escuela.DIR_CUIL;
                //vista.DIR_CELULAR = escuela.DIR_CELULAR;
                //vista.DIR_TELEFONO = escuela.DIR_TELEFONO;
                //vista.DIR_MAIL = escuela.DIR_MAIL;
                //vista.ENC_APELLIDO = escuela.ENC_APELLIDO;
                //vista.ENC_NOMBRE = escuela.ENC_NOMBRE;
                //vista.ENC_DNI = escuela.ENC_DNI;
                //vista.ENC_CUIL = escuela.ENC_CUIL;
                //vista.ENC_CELULAR = escuela.ENC_CELULAR;
                //vista.ENC_TELEFONO = escuela.ENC_TELEFONO;
                //vista.ENC_MAIL = escuela.ENC_MAIL;
                //vista.ENC_ID_LOCALIDAD = escuela.ENC_ID_LOCALIDAD;
                //vista.ENC_N_LOCALIDAD = escuela.ENC_N_LOCALIDAD;

                CargarDepartamento(vista, _departamentoRepositorio.GetDepartamentos());
                int idlocalidad = loc == null ? 0 : Convert.ToInt32(loc);
                var d = _localidadRepositorio.GetLocalidad(idlocalidad);
                string idDep = d.IdDepartamento == null ? "0" : d.IdDepartamento.ToString();
                vista.departamentos.Selected = idDep;
                CargarLocalidad(vista, _localidadRepositorio.GetLocalidadesByDepto(idDep == null ? 0 : Convert.ToInt32(idDep)));
                vista.Localidades.Selected = loc;
                CargarBarrio(vista, (from b in _localidadRepositorio.GetBarrios().Where(x => x.ID_LOCALIDAD == idlocalidad).DefaultIfEmpty()
                                     select b).ToList());
                vista.barrios.Selected = bb;
            }
            else
            {
                CargarDepartamento(vista, _departamentoRepositorio.GetDepartamentos());


                if (vista.departamentos.Selected != "0" && vista.departamentos.Selected != null)
                {
                    CargarLocalidad(vista, _localidadRepositorio.GetLocalidadesByDepto(Convert.ToInt32(vista.departamentos.Selected)));
                    vista.Localidades.Selected = loc;
                }
                else
                { CargarLocalidad(vista, _localidadRepositorio.GetLocalidades()); }

                if (vista.Localidades.Selected != "0" && vista.Localidades.Selected != null)
                {
                    CargarBarrio(vista, (from b in _localidadRepositorio.GetBarrios().Where(x => x.ID_LOCALIDAD == Convert.ToInt32(loc)).DefaultIfEmpty()
                                         select b).ToList());
                }
                else
                {
                    CargarBarrio(vista, _localidadRepositorio.GetBarrios());
                    vista.barrios.Selected = bb;
                }


            }



            if (vista.Accion == "Ver" || vista.Accion == "Eliminar")
            {
                vista.departamentos.Enabled = false;
                vista.Localidades.Enabled = false;
                vista.barrios.Enabled = false;
            }



            return vista;
        }

        private static void CargarDepartamento(IEscuelaVista vista, IEnumerable<IDepartamento> listadepartamentos)
        {
            foreach (var listadepartamento in listadepartamentos)
            {
                vista.departamentos.Combo.Add(new ComboItem { Id = listadepartamento.IdDepartamento, Description = listadepartamento.NombreDepartamento });

            }
        }

        private static void CargarLocalidad(IEscuelaVista vista, IList<ILocalidad> listalocalidades)
        {
            vista.Localidades.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione Localidad..." });
            foreach (var listalocalidad in listalocalidades)
            {
                vista.Localidades.Combo.Add(new ComboItem { Id = listalocalidad.IdLocalidad, Description = listalocalidad.NombreLocalidad });

            }
        }

        private static void CargarBarrio(IEscuelaVista vista, IList<IBarrio> listaBarrios)
        {
            vista.barrios.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione Barrio..." });
            if (listaBarrios[0] != null)
            {
                foreach (var barrio in listaBarrios)
                {
                    vista.barrios.Combo.Add(new ComboItem { Id = barrio.ID_BARRIO, Description = barrio.N_BARRIO });

                }
            }
        }

        public List<ILocalidad> cboCargarLocalidad(int idDep)
        {

            return _localidadRepositorio.GetLocalidadesByDepto(idDep).ToList();
        }

        public List<IBarrio> cboCargarBarrio(int idLocalidad)
        {

            return (from b in _localidadRepositorio.GetBarrios().Where(x => x.ID_LOCALIDAD == idLocalidad).DefaultIfEmpty()
                    select b).ToList();
        }

        public List<IEscuela> GetEscuelasLst(string NombreEscuela)
        {

            List<IEscuela> vista = new List<IEscuela>();

            vista = _escuelaRepositorio.GetEscuelas(NombreEscuela).ToList();

           

            return vista;

        }

    }
}
