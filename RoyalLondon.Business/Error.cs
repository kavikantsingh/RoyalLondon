using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalLondon.Business
{
    /// <summary>
    /// An Error model class
    /// </summary>
    public class Error
    {
        public string ErrorMessage { get; set; }
        public string ErrorFieldName { get; set; }
    }

    public class ValidateResult
    {
        public List<Error> lstError { get; set; }
        public List<Customer> lstCustomer { get; set; }
    }
}
