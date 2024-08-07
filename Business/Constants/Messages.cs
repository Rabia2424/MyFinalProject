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
        public static string CategoryAdded = "Category Added!";
        public static string ProductDeleted = "Product deleted!";
        public static string AuthorizationDenied = "Authorization denied!";
        public static string UserRegistered = "User registered!";
        public static string UserNotFound = "User can not be found!";
        public static string PasswordError = "Password is not verified!";
        public static string SuccessfulLogin = "User login successfully!";
        public static string UserAlreadyExists = "User is already exists!";
        public static string AccessTokenCreated = "Access token is created successfully!";
        public static string CartItemAdded = "Cart Item added successfully!";
        public static string InvalidUserId = "Invalid User Id!";
        public static string ShowCart = "You have reached the cart!";
        public static string CartItemRemoved = "Cart Item removed successfully!";
        public static string CartItemNotFound = "Cart Item not found in the card!";
    }
}
