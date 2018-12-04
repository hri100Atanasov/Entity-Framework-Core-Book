using System.Collections.Generic;

namespace _01._ComputerInventory.Models
{
    public partial class OperatingSys
    {
        public OperatingSys()
        {
            Machine = new HashSet<Machine>();
        }

        public ICollection<Machine> Machine { get; set; }

        public int OperatingSysId { get; set; }

        public string Name { get; set; }

        public bool StillSupported { get; set; }
    }
}
