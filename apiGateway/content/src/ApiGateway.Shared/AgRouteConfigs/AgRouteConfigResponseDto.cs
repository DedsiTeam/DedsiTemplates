namespace ApiGateway.AgRouteConfigs;

public class AgRouteConfigResponseDto
{
    public string RouteId { get; set; }
    public string ClusterId { get; set; }
    public AgRouteMatchResponseDto Match { get; set; }
}

public class AgRouteMatchResponseDto
{
    public string RouteId { get; set; }
    public string Path { get; set; }
}
