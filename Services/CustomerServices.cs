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
                                IsOnBoard = c.IsOnboard == true ? "YES" : "Pending"
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
                    result.Code = 400;
                    result.Success = false;
                    result.Message = "customers information cannot be empty";
                }

                if (customer.PhoneNo == null || customer.Email == null || customer.StateName == null || customer.Lga == null)
                {
                    result.Code = 400;
                    result.Success = false;
                    result.Message = "Please enter email, phone, state and lga";
                }
                   
                if (customer.Password == null || customer.Password.Length < 8)
                {
                    result.Code = 400;
                    result.Success = false;
                    result.Message = "Password should be more than 8 characters";
                }
                   
                var check = GetCustomerByEmail(customer.Email);

                if (check.Count() > 0)
                {
                    result.Code = 409;
                    result.Success = false;
                    result.Message = "Customer already exits";
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

                        result.Code = 201;
                        result.Success = true;
                        result.Message = $"Customer created but not yet onboarded; An OTO of { rand } has been sent to {customer.PhoneNo} to complete the onboarding process";

                    }
                    else
                    {
                        result.Code = 404;
                        result.Success = false;
                        result.Message = "State or Lga was not found";
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Success = false;
                result.Message = $"Error : {ex} ";
                return result;
            }

           
        }

        ResultObjects ICustomers.Onboard(CustomersModel customer)
        {
            ResultObjects result = new ResultObjects();
            try
            {
                if (customer == null)
                {
                    result.Code = 400;
                    result.Success = false;
                    result.Message = "customers information cannot be empty";
                }

                if (customer.PhoneNo == null || customer.Email == null || customer.Otp == null)
                {
                    result.Code = 400;
                    result.Success = false;
                    result.Message = "Please enter email, phone, state and lga";
                }

                var check = GetCustomerByEmail(customer.Email);

                if (check.Count() > 0)
                {
                    if (check.FirstOrDefault().IsOnboard == true)
                    {
                        result.Code = 200;
                        result.Success = true;
                        result.Message = $"Customer with email {customer.Email} has already been onboarded";
                    }
                    else
                    {
                        if (check.FirstOrDefault().PhoneNo == customer.PhoneNo)
                        {
                            if (check.FirstOrDefault().Otp == customer.Otp)
                            {
                                check.FirstOrDefault().IsOnboard = true;
                                 _context.SaveChangesAsync();

                                result.Code = 202;
                                result.Success = true;
                                result.Message = $"Customer with email {customer.Email} onboarded successfully";
                            }
                            else
                            {
                                result.Code = 404;
                                result.Success = false;
                                result.Message = "Incorrect OTP for this customer";
                            }
                        }
                        else
                        {
                            result.Code = 406;
                            result.Success = false;
                            result.Message = "Customer phone number does not match";
                        }
                    }
                }
                else
                {
                    result.Code = 404;
                    result.Success = false;
                    result.Message = "This customer cannot be found";
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Success = false;
                result.Message = $"Error : {ex} ";
            }
            return result;
        }
    }

}
