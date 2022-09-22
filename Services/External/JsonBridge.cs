using Newtonsoft.Json;

namespace Services.External
{
    public class JsonBridge<TIn,TOut>
    {
        public string Serialize(TIn item)
        {
            return JsonConvert.SerializeObject(item);
        }

        public string SerializeList(ICollection<TIn> items)
        {
            return JsonConvert.SerializeObject(items);
        }

        public ICollection<TOut>? DeserializeList(string json)
        {
            return JsonConvert.DeserializeObject<ICollection<TOut>>(json);
        }

        public TOut? Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<TOut>(json);
        }
    }
}
