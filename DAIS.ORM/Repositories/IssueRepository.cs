using DAIS.ORM.DTO;
using DAIS.ORM.Framework;

namespace DAIS.ORM.Repositories
{
    public class IssueRepository : RepositoryBase<IssueDTO>
    {
        public IssueRepository(IDatabase db) : base(db)
        {
        }
    }
}
