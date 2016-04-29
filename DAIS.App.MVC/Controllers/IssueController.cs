using System;
using System.Web.Mvc;
using DAIS.ORM.DTO;
using DAIS.App.MVC.Models;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;

namespace DAIS.App.MVC.Controllers
{
    public class IssueController : ControllerBase
    {
        // GET: Issues/Create
        [HttpGet]
        [Route("Issue/Create")]
        public ActionResult IssueCreate()
        {
            return View(nameof(IssueCreate));
        }

        [HttpPost]
        [Route("Issue/Create")]
        [ValidateAntiForgeryToken]
        public ActionResult IssueCreate(IssueCreateModel model)
        {
            if (SelectedUser == null)
            {
                TempData["Alerts"] = new AlertsListModel(AlertModelFactory.Instance.Create(AlertCode.NotSelectedUser));
                return View(nameof(IssueCreate), model);
            }

            try
            {
                if (!ModelState.IsValid || SelectedUser == null)
                {
                    TempData["Alerts"] = new AlertsListModel(AlertModelFactory.Instance.Create(AlertCode.InvalidFormData));
                    return View(nameof(IssueCreate), model);
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
                return RedirectToAction(nameof(IssueCreate));
            }
            catch (Exception ex)
            {
                // TODO: Log

                ViewBag.Alerts = new AlertsListModel(AlertModelFactory.Instance.Create(AlertCode.Error));
                return View(nameof(IssueCreate), model);
            }
        }

        [Route("Issue/List")]
        public ActionResult IssuesList()
        {
            var issues = issueRepository.Select().OrderBy(i => i.LastUpdatedAt).Select(i => new IssueShortDetailModel
            {
                Id = i.Id,
                Description = i.Description,
                EstimatedTime = i.EstimatedTime,
                IssuePriority = i.IssuePriority,
                IssueType = i.IssueType,
                IssueStatus = i.IssueStatus,
                RemainingTime = i.RemainingTime,
                Summary = i.Summary,
            });

            return View(nameof(IssuesList), issues);
        }

        [Route("Issue/Delete/{id}")]
        public ActionResult IssueDelete(long id)
        {
            try
            {
                if (issueRepository.Delete(id))
                    ViewBag.Alerts = new AlertsListModel(AlertModelFactory.Instance.Create(AlertCode.Deleted));
                else
                    ViewBag.Alerts = new AlertsListModel(AlertModelFactory.Instance.Create(AlertCode.Error));
            }
            catch (Exception ex)
            {
                ViewBag.Alerts = new AlertsListModel(AlertModelFactory.Instance.Create(AlertCode.Error));
            }

            return IssuesList();
        }


        [Route("Issue/Detail/{id}")]
        public ActionResult IssueDetail(long id)
        {
            var issueDto = issueRepository.Select(id);
            var issue = Mapper.Map<IssueDTO, IssueDetailModel>(issueDto);

            issue.CreatedBy = Mapper.Map<UserDTO, UserModel>(userRepository.Select(issueDto.CreatedBy));
            if (issueDto.Assignee.HasValue)
                issue.Assignee = Mapper.Map<UserDTO, UserModel>(userRepository.Select(issueDto.Assignee));

            return View(nameof(IssueDetail), issue);
        }

        [Route("Issue/Edit/{id}")]
        public ActionResult IssueEdit(long id)
        {
            var issueDto = issueRepository.Select(id);
            var issue = Mapper.Map<IssueDTO, IssueEditModel>(issueDto);

            issue.CreatedBy = Mapper.Map<UserDTO, UserModel>(userRepository.Select(issueDto.CreatedBy));
            if (issueDto.Assignee.HasValue)
                issue.Assignee = Mapper.Map<UserDTO, UserModel>(userRepository.Select(issueDto.Assignee));
            issue.AssigneeList = new List<SelectListItem>(userRepository.Select().Select(u => new SelectListItem
            {
                Text = $"{u.FirstName} {u.Surname}",
                Value = u.Id.ToString(),
                Selected = u.Id == issueDto?.Assignee
            }));

            return View(nameof(IssueEdit), issue);
        }

        [Route("Issue/Edit/{id}")]
        [HttpPost]
        public ActionResult IssueEdit(IssueEditModel issue)
        {
            try
            {
                var issueDto = issueRepository.Select(issue.Id);
                issueDto.Id = issue.Id;
                issueDto.Assignee = issue.AssigneeId;
                issueDto.Description = issue.Description;
                issueDto.Summary = issue.Summary;
                issueDto.EstimatedTime = TimeSpan.FromHours(issue.EstimatedTime);
                issueDto.RemainingTime = TimeSpan.FromHours(issue.RemainingTime);
                issueDto.IssuePriority = issue.IssuePriority;
                issueDto.IssueType = issue.IssueType;
                issueDto.IssueStatus = issue.IssueStatus;
                issueDto.LastUpdatedAt = DateTime.Now;

                issueRepository.Update(issueDto);

                TempData["Alerts"] = new AlertsListModel(AlertModelFactory.Instance.Create(AlertCode.Edited));
                return RedirectToAction(nameof(IssuesList));
            }
            catch (Exception ex)
            {
                ViewBag.Alerts = new AlertsListModel(AlertModelFactory.Instance.Create(AlertCode.InvalidFormData));
                return IssueEdit(issue.Id);
            }
        }
    }
}
