using ApiGateway.Repositories;
using Dedsi.Ddd.CQRS.CommandHandlers;
using Dedsi.Ddd.CQRS.Commands;

namespace ApiGateway.AgClusterConfigs.CommamdHandlers;

public record CreateAgClusterConfigCommamd(string ClusterId, List<AgClusterDestinationConfigRequestDto> Destinations) : DedsiCommand<bool>;

public class CreateAgClusterConfigCommamdHandler(IAgClusterConfigRepository agClusterConfigRepository) : DedsiCommandHandler<CreateAgClusterConfigCommamd, bool>
{
    public override async Task<bool> Handle(CreateAgClusterConfigCommamd commamd, CancellationToken cancellationToken)
    {
        var destinations = commamd.Destinations
            .Select(a => new AgClusterDestinationConfig(commamd.ClusterId, a.DestinationId, a.Address))
            .ToList();

        await agClusterConfigRepository.InsertAsync(new AgClusterConfig(commamd.ClusterId, destinations), cancellationToken: cancellationToken);

        return true;
    }
}