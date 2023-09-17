using System.Text.Json.Serialization;

namespace WebTest
{
    public class Entities
    {
    }
    public class Person
    {
        public long id { get; set; } = 0;
        public string name { get; set; } = "";
        public string displayName { get; set; } = "";
        public ICollection<Skill> skills { get; set; } = new List<Skill>();
    }
    public class Skill
    {
        [JsonIgnore]
        public long id { get; set; }
        [JsonIgnore]
        public Person person { get; set; }
        [JsonIgnore]
        public long personRefKey { get; set; }
        public string name { get; set; } = "";
        private byte _level = 1;
        public byte level
        {
            get => _level;
            set
            {
                if (1 <= value && value <= 10)
                    _level = value;
                else throw new Exception("level must be 1-10");
            }
        }
    }
}
