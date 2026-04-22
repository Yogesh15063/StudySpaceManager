using StudySpace.API.DTOs.Payment;

namespace StudySpace.API.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<List<PaymentResponseDto>> GetAllPaymentsAsync();
        Task<List<PaymentResponseDto>> GetMyPaymentsAsync(int userId);
        Task<List<PaymentResponseDto>> GetOverduePaymentsAsync();
        Task<PaymentResponseDto> RecordPaymentAsync(PaymentRequestDto dto);
    }
}