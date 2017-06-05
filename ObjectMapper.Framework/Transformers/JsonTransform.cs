using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectMapper.Framework.Transformers {
    public class JsonTransform<T> : IValueTransform {
        public object UnMap(object inValue) {
            return JsonConvert.DeserializeObject<T>(inValue.ToString());
        }

        public object Map(object inValue) {
            return JsonConvert.SerializeObject(inValue);
        }
    }
}
