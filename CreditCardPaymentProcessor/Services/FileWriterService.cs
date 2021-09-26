using System.IO;
using System.Threading.Tasks;

namespace CreditCardPaymentProcessor.Services
{
    public class FileWriterService : IFileWriterService
    {
        public async Task AppendToFileAsync(string fileName, string text)
        {
            using StreamWriter file = new StreamWriter(fileName, append: true);
            await file.WriteLineAsync(text);

        }
    }
}
