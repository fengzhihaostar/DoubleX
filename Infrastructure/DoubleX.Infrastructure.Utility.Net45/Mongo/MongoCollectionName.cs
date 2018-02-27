using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// Mongo集合名称属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class MongoCollectionName : Attribute
    {
        public MongoCollectionName(string value)
        {
#if NET35
            if (string.IsNullOrEmpty(value) || value.Trim().Length == 0)
#else
            if (string.IsNullOrWhiteSpace(value))
#endif
                throw new ArgumentException("Empty collectionname not allowed", "value");

            this.Name = value;
        }

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public virtual string Name { get; private set; }
    }
}
