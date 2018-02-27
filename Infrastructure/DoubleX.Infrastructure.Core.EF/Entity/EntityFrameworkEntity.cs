using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace DoubleX.Infrastructure.Core.Entity
{
    /// <summary>
    /// Abstract Entity for all the BusinessEntities.
    /// </summary>
    [Serializable]
    //[DataContract]
    public abstract class EntityFrameworkEntity : IEntity<Guid>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
        //DatabaseGenerated(DatabaseGeneratedOption.Identity)连接现有MySql表公司Mysql有问题,所以改成在该处生成GUID
        //[Key]
        public virtual Guid Id
        {
            get
            {
                if (_id == Guid.Empty)
                    _id = Guid.NewGuid();
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        protected Guid _id = Guid.Empty;

        [DataMember]
        public virtual bool IsDelete { get; set; }

        [DataMember]
        public virtual Guid CreateId
        {
            get;
            set;
        }

        [DataMember]
        public virtual DateTime CreateDt { get; set; }

        [DataMember]
        public virtual Guid LastId { get; set; }

        [DataMember]
        public virtual DateTime LastDt { get; set; }
    }
}
