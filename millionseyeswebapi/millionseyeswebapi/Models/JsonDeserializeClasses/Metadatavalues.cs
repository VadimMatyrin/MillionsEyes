using Newtonsoft.Json;

namespace MillionsEyesWebApi.Models.JsonDeserializeClasses
{
    public class Metadatavalues
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}