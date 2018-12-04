using System;
using System.Collections.Generic;
using System.Text;

namespace _01._ComputerInventory.Models
{
   public partial class MachineWarranty
    {
        public int MachineWarrantyId { get; set; }
        public string ServiceTag { get; set; }
        public DateTime WarrantyExpiration { get; set; }
        public int MachineId { get; set; }
        public int WarrantyProviderId { get; set; }

        public WarrantyProvider WarrantyProvider { get; set; }
    }
}
