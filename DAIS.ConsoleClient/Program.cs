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
        static IDatabase db;

        private static void IssueInsert()
        {
            // (1, 'summary ...', 'description ...', SYSDATETIME(), null, 1000, 1000, 0, null, 1, null, 1, 1, 1);
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

            IssueRepository repo = new IssueRepository(db);
            repo.Insert(issue);
        }

        private static void InsertIssueType()
        {
            IssueTypeDTO issueType = new IssueTypeDTO
            {
                Id = 2,
                IsDeleted = false,
                Name = "Epic",
            };
            IssueTypeRepository repo = new IssueTypeRepository(db);
            repo.Insert(issueType);
        }

        private static void InsertStatusType()
        {
            IssueStatusDTO issueType = new IssueStatusDTO
            {
                IsDeleted = false,
                IssueType = IssueStatus.Testing,
            };
            IssueStatusRepository repo = new IssueStatusRepository(db);
            repo.Insert(issueType);
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
            UserRepository repo = new UserRepository(db);
            repo.Insert(user);
        }

        private static void InsertIssueTypeIssueStatus()
        {
            IssueTypeIssueStatusDTO typeStatus = new IssueTypeIssueStatusDTO
            {
                IssueType = IssueType.Task,
                IssueStatus = IssueStatus.Testing,
            };

            IssueTypeIssueStatusRepository repo = new IssueTypeIssueStatusRepository(db);
            repo.Insert(typeStatus);
        }

        private static void InsertIssueWorkflow()
        {
            IssueWorkflowDTO workflow = new IssueWorkflowDTO
            {
                Id = 1,
                CreatedAt = DateTime.Now,
                IssueId = 1,
                IssueStatusId = 2,
                TimeSpent = TimeSpan.FromHours(1.5d),
                UserId = 1,
            };

            IssueWorkflowRepository repo = new IssueWorkflowRepository(db);
            repo.Insert(workflow);
        }

        private static void InsertComment()
        {
            CommentDTO comment = new CommentDTO
            {
                CreatedAt = DateTime.Now,
                Id = 1,
                IsDeleted = false,
                IssueId = 1,
                Text = "Some comment's text",
                UserId = 1,
                CommentId = null,
            };

            CommentRepository repo = new CommentRepository(db);
            repo.Insert(comment);
        }

        static void Main(string[] args)
        {
            db = new Database(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\JANVA\DOCUMENTS\DAIS-VAR0065.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            // INSERTS
            //InsertIssueType();
            //InsertUser();
            //InsertStatusType();
            //InsertIssueTypeIssueStatus();
            //IssueInsert();
            //InsertIssueWorkflow();
            //InsertComment();

            // Deletes
            
        }
    }
}