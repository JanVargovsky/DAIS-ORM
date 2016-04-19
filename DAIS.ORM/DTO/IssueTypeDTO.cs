using DAIS.ORM.Framework;
using System;

namespace DAIS.ORM.DTO
{
    public enum IssueType
    {
        Epic = 1,
        Task = 2,
    }

    [TableName("issue_type")]
    public class IssueTypeDTO
    {
        [Column("id", ColumnType.PrimaryKey)]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("deleted", DeleteIndicator = true)]
        public bool IsDeleted { get; set; }

        public IssueType IssueType
        {
            get { return (IssueType)Enum.ToObject(typeof(IssueType), Id); }
            set
            {
                Id = (long)value;
                switch (value)
                {
                    case IssueType.Epic:
                        Name = "Epic";
                        break;
                    case IssueType.Task:
                        Name = "Task";
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
