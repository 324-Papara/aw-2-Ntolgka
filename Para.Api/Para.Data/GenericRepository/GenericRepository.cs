using Microsoft.EntityFrameworkCore;
using Para.Base.Entity;
using Para.Data.Context;
using Para.Data.Domain;

namespace Para.Data.GenericRepository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly ParaPostgreDbContext dbContext;

    public GenericRepository(ParaPostgreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Save()
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task<TEntity> GetById(long Id)
    {
        return await dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == Id);
    }
    
    public async Task<TEntity> GetDetails(long customerId)
    {
        if (typeof(TEntity) == typeof(Customer))
        {
            return await dbContext.Set<Customer>()
                .Include(c => c.CustomerAddresses)
                .Include(c => c.CustomerPhones)
                .Include(c => c.CustomerDetail)
                .FirstOrDefaultAsync(c => c.Id == customerId) as TEntity;
        }
        
        return await dbContext.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == customerId);
    }

    public async Task Insert(TEntity entity)
    {
        entity.IsActive = true;
        entity.InsertDate = DateTime.UtcNow;
        entity.InsertUser = "System";
        await dbContext.Set<TEntity>().AddAsync(entity);
    }

    public async Task Update(TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);
    }

    public async Task Delete(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
    }

    public async Task Delete(long Id)
    {
        var entity = await dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == Id);
        dbContext.Set<TEntity>().Remove(entity);
    }

    public async Task<List<TEntity>> GetAll()
    {
       return await dbContext.Set<TEntity>().ToListAsync();
    }
}