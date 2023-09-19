using System.Text.Json.Serialization;

namespace WebTest.Models
{
    public class Skill
    {
        [JsonIgnore]
        public long Id { get; set; }
        [JsonIgnore]
        public Person Person { get; set; }
        [JsonIgnore]
        public long PersonRefKey { get; set; }
        public string Name { get; set; } = "";
        private byte _level = 1;
        public byte Level
        {
            get => _level;
            set
            {
                if (1 <= value && value <= 10) //не понял что здесь не так
                    _level = value;
                else throw new Exception("Level must be 1-10");
            }
        }
    }
}
