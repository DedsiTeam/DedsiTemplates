using ApiGateway.Repositories;
using Dedsi.Ddd.CQRS.CommandHandlers;
using Dedsi.Ddd.CQRS.Commands;

namespace ApiGateway.AgRouteConfigs.CommamdHandlers;

public record CreateAgRouteConfigCommamd(string RouteId, string ClusterId, AgRouteMatchRequestDto MatchRequestDto) : DedsiCommand<bool>;

public class CreateAgRouteConfigCommamdHandler(IAgRouteConfigRepository agRouteConfigRepository) : DedsiCommandHandler<CreateAgRouteConfigCommamd, bool>
{
    public override async Task<bool> Handle(CreateAgRouteConfigCommamd commamd, CancellationToken cancellationToken)
    {
        var agRouteMatch = new AgRouteMatch(commamd.RouteId, commamd.MatchRequestDto.Path);

        await agRouteConfigRepository.InsertAsync(new AgRouteConfig(commamd.RouteId, commamd.ClusterId, agRouteMatch), cancellationToken: cancellationToken);

        return true;
    }
}