using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Para.Data.Context;
using Para.Data.Domain;

namespace Para.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ParaPostgreDbContext dbContext;

        public CustomersController(ParaPostgreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        public async Task<ActionResult<List<Customer>>> Get()
        {
            var entityList1 = await dbContext.Set<Customer>().Include(x=> x.CustomerAddresses).Include(x=> x.CustomerPhones).Include(x=> x.CustomerDetail).ToListAsync();
            //var entityList2 = await dbContext.Customers.Include(x=> x.CustomerAddresses).Include(x=> x.CustomerPhones).Include(x=> x.CustomerDetail).ToListAsync();
            return Ok(entityList1);
        }

        [HttpGet("{customerId}")]
        public async Task<ActionResult<Customer>> Get(long customerId)
        {
            var entity = await dbContext.Set<Customer>().Include(x=> x.CustomerAddresses).Include(x=> x.CustomerPhones).Include(x=> x.CustomerDetail).FirstOrDefaultAsync(x => x.Id == customerId);
            if (entity == null)
            {
                return BadRequest("There's no customer with the given id.");
            }
            return Ok(entity);
        }
        
        [HttpGet("detailed")]
        public async Task<ActionResult<List<Customer>>> GetDetails()
        {
            var customers = await dbContext.Set<Customer>().Include(x=> x.CustomerAddresses).Include(x=> x.CustomerPhones).ToListAsync();
            //var entityList2 = await dbContext.Customers.Include(x=> x.CustomerAddresses).Include(x=> x.CustomerPhones).Include(x=> x.CustomerDetail).ToListAsync();
            return Ok(customers);
        }
        
        [HttpGet("{customerId}/details")]
        public async Task<ActionResult<List<Customer>>> GetDetailsById(long customerId)
        {
            var customers = await dbContext.Set<Customer>()
                .Include(x=> x.CustomerAddresses)
                .Include(x=> x.CustomerPhones)
                .FirstOrDefaultAsync(x => x.Id == customerId);
            //var entityList2 = await dbContext.Customers.Include(x=> x.CustomerAddresses).Include(x=> x.CustomerPhones).Include(x=> x.CustomerDetail).ToListAsync();
            return Ok(customers);
        }
        
        [HttpGet("{name}/view-details")]
        public async Task<ActionResult<List<Customer>>> GetByName(string name)
        {
            var customers = await dbContext.Customers
                .Include(x=> x.CustomerAddresses)
                .Include(x=> x.CustomerPhones)
                .Include(x=> x.CustomerDetail)
                .Where(x=> x.FirstName == name).ToListAsync();
            if (customers == null)
            {
                return NotFound("There's no customer named" + name);
            }
            //var entityList2 = await dbContext.Customers.Include(x=> x.CustomerAddresses).Include(x=> x.CustomerPhones).Include(x=> x.CustomerDetail).ToListAsync();
            return Ok(customers);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            dbContext.Set<Customer>().AddAsync(customer);
            await dbContext.SaveChangesAsync();
            return Ok(customer);
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> Put(long customerId, [FromBody] Customer customer)
        {
            
            if (customerId != customer.Id)
            {
                return BadRequest("Customer ID mismatch.");
            }
            
            dbContext.Set<Customer>().Update(customer);
            await dbContext.SaveChangesAsync();
            return Ok(customer);
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> Delete(long customerId)
        {
            var entity = await dbContext.Set<Customer>().FirstOrDefaultAsync(x => x.Id == customerId);
            if (entity == null)
            {
                return BadRequest("There's no customer with the given id.");
            }
            dbContext.Set<Customer>().Remove(entity);
            await dbContext.SaveChangesAsync();
            return Ok(entity);
        }
    }
}