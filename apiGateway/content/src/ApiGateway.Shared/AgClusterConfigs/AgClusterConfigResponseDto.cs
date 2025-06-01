namespace ApiGateway.AgClusterConfigs;

public class AgClusterConfigResponseDto
{
    public string ClusterId { get; set; }
    public List<AgClusterDestinationConfigResponseDto> Destinations { get; set; }
}

public class AgClusterDestinationConfigResponseDto
{
    public string DestinationId { get; set; }
    public string Address { get; set; }
}
