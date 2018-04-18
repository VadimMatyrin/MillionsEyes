using Newtonsoft.Json;

namespace MillionsEyesWebApi.Models.JsonDeserializeClasses
{
    public class Name
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("localizedValue")]
        public string LocalizedValue { get; set; }
    }
}