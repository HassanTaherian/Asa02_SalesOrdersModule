using Newtonsoft.Json;

namespace WebClientTest
{
    public class JsonBridge<T>
    {
        public string Serialize(T item)
        {
            return JsonConvert.SerializeObject(item);
        }

        public string SerializeList(ICollection<T> items)
        {
            return JsonConvert.SerializeObject(items);
        }

        public ICollection<T>? DeserializeList(string json)
        {
            return JsonConvert.DeserializeObject<ICollection<T>>(json);
        }

        public T? Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
