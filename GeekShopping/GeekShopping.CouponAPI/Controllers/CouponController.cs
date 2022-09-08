using GeekShopping.CouponAPI.Data.ValueObjects;
using GeekShopping.CouponAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CouponController : Controller
    {
        private readonly ICouponRepository _couponRepository;

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
        }

        [HttpGet("{couponCode}")]
        [Authorize]
        public async Task<ActionResult<CouponVO>> FindById(string couponCode)
        {
            var coupon = await _couponRepository.GetCouponByCouponCode(couponCode);
            if (coupon == null) return NotFound();
            return Ok(coupon);
        }        
    }
}
