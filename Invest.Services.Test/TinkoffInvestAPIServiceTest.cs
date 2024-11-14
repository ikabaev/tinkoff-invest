namespace Invest.Services.Test
{
    public class TinkoffInvestAPIServiceTest
    {
        List<float> _quotes = [];

        [SetUp]
        public void Setup()
        {
        }

        private bool TestOpen(float quote)
        {
            return false;
        }

        private bool TestClose(float quote)
        {
            return false;
        }

        [Test]
        public void OpenSandboxAccountTest()
        {
            //var service = new TinkoffInvestAPIService();
            //var result = service.OpenSandboxAccount();
            //Assert.True(result);
        }

        [Test]
        public void MaxPL()
        {
            foreach (var quote in _quotes)
            {
                if (this.TestOpen(quote)) { 
                    
                }

                if (this.TestClose(quote))
                {

                }
            }
        }
    }
}