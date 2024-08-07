using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICartService
    {
        void InitializeCart(int userId);
        IDataResult<Cart> GetCartByUserId(int userId);
        IResult AddToCart(int userId,CartItem cartItem);
        IResult RemoveFromCart(int userId,CartItem cartItem);
        IResult UpdateCartItem(int userId, Cart cart);
    }
}
