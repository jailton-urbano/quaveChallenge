using System;
using System.Text.Json.Serialization;

namespace QuaveChallenge.API.Models
{
    public class Community
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public ICollection<Person>? People { get; set; }
    }
} 