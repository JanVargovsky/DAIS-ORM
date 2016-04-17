using DAIS.ORM.DTO;
using DAIS.ORM.Framework;
using System;

namespace DAIS.ORM.Repositories
{
    public class IssueTypeIssueStatusRepository : RepositoryBase<IssueTypeIssueStatusDTO>
    {
        public IssueTypeIssueStatusRepository(IDatabase db) : base(db)
        {
        }

        public override bool Update(IssueTypeIssueStatusDTO @object)
        {
            throw new NotSupportedException();
        }
    }
}
