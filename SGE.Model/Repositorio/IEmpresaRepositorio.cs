using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using System;
using SGE.Model.Entidades;

namespace SGE.Model.Repositorio
{
    public interface IEmpresaRepositorio
    {
        IList<IEmpresa> GetEmpresas();
        IList<IEmpresa> GetEmpresas(int skip, int make);
        IList<IEmpresa> GetEmpresas(string descripcion, string cuit);
        IList<IEmpresa> GetEmpresas(string descripcion, string cuit, int skip, int take);
        IEmpresa GetEmpresa(int id);
        int AddEmpresa(IEmpresa empresa);
        void UpdateEmpresa(IEmpresa empresa);
        void DeleteEmpresa(IEmpresa empresa);
        bool ExistsCuit(string cuit);
        int GetEmpresasCount();
        bool EmpresaEnUso(int idEmpresa);
        int AddUsuariosEmpresa(IEmpresa empresa);
        void UpdateUsuarioEmpresa(IEmpresa empresa);
        bool ExistsEmpresa(string cuit, string descripcion);
        bool EsNuevaEmpresa(int? idEmpresa);
        int GetFichasCount(int idEmpresa);
        bool UpdateFichaPpp(int origen, int destino);

        IEmpresa GetEmpresa(string cuit);

        //22012013 se añade para obtener la cantidad de beneficiarios activos por programa y empresa
        int GetBenefActivosEmpCount(int idEmpresa, int intPrograma);
        //22102019 se añade para obtener la cantidad de beneficiarios retenidos por programa y empresa
        int GetBenefRetenidosEmpCount(int idEmpresa, int intPrograma);
        // 06/03/2013 - DI CAMPLI LEANDRO - BLANQUECAR CUENTA DE USUARIO DE EMPRESA
        void BlanquearCuentaUsr(IEmpresa emp);
        int GetBenefActDiscEmpCount(int idEmpresa, int intPrograma);
        IList<IRecupero> udpCuponRecupero(IList<IRecupero> lrecupero);
        IList<IRecupero> getRecupero(string nombreArchivo);
        IList<IRecuperoDebito> getLiqEmpresaDebito(int idconcepto);
        IList<IRecuperoDebito> getRecuperosDebito(int idrecupero, int idconcepto);
        int AddRecupero(IList<RecuperoDebEmpresa> recupero, int idConcepto, DateTime fechaproceso, string nombreArchivo);
        IList<IRecuperoDebEmpresa> getRecuperosEmpresa(int idrecupero);
        IList<IAporteDebito> getAporteDebito();
        int udpRecuperoDebito(IList<IRecuperoDebEmpresa> lrecupero, int idRecupero);
        int AddRecuperoBatch(IList<RecuperoDebEmpresa> recupero, int idConcepto, DateTime fechaproceso, string nombreArchivo);
    }


}
