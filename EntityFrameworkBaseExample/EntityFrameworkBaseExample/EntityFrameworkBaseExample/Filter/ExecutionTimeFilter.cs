using EntityFrameworkBaseExample.Entity;
using System;
using System.Diagnostics;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EntityFrameworkBaseExample.Filter
{
    public class ExecutionTimeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //执行Executing方法
            base.OnActionExecuting(actionContext);
            //添加stopwatch到当前 request properties属性中
            actionContext.Request.Properties.Add("Time", Stopwatch.StartNew());    
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //执行Executed
            base.OnActionExecuted(actionExecutedContext);
            try
            {
                //获取添加到Executing方法中的stopwatch
                var stopwatch = (Stopwatch)actionExecutedContext.Request.Properties["Time"];
                //removing the key
                actionExecutedContext.Request.Properties.Remove("Time");

                //计算请求耗时时间
                var elapsedTime = stopwatch.Elapsed;
                if (!(elapsedTime.TotalSeconds > 10)) return;
                //耗时时间大于10秒将持久化到时间
                using (var dbContext = new EFDbContext())
                {
                    var error = new Error
                    {
                        TotalSeconds = (decimal)elapsedTime.TotalSeconds,
                        Active = true,
                        CommandType = "Action Context",
                        CreateDate = DateTime.Now,
                        Exception = Convert.ToString(actionExecutedContext.Request),
                        FileName = "",
                        InnerException = actionExecutedContext.Response.ToString(),
                        Parameters = "",
                        Query = "",
                        RequestId = 0
                    };
                    dbContext.Errors.Add(error);
                    dbContext.SaveChanges();
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}