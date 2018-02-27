﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Repository;

namespace DoubleX.Module.Member
{
    /// <summary>
    /// 会员数据持久操作
    /// </summary>
    public interface IMemberRepository : IRepository<MemberEntity, Guid>
    {
        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="memberId">会员Id</param>
        MemberDetailModel GetDetail(Guid memberId);
    }
}
