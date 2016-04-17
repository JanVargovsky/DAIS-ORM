using DAIS.ORM;
using DAIS.ORM.DTO;
using DAIS.ORM.Framework;
using DAIS.ORM.Repositories;
using System;
using System.Linq;
using System.Text;

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

        public Program()
        {
            db = new Database(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\JANVA\DOCUMENTS\DAIS-VAR0065.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            issueRepo = new IssueRepository(db);
            issueTypeRepo = new IssueTypeRepository(db);
            issueStatusRepo = new IssueStatusRepository(db);
            userRepo = new UserRepository(db);
            issueTypeIssueStatusRepo = new IssueTypeIssueStatusRepository(db);
            issueWorkflowRepo = new IssueWorkflowRepository(db);
            commentRepo = new CommentRepository(db);
        }

        public void Dispose() => db.Dispose();

        private void IssueInsert()
        {
            IssueDTO issue = new IssueDTO
            {
                Id = 2,
                Summary = "Summary ...",
                Description = "Description ...",
                Created = DateTime.Now,
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
                Id = 2,
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
                Id = 2,
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
                Id = 2,
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
                Id = 2,
                IsDeleted = false,
                IssueId = 1,
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

        static void Main(string[] args)
        {
            using (Program p = new Program())
            {
                //p.InsertIssueType();
                //p.InsertUser();
                //p.InsertStatusType();
                //p.InsertIssueTypeIssueStatus();
                //p.IssueInsert();
                //p.InsertIssueWorkflow();
                //p.InsertComment();

                //p.InsertComment();
                //p.DeleteComment();
                //p.DeleteWorkflow();

                //p.UpdateComment();
                //p.SelectComment();
                //p.SelectComments();
                //p.SelectStatuses();

                //var users = p.userRepo.Select().ToArray();
            }
        }
    }
}