using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEMA_BANK.Helpers;
using WEMA_BANK.Models;
using WEMA_BANK.Models.DB;

namespace WEMA_BANK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly WEMAContext _context;
        private readonly HelperClass _helper = new HelperClass();

        public CustomersController(WEMAContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customers>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }


     
        private async Task<List<Customers>> GetCustomerByEmail(string email)
        {
            var customer = await _context.Customers.Where(x =>x.Email.ToLower() == email.ToLower()).ToListAsync();

            if (customer == null)
            {
                return customer;
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        // POST: api/Customers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(CustomersModel customer)
        {
            try
            {
                if (customer == null)
                    return StatusCode(400, (new { success = false, message = "customers information cannot be empty" }));

                if (customer.PhoneNo == null || customer.Email == null || customer.StateName == null || customer.Lga == null)
                    return StatusCode(400, (new { success = false, message = "Please enter email, phone, state and lga" }));

                if (customer.Password == null || customer.Password.Length < 8)
                    return StatusCode(406, (new { success = false, message = "Password should be more than 8 characters" }));

                var check = GetCustomerByEmail(customer.Email);

                if (check.Result.Count() > 0)
                {
                    return StatusCode(309, (new { success = false, message = "Customer already exits" }));
                }
                else
                {
                    // find state 
                    var checkState = from s in _context.States
                                     join l in _context.Lga on s.StateId equals l.StateId
                                     where s.StateName.ToUpper() == customer.StateName.ToUpper() && l.LgaName.ToLower() == customer.Lga.ToLower()
                                     select new
                                     {
                                         StateName = s.StateName,
                                         StateId = s.StateId
                                     };

                    if (checkState.Count() > 0)
                    {
                        // encrypt passwor
                        var pass = _helper.Encrypt(customer.Password);

                        // genetate Random OTP 
                        var rand = _helper.OTP();

                        Customers cus = new Customers()
                        {
                            Email = customer.Email,
                            PhoneNo = customer.PhoneNo,
                            Password = pass,
                            IsOnboard = false,
                            Otp = rand,
                            State = checkState.FirstOrDefault().StateId
                        };

                        _context.Customers.Add(cus);
                        await _context.SaveChangesAsync();

                        return StatusCode(201, (new { success = true, message = $"Customer created but not yet onboarded; An OTO of { rand } has been sent to {customer.PhoneNo} to complete the onboarding process" }));
                    }
                    else
                    {
                        return StatusCode(404, (new { success = false, message = "State or Lga was not found" }));
                    }
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, (new { success = false, message = $"Error : {ex} " }));
            }
        }


        // POST: api/Customers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("Onboard")]
        public async Task<ActionResult<Customer>> Onboard(CustomersModel customer)
        {
            try
            {
                if (customer == null)
                    return StatusCode(400, (new { success = false, message = "customers information cannot be empty" }));

                if (customer.PhoneNo == null || customer.Email == null || customer.otp == null)
                    return StatusCode(400, (new { success = false, message = "Please enter email, phone, and otp" }));

                var check = GetCustomerByEmail(customer.Email);

                if (check.Result.Count() > 0)
                {
                    if(check.Result.FirstOrDefault().IsOnboard == true)
                    {
                        return StatusCode(200, (new { success = true, message = $"Customer with email {customer.Email} has already been onboarded" }));
                    }
                    else
                    {
                        if (check.Result.FirstOrDefault().PhoneNo == customer.PhoneNo)
                        {
                            if (check.Result.FirstOrDefault().Otp == customer.otp)
                            {
                                check.Result.FirstOrDefault().IsOnboard = true;
                                await _context.SaveChangesAsync();
                                return StatusCode(202, (new { success = true, message = $"Customer with email {customer.Email} onboarded successfully" }));
                            }
                            else
                            {
                                return StatusCode(404, (new { success = false, message = "Incorrect OTP for this customer" }));
                            }
                        }
                        else
                        {
                            return StatusCode(406, (new { success = false, message = "Customer phone number does not match" }));
                        }
                    }
                }
                else
                {
                    return StatusCode(309, (new { success = false, message = "This customer cannot be found" }));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, (new { success = false, message = $"Error : {ex} " }));
            }
        }



        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customers>> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
        }



        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
