using MasterDetails.Models;
using MasterDetails.Repositories.interfaces;
using Masters_Details_CRUD.Repositories;

namespace MasterDetails.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private PatientDbContext db;
        public UnitOfWork(PatientDbContext db)
        {
            this.db = db;
        }
        public IGenericRepo<T> GetRepo<T>() where T : class, new()
        {
            return new GenericRepo<T>(db);
        }

        public async Task<bool> SaveAsync()
        {
            int rowsEffected = await db.SaveChangesAsync();
            return rowsEffected > 0;
        }
    }
}
