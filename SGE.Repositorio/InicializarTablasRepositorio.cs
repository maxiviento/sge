using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SGE.Model.Comun;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class InicializarTablasRepositorio : IInicializarTablasRepositorio
    {
        private readonly DataSGE _mdb;
        private readonly List<Registro> _tabla;

        public InicializarTablasRepositorio()
        {
            _mdb = new DataSGE();
            _tabla = new List<Registro>();
        }

        public int InsertarFormularios()
        {
            int total = 0;
            CargarFormularios();

            foreach (var fila in _tabla)
            {
                var row = new T_FORMULARIOS
                {
                    ID_FORMULARIO = Convert.ToInt16(fila.Clave),
                    N_FORMULARIO = fila.Valor
                };

                if (_mdb.T_FORMULARIOS.Where(c => c.ID_FORMULARIO == row.ID_FORMULARIO).ToList().Count() == 0)
                {
                    total++;
                    _mdb.T_FORMULARIOS.AddObject(row);
                }
            }

            _mdb.SaveChanges();
            return total;
        }

        public int InsertarAcciones()
        {
            int total = 0;
            CargarAcciones();

            foreach (var fila in _tabla)
            {
                var row = new T_ACCIONES
                {
                    ID_ACCION = Convert.ToInt16(fila.Clave),
                    N_ACCION = fila.Valor
                };

                if (_mdb.T_ACCIONES.Where(c => c.ID_ACCION == row.ID_ACCION).ToList().Count() == 0)
                {
                    total++;
                    _mdb.T_ACCIONES.AddObject(row);
                }
            }

            _mdb.SaveChanges();
            return total;
        }

        public int InsertarFormularioAccion()
        {
            int total = 0;
            CargarFormularioAccion();

            foreach (var fila in _tabla)
            {
                var idFormulario = Convert.ToInt16(fila.Clave);
                var formulario = _mdb.T_FORMULARIOS.FirstOrDefault(c => c.ID_FORMULARIO == idFormulario);

                var idAccion = Convert.ToInt16(fila.Valor);
                var accion = _mdb.T_ACCIONES.FirstOrDefault(c => c.ID_ACCION == idAccion);

                if (formulario != null)
                {
                    if (accion == null)
                    {
                        accion = new T_ACCIONES
                        {
                            ID_ACCION = idAccion
                        };
                    }

                    if (formulario.T_ACCIONES.Where(c => c.ID_ACCION == accion.ID_ACCION).ToList().Count() == 0)
                    {
                        total++;
                        formulario.T_ACCIONES.Add(accion);
                        _mdb.ObjectStateManager.ChangeObjectState(accion, EntityState.Unchanged);
                    }
                }
            }

            _mdb.SaveChanges();
            return total;
        }

        public int InsertarEstados()
        {
            int total = 0;
            CargarEstados();

            foreach (var fila in _tabla)
            {
                var row = new T_ESTADOS
                {
                    ID_ESTADO = Convert.ToInt16(fila.Clave),
                    N_ESTADO = fila.Valor
                };

                if (_mdb.T_ESTADOS.Where(c => c.ID_ESTADO == row.ID_ESTADO).ToList().Count() == 0)
                {
                    total++;
                    _mdb.T_ESTADOS.AddObject(row);
                }
            }

            _mdb.SaveChanges();
            return total;
        }

        public int InsertarTipoFicha()
        {
            int total = 0;
            CargarTipoFicha();

            foreach (var fila in _tabla)
            {
                var row = new T_TIPO_FICHA
                {
                    ID_TIPO_FICHA = Convert.ToInt16(fila.Clave),
                    N_TIPO_FICHA = fila.Valor
                };

                if (_mdb.T_TIPO_FICHA.Where(c => c.ID_TIPO_FICHA == row.ID_TIPO_FICHA).ToList().Count() == 0)
                {
                    total++;
                    _mdb.T_TIPO_FICHA.AddObject(row);
                }
            }

            _mdb.SaveChanges();
            return total;
        }

        public int InsertarProgramas()
        {
            int total = 0;
            CargarProgramas();

            foreach (var fila in _tabla)
            {
                var row = new T_PROGRAMAS
                {
                    ID_PROGRAMA = Convert.ToInt16(fila.Clave),
                    N_PROGRAMA = fila.Valor
                };

                if (_mdb.T_PROGRAMAS.Where(c => c.ID_PROGRAMA == row.ID_PROGRAMA).ToList().Count() == 0)
                {
                    total++;
                    _mdb.T_PROGRAMAS.AddObject(row);
                }
            }

            _mdb.SaveChanges();
            return total;
        }

        public int InsertarEstadosInscriptos()
        {
            int total = 0;
            CargarEstadosInscriptos();

            foreach (var fila in _tabla)
            {
                var row = new T_ESTADOS_FICHA
                {
                    ID_ESTADO_FICHA = Convert.ToInt16(fila.Clave),
                    N_ESTADO_FICHA = fila.Valor
                };

                if (_mdb.T_ESTADOS_FICHA.Where(c => c.ID_ESTADO_FICHA == row.ID_ESTADO_FICHA).ToList().Count() == 0)
                {
                    total++;
                    _mdb.T_ESTADOS_FICHA.AddObject(row);
                }
            }

            _mdb.SaveChanges();
            return total;
        }

        public int InsertarEstadoBeneficiarioLiquidacion()
        {
            int total = 0;
            CargarEstadosBeneficiarioLiquidacion();

            foreach (var fila in _tabla)
            {
                var row = new T_ESTADOS_BEN_CON_LIQ
                {
                    ID_EST_BEN_CON_LIQ = Convert.ToInt16(fila.Clave),
                    N_EST_BEN_CON_LIQ = fila.Valor
                };

                if (_mdb.T_ESTADOS_BEN_CON_LIQ.Where(c => c.ID_EST_BEN_CON_LIQ == row.ID_EST_BEN_CON_LIQ).ToList().Count() == 0)
                {
                    total++;
                    _mdb.T_ESTADOS_BEN_CON_LIQ.AddObject(row);
                }
            }

            _mdb.SaveChanges();
            return total;
        }

        private void CargarFormularios()
        {
            _tabla.Clear();
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmUsuario).ToString(), "ABM USUARIO"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmBeneficiario).ToString(), "ABM BENEFICIARIO"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmFicha).ToString(), "ABM INSCRIPCIONES"));
            _tabla.Add(new Registro(((int)Enums.Formulario.Liquidaciones).ToString(), "LIQUIDACIONES"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmCausaRechazo).ToString(), "ABM CAUSA RECHAZO"));
            _tabla.Add(new Registro(((int)Enums.Formulario.CambiarComntraseña).ToString(), "CAMBIAR COMNTRASEÑA"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AsignarPermisos).ToString(), "ASIGNAR PERMISOS"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmConceptos).ToString(), "ABM CONCEPTOS"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmRoles).ToString(), "ABM ROLES"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmTipoRechazo).ToString(), "ABM TIPO RECHAZO"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmProgramas).ToString(), "ABM PROGRAMAS"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmInstituciones).ToString(), "ABM INSTITUCIONES"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmDepartamentos).ToString(), "ABM DEPARTAMENTOS"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmEmpresas).ToString(), "ABM EMPRESAS"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmUniversidades).ToString(), "ABM UNIVERSIDADES"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmLocalidades).ToString(), "ABM LOCALIDADES"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmSedes).ToString(), "ABM SEDES"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmCarreras).ToString(), "ABM CARRERAS"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmSucursalesCobertura).ToString(), "ABM SUCURSALES COBERTURA"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmFichaObservacion).ToString(), "ABM FICHA OBSERVACIONES"));
            _tabla.Add(new Registro(((int)Enums.Formulario.InicializarTablas).ToString(), "INICIALIZAR TABLAS"));
            _tabla.Add(new Registro(((int)Enums.Formulario.ControlDatos).ToString(), "CONTROL DE DATOS"));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmSubprogramas).ToString(), "ABM SUBPROGRAMAS"));

            /* SECTOR DE REPORTES*/
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesMomentoSolicitarRep).ToString(), "REP CANT POSTULANTES MOMENTO SOLICITAR REP"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantEmpresasMomentoSolicitarRep).ToString(), "REP CANT EMPRESAS MOMENTO SOLICITAR REP"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPendientesMomentoSolicitarRep).ToString(), "REP CANT POSTULANTES PENDIENTES MOMENTO SOLICITAR REP"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPorFechaCarga).ToString(), "REP CANT POSTULANTES POR FECHA CARGA"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesDistribuidosInteriorPciaYCapital).ToString(), "REP CANT POSTULANTES DISTRIBUIDOS INTERIOR PCIA Y CAPITAL"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPorDepartamento).ToString(), "REP CANT POSTULANTES POR DEPARTAMENTO"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPorDepartamentoYLocalidad).ToString(), "REP CANT POSTULANTES POR DEPARTAMENTO Y LOCALIDAD"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPorBarrioDeCapital).ToString(), "REP CANT POSTULANTES POR BARRIO DE CAPITAL"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesMenoresEdad).ToString(), "REP CANT POSTULANTES MENORES EDAD"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesConDiscapacidad).ToString(), "REP CANT POSTULANTES CON DISCAPACIDAD"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPorUniversidadInstTerciaria).ToString(), "REP CANT POSTULANTES POR UNIVERSIDAD INST TERCIARIA"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPorUniversidadInstTerciariaYCarrera).ToString(), "REP CANT POSTULANTES POR UNIVERSIDAD INST TERCIARIA Y CARRERA"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesQueOptaronCarreraDistinta).ToString(), "REP CANT POSTULANTES QUE OPTARON CARRERA DISTINTA"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPorCadaUnaRamasEstrategicas).ToString(), "REP CANT POSTULANTES POR CADA UNA RAMAS ESTRATEGICAS"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPorEmpresa).ToString(), "REP CANT POSTULANTES POR EMPRESA"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantBeneficiariosPorDepartamentoLocalidadPrograma).ToString(), "REP CANT BENEFICIARIOS POR DEPARTAMENTO LOCALIDAD PROGRAMA"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepNominaBeneficiariosParaArt).ToString(), "REP NOMINA BENEFICIARIOS PARA ART"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepNominaBeneficiariosAño2012).ToString(), "REP NOMINA BENEFICIARIOS AÑO 2012"));
            _tabla.Add(new Registro(((int)Enums.Formulario.RepAnexoIResolucion).ToString(), "REP ANEXO I DE RESOLUCIÓN PPP"));
            _tabla.Add(new Registro(((int)Enums.Formulario.RepDepLocProEst).ToString(), "REP DEP. LOC. PROG. EST."));
            _tabla.Add(new Registro(((int)Enums.Formulario.RepArtHorariosEntPpp).ToString(), "REP ART CON HORARIOS DE ENTRENAMIENTO - PPP"));
            _tabla.Add(new Registro(((int)Enums.Formulario.RepArtHorariosEntReconversion).ToString(), "REP ART CON HORARIOS DE ENTRENAMIENTO - RECONVERSION PRODUCTIVA"));
            _tabla.Add(new Registro(((int)Enums.Formulario.RepArtHorariosEntEfectores).ToString(), "REP ART CON HORARIOS DE ENTRENAMIENTO - EFECTORES SOCIALES"));
            _tabla.Add(new Registro(((int)Enums.Formulario.RepArtHorariosEntVat).ToString(), "REP ART CON HORARIOS DE ENTRENAMIENTO - VAT"));
            _tabla.Add(new Registro(((int)Enums.Formulario.RepArtHorariosEntPppp).ToString(), "REP ART CON HORARIOS DE ENTRENAMIENTO - PPP PROFESIONAL"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepBecasAcademicas).ToString(), "REP ART BECAS ACADÉMICAS"));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepEfectoresYReconversion).ToString(), "REP EFECTORES SOCIALES Y RECONVERSIÓN"));
        }

        private void CargarAcciones()
        {
            _tabla.Clear();
            _tabla.Add(new Registro(((int)Enums.Acciones.Agregar).ToString(), "Agregar"));
            _tabla.Add(new Registro(((int)Enums.Acciones.Modificar).ToString(), "Modificar"));
            _tabla.Add(new Registro(((int)Enums.Acciones.Eliminar).ToString(), "Eliminar"));
            _tabla.Add(new Registro(((int)Enums.Acciones.Consultar).ToString(), "Consultar"));
            _tabla.Add(new Registro(((int)Enums.Acciones.CambiarEstado).ToString(), "Cambiar Estado"));
            _tabla.Add(new Registro(((int)Enums.Acciones.Exportar).ToString(), "Exportar"));
            _tabla.Add(new Registro(((int)Enums.Acciones.Importar).ToString(), "Importar"));
            _tabla.Add(new Registro(((int)Enums.Acciones.GenerarArchivo).ToString(), "Generar Archivo"));
            _tabla.Add(new Registro(((int)Enums.Acciones.VerArchivo).ToString(), "Consultar Archivos"));
            _tabla.Add(new Registro(((int)Enums.Acciones.AgregarApoderado).ToString(), "Agregar Apoderado"));
            _tabla.Add(new Registro(((int)Enums.Acciones.ModificarApoderado).ToString(), "Modificar Apoderado"));
            _tabla.Add(new Registro(((int)Enums.Acciones.EliminarApoderado).ToString(), "Eliminar Apoderado"));
            _tabla.Add(new Registro(((int)Enums.Acciones.ConsultarApoderado).ToString(), "Consultar Apoderado"));
            _tabla.Add(new Registro(((int)Enums.Acciones.VerEfectoresSociales).ToString(), "Ver Efectores Sociales"));
        }

        private void CargarFormularioAccion()
        {
            _tabla.Clear();
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmUsuario).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmUsuario).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmUsuario).ToString(), ((int)Enums.Acciones.Eliminar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmUsuario).ToString(), ((int)Enums.Acciones.Consultar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AbmBeneficiario).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmBeneficiario).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmBeneficiario).ToString(), ((int)Enums.Acciones.Exportar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmBeneficiario).ToString(), ((int)Enums.Acciones.Importar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmBeneficiario).ToString(), ((int)Enums.Acciones.GenerarArchivo).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmBeneficiario).ToString(), ((int)Enums.Acciones.VerArchivo).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmBeneficiario).ToString(), ((int)Enums.Acciones.AgregarApoderado).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmBeneficiario).ToString(), ((int)Enums.Acciones.ModificarApoderado).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmBeneficiario).ToString(), ((int)Enums.Acciones.EliminarApoderado).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmBeneficiario).ToString(), ((int)Enums.Acciones.ConsultarApoderado).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmBeneficiario).ToString(), ((int)Enums.Acciones.VerEfectoresSociales).ToString()));



            _tabla.Add(new Registro(((int)Enums.Formulario.AbmFicha).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmFicha).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmFicha).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmFicha).ToString(), ((int)Enums.Acciones.Exportar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmFicha).ToString(), ((int)Enums.Acciones.CambiarEstado).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmFicha).ToString(), ((int)Enums.Acciones.VerEfectoresSociales).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.Liquidaciones).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.Liquidaciones).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.Liquidaciones).ToString(), ((int)Enums.Acciones.Eliminar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.Liquidaciones).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.Liquidaciones).ToString(), ((int)Enums.Acciones.VerEfectoresSociales).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AbmCausaRechazo).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmCausaRechazo).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmCausaRechazo).ToString(), ((int)Enums.Acciones.Eliminar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmCausaRechazo).ToString(), ((int)Enums.Acciones.Consultar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.CambiarComntraseña).ToString(), ((int)Enums.Acciones.Modificar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AsignarPermisos).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AsignarPermisos).ToString(), ((int)Enums.Acciones.Consultar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AbmConceptos).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmConceptos).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmConceptos).ToString(), ((int)Enums.Acciones.Consultar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AbmRoles).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmRoles).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmRoles).ToString(), ((int)Enums.Acciones.Eliminar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmRoles).ToString(), ((int)Enums.Acciones.Consultar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AbmTipoRechazo).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmTipoRechazo).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmTipoRechazo).ToString(), ((int)Enums.Acciones.Eliminar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmTipoRechazo).ToString(), ((int)Enums.Acciones.Consultar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AbmProgramas).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmProgramas).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmProgramas).ToString(), ((int)Enums.Acciones.Eliminar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmProgramas).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmProgramas).ToString(), ((int)Enums.Acciones.VerEfectoresSociales).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AbmInstituciones).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmInstituciones).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmInstituciones).ToString(), ((int)Enums.Acciones.Eliminar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmInstituciones).ToString(), ((int)Enums.Acciones.Consultar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AbmDepartamentos).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmDepartamentos).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmDepartamentos).ToString(), ((int)Enums.Acciones.Eliminar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmDepartamentos).ToString(), ((int)Enums.Acciones.Consultar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AbmEmpresas).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmEmpresas).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmEmpresas).ToString(), ((int)Enums.Acciones.Eliminar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmEmpresas).ToString(), ((int)Enums.Acciones.Consultar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AbmUniversidades).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmUniversidades).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmUniversidades).ToString(), ((int)Enums.Acciones.Eliminar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmUniversidades).ToString(), ((int)Enums.Acciones.Consultar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AbmLocalidades).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmLocalidades).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmLocalidades).ToString(), ((int)Enums.Acciones.Eliminar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmLocalidades).ToString(), ((int)Enums.Acciones.Consultar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AbmSedes).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmSedes).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmSedes).ToString(), ((int)Enums.Acciones.Eliminar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmSedes).ToString(), ((int)Enums.Acciones.Consultar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AbmCarreras).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmCarreras).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmCarreras).ToString(), ((int)Enums.Acciones.Eliminar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmCarreras).ToString(), ((int)Enums.Acciones.Consultar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AbmSucursalesCobertura).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmSucursalesCobertura).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmSucursalesCobertura).ToString(), ((int)Enums.Acciones.Eliminar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmSucursalesCobertura).ToString(), ((int)Enums.Acciones.Consultar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AbmFichaObservacion).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmFichaObservacion).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmFichaObservacion).ToString(), ((int)Enums.Acciones.Eliminar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmFichaObservacion).ToString(), ((int)Enums.Acciones.Consultar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.AbmSubprogramas).ToString(), ((int)Enums.Acciones.Agregar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmSubprogramas).ToString(), ((int)Enums.Acciones.Modificar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmSubprogramas).ToString(), ((int)Enums.Acciones.Eliminar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.AbmSubprogramas).ToString(), ((int)Enums.Acciones.Consultar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.InicializarTablas).ToString(), ((int)Enums.Acciones.Agregar).ToString()));

            _tabla.Add(new Registro(((int)Enums.Formulario.ControlDatos).ToString(), ((int)Enums.Acciones.Agregar).ToString()));

            /* SECTOR DE REPORTES */
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesMomentoSolicitarRep).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantEmpresasMomentoSolicitarRep).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPendientesMomentoSolicitarRep).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPorFechaCarga).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesDistribuidosInteriorPciaYCapital).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPorDepartamento).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPorDepartamentoYLocalidad).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPorBarrioDeCapital).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesMenoresEdad).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesConDiscapacidad).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPorUniversidadInstTerciaria).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPorUniversidadInstTerciariaYCarrera).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesQueOptaronCarreraDistinta).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPorCadaUnaRamasEstrategicas).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantPostulantesPorEmpresa).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepCantBeneficiariosPorDepartamentoLocalidadPrograma).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepNominaBeneficiariosParaArt).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepNominaBeneficiariosAño2012).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.RepAnexoIResolucion).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.RepDepLocProEst).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.RepArtHorariosEntPpp).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.RepArtHorariosEntReconversion).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.RepArtHorariosEntEfectores).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.RepArtHorariosEntVat).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            _tabla.Add(new Registro(((int)Enums.Formulario.RepArtHorariosEntPppp).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepBecasAcademicas).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
            //_tabla.Add(new Registro(((int)Enums.Formulario.RepEfectoresYReconversion).ToString(), ((int)Enums.Acciones.Consultar).ToString()));
        }

        private void CargarEstados()
        {
            _tabla.Clear();
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiario.Postulante).ToString(), "POSTULANTE"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiario.Activo).ToString(), "ACTIVO"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiario.BajaPorRenuncia).ToString(), "BAJA POR RENUNCIA"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiario.BajaDeOficio).ToString(), "BAJA DE OFICIO"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiario.BajaPedidoPorEmpresa).ToString(), "BAJA PEDIDO POR EMPRESA"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiario.BeneficiarioRetenido).ToString(), "BENEFICIARIO RETENIDO"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiario.Pendiente).ToString(), "PENDIENTE"));
        }

        private void CargarEstadosBeneficiarioLiquidacion()
        {
            _tabla.Clear();
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.EnviadoAlBanco).ToString(), "ENVIADO AL BANCO"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.Excluido).ToString(), "EXCLUIDO"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.ImputadoEnCuenta).ToString(), "IMPUTADO EN CUENTA"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.NoImputado).ToString(), "NO IMPUTADO"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto).ToString(), "EXCLUIDO NO APTO"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CuentaInexistente).ToString(), "CUENTA INEXISTENTE"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CuentaBloqueada).ToString(), "CUENTA BLOQUEADA"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CuentaCerrada).ToString(), "CUENTA CERRADA"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CatcuentaActualizandose).ToString(), "CATCUENTA ACTUALIZANDOSE"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.Ca0CuentaDeAcreditacionEnCero).ToString(), "CA0CUENTA DE ACREDITACION EN CERO"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.Ca1CuentaDeAcreditacionInexistente).ToString(), "CA1CUENTA DE ACREDITACION INEXISTENTE"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.Ca2CuentaDeAcreditacionConError).ToString(), "CA2CUENTA DE ACREDITACION CON ERROR"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CbdcuentaConBloqueoDeDebitos).ToString(), "CBDCUENTA CON BLOQUEO DE DÉBITOS"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CbtcuentaConBloqueoTotal).ToString(), "CBTCUENTA CON BLOQUEO TOTAL"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CcccuentaCerradaPorRechazoDeCheque).ToString(), "CCCCUENTA CERRADA POR RECHAZO DE CHEQUE"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CcjcuentaConHijas).ToString(), "CCJCUENTA CON HIJAS"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CcmcuentaConMovimientos).ToString(), "CCMCUENTA CON MOVIMIENTOS"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CcscuentaConSaldo).ToString(), "CCSCUENTA CON SALDO"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CecctaExisteRelCli).ToString(), "CECCTA.EXISTE REL. CLI."));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CficuentaConFondoInsuficiente).ToString(), "CFICUENTA CON FONDO INSUFICIENTE"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CiacuentaInactiva).ToString(), "CIACUENTA INACTIVA"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CihcuentaInhabilitada).ToString(), "CIHCUENTA INHABILITADA"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CilcuentaLibradoraInexistente).ToString(), "CILCUENTA LIBRADORA INEXISTENTE"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CizcuentaInmovilizada).ToString(), "CIZCUENTA INMOVILIZADA"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.Ci1CuentaIncompleta).ToString(), "CI1CUENTA INCOMPLETA"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CmecuentaConMovEnActualEjercicio).ToString(), "CMECUENTA CON MOV. EN ACTUAL EJERCICIO"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CnecuentaNoPuedeRealizarEntrega).ToString(), "CNECUENTA NO PUEDE REALIZAR ENTREGA"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CnocuentaNoOperativa).ToString(), "CNOCUENTA NO OPERATIVA"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CsccuentaConSuspensionPagoDeCheque).ToString(), "CSCCUENTA CON SUSPENSIÓN PAGO DE CHEQUE"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CsecuentaSuperiorInexistente).ToString(), "CSECUENTA SUPERIOR INEXISTENTE"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.Ct0CuentaEnCero).ToString(), "CT0CUENTA EN CERO"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.Ct1NroCuentaInvalido).ToString(), "CT1NRO. CUENTA INVÁLIDO"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CuecuentaEmbargada).ToString(), "CUECUENTA EMBARGADA"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.CxpcuentaNoEsDePesos).ToString(), "CXPCUENTA NO ES DE PESOS"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.IntcuentaSinIntereses).ToString(), "INTCUENTA SIN INTERESES"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.ManmovimientoAnulado).ToString(), "MANMOVIMIENTO ANULADO"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.McpcuentaPrincipalNoEsEnPesos).ToString(), "MCPCUENTA PRINCIPAL NO ES EN PESOS"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.MdcmonedaDistintaDeLaCuenta).ToString(), "MDCMONEDA DISTINTA DE LA CUENTA"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.MdemovimientoDevuelto).ToString(), "MDEMOVIMIENTO DEVUELTO"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.MeimovConEstadoIncorrectoPOperacion).ToString(), "MEIMOV.CON ESTADO INCORRECTO P/OPERACION"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.MivmonedaInvalida).ToString(), "MIVMONEDA INVALIDA"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.MoimonedaInexistente).ToString(), "MOIMONEDA INEXISTENTE"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.NainumeroDeAcuerdoInvalido).ToString(), "NAINÚMERO DE ACUERDO INVÁLIDO"));
            _tabla.Add(new Registro(((int)Enums.EstadoBeneficiarioLiquidacion.SaisucursalDeAcreditacionInvalida).ToString(), "SAISUCURSAL DE ACREDITACION INVALIDA"));
        }

        private void CargarEstadosInscriptos()
        {
            _tabla.Clear();
            _tabla.Add(new Registro(((int)Enums.EstadoFicha.Apta).ToString(), "APTA"));
            _tabla.Add(new Registro(((int)Enums.EstadoFicha.NoApta).ToString(), "NO APTA"));
            _tabla.Add(new Registro(((int)Enums.EstadoFicha.Beneficiario).ToString(), "BENEFICIARIO"));
            _tabla.Add(new Registro(((int)Enums.EstadoFicha.RechazoFormal).ToString(), "RECHAZO FORMAL"));
            _tabla.Add(new Registro(((int)Enums.EstadoFicha.FueraCupoEmpresa).ToString(), "FUERA DE CUPO DE EMPRESA"));
            _tabla.Add(new Registro(((int)Enums.EstadoFicha.FueraCupoPrograma).ToString(), "FUERA DE CUPO DE PROGRAMA"));
            _tabla.Add(new Registro(((int)Enums.EstadoFicha.Duplicado).ToString(), "DUPLICADO"));
        }

        private void CargarTipoFicha()
        {
            _tabla.Clear();
            _tabla.Add(new Registro(((int)Enums.TipoFicha.Terciaria).ToString(), "TERCIARIA"));
            _tabla.Add(new Registro(((int)Enums.TipoFicha.Universitaria).ToString(), "UNIVERSITARIA"));
            _tabla.Add(new Registro(((int)Enums.TipoFicha.Ppp).ToString(), "PPP"));
            _tabla.Add(new Registro(((int)Enums.TipoFicha.PppProf).ToString(), "PPP PROFESIONAL"));
            _tabla.Add(new Registro(((int)Enums.TipoFicha.Vat).ToString(), "VAT"));
            _tabla.Add(new Registro(((int)Enums.TipoFicha.ReconversionProductiva).ToString(), "RECONVERSIÓN PRODUCTIVA"));
            _tabla.Add(new Registro(((int)Enums.TipoFicha.EfectoresSociales).ToString(), "EFECTORES SOCIALES"));
        }

        private void CargarProgramas()
        {
            _tabla.Clear();
            _tabla.Add(new Registro(((int)Enums.Programas.Terciaria).ToString(), "TERCIARIA"));
            _tabla.Add(new Registro(((int)Enums.Programas.Universitaria).ToString(), "UNIVERSITARIA"));
            _tabla.Add(new Registro(((int)Enums.Programas.Ppp).ToString(), "PPP"));
            _tabla.Add(new Registro(((int)Enums.Programas.PppProf).ToString(), "PPP PROFESIONAL"));
            _tabla.Add(new Registro(((int)Enums.Programas.Vat).ToString(), "VAT"));
            _tabla.Add(new Registro(((int)Enums.Programas.ReconversionProductiva).ToString(), "RECONVERSIÓN PRODUCTIVA"));
            _tabla.Add(new Registro(((int)Enums.Programas.EfectoresSociales).ToString(), "EFECTORES SOCIALES"));
        }
    }

    public class Registro
    {
        public Registro(string clave, string valor)
        {
            Clave = clave;
            Valor = valor;
        }

        public string Clave { get; set; }
        public string Valor { get; set; }
    }
}
