namespace SGE.Servicio.VistaInterfaces
{
    public interface IUsuarioPasswordVista
    {
        string Login { get; set; }
        string OldPassword { get; set; }
        string NewPassword { get; set; }
        string NewPasswordConfirm { get; set; }
    }
}
