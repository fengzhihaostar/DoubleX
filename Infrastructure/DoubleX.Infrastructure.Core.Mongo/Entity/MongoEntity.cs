using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DoubleX.Infrastructure.Core.Entity
{
    /// <summary>
    /// Abstract Entity for all the BusinessEntities.
    /// </summary>
    [DataContract]
    [Serializable]
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class MongoEntity : IEntity<string>
    {
        /// <summary>
        /// Gets or sets the id for this object (the primary record for an entity).
        /// </summary>
        /// <value>The id for this object (the primary record for an entity).</value>
        [BsonId]
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }

        [DataMember]
        [BsonDefaultValue(false)]
        public virtual bool IsDelete { get; set; }

        [DataMember]
        public virtual string CreateId
        {
            get;
            set;
        }

        [DataMember]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public virtual DateTime CreateDt { get; set; }

        [DataMember]
        public virtual string LastId { get; set; }

        [DataMember]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public virtual DateTime LastDt { get; set; }
    }
}
