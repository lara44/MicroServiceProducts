

namespace Domain.ValueObjects
{
    public class Price
    {
        public decimal Amount { get; private set; }

        public Price(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));

            if (decimal.Round(amount, 2) != amount)
                throw new ArgumentException("Amount can only have up to 2 decimal places.", nameof(amount));

            Amount = amount;
        }
    }
}