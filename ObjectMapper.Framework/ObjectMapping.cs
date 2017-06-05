using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using ObjectMapper.Framework.Transformers;

namespace ObjectMapper.Framework {
    internal class ObjectMapping<T> : IObjectMapping<T> where T : class, new() {
        private Dictionary<PropertyInfo, object> _customMapping;

        public ObjectMapping() {
            _customMapping = new Dictionary<PropertyInfo, object>();
        }

        public IObjectMapping<T> CustomMap(Expression<Func<T, object>> lambda, object propertyValue) {
            var body = lambda.Body;

            if (body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;

            var property = (body as MemberExpression).Member as PropertyInfo;

            _customMapping.Add(property, propertyValue);

            return this;
        }

        public T Map(object source) {
            try {
                return InternalMap(source);
            }
            catch (MappingException ex) {
                ex.SourceType = source.GetType();
                ex.DestinationType = typeof(T);
                throw ex;
            }
            catch (Exception ex) {
                throw new MappingException("Unknown error. Se innerexception for more details", ex, source.GetType(), typeof(T));
            }
        }

        public List<T> MapMany(object[] sourceList) {
            var ret = new List<T>();

            foreach (var source in sourceList) {
                ret.Add(Map(source));
            }

            return ret;
        }

        public T UnMap(object source) {
            try {
                return InternalUnMap(source);
            }
            catch (MappingException ex) {
                ex.SourceType = source.GetType();
                ex.DestinationType = typeof(T);
                throw ex;
            }
            catch (Exception ex) {
                throw new MappingException("Unknown error. Se innerexception for more details", ex, source.GetType(), typeof(T));
            }
        }

        public List<T> UnMapMany(object[] sourceList) {
            var ret = new List<T>();

            foreach (var source in sourceList) {
                ret.Add(UnMap(source));
            }

            return ret;
        }

        #region [ Private Methods ]

        #region [ Mapping ]

        private T InternalMap(object source) {
            var destination = new T();

            if (source != null) {
                foreach (var prop in source.GetProperties()) {
                    try {
                        MapProperty(prop, source, destination);
                    }
                    catch (Exception ex) {
                        throw new MappingException($"Failed to map the source property: {prop.Name}", ex);
                    }
                }
            }

            foreach (var customProp in _customMapping) {
                try {
                    MapCustomProperty(customProp.Key, customProp.Value, destination);
                }
                catch (Exception ex) {
                    throw new MappingException($"Failed to map the custom property: {customProp.Key}", ex);
                }
            }

            return destination;
        }

        private void MapProperty(PropertyInfo prop, object source, object destination) {
            // Get the mapping of the property
            var mapperAttribute = prop.GetCustomAttribute<PropertyMapAttribute>();
            var mapperObjectAttribute = prop.GetCustomAttribute<PropertyObjectMapAttribute>();

            // If exists a map, execute it
            if (mapperAttribute != null) {
                // Name of the destination property
                var destPropName = string.Empty;

                // If the PropertyName in the Annotation was not especified, use the source property name.
                if (string.IsNullOrWhiteSpace(mapperAttribute.PropertyName)) {
                    destPropName = prop.Name;
                }
                // Else, use the name in the annotations
                else {
                    destPropName = mapperAttribute.PropertyName;
                }

                // Get the destination Property based on the name
                var destinationProp = destination.GetProperty(destPropName);

                // Get the value in the source
                var value = prop.GetValue(source);

                // If the property has a value transformer, transform the value
                if (mapperAttribute.Transformer != null) {
                    var transformer = (IValueTransform)Activator.CreateInstance(mapperAttribute.Transformer);

                    value = transformer.Map(value);
                }

                // Set the value in the destination
                destinationProp.SetValue(destination, value);
            }
            // If the map is a object mapping, execute the object map
            else if (mapperObjectAttribute != null) {
                MapObjectProperty(prop, source, destination);
            }
        }

        private void MapCustomProperty(PropertyInfo prop, object value, object destination) {
            // Set the value in the destination
            prop.SetValue(destination, value);
        }

        private void MapObjectProperty(PropertyInfo prop, object source, object destination) {
            // Get the mapping of the property
            var mapperAttribute = prop.GetCustomAttribute<PropertyObjectMapAttribute>();

            // If exists a map, execute it
            if (mapperAttribute != null) {
                // If the source object maps direct to the destination object, do the mapping
                if (string.IsNullOrWhiteSpace(mapperAttribute.PropertyName)) {
                    foreach (var internalProp in prop.PropertyType.GetProperties()) {
                        MapProperty(internalProp, prop.GetValue(source), destination);
                    }
                }
                // If the source object maps to another object in the destination, create the object and then do the mapping
                else {
                    // Get the property info of the destination object
                    var destinationObjectProperty = destination.GetProperty(mapperAttribute.PropertyName);

                    // Get the object of the destination property
                    var destinationObject = destinationObjectProperty.GetValue(destination);

                    // If the object is null, create it
                    if (destinationObject == null) {
                        destinationObject = Activator.CreateInstance(destinationObjectProperty.PropertyType);
                    }

                    // Do the mapping
                    foreach (var internalProp in prop.PropertyType.GetProperties()) {
                        MapProperty(internalProp, prop.GetValue(source), destinationObject);
                    }

                    // Associate the created object to the destination
                    destinationObjectProperty.SetValue(destination, destinationObject);
                }
            }
        }

        #endregion

        #region [ UnMapping ]

        private T InternalUnMap(object source) {
            var destination = new T();

            foreach (var prop in destination.GetProperties()) {
                try {
                    UnMapProperty(prop, source, destination);
                }
                catch (Exception ex) {
                    throw new MappingException($"Failed to map the source property: {prop.Name}", ex);
                }
            }

            foreach (var customProp in _customMapping) {
                try {
                    MapCustomProperty(customProp.Key, customProp.Value, destination);
                }
                catch (Exception ex) {
                    throw new MappingException($"Failed to map the custom property: {customProp.Key}", ex);
                }
            }

            return destination;
        }

        private void UnMapProperty(PropertyInfo prop, object source, object destination) {
            // Get the mapping of the property
            var mapperAttribute = prop.GetCustomAttribute<PropertyMapAttribute>();
            var mapperObjectAttribute = prop.GetCustomAttribute<PropertyObjectMapAttribute>();

            // If exists a map, execute it
            if (mapperAttribute != null) {
                // Name of the source property
                var sourcePropName = string.Empty;

                // If the PropertyName in the Annotation was not especified, use the destination property name.
                if (string.IsNullOrWhiteSpace(mapperAttribute.PropertyName)) {
                    sourcePropName = prop.Name;
                }
                // Else, use the name in the annotations
                else {
                    sourcePropName = mapperAttribute.PropertyName;
                }

                // Get the source Property based on the name
                var sourceProp = source.GetProperty(sourcePropName);

                // Get the value in the source
                var value = sourceProp.GetValue(source);

                // If the property has a value transformer, transform the value
                if (mapperAttribute.Transformer != null) {
                    var transformer = (IValueTransform)Activator.CreateInstance(mapperAttribute.Transformer);

                    value = transformer.UnMap(value);
                }

                // Set the value in the destination
                prop.SetValue(destination, value);
            }
            // If the map is a object mapping, execute the object map
            else if (mapperObjectAttribute != null) {
                UnMapObjectProperty(prop, source, destination);
            }
        }

        private void UnMapObjectProperty(PropertyInfo prop, object source, object destination) {
            // Get the mapping of the property
            var mapperAttribute = prop.GetCustomAttribute<PropertyObjectMapAttribute>();

            // If exists a map, execute it
            if (mapperAttribute != null) {
                // If the destination object maps direct to the source object, do the mapping
                if (string.IsNullOrWhiteSpace(mapperAttribute.PropertyName)) {
                    // Get the object of the destination property
                    var destinationObject = prop.GetValue(destination);

                    // If the object is null, create it
                    if (destinationObject == null) {
                        destinationObject = Activator.CreateInstance(prop.PropertyType);
                    }

                    foreach (var internalProp in prop.PropertyType.GetProperties()) {
                        UnMapProperty(internalProp, source, destinationObject);
                    }

                    prop.SetValue(destination, destinationObject);
                }
                // If the destination object maps to another object in the source, create the object and then do the mapping
                else {
                    // Get the source object property
                    var sourceObjectProperty = source.GetProperty(mapperAttribute.PropertyName);

                    // Get the object of the destination property
                    var destinationObject = prop.GetValue(destination);

                    // If the object is null, create it
                    if (destinationObject == null) {
                        destinationObject = Activator.CreateInstance(prop.PropertyType);
                    }

                    // Do the mapping
                    foreach (var internalProp in prop.PropertyType.GetProperties()) {
                        UnMapProperty(internalProp, sourceObjectProperty.GetValue(source), destinationObject);
                    }

                    // Associate the created object to the destination
                    prop.SetValue(destination, destinationObject);
                }
            }
        }

        #endregion

        #endregion
    }
}
