using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudySpace.API.DTOs.Payment;
using StudySpace.API.Helpers;
using StudySpace.API.Services.Interfaces;
using System.Security.Claims;

namespace StudySpace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _paymentService.GetAllPaymentsAsync();
            return Ok(ApiResponse<List<PaymentResponseDto>>.Ok(result));
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyPayments()
        {
            var userId = int.Parse(User.FindFirst("userId")!.Value);
            var result = await _paymentService.GetMyPaymentsAsync(userId);
            return Ok(ApiResponse<List<PaymentResponseDto>>.Ok(result));
        }

        [HttpGet("overdue")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetOverdue()
        {
            var result = await _paymentService.GetOverduePaymentsAsync();
            return Ok(ApiResponse<List<PaymentResponseDto>>.Ok(result));
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> RecordPayment([FromBody] PaymentRequestDto dto)
        {
            var result = await _paymentService.RecordPaymentAsync(dto);
            return Ok(ApiResponse<PaymentResponseDto>.Ok(result,
                "Payment recorded successfully"));
        }
    }
}