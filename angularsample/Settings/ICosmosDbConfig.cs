namespace AngularPerformanceSamples.Settings
{
    public interface ICosmosDbConfig
    {
        string EndpointUrl { get; }
        string PrimaryKey { get; }
    }
}
