using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Core.Entity
{
    /// <summary>
    /// 数据实体接口
    /// </summary>
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }

    /// <summary>
    /// 数据实体接口
    /// </summary>
    public interface IEntity : IEntity<string>
    {

    }
}
