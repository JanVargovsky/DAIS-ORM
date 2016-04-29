using DAIS.ORM.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DAIS.App.MVC.Models
{
    public class IssueCreateModel
    {
        [Required]
        public string Summary { get; set; }

        [Required]
        public IssuePriority IssuePriority { get; set; }

        [Required]
        public IssueType IssueType { get; set; }

        [Required]
        public IssueStatus IssueStatus { get; set; }

        [Required]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Type number of remaining hours")]
        [DisplayFormat(DataFormatString = "{0:hh}", ApplyFormatInEditMode = true)]
        public TimeSpan? RemainingTime { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Type number of estimated hours")]
        [DisplayFormat(DataFormatString = "{0:hh\\h}", ConvertEmptyStringToNull = true)]
        public TimeSpan? EstimatedTime { get; set; }
    }

    public class IssueShortDetailModel
    {
        public long Id { get; set; }

        public string Summary { get; set; }

        public IssuePriority IssuePriority { get; set; }

        public IssueType IssueType { get; set; }

        public IssueStatus IssueStatus { get; set; }

        public string Description { get; set; }

        public TimeSpan RemainingTime { get; set; }

        public TimeSpan EstimatedTime { get; set; }

        public string DescriptionShort => Description.Length < 20 ? Description : Description.Substring(0, 20);
    }

    public class IssueDetailModel
    {
        public long Id { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public IssuePriority IssuePriority { get; set; }

        public IssueType IssueType { get; set; }

        public IssueStatus IssueStatus { get; set; }

        public UserModel CreatedBy { get; set; }

        public UserModel Assignee { get; set; }

        public TimeSpan RemainingTime { get; set; }

        public TimeSpan EstimatedTime { get; set; }
    }

    public class IssueEditModel
    {
        public long Id { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public IssuePriority IssuePriority { get; set; }

        public IssueType IssueType { get; set; }

        public IssueStatus IssueStatus { get; set; }

        public long? AssigneeId { get; set; }
        public UserModel Assignee { get; set; }

        [Editable(false)]
        public UserModel CreatedBy { get; set; }

        public IEnumerable<SelectListItem> AssigneeList { get; set; }

        public double RemainingTime { get; set; }

        public double EstimatedTime { get; set; }
    }
}