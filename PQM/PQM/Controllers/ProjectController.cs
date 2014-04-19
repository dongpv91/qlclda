using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PQM.Models;
using PQM.ViewModels;

namespace PQM.Controllers
{
    public class ProjectController : Controller
    {
        private project_infoEntities db = new project_infoEntities();

        // GET: /Project/
        public ActionResult Index()
        {
            var projects = db.projects.Include(p => p.customer).Include(p => p.end_project).Include(p => p.type_project).Include(p => p.quality);
            return View(projects.ToList());
        }

        // GET: /Project/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: /Project/Create
        public ActionResult Create()
        {
            var project = new project();
            project.teams = new List<team>();
            PopulateAssignedTeamData(project);

            ViewBag.id_customer = new SelectList(db.customers, "id", "company");
            ViewBag.id = new SelectList(db.end_project, "id", "difficulty");
            ViewBag.id_type_project = new SelectList(db.type_project, "id", "name");
            ViewBag.id = new SelectList(db.qualities, "id", "id");
            return View();
        }

        // POST: /Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_customer,id_type_project,name_project,expected_start_date,expected_end_date,actual_start_date,actual_end_date,technology,purpose,estimator,scale_estimator,contract_value,pm,brse,current_cost,estimated_cost,software,hardware")] project project, string[] selectedTeams)
        {
            if (selectedTeams != null)
            {
                project.teams = new List<team>();
                foreach (var team in selectedTeams)
                {
                    var teamToAdd = db.teams.Find(int.Parse(team));
                    project.teams.Add(teamToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                db.projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_customer = new SelectList(db.customers, "id", "company", project.id_customer);
            ViewBag.id = new SelectList(db.end_project, "id", "difficulty", project.id);
            ViewBag.id_type_project = new SelectList(db.type_project, "id", "name", project.id_type_project);
            ViewBag.id = new SelectList(db.qualities, "id", "id", project.id);

            PopulateAssignedTeamData(project);
            return View(project);
        }

        // GET: /Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //project project = db.projects.Find(id);

            project project = db.projects.Include(i => i.teams).Where(i => i.id == id).Single();
            PopulateAssignedTeamData(project);

            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_customer = new SelectList(db.customers, "id", "company", project.id_customer);
            ViewBag.id = new SelectList(db.end_project, "id", "difficulty", project.id);
            ViewBag.id_type_project = new SelectList(db.type_project, "id", "name", project.id_type_project);
            ViewBag.id = new SelectList(db.qualities, "id", "id", project.id);
            return View(project);
        }

        // POST: /Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedTeams)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var projectToUpdate = db.projects.Include(i => i.teams).Where(i => i.id == id).Single();

            if (TryUpdateModel(projectToUpdate, ""))
            {
                try
                {
                    UpdateProjectTeams(selectedTeams, projectToUpdate);

                    db.Entry(projectToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedTeamData(projectToUpdate);
            return View(projectToUpdate);
        }
        
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,id_customer,id_type_project,name_project,expected_start_date,expected_end_date,actual_start_date,actual_end_date,technology,purpose,estimator,scale_estimator,contract_value,pm,brse,current_cost,estimated_cost,software,hardware")] project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_customer = new SelectList(db.customers, "id", "company", project.id_customer);
            ViewBag.id = new SelectList(db.end_project, "id", "difficulty", project.id);
            ViewBag.id_type_project = new SelectList(db.type_project, "id", "name", project.id_type_project);
            ViewBag.id = new SelectList(db.qualities, "id", "id", project.id);
            return View(project);
        }
        */

        // GET: /Project/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: /Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            project project = db.projects.Find(id);
            db.projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Gán dữ liệu vào bảng trung gian Team_Project
        private void PopulateAssignedTeamData(project project)
        {
            var allTeams = db.teams;
            var projectTeams = new HashSet<long>(project.teams.Select(c => c.id));
            var viewModel = new List<AssignedTeamData>();
            foreach (var team in allTeams)
            {
                viewModel.Add(new AssignedTeamData
                {
                    TeamID = team.id,
                    Name = team.name,
                    Assigned = projectTeams.Contains(team.id)
                });
            }
            ViewBag.Teams = viewModel;
        }

        // Update dữ liệu trong bảng Team_Project
        private void UpdateProjectTeams(string[] selectedTeams, project projectToUpdate)
        {
            if (selectedTeams == null)
            {
                projectToUpdate.teams = new List<team>();
                return;
            }

            var selectedTeamsHS = new HashSet<string>(selectedTeams);
            var projectTeams = new HashSet<long>(projectToUpdate.teams.Select(c => c.id));

            foreach (var team in db.teams)
            {
                if (selectedTeamsHS.Contains(team.id.ToString()))
                {
                    if (!projectTeams.Contains(team.id))
                    {
                        projectToUpdate.teams.Add(team);
                    }
                }
                else
                {
                    if (projectTeams.Contains(team.id))
                    {
                        projectToUpdate.teams.Remove(team);
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
