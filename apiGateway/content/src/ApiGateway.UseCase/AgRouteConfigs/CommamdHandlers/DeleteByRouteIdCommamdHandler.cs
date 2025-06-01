using ApiGateway.Repositories;
using Dedsi.Ddd.CQRS.CommandHandlers;
using Dedsi.Ddd.CQRS.Commands;

namespace ApiGateway.AgRouteConfigs.CommamdHandlers;

public record DeleteByRouteIdCommamd(string RouteId) : DedsiCommand<bool>;

public class DeleteByRouteIdCommamdHandler(IAgRouteConfigRepository agRouteConfigRepository) : DedsiCommandHandler<DeleteByRouteIdCommamd, bool>
{
    public override Task<bool> Handle(DeleteByRouteIdCommamd command, CancellationToken cancellationToken)
    {
        return agRouteConfigRepository.DeleteByRouteIdAsync(command.RouteId, cancellationToken);
    }
}
