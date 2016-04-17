using DAIS.ORM.Framework;
using System;

namespace DAIS.ORM.DTO
{
    public enum IssueType
    {
        Task = 1,
    }

    [TableName("issue_type")]
    public class IssueTypeDTO
    {
        [Column("id", ColumnType.PrimaryKey)]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        public IssueType IssueType
        {
            get { return (IssueType)Enum.ToObject(typeof(IssueType), Id); }
            set { Id = (long)value; }
        }
    }
}
