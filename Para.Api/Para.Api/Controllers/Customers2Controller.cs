using Microsoft.AspNetCore.Mvc;
using Para.Api.Dto;
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
        
        [HttpGet("{customerId}/details")]
        public async Task<ActionResult<CustomerDto>> GetDetailed(long customerId)
        {
            var entity = await unitOfWork.CustomerRepository.GetDetails(customerId);
            if (entity == null)
            {
                return NotFound("There is no customer with the given Id.");
            }

            var customerDto = new CustomerDto
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                IdentityNumber = entity.IdentityNumber,
                Email = entity.Email,
                CustomerNumber = entity.CustomerNumber,
                DateOfBirth = entity.DateOfBirth,
                CustomerAddresses = entity.CustomerAddresses?.Select(addr => new CustomerAddressDto
                {
                    Country = addr.Country,
                    City = addr.City,
                    AddressLine = addr.AddressLine,
                    ZipCode = addr.ZipCode
                }).ToList(),
                CustomerPhones = entity.CustomerPhones?.Select(phone => new CustomerPhoneDto
                {
                    CountyCode = phone.CountyCode,
                    Phone = phone.Phone
                }).ToList()
            };

            return Ok(customerDto);
        }

        [HttpGet("{name}/view-details")]
        public async Task<ActionResult<List<CustomerDto>>> GetDetailedByName(string name)
        {
            var entity = await unitOfWork.CustomerRepository.GetDetailsByName(name);
            if (entity == null || !entity.Any())
            {
                return NotFound("There is no customer with the given name.");
            }

            var customerDto = entity.Select(customer => new CustomerDto
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                IdentityNumber = customer.IdentityNumber,
                Email = customer.Email,
                CustomerNumber = customer.CustomerNumber,
                DateOfBirth = customer.DateOfBirth,
                CustomerAddresses = customer.CustomerAddresses?.Select(addr => new CustomerAddressDto
                {
                    Country = addr.Country,
                    City = addr.City,
                    AddressLine = addr.AddressLine,
                    ZipCode = addr.ZipCode
                }).ToList(),
                CustomerPhones = customer.CustomerPhones?.Select(phone => new CustomerPhoneDto
                {
                    CountyCode = phone.CountyCode,
                    Phone = phone.Phone
                }).ToList()
            }).ToList();

            return Ok(customerDto);
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
                return BadRequest("You cannot change the id of a customer.");
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