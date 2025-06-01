using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace ApiGateway.AgClusterConfigs;


public class AgClusterConfig : AggregateRoot
{
    protected AgClusterConfig()
    {
    }

    public AgClusterConfig(string clusterId, List<AgClusterDestinationConfig> destinations)
    {
        ClusterId = clusterId;
        Destinations = destinations;
    }

    public string ClusterId { get; private set; }

    public void ChangeClusterId(string newClusterId)
    {
        ClusterId = Check.NotNullOrWhiteSpace(newClusterId, nameof(ClusterId));
    }

    public ICollection<AgClusterDestinationConfig> Destinations { get; private set; } = new List<AgClusterDestinationConfig>();

    public override object?[] GetKeys()
    {
        return [ClusterId];
    }
}

public class AgClusterDestinationConfig
{
    public AgClusterDestinationConfig(string clusterId, string destinationId, string address)
    {
        ClusterId = clusterId;
        DestinationId = destinationId;
        Address = address;
    }

    public string ClusterId { get; private set; }

    public string DestinationId { get; private set; }
    
    public string Address { get; private set; }
}
