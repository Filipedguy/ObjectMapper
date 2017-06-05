using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectMapper.Framework {
    public interface IMapper {
        IObjectMapping<T> CreateMapper<T>() where T : class, new();
    }
}
