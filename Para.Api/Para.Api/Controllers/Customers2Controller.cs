using Microsoft.AspNetCore.Mvc;
using Para.Data.Domain;
using Para.Data.UnitOfWork;

namespace Para.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Customers2Controller : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public Customers2Controller(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<ActionResult<List<Customer>>> Get()
        {
            var entityList = await unitOfWork.CustomerRepository.GetAll();
            return Ok(entityList);
        }

        [HttpGet("{customerId}")]
        public async Task<ActionResult<Customer>> Get(long customerId)
        {
            var entity = await unitOfWork.CustomerRepository.GetById(customerId);
            if (entity == null)
            {
                return NotFound("There is no customer with the given Id.");
            }
            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> Post([FromBody] Customer customer)
        {
            var entity = await unitOfWork.CustomerRepository.GetById(customer.Id);
            if (entity != null)
            {
                return StatusCode(409, "There's a customer with the given Id.");
            }
            await unitOfWork.CustomerRepository.Insert(customer);
            await unitOfWork.Complete();
            return CreatedAtAction(nameof(Get), new { customerId = customer.Id }, customer);
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> Put(long customerId, [FromBody] Customer customer)
        {
            if (customerId != customer.Id)
            {
                return BadRequest("There is no customer with the given Id.");
            }
            
            await unitOfWork.CustomerRepository.Update(customer);
            await unitOfWork.Complete();
            return NoContent();
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> Delete(long customerId)
        {
            var existingCustomer = await unitOfWork.CustomerRepository.GetById(customerId);
            if (existingCustomer == null)
            {
                return NotFound("There is no customer with the given Id.");
            }

            await unitOfWork.CustomerRepository.Delete(existingCustomer);
            await unitOfWork.Complete();
            return NoContent();
        }
    }
}