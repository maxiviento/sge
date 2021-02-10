using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class HorarioEmpresaRepositorio : BaseRepositorio, IHorarioEmpresaRepositorio
    {
        private readonly DataSGE _mdb;

        public HorarioEmpresaRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IHorarioEmpresa> QHoarioEmpresa()
        {
            var a =
                _mdb.T_EMP_HORARIOS.Select(
                    c =>
                    new HorarioEmpresa
                        {
                            FechaSistema = c.FEC_SIST,
                            HoraDesde = c.HORAS_DESDE,
                            HoraHasta = c.HORAS_HASTA,
                            IdDia = c.ID_DIA,
                            IdEmpresa = c.ID_EMPRESA,
                            IdEmpresaHorario = c.ID_EMP_HORARIO,
                            IdUsuarioSistema = c.ID_USR_SIST,
                            MinutosDesde = c.MINUTOS_DESDE,
                            MinutosHasta = c.MINUTOS_HASTA,
                            UsuarioSistema = c.T_USUARIOS.LOGIN
                        });

            return a;
        }

        public IList<IHorarioEmpresa> GetHorariosEmpresa(int? idempresa)
        {
            return QHoarioEmpresa().Where(c => c.IdEmpresa == idempresa).ToList();
        }

        public IList<IHorarioEmpresa> GetHorariosEmpresa(int? idempresa, string iddia)
        {
            return QHoarioEmpresa().Where(c => c.IdEmpresa == idempresa && c.IdDia == iddia).ToList();
        }

        public bool AddHoarioEmpresa(IHorarioEmpresa horarioempresa)
        {

            try
            {
                AgregarDatos(horarioempresa);

                var emprhorario = new T_EMP_HORARIOS
                {
                    HORAS_DESDE = horarioempresa.HoraDesde,
                    HORAS_HASTA = horarioempresa.HoraHasta,
                    ID_DIA = horarioempresa.IdDia,
                    ID_EMPRESA = horarioempresa.IdEmpresa,
                    ID_EMP_HORARIO = SecuenciaRepositorio.GetId(),
                    ID_USR_SIST = horarioempresa.IdUsuarioSistema,
                    MINUTOS_DESDE = horarioempresa.MinutosDesde,
                    MINUTOS_HASTA = horarioempresa.MinutosHasta
                };


                _mdb.T_EMP_HORARIOS.AddObject(emprhorario);

                _mdb.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IList<IHorarioEmpresa> GetHorarioEmpresaForFicha(int? idempresaHorario)
        {
            return QHoarioEmpresa().Where(c => c.IdEmpresaHorario == idempresaHorario
                ).ToList();
        }

        public IList<IHorarioEmpresa> GetHorarioEmpresaForFicha(int? idempresa, string iddia)
        {
            return QHoarioEmpresa().Where(c => c.IdEmpresa == idempresa
                && c.IdDia== iddia
                ).ToList();
        }
    }
}
