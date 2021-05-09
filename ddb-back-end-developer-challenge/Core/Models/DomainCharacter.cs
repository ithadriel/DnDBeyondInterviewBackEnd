using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddb_back_end_developer_challenge.Core.Models
{
    public class DomainCharacter
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<string> Resistances { get; set; }
        public List<string> Immunities { get; set; }
        public List<string> Vulnerabilities { get; set; }
        public int MaxHitpoints { get; set; }
        public int CurrentHitpoints { get; set; }
        public int TemporaryHitpoints { get; set; } = 0;
    }
}
