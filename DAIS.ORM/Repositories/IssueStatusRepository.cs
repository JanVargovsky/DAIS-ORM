using DAIS.ORM.DTO;
using DAIS.ORM.Framework;

namespace DAIS.ORM.Repositories
{
    public class IssueStatusRepository : RepositoryBase<IssueStatusDTO>
    {
        public IssueStatusRepository(IDatabase db) : base(db)
        {
        }
    }
}
