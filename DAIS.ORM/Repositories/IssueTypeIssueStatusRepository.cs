using DAIS.ORM.DTO;
using DAIS.ORM.Framework;

namespace DAIS.ORM.Repositories
{
    public class IssueTypeIssueStatusRepository : RepositoryBase<IssueTypeIssueStatusDTO>
    {
        public IssueTypeIssueStatusRepository(IDatabase db) : base(db)
        {
        }
    }
}
