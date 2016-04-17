using DAIS.ORM.Framework;
using System;

namespace DAIS.ORM.DTO
{
    [TableName("issue_workflow")]
    public class IssueWorkflowDTO
    {
        [Column("id", ColumnType.PrimaryKey)]
        public long Id { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("time_spent")]
        public long TimeSpentTicks { get; set; }

        [Column("user_id", ColumnType.ForeignKey)]
        public long UserId { get; set; }

        [Column("issue_id", ColumnType.ForeignKey)]
        public long IssueId { get; set; }

        [Column("issue_status_id", ColumnType.ForeignKey)]
        public long IssueStatusId { get; set; }

        public TimeSpan TimeSpent
        {
            get
            {
                return TimeSpan.FromTicks(TimeSpentTicks);
            }
            set
            {
                TimeSpentTicks = value.Ticks;
            }
        }
    }
}
