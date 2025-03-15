namespace MicroServiceProducts.Domain.Common.Helpers
{    
    public class DecimalValidationHelper
    {
        public static bool IsDecimal(string value)
        {
            return decimal.TryParse(value, out _);
        }
    }
}
