using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Classes
{
    public class JsonDeserializer
    {
        public static Planet ReadPlanetJson(string json)
        {
            var serializer = new JsonSerializer();
            Planet planet;
            using (var streamReader = new StreamReader(json))
            using (var textReader = new JsonTextReader(streamReader))
            {
                planet = serializer.Deserialize<Planet>(textReader);
            }

            return planet;
        }
    }
}