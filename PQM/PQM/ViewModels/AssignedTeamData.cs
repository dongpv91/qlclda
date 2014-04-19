using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PQM.ViewModels
{
    public class AssignedTeamData
    {
        public long TeamID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}