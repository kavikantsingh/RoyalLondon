using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalLondon.Business
{
    /// <summary>
    /// ICSVFileValidator
    /// </summary>
    public interface ICSVFileValidator
    {
        /// <summary>
        /// This interface implemented for validate CSV file
        /// </summary>
        /// <param name="csvFileData"></param>
        /// <returns></returns>
        ValidateResult ValidateCSVFileData(string csvFileData, int totalMonths, decimal creditCharge);
    }
}
