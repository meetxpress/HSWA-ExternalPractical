using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace External.Filters {
    public class CustomException : ExceptionFilterAttribute {        
        public override void OnException(HttpActionExecutedContext actionExecutedContext) {
            base.OnException(actionExecutedContext);
            //While execution, if it throws DbUpdateException, then it will  return message with 'Provide Valid Data'
            if (actionExecutedContext.Exception is DbUpdateException) {
                HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.BadRequest);
                res.Content = new StringContent("Provide Valid Data!");
                res.ReasonPhrase = "Not able to Insert!";
                actionExecutedContext.Response = res;
            } else {
                //or any other type of exception will be handled by this code of block 
                //with 'Something went wrong'
                HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                res.Content = new StringContent("Something went wrong!");
                actionExecutedContext.Response = res;
            }
        }
    }
}