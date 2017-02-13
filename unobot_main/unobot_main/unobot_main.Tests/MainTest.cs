using Amazon.Lambda.TestUtilities;
using Xunit;

namespace unobot_main.Tests
{
    public class MainTest
    {
        [Fact]
        public void TestToUpperFunction()
        {
            // Invoke the lambda function and confirm the string was upper cased.
            var function = new Main();
            var context = new TestLambdaContext();
            var upperCase = function.MainHandler("hello world", context);

            Assert.Equal("HELLO WORLD", upperCase);
        }
    }
}