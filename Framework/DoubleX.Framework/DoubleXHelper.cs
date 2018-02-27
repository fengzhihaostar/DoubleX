using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Reflection;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Infrastructure.Core.Config;

namespace DoubleX.Framework
{
    public class DoubleXHelper
    {
        #region 公共属性

        /// <summary>
        /// Web应用程序项目版本号
        /// </summary>
        public static string VersionNo
        {
            get
            {
                return DllHelper.GetVersion(SettingConfig.GetValue(KeyModel.Config.Setting.KeyApplaction, KeyModel.Config.Setting.GroupSystem));
            }
        }

        #endregion

        #region 生成标识

        /// <summary>
        /// 生成标识
        /// </summary>
        public static Guid GenerateVisitId()
        {
            return GuidHelper.NewId();
        }

        #endregion

        #region 公共操作

        /// <summary>
        /// 获取描述信息（无HTML）
        /// </summary>
        /// <param name="content"></param>
        /// <param name="length"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static string GetDescript(string content, int length = 0, int startIndex = 0)
        {
            if (VerifyHelper.IsEmpty(content))
                return "";
            if (length == 0)
            {
                length = IntHelper.Get(SettingConfig.GetValue(KeyModel.Config.Setting.KeyDescriptLength));
            }
            content = HtmlsHelper.Remove(HtmlsHelper.Decode(content));
            return StringHelper.ToCutString(content, length, startIndex: startIndex);
        }
        
        #endregion

        #region 结果信息

        /// <summary>
        /// 请求信息转为结果实体(动态结果实体)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ResultModel GetResult(RequestModel request)
        {
            ResultModel result = new ResultModel();
            //result.Passport = AccountService.GetPassport();
            return result;
        }

        /// <summary>
        /// 请求信息转为结果实体(泛型结果实体)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ResultModel<TEntity> GetResult<TEntity>(RequestModel request)
        {
            ResultModel<TEntity> result = new ResultModel<TEntity>();
            //result.Passport = AccountService.GetPassport();
            return result;
        }

        /// <summary>
        /// 获取ResultModel消息对象(根据Exception)
        /// </summary>
        public static ResultModel GetResult(Exception ex)
        {
            var result = new ResultModel() { Code = EnumHelper.GetValue(EnumResultCode.未知异常) };

            if (VerifyHelper.IsEmpty(ex))
            {
                return result;
            }

            DefaultException defEx = ex as DefaultException;
            if (!VerifyHelper.IsEmpty(defEx))
            {
                result.Code = EnumHelper.GetValue(defEx.Code);
                result.Message = defEx.Message;
                return result;
            }

            MessageException msgEx = ex as MessageException;
            if (!VerifyHelper.IsEmpty(msgEx))
            {
                result.Code = EnumHelper.GetValue(msgEx.Code);
                result.Message = msgEx.Message;
                return result;
            }

            result.Message = ex.Message;
            return result;
        }

        #endregion
    }
}
