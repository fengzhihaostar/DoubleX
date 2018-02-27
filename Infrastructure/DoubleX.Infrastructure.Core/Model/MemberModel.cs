using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Core.Model
{
    /// <summary>
    ///会员信息（用户包含：访问者）
    /// </summary>
    public class MemberModel
    {
        /// <summary>
        /// 会员Id
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 显示别名
        /// </summary>
        public string NameTag { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 会员类型
        /// </summary>
        public EnumMemberType MemeberType
        {
            get
            {
                if (memberType == EnumMemberType.Default)
                {
                    memberType = EnumMemberType.临时访问;
                }
                return memberType;
            }
            set
            {
                memberType = value;
            }
        }
        private EnumMemberType memberType = EnumMemberType.Default;

        /// <summary>
        /// 最后登录IP
        /// </summary>
        public string LastLoginIP { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginDt { get; set; }
    }
}
