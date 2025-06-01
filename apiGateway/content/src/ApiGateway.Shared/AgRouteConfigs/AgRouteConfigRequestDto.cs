namespace ApiGateway.AgRouteConfigs;

public class AgRouteConfigRequestDto
{
    public string RouteId { get; set; }
    public string ClusterId { get; set; }
    public AgRouteMatchRequestDto Match { get; set; }
}

public class AgRouteMatchRequestDto
{
    public string Path { get; set; }
}
