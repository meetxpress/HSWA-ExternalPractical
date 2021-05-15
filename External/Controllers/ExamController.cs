using External.Filters;
using External.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace External.Controllers {
    public class ExamController : ApiController {
        DbExternalEntities db = new DbExternalEntities();
        /*
         * Created an action that's name starts with Post keyword and
         * passing a values that will be stored in the database and 
         * applieng authorization filter for narrow down the access of this operation
        */
        [Autho]
        [CustomException]
        public String PostAppData(Appointment_Master apm) {
            if(apm != null) {
                db.Appointment_Master.Add(apm);
                db.SaveChanges();                    
                return "Appoinment has been booked!";
           } else {
                return "Error Accured!";
           }
        }

        /*
         * To display all the records of the table Appointment_master
         */
        public List<Appointment_Master> GetAppDetails() {
            return db.Appointment_Master.ToList();
        }

        /*
         * To display specific records of the table Appointment_master 
         * order by appointment Id
         */
        public Appointment_Master GetAppDetails(int apid) {
            return db.Appointment_Master.Find(apid);
        }
    }
}
