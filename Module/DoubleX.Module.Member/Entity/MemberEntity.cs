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
    /// ��Աʵ��
    /// </summary>
    [Table(TableNameKey.Member)]
    public partial class MemberEntity : EntityFrameworkEntity
    {
        /// <summary>
        /// ��¼�˺�
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Account { get; set; }

        /// <summary>
        /// �û���������������˾���ƣ�
        /// </summary>
        [Required]
        [StringLength(200)]
        public string NameTag { get; set; }

        /// <summary>
        /// ��¼����
        /// </summary>
        [Required]
        [StringLength(200)]
        public virtual string Password { get; set; }

        /// <summary>
        /// ��ʵ����
        /// </summary>
        [StringLength(200)]
        public string RealName { get; set; }

        /// <summary>
        /// �Ա�
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [StringLength(200)]
        public string Email { get; set; }

        /// <summary>
        ///�����Ƿ���֤
        /// </summary>
        public bool EmailIsVerify { get; set; }

        /// <summary>
        /// �ֻ�
        /// </summary>
        [StringLength(200)]
        public string Mobile { get; set; }

        /// <summary>
        /// �ֻ��Ƿ���֤
        /// </summary>
        public bool MobileIsVerify { get; set; }

        /// <summary>
        /// ֤������
        /// </summary>
        [StringLength(200)]
        public string Credits { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [StringLength(200)]
        public string Country { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [StringLength(200)]
        public string Area { get; set; }

        /// <summary>
        /// ��ϸ��ַ
        /// </summary>
        [StringLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// ����¼IP
        /// </summary>
        [StringLength(200)]
        public string LastLoginIP { get; set; }

        /// <summary>
        /// ����¼ʱ��
        /// </summary>
        public DateTime LastLoginDt { get; set; }

        /// <summary>
        /// �˻����
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// �˺�����
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// �˺�״̬
        /// </summary>
        public int State { get; set; }
    }
}