﻿using DAIS.ORM.Framework;
using System;

namespace DAIS.ORM.DTO
{
    [TableName("issue")]
    public class IssueDTO
    {
        [Column("id", ColumnType.PrimaryKey)]
        public long Id { get; set; }

        [Column("summary")]
        public string Summary { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("created_at")]
        public DateTime Created { get; set; }

        [Column("last_updated_at")]
        public DateTime? LastUpdatedAt { get; set; }

        [Column("time_remaining")]
        public long RemainingTimeTicks { get; set; }

        [Column("time_estimated")]
        public long EstimatedTime { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }


        // FKs
        [Column("issue_id", ColumnType.ForeignKey)]
        public long? IssueId { get; set; }

        [Column("created_by", ColumnType.ForeignKey)]
        public long CreatedBy { get; set; }

        [Column("assignee", ColumnType.ForeignKey)]
        public long? Assignee { get; set; }

        [Column("issue_type_id", ColumnType.ForeignKey)]
        public long IssueTypeId { get; set; }

        [Column("issue_status_id", ColumnType.ForeignKey)]
        public long IssueStatusId { get; set; }

        [Column("issue_priority_id", ColumnType.ForeignKey)]
        public long IssuePriorityId { get; set; }

        // Converters
        public TimeSpan TimeRemaining
        {
            get
            {
                return TimeSpan.FromTicks(RemainingTimeTicks);
            }
            set
            {
                RemainingTimeTicks = value.Ticks;
            }
        }
    }
}
