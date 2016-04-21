using DAIS.ORM.DTO;
using DAIS.ORM.Framework;
using DAIS.ORM.Repositories;
using System;
using System.Linq;
using System.Text;
using System.Transactions;
using static System.Console;

namespace DAIS.ConsoleClient
{
    internal class Program : IDisposable
    {
        private readonly IDatabase db;
        private readonly IssueRepository issueRepo;
        private readonly IssueTypeRepository issueTypeRepo;
        private readonly IssueStatusRepository issueStatusRepo;
        private readonly UserRepository userRepo;
        private readonly IssueTypeIssueStatusRepository issueTypeIssueStatusRepo;
        private readonly IssueWorkflowRepository issueWorkflowRepo;
        private readonly CommentRepository commentRepo;
        private readonly IssuePriorityRepository issuePriorityRepo;

        public Program()
        {
            db = ConnectionManager.Instance.Create();
            issueRepo = new IssueRepository(db);
            issueTypeRepo = new IssueTypeRepository(db);
            issueStatusRepo = new IssueStatusRepository(db);
            userRepo = new UserRepository(db);
            issueTypeIssueStatusRepo = new IssueTypeIssueStatusRepository(db);
            issueWorkflowRepo = new IssueWorkflowRepository(db);
            commentRepo = new CommentRepository(db);
            issuePriorityRepo = new IssuePriorityRepository(db);
        }

        public void Dispose() => db.Dispose();

        private void Init()
        {
            TransactionScope transaction = null;
            try
            {
                transaction = new TransactionScope();

                issueStatusRepo.Insert(new IssueStatusDTO { IssueType = IssueStatus.Open });
                issueStatusRepo.Insert(new IssueStatusDTO { IssueType = IssueStatus.InProgress });
                issueStatusRepo.Insert(new IssueStatusDTO { IssueType = IssueStatus.CodeReview });
                issueStatusRepo.Insert(new IssueStatusDTO { IssueType = IssueStatus.Testing });
                issueStatusRepo.Insert(new IssueStatusDTO { IssueType = IssueStatus.Closed });

                issueTypeRepo.Insert(new IssueTypeDTO { IssueType = IssueType.Epic });
                issueTypeRepo.Insert(new IssueTypeDTO { IssueType = IssueType.Task });

                issueTypeIssueStatusRepo.Insert(new IssueTypeIssueStatusDTO { IssueType = IssueType.Epic, IssueStatus = IssueStatus.Open });
                issueTypeIssueStatusRepo.Insert(new IssueTypeIssueStatusDTO { IssueType = IssueType.Epic, IssueStatus = IssueStatus.InProgress });
                issueTypeIssueStatusRepo.Insert(new IssueTypeIssueStatusDTO { IssueType = IssueType.Epic, IssueStatus = IssueStatus.CodeReview });
                issueTypeIssueStatusRepo.Insert(new IssueTypeIssueStatusDTO { IssueType = IssueType.Epic, IssueStatus = IssueStatus.Testing });
                issueTypeIssueStatusRepo.Insert(new IssueTypeIssueStatusDTO { IssueType = IssueType.Epic, IssueStatus = IssueStatus.Closed });
                issueTypeIssueStatusRepo.Insert(new IssueTypeIssueStatusDTO { IssueType = IssueType.Task, IssueStatus = IssueStatus.Open });
                issueTypeIssueStatusRepo.Insert(new IssueTypeIssueStatusDTO { IssueType = IssueType.Task, IssueStatus = IssueStatus.InProgress });
                issueTypeIssueStatusRepo.Insert(new IssueTypeIssueStatusDTO { IssueType = IssueType.Task, IssueStatus = IssueStatus.Closed });

                issuePriorityRepo.Insert(new IssuePriorityDTO { IssuePriority = IssuePriority.Critical });
                issuePriorityRepo.Insert(new IssuePriorityDTO { IssuePriority = IssuePriority.HiPrio });
                issuePriorityRepo.Insert(new IssuePriorityDTO { IssuePriority = IssuePriority.Normal });
                issuePriorityRepo.Insert(new IssuePriorityDTO { IssuePriority = IssuePriority.NotImportant });

                transaction.Complete();
                WriteLine("Seed done");
            }
            catch (Exception ex)
            {
                WriteLine("Init failed (most probably it has been already done)");
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        static void Main(string[] args)
        {
            using (Program p = new Program())
            {
                // fills database with seed data
                //p.Init();

                // INSERT USER
                UserDTO user = new UserDTO
                {
                    FirstName = "Jmeno",
                    Surname = "Prijmeni",
                    Password = Encoding.ASCII.GetBytes("123456"),
                    Email = "jmeno.prijmeni@domena.x",
                    UserName = "username",
                };
                bool insertedUser = p.userRepo.Insert(user);

                // INSERT ISSUE
                IssueDTO issue = new IssueDTO
                {
                    CreatedBy = user.Id,
                    Summary = "Summary ...",
                    Description = "Description ...",
                    Created = DateTime.Now.AddDays(-10),
                    LastUpdatedAt = null,
                    RemainingTimeTicks = TimeSpan.FromHours(2).Ticks,
                    EstimatedTime = TimeSpan.FromHours(2).Ticks,
                    IssuePriorityId = 1,
                    IssueStatusId = 1,
                    IssueTypeId = 1,
                };
                bool insertedIssue = p.issueRepo.Insert(issue);

                // INSERT WORKFLOW
                IssueWorkflowDTO workflow = new IssueWorkflowDTO
                {
                    CreatedAt = DateTime.Now,
                    IssueId = issue.Id,
                    UserId = user.Id,
                    IssueStatusId = 2,
                    TimeSpent = TimeSpan.FromHours(1.5d),
                };
                bool insertedWorkflow = p.issueWorkflowRepo.Insert(workflow);

                // INSERT COMMENT
                CommentDTO comment = new CommentDTO
                {
                    CreatedAt = DateTime.Now,
                    IssueId = issue.Id,
                    UserId = user.Id,
                    Text = "Some comment's text",
                };
                bool insertedComment = p.commentRepo.Insert(comment);

                // RPC
                var mostActiveUsers = p.userRepo.MostActiveUsersForLastNDays(2).ToArray(); // fnc1
                var nonOpenIssues = p.issueRepo.NonOpenIssuesForLastNDays(2).ToArray(); // fnc2
                var issueUpdated = p.issueRepo.UpdateIssueStatus(issue.Id, user.Id, "Testing", "Ready to testing.", TimeSpan.FromHours(7)); // fnc3
                var loggedWOrk = p.issueRepo.LogWork(issue.Id, user.Id, TimeSpan.FromMinutes(1), "some test comment"); // fnc4
                var closed = p.issueRepo.CloseIssues(issue.Id, user.Id, "Its already fixed."); // fnc5

                // Selects
                var allComments = p.commentRepo.Select().ToArray();
                var allIssuePriorities = p.issuePriorityRepo.Select().ToArray();
                var allIssues = p.issueRepo.Select().ToArray();
                var allIssueStatuses = p.issueStatusRepo.Select().ToArray();
                var allIssueTypes = p.issueTypeRepo.Select().ToArray();
                var allIssueTypeIssueStatuses = p.issueTypeIssueStatusRepo.Select().ToArray();
                var allIssueWorkflows = p.issueWorkflowRepo.Select().ToArray();
                var allUsers = p.userRepo.Select().ToArray();

                // DELETE INSERTED ROWS
                // Note#1: Its just a soft delete
                // Note#2: Except that rows which were created by internal procedures/functions
                bool deleted = new bool[]{ p.commentRepo.Delete(comment.Id),
                p.issueWorkflowRepo.Delete(workflow.Id),
                p.issueRepo.Delete(issue.Id),
                p.userRepo.Delete(user.Id) }.All(t => t);
            }
        }
    }
}