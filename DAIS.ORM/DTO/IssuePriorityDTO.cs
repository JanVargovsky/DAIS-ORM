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
                        Value = 10;
                        break;
                    case IssuePriority.Critical:
                        Name = "Critical";
                        Value = 30;
                        break;
                    case IssuePriority.Normal:
                        Name = "Normal";
                        Value = 50;
                        break;
                    case IssuePriority.NotImportant:
                        Name = "Not important";
                        Value = 100;
                        break;
                    default:
                        Name = "Not defined";
                        break;
                }
            }
        }
    }
}
