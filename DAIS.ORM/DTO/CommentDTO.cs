using DAIS.ORM.Framework;
using System;

namespace DAIS.ORM.DTO
{
    [TableName("comment")]
    public class CommentDTO
    {
        [Column("id", ColumnType.PrimaryKey)]
        public long Id { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("deleted", DeleteIndicator = true)]
        public bool IsDeleted { get; set; }

        [Column("user_id", ColumnType.ForeignKey)]
        public long UserId { get; set; }

        [Column("issue_id", ColumnType.ForeignKey)]
        public long IssueId { get; set; }

        [Column("comment_id", ColumnType.ForeignKey)]
        public long? CommentId { get; set; }

    }
}
