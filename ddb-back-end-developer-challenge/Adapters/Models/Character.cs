using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ddb_back_end_developer_challenge.Adapters.Rest.Models
{
    public class Character
    {
        [Required]
        public string Name { get; set; }

        public int Level { get; set; }
        [Required]
        public List<RpgClass> Classes { get; set; }

        [Required]
        public Stats Stats { get; set; }
        public List<Item> Items { get; set; }
        public List<CharacterDefense> Defenses { get; set; }
    }

    public class RpgClass
    {
        public string Name { get; set; }
        [Required]
        public int HitDiceValue { get; set; }
        [Required]
        public int ClassLevel { get; set; }
    }

    public class Stats
    {
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        [Required]
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public Modifier Modifier { get; set; }
    }

    public class Modifier
    {
        public string AffectedObject { get; set; }
        public string AffectedValue { get; set; }
        public int Value { get; set; }
    }

    public class CharacterDefense
    {
        public string Type { get; set; }
        public string Defense { get; set; }
    }
}
