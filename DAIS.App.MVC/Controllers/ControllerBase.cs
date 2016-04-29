using DAIS.App.MVC.Models;
using DAIS.ORM;
using DAIS.ORM.Framework;
using DAIS.ORM.Repositories;
using System.Web.Mvc;

namespace DAIS.App.MVC.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private readonly IDatabase db = ConnectionManager.Instance.Create();
        protected readonly IssueRepository issueRepository;
        protected readonly IssueTypeRepository issueTypeRepository;
        protected readonly IssueStatusRepository issueStatusRepository;
        protected readonly UserRepository userRepository;
        protected readonly IssueTypeIssueStatusRepository issueTypeIssueStatusRepository;
        protected readonly IssueWorkflowRepository issueWorkflowRepository;
        protected readonly CommentRepository commentRepository;
        protected readonly IssuePriorityRepository issuePriorityRepository;

        public UserModel SelectedUser
        {
            get
            {
                return (UserModel)Session["SelectedUser"];
            }
            set
            {
                Session["SelectedUser"] = value;
            }
        }

        #region FUTURE
#if FUTURE
        // TODO: Use proper IoC
        public ControllerBase(IssueRepository issueRepository,
            IssueTypeRepository issueTypeRepository,
            IssueStatusRepository issueStatusRepository,
            UserRepository userRepository,
            IssueTypeIssueStatusRepository issueTypeIssueStatusRepository,
            IssueWorkflowRepository issueWorkflowRepository,
            CommentRepository commentRepository,
            IssuePriorityRepository issuePriorityRepository)
        {
            this.issueRepository = issueRepository;
            this.issueTypeRepository = issueTypeRepository;
            this.issueStatusRepository = issueStatusRepository;
            this.userRepository = userRepository;
            this.issueTypeIssueStatusRepository = issueTypeIssueStatusRepository;
            this.issueWorkflowRepository = issueWorkflowRepository;
            this.commentRepository = commentRepository;
            this.issuePriorityRepository = issuePriorityRepository;
        }
#endif
        #endregion

        public ControllerBase()
        {
            this.issueRepository = new IssueRepository(db);
            this.issueTypeRepository = new IssueTypeRepository(db);
            this.issueStatusRepository = new IssueStatusRepository(db);
            this.userRepository = new UserRepository(db);
            this.issueTypeIssueStatusRepository = new IssueTypeIssueStatusRepository(db);
            this.issueWorkflowRepository = new IssueWorkflowRepository(db);
            this.commentRepository = new CommentRepository(db);
            this.issuePriorityRepository = new IssuePriorityRepository(db);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            // TODO: Log

            if (filterContext.HttpContext.Request.IsLocal)
                return;

            filterContext.Controller.TempData["Alerts"] = new AlertsListModel(AlertModelFactory.Instance.Create(AlertCode.Error));

            filterContext.HttpContext.Response.RedirectToRoute(new { controller = "Home", action = "Index" });
            filterContext.ExceptionHandled = true;
        }
    }
}