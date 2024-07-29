namespace Hng.Application.Utils
{
    public class GenerateTransactionReference
    {
        public static string GenerateReference() => $"hng{DateTime.Now.Ticks}";
    }
}
