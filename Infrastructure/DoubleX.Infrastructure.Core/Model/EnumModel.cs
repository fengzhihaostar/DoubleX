using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Core.Model
{
    /// <summary>
    /// 结果编码
    /// </summary>
    public enum EnumResultCode
    {
        //将60000000开头的错误视为错误编码
        //  60     00     00       00
        //  ┇      ┇       ┇       ┇
        // 类型   模块     二级    功能
        //(如下) (递增)   (递增)  (递增)
        //60程序错误，61标识为业务错误,62项目API服务业务错误

        操作成功 = 0,
        未知异常 = 60000001,
        请求错误 = 60000002,
        参数错误 = 60000003,
        接口错误 = 60000004,
        提示信息 = 60000005,
        跳转地址 = 60000006,
        上传错误 = 60000101,

        #region 基本信息(61010000)

        //模块默认提示
        请选择要处理信息 = 61010001,
        验证码发送失败 = 61010002,
        验证码错误 = 61010003,
        验证码己失效 = 61010004,

        //语言信息错误(610101XX)

        //领域信息错误(610102XX)

        //上传操作错误(610103XX)

        #endregion

        #region 组织机构(61020000)

        //模块默认提示
        账号发生错误 = 61020000,

        //判断类错误
        请输入登录账号或密码 = 61020101,
        请输入邮箱地址 = 61020102,
        请输入手机号码 = 61020103,

        //提示类错误
        账号错误 = 61020201,
        密码错误 = 61020202,

        #endregion
    }

    /// <summary>
    /// 消息编辑
    /// </summary>
    public enum EnumMessageCode
    {

        //将8000开头的错误视为错误编码
        //   80      00    00
        //   ┇        ┇     ┇ 
        // 大模块   小模块  消息
        //(如下) (递增) (递增)
        //800000，为默认公共，其它递增

        //80程序消息
        信息错误 = 800000,

        //81公共模块
        请输入验证码 = 810100,
        请输入验证码接收地址 = 810101,
        验证码错误 = 810102,
        支付失败 = 810200,

        //83用户模块
        请输入手机号码 = 830101,
        请输入邮箱地址 = 830102,
        请输入密码 = 830103,
        该账号己注册 = 830104,
        注册失败 = 830105,
        该手机号码己绑定 = 830106,
        该邮箱地址己绑定 = 830107,
        绑定失败 = 830108,
        未找到账号信息 = 830109,
        找回密码失败 = 830109,


    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum EnumSex
    {
        保密 = 0,
        男 = 1,
        女 = 2
    }

    /// <summary>
    /// 租户类型
    /// </summary>
    public enum EnumTenantType
    {
        Default = 0,
        管理系统 = 1,
        租户系统 = 2
    }

    /// <summary>
    /// 职员类型
    /// </summary>
    public enum EnumEmployeeType
    {
        Default = 0,
        /// <summary>
        /// account=administrator
        /// </summary>
        超级管理员 = 1,
        /// <summary>
        /// account=admin
        /// </summary>
        租户管理 = 2,
        租户职员 = 3
    }

    /// <summary>
    /// 会员类型
    /// </summary>
    public enum EnumMemberType
    {
        Default = 0,
        临时访问 = 1,
        登录会员 = 2
    }

    /// <summary>
    /// 会员状态
    /// </summary>
    public enum EnumMemberState
    {
        Default = 0,
        启用 = 1,
        禁止 = 2,
    }

    /// <summary>
    /// 支付方式
    /// </summary>
    public enum EnumPaymentType
    {
        Default = 0,
        支付宝 = 1,
        微信 = 2,
        第三方支付 = 3,
        网银支付 = 4
    }

    /// <summary>
    /// 充值状态
    /// </summary>
    public enum EnumRechargeState
    {
        Default = 0,
        待支付 = 1,
        支付中 = 2,
        己支付 = 3
    }

    /// <summary>
    /// 费用记录
    /// </summary>
    public enum EnumCostType
    {
        Default = 0,
        充值记录 = 1,
        消费记录 = 2
    }
}
