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

        private void IssueInsert()
        {
            IssueDTO issue = new IssueDTO
            {
                Summary = "Summary #3 ...",
                Description = "Description #3 ...",
                Created = DateTime.Now.AddDays(10),
                LastUpdatedAt = null,
                RemainingTimeTicks = TimeSpan.FromMinutes(2).Ticks,
                EstimatedTime = TimeSpan.FromMinutes(2).Ticks,
                IsDeleted = false,
                IssueId = null,
                IssuePriorityId = 1,
                IssueStatusId = 1,
                IssueTypeId = 1,
                Assignee = null,
                CreatedBy = 1,
            };

            issueRepo.Insert(issue);
        }

        private void InsertIssueType()
        {
            IssueTypeDTO issueType = new IssueTypeDTO
            {
                IsDeleted = false,
                Name = "Epic",
            };

            issueTypeRepo.Insert(issueType);
        }

        private void InsertStatusType()
        {
            IssueStatusDTO issueType = new IssueStatusDTO
            {
                IsDeleted = false,
                IssueType = IssueStatus.Testing,
            };

            issueStatusRepo.Insert(issueType);
        }

        private void InsertUser()
        {
            UserDTO user = new UserDTO
            {
                FirstName = "Jmeno",
                Surname = "Prijmeni",
                IsDeleted = false,
                Password = Encoding.ASCII.GetBytes("123456"),
                ShowEmail = false,
                Email = "jmeno.prijmeni@domena.x",
                UserName = "username",
            };

            userRepo.Insert(user);
        }

        private void InsertIssueTypeIssueStatus()
        {
            IssueTypeIssueStatusDTO typeStatus = new IssueTypeIssueStatusDTO
            {
                IssueType = IssueType.Task,
                IssueStatus = IssueStatus.Testing,
            };


            issueTypeIssueStatusRepo.Insert(typeStatus);
        }

        private void InsertIssueWorkflow()
        {
            IssueWorkflowDTO workflow = new IssueWorkflowDTO
            {
                CreatedAt = DateTime.Now,
                IssueId = 1,
                IssueStatusId = 2,
                TimeSpent = TimeSpan.FromHours(1.5d),
                UserId = 1,
            };


            issueWorkflowRepo.Insert(workflow);
        }

        private void DeleteWorkflow()
        {
            issueWorkflowRepo.Delete(1);
        }

        private void InsertComment()
        {
            CommentDTO comment = new CommentDTO
            {
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                IssueId = 4,
                Text = "Some comment's text",
                UserId = 1,
                CommentId = null,
            };


            commentRepo.Insert(comment);
        }

        private void DeleteComment()
        {
            commentRepo.Delete(1);
        }

        private void UpdateComment()
        {
            CommentDTO comment = new CommentDTO
            {
                CreatedAt = DateTime.Now,
                Id = 1,
                IsDeleted = false,
                IssueId = 1,
                Text = "Some comment's text - updated at " + DateTime.Now.ToShortTimeString(),
                UserId = 1,
                CommentId = null,
            };
            commentRepo.Update(comment);
        }

        private void PrintComment(params CommentDTO[] comments)
        {
            foreach (var c in comments)
                WriteLine($"Id={c.Id}, UserId={c.UserId}, CreatedAt={c.CreatedAt}, Text={c.Text}, IssueId={c.IssueId}");
        }

        private void SelectComment()
        {
            CommentDTO c = commentRepo.Select(1);
            WriteLine($"Id={c.Id}, UserId={c.UserId}, CreatedAt={c.CreatedAt}, Text={c.Text}, IssueId={c.IssueId}");
        }

        private void SelectComments()
        {
            var comments = commentRepo.Select();
            foreach (var c in comments)
                WriteLine($"Id={c.Id}, UserId={c.UserId}, CreatedAt={c.CreatedAt}, Text={c.Text}, IssueId={c.IssueId}");
        }

        private void SelectStatuses()
        {
            var statuses = issueStatusRepo.Select();
            foreach (var status in statuses)
                WriteLine($"{status.Id} - {status.IssueType}");
        }

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
                // INIT
                p.Init();

                //p.InsertUser();
                //p.IssueInsert();
                //p.InsertComment();

                //p.InsertComment();
                //p.DeleteComment();
                //p.DeleteWorkflow();

                //p.UpdateComment();
                //p.SelectComment();
                //p.SelectComments();
                //p.SelectStatuses();

                //var users = p.userRepo.Select().ToArray();
                //var users = p.userRepo.MostActiveUsersForLastNDays(2).ToArray();

                //var issueBefore = p.issueRepo.Select(4);
                //bool updated = p.issueRepo.UpdateIssueStatus(4,1,"Testing", "Ready to testing.", TimeSpan.FromHours(7));
                //var issueAfter = p.issueRepo.Select(4);
                //var lastComment = p.commentRepo.Select().Last();
                //var lastWorkflow = p.issueWorkflowRepo.Select().Last();

                //bool logged = p.issueRepo.LogWork(7, 1, TimeSpan.FromMinutes(1));
                //bool logged2 = p.issueRepo.LogWork(7, 1, TimeSpan.FromMinutes(1), "some test comment");


                //for (int i = 0; i < 3; i++)
                //{
                //    IssueDTO issue = new IssueDTO
                //    {
                //        IssueId = 12 + i,
                //        Summary = "test issue",
                //        Description = "test issue",
                //        Created = DateTime.Now.AddDays(10),
                //        LastUpdatedAt = null,
                //        RemainingTimeTicks = TimeSpan.FromMinutes(2).Ticks,
                //        EstimatedTime = TimeSpan.FromMinutes(2).Ticks,
                //        IsDeleted = false,
                //        IssuePriorityId = 1,
                //        IssueStatusId = 1,
                //        IssueTypeId = 1,
                //        Assignee = null,
                //        CreatedBy = 1,
                //    };

                //    p.issueRepo.Insert(issue);
                //}

                p.issueRepo.CloseIssues(12, 1, "Its already fixed.");
            }
        }
    }
}