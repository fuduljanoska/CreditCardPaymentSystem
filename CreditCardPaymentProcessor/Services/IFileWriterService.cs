using System.Threading.Tasks;

namespace CreditCardPaymentProcessor.Services
{
    public interface IFileWriterService
    {
        Task AppendToFileAsync(string fileName, string text);
    }
}
