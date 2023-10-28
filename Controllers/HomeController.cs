using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProgrammingZone.Models;
using System.IO;
using System.Data;
using System.Data.SqlClient;


namespace ProgrammingZone.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        DatabaseManager db = new DatabaseManager();
        CaptchaGenerator cg = new CaptchaGenerator();
        EncryptionManager em = new EncryptionManager();
        SMSSender ss = new SMSSender();

        [HttpGet]
        public ActionResult Index()
        {
            string show = "";
            DatabaseManager db = new DatabaseManager();
            string cmd = "select * from Events order by Id asc";
            DataTable dt = db.GetAllRecord(cmd);
            show = "<div style='color:black; text-align:justify;'>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (dt.Rows.Count > 0)
                {

                    show += "<h4>" + dt.Rows[i]["Event"] + "</h4>";
                }

            }
            show += "</div>";
            ViewBag.details += show;
            return View();
        }

//............................................Code for Admin Login...........................................................//

       [HttpPost]
        public ActionResult Index(string uname, string passwd)
        {
               if (uname != null && uname != "" && passwd != null && passwd != "")
                {
                    SqlConnection con = new SqlConnection(@"Data Source=AYUSH-SINGH-CHA;Initial Catalog=ProgramminZone;Integrated Security=True");
                    con.Open();
                    string q = "select UserName,Passwd from AdminLogin where UserName='"+uname+"' and Passwd='"+passwd+"'";
              //    if (uname=="sk0525153@gmail.com" && passwd=="kingkhan")
                    SqlCommand cmd = new SqlCommand(q,con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
             
                        
                  //      Response.Write("<script>alert('Login Successfully')</script>");
                        Response.Redirect("/Home/Admin");
                        Session["MyValue"] = uname;
                     
                    }
                    else
                    {
                        Response.Write("<script>alert('Incorrect Password or UserName')</script>");
                        
                    }
                }
            return View();
        }

       public ActionResult AdminLogin()
       {
           return View();
       }

        public ActionResult Templates()
        {
            return View();
        }

//..................................................code for Generating Captcha...............................................

        [HttpGet]
        public ActionResult ContactUs()
        {
            CaptchaGenerator cg = new CaptchaGenerator();
            string cph = cg.Captcha();              //calling of catcha method.
            ViewBag.captcha = cph;                  //storing the result in a variable (captcha) to show on screen.
            return View();
        }

//...........................................code for contact us or any query...............................................

        [HttpPost]
        public ActionResult ContactUs(string uname, string txtem, string passwd, string suggest, string txtCaptcha, string txtCCaptcha, HttpPostedFileBase Fupic)
        {
            if (uname != null && uname != "" && txtem != null && txtem != "" && passwd != null && passwd != "")
            {
                if (txtCaptcha == txtCCaptcha)
                {
                    string file = Path.Combine(Server.MapPath("~/Content/pics"), Fupic.FileName);
                    Fupic.SaveAs(file);
                    string encpass = em.encrypt(passwd);
                    string cmd = "insert into enquiry values('" + uname + "','" + txtem + "','" + passwd + "','" + suggest + "','" + Fupic.FileName + "','" + DateTime.Now.ToString() +"' ,'1')";
                    if (db.InsertUpdateAndDelete(cmd) == true)
                    {
                        string cmd2 = "insert into visitors values('" + uname + "','" + encpass + "','1')";
                        if (db.InsertUpdateAndDelete(cmd2))
                        {
                            
                            Response.Write("<script>alert('Registration Successfully.........')</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('Not Registration........')</script>");
                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('Not Registration.........')</script>");
                    }
                }
                else
                {
                    Response.Write("<script>alert('Captcha Does Not Match')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('please fill all fields')</script>");
            }
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }
        
        public ActionResult HTMLPage(string Topic)
        {
             string topic = "";
            string theory = "";
            string t = "select * from HTMLContent order by Id asc";
            DatabaseManager dbt = new DatabaseManager();
             DataTable dtt = db.GetAllRecord(t);
   //          topic = "<div style='background:white;color:black' class='row panel panel-body'>";
             for (int i = 0; i < dtt.Rows.Count; i++)
             {

                 if (dtt.Rows.Count > 0)
                 {

                     topic += "<a href='/Home/HTMLPage?Topic=" + dtt.Rows[i]["topic"] + "' style='text-decoration:none;height:30px;'><input type='submit' value='" + dtt.Rows[i]["Topic"] + "' class='btn btn-block' style='background-color:orange; color:white;font-weight:bold; height:30px;'></a>";
                    
                 }
             }
            if (Topic == null || Topic == "")
            {

                DatabaseManager dbtt = new DatabaseManager();
                string cmd = "select * from HTMLContent order by Id asc";
                DataTable dt = dbtt.GetAllRecord(cmd);
      //          topic = "<div style='background:white;color:black' class='row panel panel-body'>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows.Count >0)
                    {

                    //  topic+= "<a href='/Home/HTMLPage?Topic=" + dt.Rows[i]["topic"] + "'><input type='submit' value='" + dt.Rows[i]["Topic"] + "' class='btn btn-block btn btn-info'></a>";
                        ViewBag.head=dt.Rows[i]["Topic"];
                        ViewBag.des= dt.Rows[i]["Theory"]+"";
                    }
                    break;
                }
            }
            else
            {
                string q = "select Theory from HTMLContent where topic='" + Topic + "' and Status='True'";
                ViewBag.heading = Topic; 
                //       string theory = "";
                       DatabaseManager dbtt = new DatabaseManager();

                         DataTable dt = dbtt.GetAllRecord(q);
                //      topic = "<div style='background:white;color:black' class='row panel panel-body'>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows.Count > 0)
                    {

                        theory += "<h4>" + dt.Rows[i]["Theory"] + "</h4>";
                    }

                }
            }
           
       //     topic += "</div>";
            ViewBag.topic += topic;
            ViewBag.theory += theory;
            return View();
        }
        public ActionResult CSSPage(string Topic)
        {
            string topic = "";
            string theory = "";
            string t = "select * from CSSContent order by Id asc";
            DatabaseManager dbt = new DatabaseManager();
            DataTable dtt = db.GetAllRecord(t);
            //          topic = "<div style='background:white;color:black' class='row panel panel-body'>";
            for (int i = 0; i < dtt.Rows.Count; i++)
            {

                if (dtt.Rows.Count > 0)
                {

                    topic += "<a href='/Home/CSSPage?Topic=" + dtt.Rows[i]["topic"] + "' style='text-decoration:none;height:30px;'><input type='submit' value='" + dtt.Rows[i]["Topic"] + "' class='btn btn-block' style='background-color:orange; color:white;font-weight:bold; height:30px;'></a>";

                }
            }
            if (Topic == null || Topic == "")
            {

                DatabaseManager dbtt = new DatabaseManager();
                string cmd = "select * from CSSContent order by Id asc";
                DataTable dt = dbtt.GetAllRecord(cmd);
                //          topic = "<div style='background:white;color:black' class='row panel panel-body'>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows.Count > 0)
                    {

                        //  topic+= "<a href='/Home/HTMLPage?Topic=" + dt.Rows[i]["topic"] + "'><input type='submit' value='" + dt.Rows[i]["Topic"] + "' class='btn btn-block btn btn-info'></a>";
                        ViewBag.head = dt.Rows[i]["Topic"];
                        ViewBag.des = dt.Rows[i]["Theory"] + "";
                    }
                    break;

                }
            }
            else
            {
                string q = "select Theory from CSSContent where topic='" + Topic + "' and Status='True'";
                ViewBag.heading = Topic;
                //       string theory = "";
                DatabaseManager dbtt = new DatabaseManager();

                DataTable dt = dbtt.GetAllRecord(q);
                //      topic = "<div style='background:white;color:black' class='row panel panel-body'>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows.Count > 0)
                    {

                        theory += "<h4>" + dt.Rows[i]["Theory"] + "</h4>";
                    }

                }
            }

            //     topic += "</div>";
            ViewBag.topic += topic;
            ViewBag.theory += theory;
            return View();
        }
        public ActionResult JavaScriptPage(string Topic)
        {
            string topic = "";
            string theory = "";
            string t = "select * from JavaScriptContent order by Id asc";
            DatabaseManager dbt = new DatabaseManager();
            DataTable dtt = db.GetAllRecord(t);
            //          topic = "<div style='background:white;color:black' class='row panel panel-body'>";
            for (int i = 0; i < dtt.Rows.Count; i++)
            {

                if (dtt.Rows.Count > 0)
                {

                    topic += "<a href='/Home/JavaScriptPage?Topic=" + dtt.Rows[i]["topic"] + "' style='text-decoration:none;height:30px;'><input type='submit' value='" + dtt.Rows[i]["Topic"] + "' class='btn btn-block' style='background-color:orange; color:white;font-weight:bold; height:30px;'></a>";

                }
            }
            if (Topic == null || Topic == "")
            {

                DatabaseManager dbtt = new DatabaseManager();
                string cmd = "select * from JavaScriptContent order by Id asc";
                DataTable dt = dbtt.GetAllRecord(cmd);
                //          topic = "<div style='background:white;color:black' class='row panel panel-body'>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows.Count > 0)
                    {

                        //  topic+= "<a href='/Home/HTMLPage?Topic=" + dt.Rows[i]["topic"] + "'><input type='submit' value='" + dt.Rows[i]["Topic"] + "' class='btn btn-block btn btn-info'></a>";
                        ViewBag.head = dt.Rows[i]["Topic"];
                        ViewBag.des = dt.Rows[i]["Theory"] + "";
                    }
                    break;

                }
            }
            else
            {
                string q = "select Theory from JavaScriptContent where topic='" + Topic + "' and Status='True'";
                ViewBag.heading = Topic;
                //       string theory = "";
                DatabaseManager dbtt = new DatabaseManager();

                DataTable dt = dbtt.GetAllRecord(q);
                //      topic = "<div style='background:white;color:black' class='row panel panel-body'>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows.Count > 0)
                    {

                        theory += "<h4>" + dt.Rows[i]["Theory"] + "</h4>";
                    }

                }
            }

            //     topic += "</div>";
            ViewBag.topic += topic;
            ViewBag.theory += theory;
            return View();
        }
        public ActionResult BootstrapPage(string Topic)
        {
            string topic = "";
            string theory = "";
            string t = "select * from BootstrapContent order by Id asc";
            DatabaseManager dbt = new DatabaseManager();
            DataTable dtt = db.GetAllRecord(t);
            //          topic = "<div style='background:white;color:black' class='row panel panel-body'>";
            for (int i = 0; i < dtt.Rows.Count; i++)
            {

                if (dtt.Rows.Count > 0)
                {

                    topic += "<a href='/Home/BootstrapPage?Topic=" + dtt.Rows[i]["topic"] + "' style='text-decoration:none;height:30px;'><input type='submit' value='" + dtt.Rows[i]["Topic"] + "' class='btn btn-block' style='background-color:orange; color:white;font-weight:bold; height:30px;'></a>";

                }
            }
            if (Topic == null || Topic == "")
            {

                DatabaseManager dbtt = new DatabaseManager();
                string cmd = "select * from BootstrapContent order by Id asc";
                DataTable dt = dbtt.GetAllRecord(cmd);
                //          topic = "<div style='background:white;color:black' class='row panel panel-body'>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows.Count > 0)
                    {

                        //  topic+= "<a href='/Home/HTMLPage?Topic=" + dt.Rows[i]["topic"] + "'><input type='submit' value='" + dt.Rows[i]["Topic"] + "' class='btn btn-block btn btn-info'></a>";
                        ViewBag.head = dt.Rows[i]["Topic"];
                        ViewBag.des = dt.Rows[i]["Theory"] + "";
                    }
                    break;

                }
            }
            else
            {
                string q = "select Theory from BootstrapContent where topic='" + Topic + "' and Status='True'";
                ViewBag.heading = Topic;
                //       string theory = "";
                DatabaseManager dbtt = new DatabaseManager();

                DataTable dt = dbtt.GetAllRecord(q);
                //      topic = "<div style='background:white;color:black' class='row panel panel-body'>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows.Count > 0)
                    {

                        theory += "<h4>" + dt.Rows[i]["Theory"] + "</h4>";
                    }

                }
            }

            //     topic += "</div>";
            ViewBag.topic += topic;
            ViewBag.theory += theory;
            return View();
        }
        public ActionResult SQLPage(string Topic)
        {
            string topic = "";
            string theory = "";
            string t = "select * from SQLContent order by Id asc";
            DatabaseManager dbt = new DatabaseManager();
            DataTable dtt = db.GetAllRecord(t);
            //          topic = "<div style='background:white;color:black' class='row panel panel-body'>";
            for (int i = 0; i < dtt.Rows.Count; i++)
            {

                if (dtt.Rows.Count > 0)
                {

                    topic += "<a href='/Home/SQLPage?Topic=" + dtt.Rows[i]["topic"] + "' style='text-decoration:none;height:30px;'><input type='submit' value='" + dtt.Rows[i]["Topic"] + "' class='btn btn-block' style='background-color:orange; color:white;font-weight:bold; height:30px;'></a>";

                }
            }
            if (Topic == null || Topic == "")
            {

                DatabaseManager dbtt = new DatabaseManager();
                string cmd = "select * from SQLContent order by Id asc";
                DataTable dt = dbtt.GetAllRecord(cmd);
                //          topic = "<div style='background:white;color:black' class='row panel panel-body'>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows.Count > 0)
                    {

                        //  topic+= "<a href='/Home/HTMLPage?Topic=" + dt.Rows[i]["topic"] + "'><input type='submit' value='" + dt.Rows[i]["Topic"] + "' class='btn btn-block btn btn-info'></a>";
                        ViewBag.head = dt.Rows[i]["Topic"];
                        ViewBag.des = dt.Rows[i]["Theory"] + "";
                    }
                    break;

                }
            }
            else
            {
                string q = "select Theory from SQLContent where topic='" + Topic + "' and Status='True'";
                ViewBag.heading = Topic;
                //       string theory = "";
                DatabaseManager dbtt = new DatabaseManager();

                DataTable dt = dbtt.GetAllRecord(q);
                //      topic = "<div style='background:white;color:black' class='row panel panel-body'>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows.Count > 0)
                    {

                        theory += "<h4>" + dt.Rows[i]["Theory"] + "</h4>";
                    }

                }
            }

            //     topic += "</div>";
            ViewBag.topic += topic;
            ViewBag.theory += theory;
            return View();
        }
        public ActionResult Admin()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddMaterial()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMaterial(string topic, string theory,string type)
        {
            if (type == "HTML")
            {
                string cmd = "insert into HTMLContent values('" + topic + "','" + theory + "','" + DateTime.Now.ToString() + "' ,'True')";
                if (db.InsertUpdateAndDelete(cmd) == true)
                {
                    Response.Write("<script>alert('Content Uploaded Successfully')</script>");
                }
                else
                {
                    Response.Write("<script>alert('Content Uploading Failed')</script>");
                }
            }
            if (type == "CSS")
            {
                string cmd = "insert into CSSContent values('" + topic + "','" + theory + "','" + DateTime.Now.ToString() + "' ,'True')";
                if (db.InsertUpdateAndDelete(cmd) == true)
                {
                    Response.Write("<script>alert('Content Uploaded Successfully')</script>");
                }
                else
                {
                    Response.Write("<script>alert('Content Uploading Failed')</script>");
                }
            }
            if (type == "JavaScript")
            {
                string cmd = "insert into JavaScriptContent values('" + topic + "','" + theory + "','" + DateTime.Now.ToString() + "' ,'True')";
                if (db.InsertUpdateAndDelete(cmd) == true)
                {
                    Response.Write("<script>alert('Content Uploaded Successfully')</script>");
                }
                else
                {
                    Response.Write("<script>alert('Content Uploading Failed')</script>");
                }
            }
            if (type == "Bootstrap")
            {
                string cmd = "insert into BootstrapContent values('" + topic + "','" + theory + "','" + DateTime.Now.ToString() + "' ,'True')";
                if (db.InsertUpdateAndDelete(cmd) == true)
                {
                    Response.Write("<script>alert('Content Uploaded Successfully')</script>");
                }
                else
                {
                    Response.Write("<script>alert('Content Uploading Failed')</script>");
                }
            }
            if (type == "SQL")
            {
                string cmd = "insert into SQLContent values('" + topic + "','" + theory + "','" + DateTime.Now.ToString() + "' ,'True')";
                if (db.InsertUpdateAndDelete(cmd) == true)
                {
                    Response.Write("<script>alert('Content Uploaded Successfully')</script>");
                }
                else
                {
                    Response.Write("<script>alert('Content Uploading Failed')</script>");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase Upfile, string message,string type)
        {
            DatabaseManager db = new DatabaseManager();
            string path = Path.Combine(Server.MapPath("~/Content/pic"),Upfile.FileName);
            string cmd = "insert into UploadFile values('" + message + "','" + Upfile.FileName + "','" + type + "','" + DateTime.Now.ToString() + "','True')";
            if (db.InsertUpdateAndDelete(cmd) == true)
            {
                Response.Write("<script>alert('File Uploaded Successfully')</script>");
            }
            else
            {
                Response.Write("<script>alert('Unable to upload file')</script>");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Downloads()
        {
            DatabaseManager db = new DatabaseManager();
            string cmd = "select * from UploadFile order by Id asc";
            DataTable dt = db.GetAllRecord(cmd);
            string type = "";
            string tbl = "<table id='example' class='display' style='width:100%;'>";
            tbl += "<thead><tr><th>File Name</th><th>Date</th><th>Download</th></tr></thead>";
            tbl += "<tbody>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows.Count > 0)
                {

                    type = dt.Rows[i]["Type"] + "";
                    if (type == "PPT" || type == "PDF")
                    {
                        tbl += "<tr><td>" + dt.Rows[i]["Message"] + "</td><td>" + dt.Rows[i]["DateTime"] + "</td><td><a href='../Content/pics/" + dt.Rows[i]["Doc"] + "'><span class='fa fa-download' style='color:black;'></span></a></td></tr>";
                    }
                }
            }
            tbl += "</tbody>";
            tbl += "</table>";
            //         show += "</div>";
            ViewBag.file = tbl;
            return View();
        }

        [HttpPost]
        public ActionResult Downloads(string name)
        {
            return View();
        }

        [HttpGet]
        public ActionResult DynamicEvent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DynamicEvent(string message)
        {

            string cmd = "insert into Events values('" + message + "','" + DateTime.Now.ToString() + "' ,'True')";
                if (db.InsertUpdateAndDelete(cmd) == true)
                {
                    Response.Write("<script>alert('Event Uploaded Successfully')</script>");
                }
                else
                {
                    Response.Write("<script>alert('Event Uploading Failed')</script>");
                }

            return View();
        }

//...............................................code to show records of team members...................................................

        [HttpGet]
        public ActionResult OurTeam()
        {
          //  string show = "";
           
            DatabaseManager db = new DatabaseManager();
            string cmd = "select * from Teammembers order by DateTime asc";
            DataTable dt = db.GetAllRecord(cmd);
         //   show = "<div id='example' class='display' style='background:white;color:black' class='row panel panel-body'>";
            string tbl = "<table id='example' class='display' style='width:100%;'>";
            tbl += "<thead><tr><th>Photo</th><th>Name</th><th>Email</th><th>Mobile</th><th>About</th><th>Delete</th></tr></thead>";
            tbl += "<tbody>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (dt.Rows.Count > 0)
                {

 /*                   show += "<div class='row' style='min-height:100px;'><div class='col-sm-2'><img class='img-thumbnail' style='height:80px;width:80px' src='../Content/pics/" + dt.Rows[i]["Picture"] + "'></div><div class='col-sm-8'><h4>" + dt.Rows[i]["Name"] + "</h4><h4>" + dt.Rows[i]["Email"] + "</h4><h4>" + dt.Rows[i]["Mobile"] + "</h4><h4>" + dt.Rows[i]["About"] + "</h4></div><div class='col-sm-2'><a href='/Home/Delete?del=" + dt.Rows[i]["Email"] + "'><input type='submit' class='btn btn-success' value='Delete' style='margin-top:50%;'/></a></div></div><hr style='background:black; color:black; height:5px;'>";
*/
                    tbl += "<tr><td><img class='img-thumbnail' style='height:80px;width:80px' src='../Content/pics/" + dt.Rows[i]["Picture"] + "'></td><td><h4>" + dt.Rows[i]["Name"] + "</h4></td><td><h4>" + dt.Rows[i]["Email"] + "</h4></td><td><h4>" + dt.Rows[i]["Mobile"] + "</h4></td><td><h4>" + dt.Rows[i]["About"] + "</h4></td><td><a href='/Home/Delete?del=" + dt.Rows[i]["Email"] + "'><input type='submit' class='btn btn-success' value='Delete'/></a></td></tr>";
                }

            }
            tbl += "</tbody>";
            tbl += "</table>";
            ViewBag.show = tbl;
            ViewBag.details = tbl;
    //        show += "</div>";
    //        ViewBag.details += show;
            return View();
        }

//...........................................code to add new team member...............................................

        [HttpPost]
        public ActionResult OurTeam(string mname, string email, string Mnumber, string about, HttpPostedFileBase Fupic)
        {
            if (mname != null&& mname!= "" &&email!= null &&email!= "" && Mnumber!= null &&Mnumber!= "")
            {
                
              string file = Path.Combine(Server.MapPath("~/Content/pics"), Fupic.FileName);
            Fupic.SaveAs(file);

            string cmd = "insert into TeamMembers values('" + mname + "','" + email + "','" + Mnumber + "','" + about + "','" + DateTime.Now.ToString() + "' ,'" + Fupic.FileName + "','1')";
                    if (db.InsertUpdateAndDelete(cmd) == true)
                    {
                        Session["Team"] = email;
                        Response.Write("<script>alert('Data Inserted Successfully')</script>");
               //       string msg = "thanks ! "+mname+" You Added in my Team Member in Programming Zone";
              //          ss.SendSms(Mnumber, msg);    
                  //      Response.Write("<script>alert('Data Inserted Successfully')</script>");
                        Response.Redirect("/Home/Admin");
                    }
                    else
                    {
                        Response.Write("<script>alert('Data Insertion Failed')</script>");
                    }
            }
            else
            {
                Response.Write("<script>alert('please fill all required feilds')</script>");
            }
        
            return View();
        }

        [HttpGet]
        public ActionResult FileManagement()
        {
 //           string show = "";
            DatabaseManager db = new DatabaseManager();
            string cmd = "select * from UploadFile order by Id asc";
            DataTable dt = db.GetAllRecord(cmd);
            string type = "";
 //           show = "<div style='background:white;color:black' class='row panel panel-body'>";
            string tbl = "<table id='example' class='display' style='width:100%;'>";
            tbl += "<thead><tr><th>File Name</th><th>Date</th><th>Delete</th></tr></thead>";
            tbl += "<tbody>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows.Count > 0)
                {

  /*                  show += "<div class='row' style='min-height:100px;'><div class='col-sm-8'>" + dt.Rows[i]["Message"] + "</div><div class='col-sm-2'><img class='img-thumbnail' style='height:80px;width:80px' src='../Content/pics/" + dt.Rows[i]["Doc"] + "'</div><div class='col-sm-2'></div></div>";
   */
                    type = dt.Rows[i]["Type"] + "";
                    if (type == "PPT" || type=="PDF")
                    {
                        tbl += "<tr><td>" + dt.Rows[i]["Message"] + "</td><td>" + dt.Rows[i]["DateTime"] + "</td><td><a href='../Content/pics/" + dt.Rows[i]["Doc"] + "'><span class='fa fa-trash' style='color:black;'></span></a></td></tr>";
                    }
                }
            }
            tbl += "</tbody>";
            tbl += "</table>";
   //         show += "</div>";
           ViewBag.file = tbl;
            return View();
        }

        [HttpPost]
        public ActionResult FileManagement(string a)
        {
            return View();
        }

        public ActionResult EventManagement()
        {
    //        string show = "";
            DatabaseManager db = new DatabaseManager();
            string cmd = "select * from Events order by Id asc";
            DataTable dt = db.GetAllRecord(cmd);
  //          show = "<div style='background:teal;color:white; text-align:justify;'>";
            string tbl = "<table id='example' class='display' style='width:100%;'>";
            tbl += "<thead><tr><th>Event</th><th>Date</th><th>Delete</th></tr></thead>";
            tbl += "<tbody>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (dt.Rows.Count > 0)
                {

  /*                  show += "<div class='row'><div class='col-sm-10'><h4>" + dt.Rows[i]["Event"] + "</h4></div><div class='col-sm-2'><a href='/Home/DeleteEvent?del=" + dt.Rows[i]["Id"] + "'><input type='submit' class='btn btn-success' value='Delete' style='margin-top:50%;'/></a></div></div>";
   */
                    tbl += "<tr><td>" + dt.Rows[i]["Event"] + "</td><td>" + dt.Rows[i]["Date"] + "</td><td><a href='/Home/DeleteEvent?del=" + dt.Rows[i]["Id"] + "'><input type='submit' class='btn btn-success' value='Delete' style='margin-top:50%;'/></a></td></tr>";
                }

            }
            tbl += "</tbody>";
            tbl += "</table>";
            ViewBag.show = tbl;
            ViewBag.details = tbl;
          //  show += "</div>";
         //   ViewBag.details += show;
            return View();
        }

        public ActionResult DeleteEvent(string del)
        {
            string cmd = "delete from Events where Id='" + del + "'";
            DatabaseManager db = new DatabaseManager();
            if (db.InsertUpdateAndDelete(cmd))
            {
                Response.Write("<script>alert('Data deleted successfully')</script>");
                Response.Redirect("/Home/Admin");
            }
            else
                Response.Write("<script>alert('Data can not be deleted')</script>");
            return View();
        }
        
        [HttpGet]
        public ActionResult ResponseManagement()
        {
            DatabaseManager db = new DatabaseManager();
            string cmd = "select * from Enquiry";
            DataTable dt = db.GetAllRecord(cmd);
            string tbl = "<table id='example' class='display' style='width:100%;'>";
            tbl += "<thead><tr><th>Event</th><th>Date</th><th>Delete</th><th>Date & Time</th><th>Response</th></tr></thead>";
            tbl += "<tbody>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (dt.Rows.Count > 0)
                {

                    tbl += "<tr><td>" + dt.Rows[i]["Name"] + "</td><td>" + dt.Rows[i]["Email"] + "</td><td>" + dt.Rows[i]["Suggest"] + "</td><td>" + dt.Rows[i]["DTM"] + "</td><td><a href='/Home/Mail?id=" + dt.Rows[i]["Email"] + "'><span class='fa fa-envelope' title='Send Response'></span></a></td></tr>";
                }

            }
            tbl += "</tbody>";
            tbl += "</table>";
            ViewBag.show = tbl;
            ViewBag.details = tbl;
            return View();
        }

        [HttpGet]
        public ActionResult Mail(string id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public ActionResult Mail(string id, FormCollection fc)
        {
            ViewBag.id = id;
            EmailSender em = new EmailSender();
            em.SendTo = fc[0];
            em.Subject = fc[1];
            em.MessageBody = fc[2];
            if (em.sendEmail() == true)
            {
                ViewBag.msg = "Email Send Successfully";
            }
            else
            {
                ViewBag.msg = "Unable to send mail";
            }
            return View();
        }

        [HttpPost]
        public ActionResult ResponseManagement(string name)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Settings()
        {
            return View();
        }

//................................................code to change admin login password.........................................

        [HttpPost]
        public ActionResult Settings(string OldPass, string NewPass, string ConPass)
        {
            string pass="";
            if(OldPass!=null && OldPass!="" && NewPass!=null && NewPass!="" && ConPass!=null && ConPass!="")
            {
                  if(NewPass==ConPass)
                  {
                      SqlConnection con = new SqlConnection(@"Data Source=HP\SQLEXPRESS;Initial Catalog=ProgramminZone;Integrated Security=True");
                      con.Open();
                      string q = "select Passwd from AdminLogin where Passwd='"+OldPass+"' and Status='True'";
                      SqlCommand cmd = new SqlCommand(q, con);
                      SqlDataAdapter da = new SqlDataAdapter(cmd);
                      DataTable dt = new DataTable();
                      da.Fill(dt);
                      for (int i = 0; i < dt.Rows.Count; i++)
                      {
                          if (dt.Rows.Count > 0)
                          {
                        //      string pass = dt.Rows[i]["Passwd"] + "";
                              pass = dt.Rows[i]["Passwd"] + "";
                              if (pass == OldPass)
                              {
                                  
                                  Response.Write("<script>alert('Now you can change password')</script>");
                                  string change = "update AdminLogin set Passwd='" + NewPass + "' where Passwd='" + OldPass + "'";
                                  if (db.InsertUpdateAndDelete(change) == true)
                                  {
                                      Response.Write("Password changed successfully");
                                  }
                              }
                          }
                      }
                      if (pass!=OldPass)
                      
                      {
                          Response.Write("<script>alert('invalid username or password')</script>");
                      }     
                  }
                  else
                  {
                     Response.Write("<script>alert('new password and confirm password did not match')</script>");
                  }
            }
            return View();
        }
        public ActionResult LogOut()
        {
            return View();
        }
        public ActionResult ShowMemberDetails()
        {
            string show = "";
            DatabaseManager db = new DatabaseManager();
            string cmd = "select * from Teammembers order by Id asc";
            DataTable dt = db.GetAllRecord(cmd);
            show = "<div style='background:white;color:black' class='row panel panel-body'>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                
                if (dt.Rows.Count > 0)
                {
                   
                    show += "<img class='img-thumbnail' style='height:80px;width:80px' src='../Content/pics/" + dt.Rows[i]["Picture"] + "'><h4>" + dt.Rows[i]["Name"] + "</h4><h4>" + dt.Rows[i]["Email"] + "</h4><h4>" + dt.Rows[i]["Mobile"] + "</h4><a href='/Home/Delete?del="+dt.Rows[i]["Email"]+"'><input type='submit' class='btn btn-success' value='Delete'/></a>";
                }
               
            }
            show += "</div>";
            ViewBag.details+= show;
                return View();
        }

        public ActionResult Delete(string del)
        {
            string cmd = "delete from Teammembers where email='" + del + "'";
            DatabaseManager db = new DatabaseManager();
            if (db.InsertUpdateAndDelete(cmd))
            {
                Response.Write("<script>alert('Data deleted successfully')</script>");
                Response.Redirect("/Home/Admin");
            }
            else
                Response.Write("<script>alert('Data can not be deleted')</script>");
            return View();
        }

        public ActionResult check()
        {
            DatabaseManager db = new DatabaseManager();
            string cmd = "select type,Doc from UploadFile order by Id asc";
            string type="";
            DataTable dt = db.GetAllRecord(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows.Count > 0)
                {
                    type = dt.Rows[i]["Type"] + "";
                    if (type == "PPT")
                    {
                        ViewBag.ppt = "<a href='../Content/pics/" + dt.Rows[i]["Doc"] + "'><span class='fa fa-phone' style='color:white;'></span></a>";
                    }
                    
                }
               
            }
                return View();
        }

    }
}
