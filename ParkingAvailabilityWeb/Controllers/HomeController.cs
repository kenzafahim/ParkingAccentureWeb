using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParkingAvailabilityWeb.Models;
using ParkingAvailabilityWeb.Helper;
using System.Web.Hosting;
using System.Text;
using System.Net.Mail;

namespace ParkingAvailabilityWeb.Controllers
{
    public class HomeController : Controller
    {
        private MyDatabaseEntities db = new MyDatabaseEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoginIn()
        {
            return View();
        }
        

        public ActionResult Place()
        {
            return View();
        }

        public ActionResult GetList()
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                var colList = db.Collaborators.ToList<Collaborators>();
                return Json(new { data = colList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Inscrip()
        {
            Admin admin = new Admin();
            return View("Inscrip", admin);
        }
       

        public ActionResult Register(Admin admin)
        {
            admin.IsValid = false;
            admin.Password = Helper.PasswordHelper.HashPassword(admin.Password);
            db.Admin.Add(admin);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            BuildEmailTemplate(admin.Id);
            return RedirectToAction("Login","Home");
        }

        public ActionResult Confirm(int regId)
        {
            ViewBag.regID = regId;
            return View();
        }
        public JsonResult RegisterConfirm(int regId)
        {
            Admin data = db.Admin.Where(x => x.Id == regId).FirstOrDefault();
            data.IsValid = true;
            db.SaveChanges();
            var msg = "Your Email Is Verified !";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckUser(string user)
        {
            System.Threading.Thread.Sleep(200);
            var Seach = db.Admin.Where(x => x.UserName == user).SingleOrDefault();
            if(Seach != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
            
        }
        public void BuildEmailTemplate(int regID)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            var regInfo = db.Admin.Where(x => x.Id == regID).FirstOrDefault();
            var url = "http://localhost:50402/"+"Home/Accueil?regId=" + regID;
            body = body.Replace("@ViewBag.ConfirmationLink", url);
            body = body.ToString();
            BuildEmailTemplate("Your Account Is Successfully Created", body, regInfo.Email);
        }
        public void BuildEmailTemplate(string subjectText, string bodyText,string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "kenzafahim22@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendMail(mail);
        }

        public static void SendMail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("kenzafahim22@gmail.com","kenzafahim178030");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            Admin admin = new Admin();
            return View("Login", admin);
        }

        public ActionResult Accueil(int regId)
        {
            ViewBag.regID = regId;
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            var admin2 = db.Admin.SingleOrDefault(a => a.UserName.Equals(admin.UserName));
            if (admin2 != null)
            {
                if (Helper.PasswordHelper.ValidatePassword(admin.Password, admin2.Password))
                {
                    Session["username"] = admin2.UserName;
                    return View("Accueil");
                }
            } 
                ViewBag.ErrorMessage = "Account's Invalid";
                return View("Login");
            
            
        }

        public ActionResult GetPlace()
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                var colList = db.ParkingPlace.ToList<ParkingPlace>();
                return Json(new { data = colList }, JsonRequestBehavior.AllowGet);
            }
        }
      
        [HttpGet]
        public ActionResult AddOrEdit(int id = 1)
        {
            if(id==0)
            return View(new Collaborators());
            else
            {
                using(MyDatabaseEntities db= new MyDatabaseEntities())
                {
                    return View(db.Collaborators.Where(x => x.Id == id).FirstOrDefault<Collaborators>());
                }
               
            }
        }

        [HttpGet]
        public ActionResult Afficher(int id = 1)
        {
            if (id == 0)
                return View(new Collaborators());
            else
            {
                using (MyDatabaseEntities db = new MyDatabaseEntities())
                {
                    return View(db.Collaborators.Where(x => x.Id == id).FirstOrDefault<Collaborators>());
                }

            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Collaborators coll)
        {
            using (MyDatabaseEntities db= new MyDatabaseEntities())
            {
                if (coll.Id == 0) {  
                db.Collaborators.Add(coll);
                db.SaveChanges();
                return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(coll).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);

                }
            }
            
        }
      

        [HttpGet]
        public ActionResult AddPlace(int id = 1)
        {
            if (id== 0)
                return View(new ParkingPlace());
            else
            {
                using (MyDatabaseEntities db = new MyDatabaseEntities())
                {
                    return View(db.ParkingPlace.Where(x => x.Id == id).FirstOrDefault<ParkingPlace>());
                }

            }
        }
        
        [HttpPost]
        public ActionResult AddPlace(ParkingPlace pp)
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                if (pp.Id == 0)
                {
                    db.ParkingPlace.Add(pp);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Place Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(pp).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Place Updated Successfully" }, JsonRequestBehavior.AllowGet);

                }
            }

        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using(MyDatabaseEntities db = new MyDatabaseEntities())
            {
                Collaborators coll = db.Collaborators.Where(x => x.Id == id).FirstOrDefault<Collaborators>();
                db.Collaborators.Remove(coll);
                db.SaveChanges();
                return Json(new { success = true, message="Deleted Successfully"}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeletePlace(int id)
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                ParkingPlace pp =db.ParkingPlace.Where(x => x.Id == id).FirstOrDefault<ParkingPlace>();
                db.ParkingPlace.Remove(pp);
                db.SaveChanges();
                return Json(new { success = true, message = "Place Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
    
}