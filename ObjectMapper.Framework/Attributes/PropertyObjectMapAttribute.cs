using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectMapper.Framework {
    public class PropertyObjectMapAttribute : Attribute {
        public PropertyObjectMapAttribute() {

        }

        public PropertyObjectMapAttribute(string propertyName) {
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }
    }
}
