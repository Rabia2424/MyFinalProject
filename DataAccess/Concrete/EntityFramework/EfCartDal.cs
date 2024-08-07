using Core.DataAccess.EntityFramework;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCartDal : EfEntityRepositoryBase<Cart, NorthwindContext>, ICartDal
    {
        public void AddToCart(int userId, CartItem cartItem)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                Cart checkCart = context.Carts.Include(c=>c.CartItems).FirstOrDefault(c => c.UserId == userId);
                var checkProductAlreadyInCart = checkCart.CartItems.FirstOrDefault(c=>c.ProductId == cartItem.ProductId);
                if (checkProductAlreadyInCart == null)
                {
                    checkCart.CartItems.Add(new CartItem()
                    {
                        ProductId = cartItem.ProductId,
                        Product = context.Products.FirstOrDefault(c=>c.ProductId == cartItem.ProductId),
                        CartId = checkCart.Id,
                        Quantity = cartItem.Quantity,
                        Cart = checkCart
                    });
                }
                else
                {
                    //checkProductAlreadyInCart.Quantity += cartItem.Quantity;
                    return;
                }
                context.SaveChanges();
            }
        }

        public void UpdateCartItem(int userId, Cart cart)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                if(cart != null)
                {
                    foreach (var cartItem in cart.CartItems)
                    {
                        var checkProductAlreadyInCart = context.CartItems
                    .FirstOrDefault(c => c.ProductId == cartItem.ProductId && c.CartId == cart.Id);

                        if (checkProductAlreadyInCart != null)
                        {
                            checkProductAlreadyInCart.Quantity = cartItem.Quantity;
                            context.CartItems.Update(checkProductAlreadyInCart);
                        }
                        else
                        {
                            //cartItem.CartId = cart.Id;
                            //context.CartItems.Add(cartItem);
                            return;
                        }
                    }
                    context.SaveChanges();
                }

                //var checkProductAlreadyInCart = cart.CartItems.FirstOrDefault(c => c.ProductId == cart..);
                //if (checkProductAlreadyInCart!=null)
                //{
                //    checkProductAlreadyInCart.Quantity = cartItem.Quantity;
                //    context.CartItems.Update(checkProductAlreadyInCart);
                //    context.SaveChanges();
                //}
                //else
                //{
                //    return;
                //}
            }
        }

        public Cart GetCartByUserId(int userId)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                 return context.Carts
                    .Include(c => c.CartItems)
                    .ThenInclude(c => c.Product)
                    .FirstOrDefault(u => u.UserId == userId);
            }
        }

        public IResult RemoveFromCart(int userId,CartItem cartItem)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                if(cartItem == null)
                {
                    return new ErrorResult();
                }
                context.CartItems.Remove(cartItem);
                context.SaveChanges();
                return new SuccessResult();
            }
        }
    }
}
