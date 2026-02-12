using System.Text;

namespace DomainService;

public class TransactionSizeCalculator
{
    public int Calculate(string json)
    {
        var bytes = Encoding.UTF8.GetBytes(json);
        return bytes.Length;
    }
}