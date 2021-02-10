namespace SGE.Model.Entidades.Interfaces
{
    public interface IConcepto:IComunDatos
    {
        int Id_Concepto { get; set; }
        short Año { get; set; }
        short Mes { get; set; }
        string Observacion { get; set; }

        //Estado del Concepto para la liquidacion de un beneficiario
        string Estado { get; set; }
        int? NroCuenta { get; set; }
        string Sucursal { get; set; }
        int? NroResolucion { get; set; }
        string SePagoaApoderado { get; set; }
        System.DateTime FechaLiquidacion { get; set; }

        int MontoPagado { get; set; }
    }
}
