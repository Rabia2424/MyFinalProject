using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Product Added!";
        public static string ProductNameInvalid = "ProductName is invalid!";
        public static string MaintenanceTime = "System is under maintanence!";
        public static string ProductsListed = "Product Listed!";
        public static string ProductCountExceed = "A category can have max 10 products!";
        public static string ProductNameAlreadyExists = "Product name is not valid because it is already used!";
        public static string CategoryCountExceed = "Category number can be at most 15 to add new product!";
    }
}
