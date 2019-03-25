namespace Devon4Net.Infrastructure.CircuitBreaker.Common.Entities
{
    using System;
    using System.Collections.Generic;

    public class EndPointEntity
    {
        public string Name { get; set; }
        public string BaseAddress { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public List<int> WaitAndRetrySeconds { get; set; }
        public int DurationOfBreak { get; set; }

        public EndPointEntity()
        {
        }

        public EndPointEntity(string name, string baseAddress)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(baseAddress)) throw new ArgumentNullException("name", "Name or BaseAddress cannot be null");

            Name = name;
            BaseAddress = baseAddress;
            Headers = new Dictionary<string, string>();
            WaitAndRetrySeconds = new List<int> { 1 };
        }

        public Dictionary<string, string> GetHeaders()
        {
            return Headers ?? new Dictionary<string, string>();
        }

        public List<int> GetWaitAndRetry()
        {
            return WaitAndRetrySeconds ?? new List<int> { 1 };
        }


        public void AddWaitAndRetry(int secondsToWait)
        {
            if (secondsToWait <= 0) throw new ArgumentNullException("secondsToWait", "The seconds to wait must be greater than zero");
            WaitAndRetrySeconds.Add(secondsToWait);
        }

        public void AddRequestHeader(string key, string value, bool overwriteValue = false)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value)) throw new ArgumentNullException("key", "Key or Value cannot be null");
            var keyExists = Headers.ContainsKey(key);

            if (Headers == null)
            {
                Headers = new Dictionary<string, string>
                {
                    { key, value }
                };
                return;
            }

            if (overwriteValue)
            {
                Headers.Remove(key);
            }
            else
            {
                if (keyExists) throw new ArgumentNullException("key", "The provided key already exits");
                Headers.Add(key, value);
            }
        }
    }
}
