using DAIS.ORM.DTO;
using DAIS.ORM.Framework;

namespace DAIS.ORM.Repositories
{
    public class CommentRepository : RepositoryBase<CommentDTO>
    {
        public CommentRepository(IDatabase db) : base(db)
        {
        }
    }
}
