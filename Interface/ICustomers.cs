using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WEMA_BANK.Models;
using WEMA_BANK.Models.DB;

namespace WEMA_BANK.Interface
{
    public interface ICustomers
    {
        IEnumerable<CustomerResult> GetCustomers();
        IList<Customers> GetCustomerByEmail(string email);
        ResultObjects PostCustomer(CustomersModel customer);
        ResultObjects Onboard(CustomersModel customer);
    }
}
