using System;
using System.Web.Mvc;
using DAIS.ORM.DTO;
using DAIS.App.MVC.Models.Issue;
using DAIS.App.MVC.Models;

namespace DAIS.App.MVC.Controllers
{
    public class IssueController : ControllerBase
    {
        // GET: Issues/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(nameof(Create));
        }

        [HttpPost]
        public ActionResult Create(IssueCreateModel model)
        {
            if(SelectedUser == null)
            {
                TempData["Alerts"] = new AlertsListModel(AlertModelFactory.Instance.Create(AlertCode.NotSelectedUser));
                return View(nameof(Create), model);
            }

            try
            {
                if (!ModelState.IsValid || SelectedUser == null)
                {
                    TempData["Alerts"] = new AlertsListModel(AlertModelFactory.Instance.Create(AlertCode.InvalidFormData));
                    return View(nameof(Create), model);
                }

                IssueDTO issue = new IssueDTO
                {
                    CreatedBy = SelectedUser.Id,
                    Created = DateTime.Now,
                    Summary = model.Summary,
                    Description = model.Description,
                    IssuePriorityId = (long)model.IssuePriority,
                    IssueTypeId = (long)model.IssueType,
                    IssueStatusId = (long)model.IssueStatus,
                    RemainingTime = model.RemainingTime ?? TimeSpan.Zero,
                    EstimatedTime = model.EstimatedTime ?? TimeSpan.Zero,
                };

                issueRepository.Insert(issue);

                TempData["Alerts"] = new AlertsListModel(AlertModelFactory.Instance.Create(AlertCode.Created));
                return RedirectToAction(nameof(Create));
            }
            catch (Exception ex)
            {
                // TODO: Log

                TempData["Alerts"] = new AlertsListModel(AlertModelFactory.Instance.Create(AlertCode.Error));
                return View(nameof(Create), model);
            }
        }

        /*
        
// GET: Issues
        public ActionResult Index()
        {
            return View(db.IssueDTOes.ToList());
        }

        // GET: Issues/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueDTO issueDTO = db.IssueDTOes.Find(id);
            if (issueDTO == null)
            {
                return HttpNotFound();
            }
            return View(issueDTO);
        }

        // POST: Issues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Summary,Description,Created,LastUpdatedAt,RemainingTimeTicks,EstimatedTime,IsDeleted,IssueId,CreatedBy,Assignee,IssueTypeId,IssueStatusId,IssuePriorityId,TimeRemaining")] IssueDTO issueDTO)
        {
            if (ModelState.IsValid)
            {
                db.IssueDTOes.Add(issueDTO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(issueDTO);
        }

        // GET: Issues/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueDTO issueDTO = db.IssueDTOes.Find(id);
            if (issueDTO == null)
            {
                return HttpNotFound();
            }
            return View(issueDTO);
        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Summary,Description,Created,LastUpdatedAt,RemainingTimeTicks,EstimatedTime,IsDeleted,IssueId,CreatedBy,Assignee,IssueTypeId,IssueStatusId,IssuePriorityId,TimeRemaining")] IssueDTO issueDTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(issueDTO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(issueDTO);
        }

        // GET: Issues/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueDTO issueDTO = db.IssueDTOes.Find(id);
            if (issueDTO == null)
            {
                return HttpNotFound();
            }
            return View(issueDTO);
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            IssueDTO issueDTO = db.IssueDTOes.Find(id);
            db.IssueDTOes.Remove(issueDTO);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

            */
    }
}
