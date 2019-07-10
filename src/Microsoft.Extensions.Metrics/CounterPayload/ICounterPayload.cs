namespace Microsoft.Extensions.Metrics
{
    public interface ICounterPayload
    {
        public string Name { get;}
        public string Value { get;}
        public string DisplayName { get;}
    }
}