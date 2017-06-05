using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectMapper.Framework {
    internal static class ReflectionExtensions {
        public static PropertyInfo GetProperty(this object obj, string propName) {
            var type = obj.GetType();
            return type.GetRuntimeProperty(propName);
        }

        public static IEnumerable<PropertyInfo> GetProperties(this object obj) {
            var type = obj.GetType();
            return type.GetRuntimeProperties();
        }
    }
}
