using System.Collections.Generic;

namespace SGE.Servicio.VistaInterfaces.Shared
{
    public interface IComboBox
    {
        IList<IComboItem> Combo { get; set; }
        string Selected { get; set; }
        bool Enabled { get; set; }
        bool ReadOnly { get; set; }
    }

    public interface IComboItem
    {
        int Id { get; set; }
        string Description { get; set; }
    }
}
