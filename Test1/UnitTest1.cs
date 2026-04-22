namespace Test1
{
    public class Test1
    {
        [Fact]
        public void Convert_USD_To_EUR_ShouldReturnValidResult()
        {
            // Arrange
            var service = new CurrencyService(); // your service
            string from = "USD";
            string to = "EUR";
            double amount = 10;

            // Act
            var result = service.Convert(from, to, amount);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void Convert_InvalidCurrency_ShouldThrowException()
        {
            var service = new CurrencyService();

            Assert.Throws<ArgumentException>(() =>
                service.Convert("XXX", "EUR", 10));
        }

        [Fact]
        public void Convert_ZeroAmount_ShouldReturnZero()
        {
            var service = new CurrencyService();

            var result = service.Convert("USD", "EUR", 0);

            Assert.Equal(0, result);
        }
    }
}