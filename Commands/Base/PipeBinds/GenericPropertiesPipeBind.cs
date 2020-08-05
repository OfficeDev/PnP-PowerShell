using Newtonsoft.Json.Linq;
using System.Collections;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class PropertyBagPipeBind
    {
        private readonly Hashtable _hashtable;
        private string _jsonString;
        private JObject _jsonObject;

        public PropertyBagPipeBind(Hashtable hashtable)
        {
            _hashtable = hashtable;
            _jsonString = null;
            _jsonObject = null;
        }

        public PropertyBagPipeBind(string json)
        {
            _hashtable = null;
            _jsonString = json;
            _jsonObject = JObject.Parse(json);
        }

        public string Json => _jsonString;

        public JObject JsonObject => _jsonObject ?? HashtableToJsonObject(_hashtable);

        public Hashtable Properties => _hashtable;

        public override string ToString() => Json ?? HashtableToJsonString(_hashtable);

        private string HashtableToJsonString(Hashtable hashtable)
        {
            return HashtableToJsonObject(hashtable).ToString();
        }

        private JObject HashtableToJsonObject(Hashtable hashtable)
        {
            var obj = new JObject();

            foreach (var key in hashtable.Keys)
            {
                var rawValue = hashtable[key];

                // To ensure the value is not serialized as PSObject
                object value = rawValue is PSObject
                    ? ((PSObject) rawValue).BaseObject
                    : rawValue;

                obj[key] = JToken.FromObject(value);
            }
            return obj;
        }

    }
}