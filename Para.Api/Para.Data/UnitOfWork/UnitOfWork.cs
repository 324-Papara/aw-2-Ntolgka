using Para.Data.Context;
using Para.Data.Domain;
using Para.Data.GenericRepository;

namespace Para.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ParaPostgreDbContext dbContext;
    
    public IGenericRepository<Customer> CustomerRepository { get; }
    public IGenericRepository<CustomerDetail> CustomerDetailRepository { get; }
    public IGenericRepository<CustomerAddress> CustomerAddressRepository { get; }
    public IGenericRepository<CustomerPhone> CustomerPhoneRepository { get; }
    
    

    public UnitOfWork(ParaPostgreDbContext dbContext)
    {
        this.dbContext = dbContext;

        CustomerRepository = new GenericRepository<Customer>(dbContext);
        CustomerDetailRepository = new GenericRepository<CustomerDetail>(dbContext);
        CustomerAddressRepository = new GenericRepository<CustomerAddress>(dbContext);
        CustomerPhoneRepository = new GenericRepository<CustomerPhone>(dbContext);
    }

    public void Dispose()
    {
    }

    public async Task Complete()
    {
        using (var dbTransaction = await dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                await dbContext.SaveChangesAsync();
                await dbTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}