using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEMA_BANK.Helpers;
using WEMA_BANK.Interface;
using WEMA_BANK.Models;
using WEMA_BANK.Models.DB;

namespace WEMA_BANK.Services
{
    public class CustomerServices : ICustomers
    {
        private readonly WEMAContext _context;
        private readonly HelperClass _helper = new HelperClass();

        public CustomerServices(WEMAContext context)
        {
            _context = context;
        }


        public IList<Customers> GetCustomerByEmail(string email)
        {
            var customer = _context.Customers.Where(x => x.Email.ToLower() == email.ToLower()).ToList();
            if (customer == null)
            {
                return customer;
            }
            return customer;
        }




        public IEnumerable<CustomerResult> GetCustomers()
        {
            var qry = (from c in _context.Customers
                            join l in _context.Lga on c.Location equals l.LgaId
                            join s in _context.States on l.StateId equals s.StateId
                            where (c.IsOnboard == true)
                            select new CustomerResult
                            {
                                Email = c.Email,
                                PhoneNo = c.PhoneNo,
                                StateName = s.StateName,
                                Lga = l.LgaName,
                                isOnBoard = c.IsOnboard == true ? "YES" : "Pending"
                            }).ToList();

            return qry;
        }




        public ResultObjects PostCustomer(CustomersModel customer)
        {
            ResultObjects result = new ResultObjects();
            try
            {
                if (customer == null)
                {
                    result.code = 400;
                    result.success = false;
                    result.message = "customers information cannot be empty";
                }

                if (customer.PhoneNo == null || customer.Email == null || customer.StateName == null || customer.Lga == null)
                {
                    result.code = 400;
                    result.success = false;
                    result.message = "Please enter email, phone, state and lga";
                }
                   
                if (customer.Password == null || customer.Password.Length < 8)
                {
                    result.code = 400;
                    result.success = false;
                    result.message = "Password should be more than 8 characters";
                }
                   
                var check = GetCustomerByEmail(customer.Email);

                if (check.Count() > 0)
                {
                    result.code = 400;
                    result.success = false;
                    result.message = "Customer already exits";
                }
                else
                {
                    // find state 
                    var checkState = from s in _context.States
                                     join l in _context.Lga on s.StateId equals l.StateId
                                     where s.StateName.ToUpper() == customer.StateName.ToUpper() && l.LgaName.ToLower() == customer.Lga.ToLower()
                                     select new
                                     {
                                         Location = l.LgaId
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
                            Location = checkState.FirstOrDefault().Location
                        };

                        _context.Customers.Add(cus);
                        _context.SaveChangesAsync();

                        result.code = 201;
                        result.success = true;
                        result.message = $"Customer created but not yet onboarded; An OTO of { rand } has been sent to {customer.PhoneNo} to complete the onboarding process";

                    }
                    else
                    {
                        result.code = 404;
                        result.success = false;
                        result.message = "State or Lga was not found";
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result.code = 500;
                result.success = false;
                result.message = $"Error : {ex} ";
                return result;
            }

           
        }

        ResultObjects ICustomers.Onboard(CustomersModel customer)
        {
            ResultObjects result = new ResultObjects();
            result.success = false;

            try
            {
                if (customer == null)
                {
                    result.code = 400;
                    result.message = "customers information cannot be empty";
                }

                if (customer.PhoneNo == null || customer.Email == null || customer.otp == null)
                {
                    result.code = 400;
                    result.message = "Please enter email, phone, state and lga";
                }

                var check = GetCustomerByEmail(customer.Email);

                if (check.Count() > 0)
                {
                    if (check.FirstOrDefault().IsOnboard == true)
                    {
                        result.code = 200;
                        result.message = $"Customer with email {customer.Email} has already been onboarded";
                    }
                    else
                    {
                        if (check.FirstOrDefault().PhoneNo == customer.PhoneNo)
                        {
                            if (check.FirstOrDefault().Otp == customer.otp)
                            {
                                check.FirstOrDefault().IsOnboard = true;
                                 _context.SaveChangesAsync();

                                result.code = 202;
                                result.success = true;
                                result.message = $"Customer with email {customer.Email} onboarded successfully";
                            }
                            else
                            {
                                result.code = 404;
                                result.message = "Incorrect OTP for this customer";
                            }
                        }
                        else
                        {
                            result.code = 406;
                            result.message = "Customer phone number does not match";
                        }
                    }
                }
                else
                {
                    result.code = 404;
                    result.message = "This customer cannot be found";
                }
            }
            catch (Exception ex)
            {
                result.code = 500;
                result.message = $"Error : {ex} ";
            }
            return result;
        }
    }

}
