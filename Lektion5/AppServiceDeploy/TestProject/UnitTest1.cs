using WeatherAPIDeploy;

namespace TestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Temperature_ShouldBeInValidRange()
        {
            var forecast = new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                TemperatureC = 25,
                Summary = "Warm"
            };

            Assert.InRange(forecast.TemperatureC, -50, 50);
        }

        [Fact]
        public void TemperatureF_ShouldConvertCorrectly()
        {
            var forecast = new WeatherForecast
            {
                TemperatureC = 0
            };

            var tempF = forecast.TemperatureF;

            Assert.Equal(32, tempF);
        }
    
        //[Fact]
        //public void Temperature_ShouldAlwaysFail()
        //{
        //    var forecast = new WeatherForecast
        //    {
        //        TemperatureC = 25
        //    };

        //    Assert.Fail("Detta test är designat att faila!");
        //}
    }
}
