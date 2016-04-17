using DAIS.ORM;
using DAIS.ORM.DTO;
using DAIS.ORM.Framework;
using DAIS.ORM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace DAIS.ConsoleClient
{
    internal class Program
    {
        private static IDatabase db = new Database(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\JANVA\DOCUMENTS\DAIS-VAR0065.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        private static IssueRepository issueRepo = new IssueRepository(db);
        private static IssueTypeRepository issueTypeRepo = new IssueTypeRepository(db);
        private static IssueStatusRepository issueStatusRepo = new IssueStatusRepository(db);
        private static UserRepository userRepo = new UserRepository(db);
        private static IssueTypeIssueStatusRepository issueTypeIssueStatusRepo = new IssueTypeIssueStatusRepository(db);
        private static IssueWorkflowRepository issueWorkflowRepo = new IssueWorkflowRepository(db);
        private static CommentRepository commentRepo = new CommentRepository(db);


        private static void IssueInsert()
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

        private static void InsertIssueType()
        {
            IssueTypeDTO issueType = new IssueTypeDTO
            {
                Id = 2,
                IsDeleted = false,
                Name = "Epic",
            };

            issueTypeRepo.Insert(issueType);
        }

        private static void InsertStatusType()
        {
            IssueStatusDTO issueType = new IssueStatusDTO
            {
                IsDeleted = false,
                IssueType = IssueStatus.Testing,
            };

            issueStatusRepo.Insert(issueType);
        }

        private static void InsertUser()
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

        private static void InsertIssueTypeIssueStatus()
        {
            IssueTypeIssueStatusDTO typeStatus = new IssueTypeIssueStatusDTO
            {
                IssueType = IssueType.Task,
                IssueStatus = IssueStatus.Testing,
            };


            issueTypeIssueStatusRepo.Insert(typeStatus);
        }

        private static void InsertIssueWorkflow()
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

        private static void DeleteWorkflow()
        {
            issueWorkflowRepo.Delete(1);
        }

        private static void InsertComment()
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

        private static void DeleteComment()
        {
            commentRepo.Delete(1);
        }

        private static void UpdateComment()
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

        private static void PrintComment(params CommentDTO[] comments)
        {
            foreach (var c in comments)
                WriteLine($"Id={c.Id}, UserId={c.UserId}, CreatedAt={c.CreatedAt}, Text={c.Text}, IssueId={c.IssueId}");
        }

        private static void SelectComment()
        {
            CommentDTO c = commentRepo.Select(1);
            WriteLine($"Id={c.Id}, UserId={c.UserId}, CreatedAt={c.CreatedAt}, Text={c.Text}, IssueId={c.IssueId}");
        }

        private static void SelectComments()
        {
            var comments = commentRepo.Select();
            foreach (var c in comments)
                WriteLine($"Id={c.Id}, UserId={c.UserId}, CreatedAt={c.CreatedAt}, Text={c.Text}, IssueId={c.IssueId}");
        }

        private static void SelectStatuses()
        {
            var statuses = issueStatusRepo.Select();
            foreach (var status in statuses)
                WriteLine($"{status.Id} - {status.IssueType}");
        }

        static void Main(string[] args)
        {
            //InsertIssueType();
            //InsertUser();
            //InsertStatusType();
            //InsertIssueTypeIssueStatus();
            //IssueInsert();
            //InsertIssueWorkflow();
            //InsertComment();

            //InsertComment();
            //DeleteComment();
            //DeleteWorkflow();

            //UpdateComment();
            //SelectComment();
            //SelectComments();
            SelectStatuses();
        }
    }
}