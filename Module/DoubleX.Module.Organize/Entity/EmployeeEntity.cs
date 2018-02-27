using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using DoubleX.Infrastructure.Core.Entity;

namespace DoubleX.Module.Organize
{
    /// <summary>
    /// ְԱʵ��
    /// </summary>
    [Table("TB_Admin")]
    public partial class EmployeeEntity : EntityFrameworkEntity
    {
        /// <summary>
        /// ��¼�˺�
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Account { get; set; }

        /// <summary>
        /// ��¼����
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Password { get; set; }

        /// <summary>
        /// ��¼����
        /// </summary>
        [Required]
        public int LoginCount { get; set; }
    }
}
