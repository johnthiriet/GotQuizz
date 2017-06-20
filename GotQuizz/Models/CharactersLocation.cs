using System.Collections.Generic;

namespace GotQuizz.Models
{
    public class CharactersLocation
    {
        public string _id { get; set; }
        public string slug { get; set; }
        public string name { get; set; }
        public List<string> locations { get; set; }
    }
}
