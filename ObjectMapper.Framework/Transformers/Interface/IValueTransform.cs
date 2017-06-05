using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectMapper.Framework.Transformers {
    public interface IValueTransform {
        object Map(object inValue);

        object UnMap(object outValue);
    }
}
