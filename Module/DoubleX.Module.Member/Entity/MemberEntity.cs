using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Entity;
using DoubleX.Module.Common;

namespace DoubleX.Module.Member
{
    /// <summary>
    /// 会员实体
    /// </summary>
    [Table(TableNameKey.Member)]
    public partial class MemberEntity : EntityFrameworkEntity
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Account { get; set; }

        /// <summary>
        /// 用户别名（可用作公司名称）
        /// </summary>
        [Required]
        [StringLength(200)]
        public string NameTag { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [Required]
        [StringLength(200)]
        public virtual string Password { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [StringLength(200)]
        public string RealName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(200)]
        public string Email { get; set; }

        /// <summary>
        ///邮箱是否认证
        /// </summary>
        public bool EmailIsVerify { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [StringLength(200)]
        public string Mobile { get; set; }

        /// <summary>
        /// 手机是否认证
        /// </summary>
        public bool MobileIsVerify { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        [StringLength(200)]
        public string Credits { get; set; }

        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 所属国家
        /// </summary>
        [StringLength(200)]
        public string Country { get; set; }

        /// <summary>
        /// 所属区域
        /// </summary>
        [StringLength(200)]
        public string Area { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [StringLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        [StringLength(200)]
        public string LastLoginIP { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginDt { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 账号状态
        /// </summary>
        public int State { get; set; }
    }
}