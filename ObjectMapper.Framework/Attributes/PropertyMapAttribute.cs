using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectMapper.Framework {
    public class PropertyMapAttribute : Attribute {
        public PropertyMapAttribute() {

        }

        public PropertyMapAttribute(string propertyName) {
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }

        public Type Transformer { get; set; }
    }
}
