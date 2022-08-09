using Rocky_DataAccess.Data;
using Rocky_DataAccess.Repository.IRepository;
using Rocky_Models;

namespace Rocky_DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(OrderHeader obj)
        {
            _db.OrderHeader.Update(obj);
        }
    }
}