using System.Collections.Generic;
using WEMA_BANK.Models;
using WEMA_BANK.Models.DB;

namespace WEMA_BANK.Interface
{
    public interface ICustomers
    {
        List<CustomerResult> GetCustomers();
        List<Customers> GetCustomerByEmail(string email);
        ResultObjects PostCustomer(CustomersModel customer);
        ResultObjects Onboard(CustomersModel customer);
    }
}
