using DAIS.ORM.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DAIS.App.MVC.Models
{
    public class IssueCreateModel
    {
        [Required]
        public string Summary { get; set; }

        [Required]
        [Display(Name = "Priority")]
        public IssuePriority IssuePriority { get; set; }

        [Required]
        [Display(Name = "Type")]
        public IssueType IssueType { get; set; }

        [Required]
        [Display(Name = "Status")]
        public IssueStatus IssueStatus { get; set; }

        [Required]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Type number of remaining hours")]
        [DisplayFormat(DataFormatString = "{0:hh}", ApplyFormatInEditMode = true)]
        [Display(Name = "Remaining time")]
        public TimeSpan? RemainingTime { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Type number of estimated hours")]
        [DisplayFormat(DataFormatString = "{0:hh\\h}", ConvertEmptyStringToNull = true)]
        [Display(Name = "Estimated time")]
        public TimeSpan? EstimatedTime { get; set; }
    }

    public class IssueShortDetailModel
    {
        public long Id { get; set; }

        public string Summary { get; set; }

        [Display(Name = "Priority")]
        public IssuePriority IssuePriority { get; set; }

        [Display(Name = "Type")]
        public IssueType IssueType { get; set; }

        [Display(Name = "Status")]
        public IssueStatus IssueStatus { get; set; }

        public string Description { get; set; }

        [Display(Name = "Remaining time")]
        public TimeSpan RemainingTime { get; set; }

        [Display(Name = "Estimated time")]
        public TimeSpan EstimatedTime { get; set; }

        [Display(Name = "Short description")]
        public string DescriptionShort => Description.Length < 20 ? Description : Description.Substring(0, 20);
    }

    public class IssueDetailModel
    {
        public long Id { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        [Display(Name ="Last updated at")]
        public DateTime? LastUpdatedAt { get; set; }

        [Display(Name ="Prority")]
        public IssuePriority IssuePriority { get; set; }

        [Display(Name ="Type")]
        public IssueType IssueType { get; set; }

        [Display(Name = "Status")]
        public IssueStatus IssueStatus { get; set; }

        [Display(Name = "Created by")]
        public UserModel CreatedBy { get; set; }

        public UserModel Assignee { get; set; }

        [Display(Name = "Remaining time")]
        public TimeSpan RemainingTime { get; set; }

        [Display(Name = "Estimated time")]
        public TimeSpan EstimatedTime { get; set; }
    }

    public class IssueEditModel
    {
        public long Id { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        [Display(Name = "Last updated at")]
        public DateTime? LastUpdatedAt { get; set; }

        [Display(Name = "Priority")]
        public IssuePriority IssuePriority { get; set; }

        [Display(Name = "Type")]
        public IssueType IssueType { get; set; }

        [Display(Name = "Status")]
        public IssueStatus IssueStatus { get; set; }

        [Display(Name = "Assignee")]
        public long? AssigneeId { get; set; }
        public UserModel Assignee { get; set; }

        [Editable(false)]
        public UserModel CreatedBy { get; set; }

        public IEnumerable<SelectListItem> AssigneeList { get; set; }

        [Display(Name = "Remaining time")]
        public double RemainingTime { get; set; }

        [Display(Name = "Estimated time")]
        public double EstimatedTime { get; set; }
    }

    public class IssueCloseModel
    {
        [HiddenInput]
        public long IssueId { get; set; }

        [Required]
        public string Reason { get; set; }
    }
}