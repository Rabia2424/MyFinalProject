using Core.DataAccess;
using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface ICartDal:IEntityRepository<Cart>
    {
        Cart GetCartByUserId(int userId);
        void AddToCart(int userId, CartItem cartItem);  
        IResult RemoveFromCart(int userId,CartItem cartItem);
        void UpdateCartItem(int userId, Cart cart);
    }
}
