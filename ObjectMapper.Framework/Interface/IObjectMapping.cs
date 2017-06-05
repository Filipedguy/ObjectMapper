using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ObjectMapper.Framework {
    public interface IObjectMapping<T> where T : class, new() {
        /// <summary>
        /// Map an annotated object to a new one
        /// </summary>
        /// <typeparam name="T">New object type</typeparam>
        /// <param name="source">Annotated object</param>
        /// <returns>A new object from the informed type</returns>
        T Map(object source) ;

        /// <summary>
        /// Map many annotated objects to a new list
        /// </summary>
        /// <typeparam name="T">New object type</typeparam>
        /// <param name="sourceList">Annotated array</param>
        /// <returns>A list of new objects from the informed type</returns>
        List<T> MapMany(object[] sourceList);

        /// <summary>
        /// UnMap an object to a new annotated type
        /// </summary>
        /// <typeparam name="T">The new annotated type</typeparam>
        /// <param name="source">The object to be mapped</param>
        /// <returns>A new annotated object</returns>
        T UnMap(object source);

        /// <summary>
        /// UnMap many objects to a new annotated list
        /// </summary>
        /// <typeparam name="T">The new annotated type</typeparam>
        /// <param name="sourceList">The array of objects to be mapped</param>
        /// <returns>A list of new annotated objects</returns>
        List<T> UnMapMany(object[] sourceList);

        /// <summary>
        /// Create a custom property mapping
        /// </summary>
        /// <param name="propertyName">Name of the destination property</param>
        /// <param name="propertyValue">Custom value</param>
        /// <returns>A new mapping object with the custom property</returns>
        IObjectMapping<T> CustomMap(Expression<Func<T, object>> lambda, object propertyValue);
    }
}
