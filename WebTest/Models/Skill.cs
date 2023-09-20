using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebTest.Models
{
    public class Skill
    {
        [JsonIgnore]
        public long Id { get; set; }
        [JsonIgnore]
        public long PersonRefKey { get; set; }
        public string Name { get; set; } = "";
        [Range(1, 10)]
        public byte Level { get; set; } = 1;
    }
}
