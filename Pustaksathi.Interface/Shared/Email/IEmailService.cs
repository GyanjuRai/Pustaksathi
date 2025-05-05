

using Pustaksathi.Model.Application.Members;

namespace Pustaksathi.Interface.Shared.Email
{
    public interface IEmailService
    {
        /// <summary>
        /// Send Order Confirmation Email with claimcode and membership id
        /// </summary>
        /// <returns></returns>
        public Task SendOrderConfirmationAsync(Orders order, string userName, string userEmail, string? invoiceUrl = null );
    }
}
