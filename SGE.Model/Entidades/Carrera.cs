using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Carrera : ICarrera
    {
        public Carrera()
        {
            SectorCarrera = new Sector();
            InstitucionCarrera = new Institucion();
            NivelCarrera = new Nivel();
        }

        public int IdCarrera { get; set; }
        public int? IdSector { get; set; }
        public short? IdNivel { get; set; }
        public string NombreCarrera { get; set; }
        public int? IdInstitucion { get; set; }

        public string NombreNivel { get; set; }
        public string NombreInstitucion { get; set; }
        public string NombreSector { get; set; }
        public ISector SectorCarrera { get; set; }
        public IInstitucion InstitucionCarrera { get; set; }
        public INivel NivelCarrera { get; set; }
    }
}
