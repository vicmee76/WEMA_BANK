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

        // GET: api/Customers
        [HttpGet]
        public ActionResult<IEnumerable<CustomerResult>> GetCustomers()
        {
            return _customers.GetCustomers().ToList();
        }




        private List<Customers> GetCustomerByEmail(string email)
        {
            return _customers.GetCustomerByEmail(email).ToList();
        }





        // POST: api/Customers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<Customer> PostCustomer(CustomersModel customer)
        {
            var r = _customers.PostCustomer(customer);
            return StatusCode(r.code, new { success = r.success, message = r.message});
        }







        // POST: api/Customers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("Onboard")]
        public ActionResult<Customer> Onboard(CustomersModel customer)
        {
            var r = _customers.Onboard(customer); 
            return StatusCode(r.code, new { success = r.success, message = r.message });
        }


    }
}
