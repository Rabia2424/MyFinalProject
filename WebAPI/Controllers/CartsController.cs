using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        ICartService _cartService;
        IHttpContextAccessor _httpContextAccessor;

        public CartsController(ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("getcartbyuserid")]
        public IActionResult Get()
        {
            var result = _cartService.GetCartByUserId(GetUserId());
            if(!result.Success)
            {
                return BadRequest("Invalid User ID");
            }
            return Ok(result.Data.CartItems);
        }

        [HttpGet("getcart")]
        public IActionResult GetCart()
        {
            var result = _cartService.GetCartByUserId(GetUserId());
            if (!result.Success)
            {
                return BadRequest("Invalid User ID");
            }
            return Ok(result.Data);
        }

        [HttpPost("addtocart")]
        public IActionResult Add(CartItem cartItem)
        {
            var result  = _cartService.AddToCart(GetUserId(),cartItem);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("updatecartitem")]
        public IActionResult Update(Cart cart)
        {
            var result = _cartService.UpdateCartItem(GetUserId(), cart);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("removefromcart")]
        public IActionResult Delete(CartItem cartItem)
        {
            var result = _cartService.RemoveFromCart(GetUserId(), cartItem);
            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        private ClaimsPrincipal GetUserClaims()
        {
            // HTTP başlıklarından Authorization header'ını alın
            var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            // Token'ı almak için "Bearer " öneki varsa kaldırın
            if (authHeader.StartsWith("Bearer "))
            {
                authHeader = authHeader.Substring("Bearer ".Length).Trim();
            }

            //if (string.IsNullOrWhiteSpace(authHeader))
            //{
            //    throw new ArgumentException("Token cannot be null or empty.", nameof(authHeader));
            //}

            // Token'ı çözümleyin
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(authHeader) as JwtSecurityToken;

            // Token'dan claims'i çıkarın
            var claims = jsonToken?.Claims;

            // ClaimsPrincipal nesnesini oluşturun
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            return principal;
        }

        private int GetUserId()
        {
            var principal = GetUserClaims();
            int userId;
            bool success = int.TryParse(principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out userId);
            if (success)
            {
                return userId;
            }
            else
            {
                // Dönüşüm başarısız olduysa uygun bir değer döndürün
                return -1; // veya başka bir hata kodu
            }
        }
    }
}
