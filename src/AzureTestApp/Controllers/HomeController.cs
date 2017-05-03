using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AzureTestApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace AzureTestApp.Controllers
{
    public class HomeController : Controller
    {
        private FYP_ProjectContext _context = new FYP_ProjectContext();

        #region Index
        public IActionResult Index()
        {
            var client = _context.UserAccount.FirstOrDefault();
            return View(client);
        }
        [HttpPost]
        public ActionResult Index(UserAccount user)
        {
            var account = _context.UserAccount.Where(u => u.UserName == user.UserName && u.Password == user.Password).FirstOrDefault();
            if (account != null)
            {
                HttpContext.Session.SetString("UserId", account.UserId.ToString());
                HttpContext.Session.SetString("UserName", account.UserName);               
              
                return RedirectToAction("Welcome",null, new {id = account.UserId });
            }
            else
            {
                ModelState.AddModelError("", "Username or password is worng");
            }
            return View();
        }
        #endregion

        #region Welcome
        public ActionResult Welcome(string searchBy, string search, int userID)
        {
            ViewBag.UserName = HttpContext.Session.GetString("Username");
            #region if (search != null)
            if (search != null)
            {
                if (searchBy == "Social_Security_No")
                {
                    var patientDetails = (from pA in _context.PatientAccount
                                          join g in _context.Guardians on pA.GuardianId equals g.GuardianId
                                          join s in _context.SchoolLists on pA.SchoolId equals s.SchoolId
                                          join uA in _context.UserAccount on pA.OccId equals uA.UserId

                                          where pA.SocialSecurityNo.Equals(search) || search == null
                                          select new PatientDetailsViewModel
                                          {
                                              vmPatientTable = pA,
                                              vmGuardian = g,
                                              vmSchools = s,
                                              vmUserAcc = uA
                                          }).ToList();
                    return View("Details", patientDetails);
                }
                else
                {
                    var patientDetails = (from pA in _context.PatientAccount
                                          join g in _context.Guardians on pA.GuardianId equals g.GuardianId
                                          join s in _context.SchoolLists on pA.SchoolId equals s.SchoolId
                                          join uA in _context.UserAccount on pA.OccId equals uA.UserId
                                          where pA.Name.StartsWith(search) || search == null
                                          select new PatientDetailsViewModel
                                          {
                                              vmPatientTable = pA,
                                              vmGuardian = g,
                                              vmSchools = s,
                                              vmUserAcc = uA
                                          }).ToList();
                    return View("Details", patientDetails);
                }
            }//end if (search != null)
            #endregion

            #region else if (HttpContext.Session.GetString("UserID") != null)
            else if (HttpContext.Session.GetString("UserId") != null)
            {
                DateTime today = DateTime.Today;
                ViewBag.TodaysDate = today;
                var occID = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

                var myNewTasks = (from t in _context.TaskName
                                  join otTask in _context.OtTasks on t.TaskId equals otTask.TaskId
                                  join uA in _context.UserAccount on otTask.OccId equals uA.UserId
                                  join pA in _context.PatientAccount on otTask.PpsNo equals pA.PpsNo
                                  where otTask.OccId.Equals(occID)
                                  select new MyViewModel
                                  {
                                      vmTaskTable = t,
                                      vmUserAcc = uA,
                                      vmTPOT = otTask,
                                      vmPatientTable = pA
                                  }).ToList();
                try
                {
                    if (myNewTasks.FirstOrDefault().vmUserAcc.IsAdmin == true)
                    {
                        ViewBag.Admin = "True";
                        return View(myNewTasks);
                    }
                    else
                    {
                        ViewBag.Admin = "False";
                        return View(myNewTasks);
                    }
                }
                catch (Exception)
                {
                    var myNewTasks1 = (from uA in _context.UserAccount
                                       where uA.UserId.Equals(occID)
                                       select new MyViewModel
                                       {
                                           vmUserAcc = uA,
                                       }).ToList();
                    if (myNewTasks1.FirstOrDefault().vmUserAcc.IsAdmin == true)
                    {
                        ViewBag.Admin = "True";
                        ViewBag.Flag1 = "False";
                        return View(myNewTasks1);
                    }
                    else
                    {
                        ViewBag.Admin = "False";
                        return View(myNewTasks1);
                    }
                }

            } //end if (HttpContext.Session.GetString("UserID") != null)
            #endregion

           
            return RedirectToAction("Index");
        }
        #endregion

        #region Register
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserAccount user)
        {
            if (ModelState.IsValid)
            {
                _context.UserAccount.Add(user);
                _context.SaveChanges();

                ModelState.Clear();
                ViewBag.Message = user.FirstName + " " + user.LastName + " was successfuly registered. ";
            }
            return View();
        }
        #endregion

        #region View All Patients
        public ActionResult ViewAllPatients(int? id)
        {
            var patientDetails = (from pA in _context.PatientAccount
                                  join g in _context.Guardians on pA.GuardianId equals g.GuardianId
                                  join s in _context.SchoolLists on pA.SchoolId equals s.SchoolId
                                  join uA in _context.UserAccount on pA.OccId equals uA.UserId
                                  select new PatientDetailsViewModel
                                  {
                                      vmPatientTable = pA,
                                      vmGuardian = g,
                                      vmSchools = s,
                                      vmUserAcc = uA
                                  }).ToList();

            return View(patientDetails);
        }
        #endregion

        #region Details
        //[Authorize]
        public ActionResult Details(int? id)
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                var patientDetails = (from pA in _context.PatientAccount
                                      join g in _context.Guardians on pA.GuardianId equals g.GuardianId
                                      join s in _context.SchoolLists on pA.SchoolId equals s.SchoolId
                                      join uA in _context.UserAccount on pA.OccId equals uA.UserId
                                      where pA.PpsNo.Equals(id)
                                      select new PatientDetailsViewModel
                                      {
                                          vmPatientTable = pA,
                                          vmGuardian = g,
                                          vmSchools = s,
                                          vmUserAcc = uA
                                      }).ToList();

                return View(patientDetails);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Create A New Student GET
        public IActionResult CreateStudentRecord()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                List<SchoolLists> schoolList = _context.SchoolLists.ToList();
                ViewBag.schoolList = new SelectList(schoolList, "SchoolId", "SchoolName");

                List<UserAccount> OTList = _context.UserAccount.ToList();
                ViewBag.OTList = new SelectList(OTList, "UserId", "UserName");

                return View();
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Create A New Student POST
        [HttpPost]
        public IActionResult CreateStudentRecord(PatientDetailsViewModel student)
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                
                List<SchoolLists> schoolList = _context.SchoolLists.ToList();
                ViewBag.schoolList = new SelectList(schoolList, "SchoolId", "SchoolName");
                var date = DateTime.Now.ToString("dd/MM/yyyy");

                List<UserAccount> OTList = _context.UserAccount.ToList();
                ViewBag.OTList = new SelectList(OTList, "UserId", "UserName");

                if (ModelState.IsValid)
                {
                    _context.Guardians.Add(student.vmGuardian);
                    _context.SaveChanges();

                    var id = _context.Guardians.LastOrDefault();
                    PatientAccount newStudent = new PatientAccount()
                    {
                        SocialSecurityNo = student.vmPatientTable.SocialSecurityNo,
                        DoB = student.vmPatientTable.DoB,
                        Name = student.vmPatientTable.Name,
                        AddressLineOne = student.vmPatientTable.AddressLineOne,
                        City = student.vmPatientTable.City,
                        County = student.vmPatientTable.County,
                        SchoolId = student.vmPatientTable.SchoolId,
                        GuardianId = Convert.ToInt32(id.GuardianId),
                        OccId = student.vmPatientTable.OccId,
                        OpenDate = Convert.ToDateTime(date)
                    };
                    _context.PatientAccount.Add(newStudent);
                    _context.SaveChanges();

                    ModelState.Clear();
                    ViewBag.Message = student.vmPatientTable.Name + " " + " was successfuly added. ";
                }
                return View();
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete Student
        public IActionResult DeletePatient(int? id)
        {
            ViewBag.PatientID = id;

            if (HttpContext.Session.GetString("UserId") != null)
            {
                              
                DateTime today = DateTime.Today;
                ViewBag.TodaysDate = today;
                var occID = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
                var myNewTasks = (from pA in _context.PatientAccount
                                  join gA in _context.Guardians on pA.GuardianId equals gA.GuardianId
                                  join uA in _context.UserAccount on pA.OccId equals uA.UserId
                                  where pA.OccId.Equals(occID)
                                  select new PatientDetailsViewModel
                                  {
                                      vmGuardian = gA,
                                      vmUserAcc = uA,
                                      vmPatientTable = pA
                                  }).ToList();

                return View(myNewTasks);
            }
            return View();
        }
        //[HttpPost]
        public IActionResult DeletePatient1(int? id)
        {
            ViewBag.PatientID = id;

            PatientAccount removePatient = _context.PatientAccount.Where(c => c.PpsNo == id).Single();
            Guardians removeGuardian = _context.Guardians.Where(c => c.GuardianId == removePatient.GuardianId).Single();
            try
            {
                Notes removeNotes = _context.Notes.Where(c => c.PpsNo == removePatient.PpsNo).Single();
                _context.Notes.Remove(removeNotes);
            }
            catch (Exception)
            {

            }

            if (ModelState.IsValid)
            {
                _context.PatientAccount.Remove(removePatient);
                _context.Guardians.Remove(removeGuardian);
                _context.SaveChanges();
                ViewBag.Message = "Patient was successfuly removed. ";
            }

            return RedirectToAction("DeletePatient");
        }
        #endregion

        #region ReassignPatient GET
        public IActionResult ReassignPatient(int id, string search)
        {
            var searchID = Convert.ToInt32(search);
            List<SchoolLists> schoolList = _context.SchoolLists.ToList();

            ViewBag.schoolList = new SelectList(schoolList, "SchoolId", "SchoolName");
            ViewBag.PatientNumber = id;

            List<UserAccount> OTList = _context.UserAccount.ToList();
            ViewBag.OTList = new SelectList(OTList, "UserId", "FirstName");

            
            return PartialView("_ReassignPatient");
        }
        #endregion

        #region ReassignPatient POST
        [HttpPost]
        public IActionResult ReassignPatient(int id, int id2, PatientDetailsViewModel student)
        {
            PatientAccount patient = _context.PatientAccount.Single(p => p.PpsNo == id);
            if (ModelState.IsValid)
            {
                patient.OccId = student.vmPatientTable.OccId;
                _context.PatientAccount.Update((patient));
                _context.SaveChanges();
            }

            return RedirectToAction("Details", new { id = id });
        }
        #endregion

        #region View PatientNotes
        public ActionResult PatientNotes(int? id)
        {
            var vm = new MyViewModel();
            ViewBag.PatId = id;

            var patientDetails = (from pA in _context.PatientAccount
                                  join g in _context.Guardians on pA.GuardianId equals g.GuardianId
                                  join s in _context.SchoolLists on pA.SchoolId equals s.SchoolId
                                  join uA in _context.UserAccount on pA.OccId equals uA.UserId
                                  join nO in _context.Notes on pA.PpsNo equals nO.PpsNo
                                  where pA.PpsNo.Equals(id)
                                  select new PatientDetailsViewModel
                                  {
                                      vmPatientTable = pA,
                                      vmGuardian = g,
                                      vmSchools = s,
                                      vmUserAcc = uA,
                                      vmNoteTable = nO
                                  }).ToList();
            return PartialView("_PatientNotes", patientDetails);
        }
        #endregion

        #region Add Note
        public IActionResult AddNote(int? id)
        {
            ViewBag.PatientID = id;
            return PartialView("_AddNote");
        }
        [HttpPost]
        public IActionResult AddNote(PatientDetailsViewModel student, int id)
        {
            ViewBag.PatientID = id;

            if (ModelState.IsValid)
            {
                Notes newStudent = new Notes()
                {
                    NoteTitle = student.vmNoteTable.NoteTitle,
                    NoteDate = DateTime.Now,
                    Details = student.vmNoteTable.Details,
                    PpsNo = ViewBag.PatientID
                };
                _context.Notes.Add(newStudent);
                _context.SaveChanges();
                ModelState.Clear();
                ViewBag.Message = "Note was successfuly added. ";
            }
            return RedirectToAction("Details", new { id = id });
        }
        #endregion

        #region Add Task
        public IActionResult AddTask(int? id)
        {
            List<UserAccount> OTList = _context.UserAccount.ToList();
            ViewBag.OTList = new SelectList(OTList, "UserId", "FirstName");

            List<Tasks> taskList = _context.TaskName.ToList();
            ViewBag.taskList = new SelectList(taskList, "TaskId", "TaskType");
                        
            return PartialView("_AddATask");
        }
        [HttpPost]
        public IActionResult AddTask(PatientDetailsViewModel student, int id)
        {
            ViewBag.PatientID = id;

            if (ModelState.IsValid)
            {
                OtTasks newTask = new OtTasks()
                {
                    TaskId = student.vmTaskTable.TaskId,
                    OccId = student.vmPatientTable.OccId,
                    PpsNo = ViewBag.PatientID,
                    DueDate = student.vmTaskTable.DueDate,
                    Completed = false
                };

                _context.OtTasks.Add(newTask);
                _context.SaveChanges();
                ModelState.Clear();
                ViewBag.Message = "Note was successfuly added. ";
            }
            return RedirectToAction("Details", new { id = id });
        }
        #endregion

        #region View Task Details
        public ActionResult TaskDetails(int? id, int? id2, DateTime duedate)
        {
            var taskDetails = (from tS in _context.OtTasks
                               join tT in _context.TaskName on tS.TaskId equals tT.TaskId
                               join pA in _context.PatientAccount on tS.PpsNo equals pA.PpsNo
                               where tS.PpsNo.Equals(id) && tS.TaskId.Equals(id2) && tS.DueDate.Equals(duedate)
                               select new PatientDetailsViewModel
                               {
                                   vmTPOT = tS,
                                   vmTaskTable = tT,
                                   vmPatientTable = pA,
                               }).ToList();
            return View(taskDetails);
        }
        #endregion

        #region Complete Task
        [HttpPost]
        public IActionResult CompleteTask(int id, int pid)
        {
            ViewBag.PatientID = id;
            OtTasks otp = _context.OtTasks.FirstOrDefault(ot => ot.OtTaskId == pid && ot.PpsNo == id);
            if (ModelState.IsValid)
            {
                otp.Completed = true;
                _context.OtTasks.Update(otp);
                _context.SaveChanges();
                ViewBag.Message = "Updated. ";
                ModelState.Clear();
            };
            return RedirectToAction("Welcome");
        }
        #endregion

        #region Remove OT
        public IActionResult RemoveOT(int? id)
        {
            var otDetails = (from uA in _context.UserAccount

                             select new PatientDetailsViewModel
                             {
                                 vmUserAcc = uA,
                             }).ToList();

            return View(otDetails);
        }
        public IActionResult RemoveOTNext(int? id)
        {
            var otDetails = (from uA in _context.UserAccount
                             where uA.UserId.Equals(id)
                             select new PatientDetailsViewModel
                             {
                                 vmUserAcc = uA,
                             }).ToList();
            var test = id;

            return View(otDetails);
        }
        public IActionResult RemoveOTNextLast(int? id)
        {
            var otDetails = (from uA in _context.UserAccount
                             where uA.UserId.Equals(id)
                             select new PatientDetailsViewModel
                             {
                                 vmUserAcc = uA,
                             }).ToList();
            var test = id;
            return View(otDetails);
        }
        #endregion

        #region Get Patients for specific therapist with tasks
        public ActionResult Therapist_Patient_Details(int? id, PatientDetailsViewModel student)
        {
            List<UserAccount> OTList2 = _context.UserAccount.ToList();
            ViewBag.OTList2 = new SelectList(OTList2, "UserId", "FirstName");

            var patientDetails = (from pA in _context.PatientAccount
                                  join g in _context.Guardians on pA.GuardianId equals g.GuardianId
                                  join s in _context.SchoolLists on pA.SchoolId equals s.SchoolId
                                  join uA in _context.UserAccount on pA.OccId equals uA.UserId
                                  join otT in _context.OtTasks on pA.PpsNo equals otT.PpsNo
                                  join tsk in _context.TaskName on otT.TaskId equals tsk.TaskId
                                  where pA.OccId.Equals(id) && otT.OccId.Equals(id) && otT.Completed == false
                                  where pA.OccId.Equals(id) && otT.OccId.Equals(id)
                                  select new PatientDetailsViewModel
                                  {
                                      vmPatientTable = pA,
                                      vmGuardian = g,
                                      vmSchools = s,
                                      vmUserAcc = uA,
                                      vmTPOT = otT,
                                      vmTaskTable = tsk
                                  }).ToList();

            ViewBag.noteList = patientDetails;
            ViewBag.OT_Id = id;
            return View(patientDetails);
        }
        #endregion

        #region Get Patients for specific therapist
        public ActionResult Therapist_Patient_Details_For_Deleting(int? id, PatientDetailsViewModel student)
        {
            List<UserAccount> OTList2 = _context.UserAccount.ToList();
            ViewBag.OTList2 = new SelectList(OTList2, "UserId", "FirstName");

            var patientDetails = (from pA in _context.PatientAccount
                                  join g in _context.Guardians on pA.GuardianId equals g.GuardianId
                                  join s in _context.SchoolLists on pA.SchoolId equals s.SchoolId
                                  join uA in _context.UserAccount on pA.OccId equals uA.UserId
                                  where pA.OccId.Equals(id)
                                  select new PatientDetailsViewModel
                                  {
                                      vmPatientTable = pA,
                                      vmGuardian = g,
                                      vmSchools = s,
                                      vmUserAcc = uA
                                  }).ToList();

            ViewBag.noteList = patientDetails;
            ViewBag.OT_Id = id;
            return View(patientDetails);
        }
        #endregion


        #region Error
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
