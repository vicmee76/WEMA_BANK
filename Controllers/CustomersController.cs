using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEMA_BANK.Helpers;
using WEMA_BANK.Interface;
using WEMA_BANK.Models;
using WEMA_BANK.Models.DB;

namespace WEMA_BANK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomers _customers;

        public CustomersController(ICustomers customers)
        {
            _customers = customers;
        }



        /// <summary>
        /// This returns a list of all onboarded customers
        /// </summary>
        /// <returns>Returns a list of all onboarded customers</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/Customers
        /// 
        /// </remarks>
        /// <response code="200">Returns a list of all onboarded customers </response>
        [ProducesResponseType(typeof(CustomerResult), 200)]
        [Produces("application/json")]
        [HttpGet]
        public ActionResult<IEnumerable<CustomerResult>> GetCustomers()
        {
            return _customers.GetCustomers().ToList();
        }




        public List<Customers> GetCustomerByEmail(string email)
        {
            return _customers.GetCustomerByEmail(email).ToList();
        }






        /// <summary>
        /// This endpoint creates a customer and send an OTP for onboarding
        /// </summary>
        /// <returns>Returns a success or error message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/Customers
        /// 
        /// </remarks>
        /// <response code="201">Returns a success model</response>
        /// <response code="400">Returns an error message if required parameters are empty</response>
        /// <response code="409">Returns an error message if data already exits</response>
        /// <response code="404">Returns an error message if state or lga does not match</response>
        /// <response code="500">Returns an error message internal server error</response>
        [ProducesResponseType(typeof(ResultObjects), 201)]
        [ProducesResponseType(typeof(ResultObjects), 400)]
        [ProducesResponseType(typeof(ResultObjects), 409)]
        [ProducesResponseType(typeof(ResultObjects), 404)]
        [ProducesResponseType(typeof(ResultObjects), 500)]
        [Produces("application/json")]
        [HttpPost]
        public ActionResult<Customer> PostCustomer(CustomersModel customer)
        {
            var r = _customers.PostCustomer(customer);
            return StatusCode(r.Code, new { r.Success, r.Message});
        }








        /// <summary>
        /// This endpoint creates a customer and send an OTP for onboarding
        /// </summary>
        /// <returns>Returns a success or error message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/Customers/Onboard
        /// 
        /// </remarks>
        /// <response code="202">Returns a success model of onboarded customer</response>
        /// <response code="200">Returns a success model if the customer has already been onboarded</response>
        /// <response code="400">Returns an error message if required parameters are empty</response>
        /// <response code="406">Returns an error message if customer phone number does not match</response>
        /// <response code="404">Returns an error message if customer details cannot be found or OTP is incorrect</response>
        /// <response code="500">Returns an error message internal server error</response>
        [ProducesResponseType(typeof(ResultObjects), 202)]
        [ProducesResponseType(typeof(ResultObjects), 200)]
        [ProducesResponseType(typeof(ResultObjects), 400)]
        [ProducesResponseType(typeof(ResultObjects), 406)]
        [ProducesResponseType(typeof(ResultObjects), 404)]
        [ProducesResponseType(typeof(ResultObjects), 500)]
        [Produces("application/json")]
        [HttpPost]
        [Route("Onboard")]
        public ActionResult<Customer> Onboard(CustomersModel customer)
        {
            var r = _customers.Onboard(customer); 
            return StatusCode(r.Code, new { r.Success,  r.Message });
        }


    }
}
