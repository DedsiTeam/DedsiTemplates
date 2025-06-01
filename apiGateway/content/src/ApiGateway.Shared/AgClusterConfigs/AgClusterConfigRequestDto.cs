namespace ApiGateway.AgClusterConfigs;
public class AgClusterConfigRequestDto
{
    public string ClusterId { get; set; }

    public List<AgClusterDestinationConfigRequestDto> Destinations { get; set; }
}

public class AgClusterDestinationConfigRequestDto
{

    public string DestinationId { get; set; }

    public string Address { get; set; }
}