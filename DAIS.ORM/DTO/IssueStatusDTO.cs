using System;
using DAIS.ORM.Framework;

namespace DAIS.ORM.DTO
{

    public enum IssueStatus
    {
        Open = 1,
        InProgress = 2,
        Closed = 3,
        Testing = 4,
    }

    [TableName("issue_status")]
    public class IssueStatusDTO
    {
        [Column("id", ColumnType.PrimaryKey)]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("deleted", DeleteIndicator = true)]
        public bool IsDeleted { get; set; }

        public IssueStatus IssueType
        {
            get { return (IssueStatus)Enum.ToObject(typeof(IssueStatus), Id); }
            set
            {
                Id = (long)value;
                switch (value)
                {
                    case IssueStatus.Open:
                        Name = "Open";
                        break;
                    case IssueStatus.InProgress:
                        Name = "In Progress";
                        break;
                    case IssueStatus.Closed:
                        Name = "Closed";
                        break;
                    case IssueStatus.Testing:
                        Name = "Testing";
                        break;
                    default:
                        Name = "Not defined";
                        break;
                }
            }
        }
    }
}