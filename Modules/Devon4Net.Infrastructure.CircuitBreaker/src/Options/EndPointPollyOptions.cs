namespace Devon4Net.Infrastructure.CircuitBreaker.Options
{
    using Infrastructure.CircuitBreaker.Common.Entities;

    public class EndPointPollyOptions
    {
        public EndPointEntity[] CircuitBreaker { get; set; }
        public EndPointPollyOptions()
        {

        }
    }
}
