namespace SGE.Model.Entidades.Interfaces
{
    public interface ICarrera
    {
        int IdCarrera { get; set; }
        int? IdSector { get; set; }
        short? IdNivel { get; set; }
        string NombreCarrera { get; set; }
        int? IdInstitucion { get; set; }

        string NombreNivel { get; set; }
        string NombreInstitucion { get; set; }
        string NombreSector { get; set; }

        ISector SectorCarrera { get; set; }
        IInstitucion InstitucionCarrera { get; set; }
        INivel NivelCarrera { get; set; }

    }
}
