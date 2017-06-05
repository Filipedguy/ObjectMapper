using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectMapper.Framework {
    public class MappingException : Exception {
        public Type SourceType { get; set; }
        public Type DestinationType { get; set; }

        public MappingException() : base() {

        }

        public MappingException(string message) : base(message) {

        }

        public MappingException(string message, Exception innerException) : base(message, innerException) {

        }

        public MappingException(string message, Type sourceType, Type destinationType) : this(message) {
            SourceType = sourceType;
            DestinationType = destinationType;
        }

        public MappingException(string message, Exception innerException, Type sourceType, Type destinationType) : this(message, innerException) {
            SourceType = sourceType;
            DestinationType = destinationType;
        }
    }
}
