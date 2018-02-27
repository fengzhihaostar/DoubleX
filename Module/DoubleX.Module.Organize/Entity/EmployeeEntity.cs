using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using DoubleX.Infrastructure.Core.Entity;

namespace DoubleX.Module.Organize
{
    /// <summary>
    /// 职员实体
    /// </summary>
    [Table("TB_Admin")]
    public partial class EmployeeEntity : EntityFrameworkEntity
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Account { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Password { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        [Required]
        public int LoginCount { get; set; }
    }
}
