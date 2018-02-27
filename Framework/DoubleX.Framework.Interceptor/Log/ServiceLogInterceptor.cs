using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using DoubleX.Framework.Web;

namespace DoubleX.Framework.Interceptor
{
    /// <summary>
    /// 业务日志拦截器
    /// </summary>
    public class ServiceLogInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                //var context = WebHelper.GetContext();

                if (invocation.Method.Name == "GetI")
                {
                    //invocation.ReturnValue = 1 + (int)invocation.ReturnValue;
                }

                invocation.Proceed();
            }
            catch (Exception ex)
            {
                //invocation.ReturnValue = xxx;
                throw ex;
            }

        }
    }
}
