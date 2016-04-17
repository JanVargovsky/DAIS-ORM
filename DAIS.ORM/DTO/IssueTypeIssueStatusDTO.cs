using DAIS.ORM.Framework;
using System;

namespace DAIS.ORM.DTO
{
    [TableName("issue_type_issue_status")]
    public class IssueTypeIssueStatusDTO
    {
        [Column("issue_status_id", ColumnType.PrimaryKey | ColumnType.ForeignKey)]
        public long IssueStatusId { get; set; }

        [Column("issue_type_id", ColumnType.PrimaryKey | ColumnType.ForeignKey)]
        public long IssueTypeId { get; set; }

        public IssueStatus IssueStatus
        {
            get
            {
                return (IssueStatus)Enum.ToObject(typeof(IssueStatus), IssueStatusId);
            }
            set
            {
                IssueStatusId = (long)value;
            }
        }

        public IssueType IssueType
        {
            get
            {
                return (IssueType)Enum.ToObject(typeof(IssueType), IssueTypeId);
            }
            set
            {
                IssueTypeId = (long)value;
            }
        }
    }
}
