using System.Collections.Generic;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista.Shared
{
    public class ComboBox : IComboBox
    {
        public ComboBox()
        {
            Combo = new List<IComboItem>();
            Enabled = true;
            ReadOnly = false;
        }
        public IList<IComboItem> Combo { get; set; }
        public string Selected { get; set; }
        public bool Enabled { get; set; }
        public bool ReadOnly { get; set; }
    }

    public class ComboItem : IComboItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
