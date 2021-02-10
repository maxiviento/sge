using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class EstadoBeneficiario : IEstadoBeneficiario
    {
        public int Id_Estado_Beneficiario { get; set; }
        public string Estado_Beneficiario { get; set; }
    }
}
