using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UPPRB_Web.Infrastructure.Utility
{
    public class CustomExceptionFilter : ActionFilterAttribute, IExceptionFilter
    {
        ILog logger = LogManager.GetLogger(typeof(CustomExceptionFilter));
        public void OnException(ExceptionContext filterContext)
        {
            Exception e = filterContext.Exception;
            //filterContext.ExceptionHandled = true;
            //filterContext.Result = new ViewResult()
            //{
            //    ViewName = "ExceptionPage"
            //};
            logger.Error(e.InnerException);
        }
    }

    //public interface IExceptionFilter
    //{
    //    void OnException(ExceptionContext filterContext);
    //}
}