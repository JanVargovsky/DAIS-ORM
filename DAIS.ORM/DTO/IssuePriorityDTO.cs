using DAIS.ORM.Framework;
using System;

namespace DAIS.ORM.DTO
{
    public enum IssuePriority
    {
        HiPrio = 1,
        Critical = 2,
        Normal = 3,
        NotImportant = 4
    }

    [TableName("issue_priority")]
    public class IssuePriorityDTO
    {
        [Column("id", ColumnType.PrimaryKey)]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("value")]
        public int Value { get; set; }

        [Column("deleted", DeleteIndicator = true)]
        public bool IsDeleted { get; set; }

        public IssuePriority IssuePriority
        {
            get { return (IssuePriority)Enum.ToObject(typeof(IssuePriority), Id); }
            set
            {
                Id = (long)value;
                switch (value)
                {
                    case IssuePriority.HiPrio:
                        Name = "Hi-Prio";
                        break;
                    case IssuePriority.Critical:
                        Name = "Critical";
                        break;
                    case IssuePriority.Normal:
                        Name = "Normal";
                        break;
                    case IssuePriority.NotImportant:
                        Name = "Not important";
                        break;
                    default:
                        Name = "Not defined";
                        break;
                }
            }
        }
    }
}
