using SGE.Servicio.Comun;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;
using System.Web;
using SGE.Model.Entidades.Interfaces;
using System.Collections.Generic;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IEmpresaServicio : IBaseServicio
    {
        IEmpresasVista GetEmpresas();
        IEmpresasVista GetIndex();
        IEmpresasVista GetEmpresas(string descripcion, string cuit);
        IEmpresaVista GetEmpresa(int id, string accion);
        IEmpresasVista GetEmpresas(Pager pager, int id, string descripcion,string cuit);
        int AddEmpresa(IEmpresaVista emp);
        int UpdateEmpresa(IEmpresaVista emp);
        int DeleteEmpresa(IEmpresaVista emp);

        bool ExistsCuit(string cuit); //valida q no repita un cuil existente
        bool EmpresaEnUso(int idEmpresa); //para validar si puede eliminarla, si tiene fichas asociadas
        int AddUsuarioEmpresa(IEmpresaVista usuemp);
        int UpdateUsuarioEmpresa(IEmpresaVista usuemp);
        bool ExistsEmpresa(string cuit, string descripcion); //para saber si tiene que dar de alta la empresa antes de cargar una sede
        bool EsNuevaEmpresa(int? idEmpresa);
        
        IEmpresasVista CargarListaEmpresasDestino();
        IEmpresasVista CargarListaEmpresasDestino(string descripcion, string cuit, int idempresaorigen);
        IEmpresasVista CargarListaEmpresasDestino(Pager pager, int id, string descripcion, string cuit);

        bool DesvincularFichas(int idOrigen, int idDestino);

        IEmpresaVista BuscarFicha(IEmpresaVista vista);

        IEmpresaVista GetEmpresa(string cuit, string accion);

        void BlanquearCuentaUsr(IEmpresaVista model); // 06/03/2013 - DI CAMPLI LEANDRO - BLANQUEAR CONTRASEÑA DE EMPRESA

                // 16/01/2014 - DI CAMPLI LEANDRO - EXPORTAR LAS FICHAS DESDE EL FORMULARIO DE EMPRESA
        byte[] ExportFichasEmpresa(IEmpresaVista model);
        //02/06/2019 - RECUPERO CUPONES DEL BANCO
        IRecuperoVista indexRecupero();
        IRecuperoVista SubirArchivoRecupero(HttpPostedFileBase archivo, IRecuperoVista model);
        IRecuperoVista ProcesarArchivo(IRecuperoVista model);
        byte[] ExportRecupero(IRecuperoVista model);
        IRecuperoDebitoVista indexRecuperoDebito();
        IRecuperoDebitoVista addLiqEmpresaDebito(IRecuperoDebitoVista model);
        IRecuperoDebitoVista getRecuperosConcepto(IRecuperoDebitoVista model);
        string GenerarArchivo(int idrecupero, string nomfile);
        IRecuperoDebitoVista SubirArchivoRecuperoDebito(HttpPostedFileBase archivo, IRecuperoDebitoVista model);
        IRecuperoDebitoVista ProcesarArchivoDeb(IRecuperoDebitoVista model);
    }
}
