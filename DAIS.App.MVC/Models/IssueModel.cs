using DAIS.ORM.DTO;
using System;
using System.ComponentModel.DataAnnotations;

namespace DAIS.App.MVC.Models.Issue
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

        [Required(AllowEmptyStrings = true, ErrorMessage ="Type number of remaining hours")]
        [DisplayFormat(DataFormatString = "{0:hh}", ApplyFormatInEditMode = true)]
        public TimeSpan? RemainingTime { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Type number of estimated hours")]
        [DisplayFormat(DataFormatString = "{0:hh\\h}", ConvertEmptyStringToNull = true)]
        public TimeSpan? EstimatedTime { get; set; }
    }

    public class IssueListDetailModel
    {
        public string Summary { get; set; }

        public IssuePriority IssuePriority { get; set; }

        public IssueType IssueType { get; set; }

        public IssueStatus IssueStatus { get; set; }

        public string Description { get; set; }

        public TimeSpan RemainingTime { get; set; }

        public TimeSpan EstimatedTime { get; set; }

        public DateTime Created { get; set; }

        public UserModel CreatedBy { get; set; }

        public UserModel Assignee { get; set; }

    }

    public class IssueListModel
    {

    }
}