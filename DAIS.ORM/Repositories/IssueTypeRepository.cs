using DAIS.ORM.DTO;
using DAIS.ORM.Framework;

namespace DAIS.ORM.Repositories
{
    public class IssueTypeRepository : RepositoryBase<IssueTypeDTO>
    {
        public IssueTypeRepository(IDatabase db) : base(db)
        {
        }
    }
}
