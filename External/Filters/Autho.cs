using External.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace External.Filters {
    public class Autho :AuthorizationFilterAttribute{
        /*
         * The Authorization class is created to apply authorization filter on action 
         */
        public override void OnAuthorization(HttpActionContext actionContext) {
            base.OnAuthorization(actionContext);
            /*
             * if the authorization values won't be provided by performer, It will return
             * 'Authorization data is missing' with status code.
             */
            if (actionContext.Request.Headers.Authorization == null) {
                HttpResponseMessage httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                httpResponse.Content = new StringContent("Authorization data is missing");
                httpResponse.ReasonPhrase = "No Data for Authorization";
                actionContext.Response = httpResponse;
            } else {
                /*
                 * if the authorization values are provided by performer, 
                 * It will return try to match the data from User_master.
                 */
                String encodedData = actionContext.Request.Headers.Authorization.Parameter;
                //decording the authorization data
                String decodedData = Encoding.UTF8.GetString(Convert.FromBase64String(encodedData));
                String[] udata = decodedData.Split(':');
                String uname = udata[0];
                String upass = udata[1];

                DbExternalEntities dbb = new DbExternalEntities();
                User_Master u1 = dbb.User_Master.Where(u => u.user_id + "" == uname && u.user_password.Equals(upass)).FirstOrDefault();
                if (u1 != null) {
                    /*
                     * if the performer is authorization, then the action will be performed.
                     */
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(u1.user_id.ToString()), null);
                } else {
                    /*
                     * Else It will return 'You are not an Authorize user to perform this operation!' 
                     * with status code.
                     */
                    HttpResponseMessage httpResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    httpResponse.Content = new StringContent("You are not an Authorize user to perform this operation!");
                    httpResponse.ReasonPhrase = "Not Authorized!";
                    actionContext.Response = httpResponse;
                }
            }
        }
    }
}