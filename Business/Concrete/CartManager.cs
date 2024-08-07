using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CartManager : ICartService
    {
        ICartDal _cartDal;

        public CartManager(ICartDal cartDal)
        {
            _cartDal = cartDal;
        }

        public IResult AddToCart(int userId, CartItem cartItem)
        {
            _cartDal.AddToCart(userId, cartItem);
            return new SuccessResult(Messages.CartItemAdded);
        }

        public IDataResult<Cart> GetCartByUserId(int userId)
        {
            var cart =  _cartDal.GetCartByUserId(userId);
            if(cart == null)
            {
                return new ErrorDataResult<Cart>(cart,Messages.InvalidUserId);
            }
            return new SuccessDataResult<Cart>(cart,Messages.ShowCart);
        }

        public void InitializeCart(int userId)
        {
            _cartDal.Add(new Cart { UserId = userId});
        }

        public IResult RemoveFromCart(int userId, CartItem cartItem)
        {
            var result = _cartDal.RemoveFromCart(userId, cartItem);
            if (result.Success)
            {
                return new SuccessResult(Messages.CartItemRemoved);
            }
            else
            {
                return new ErrorResult(Messages.CartItemNotFound);
            }
           
        }

        public IResult UpdateCartItem(int userId, Cart cart)
        {
            _cartDal.UpdateCartItem(userId, cart);  
            return new SuccessResult(Messages.CartItemAdded);
        }
    }
}
