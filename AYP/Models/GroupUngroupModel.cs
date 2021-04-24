using System;
using System.Collections.Generic;
using System.Text;
using AYP.Entities;
using AYP.ViewModel;
using System.Collections.Generic;

namespace AYP.Models
{
    public class GroupUngroupModel
    {
        public string Name { get; set; }
        public Guid UniqueId { get; set; }
        public List<NodeViewModel> NodeList { get; set; }
        public List<ConnectViewModel> InternalConnectList { get; set; }
        public List<ConnectViewModel> ExternalConnectList { get; set; }
        public List<AgArayuzu> AgArayuzuList { get; set; }
        public List<GucArayuzu> GucArayuzuList { get; set; }

    }
}
