using DAIS.ORM.DTO;
using DAIS.ORM.Framework;

namespace DAIS.ORM.Repositories
{
    public class IssueWorkflowRepository : RepositoryBase<IssueWorkflowDTO>
    {
        public IssueWorkflowRepository(IDatabase db) : base(db)
        {
        }
    }
}
