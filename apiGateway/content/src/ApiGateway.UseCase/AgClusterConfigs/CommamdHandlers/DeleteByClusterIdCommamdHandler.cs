using ApiGateway.Repositories;
using Dedsi.Ddd.CQRS.CommandHandlers;
using Dedsi.Ddd.CQRS.Commands;

namespace ApiGateway.AgClusterConfigs.CommamdHandlers;

public record DeleteByClusterIdCommamd(string ClusterId) : DedsiCommand<bool>;

public class DeleteByClusterIdCommamdHandler(IAgClusterConfigRepository agClusterConfigRepository) : DedsiCommandHandler<DeleteByClusterIdCommamd, bool>
{
    public override Task<bool> Handle(DeleteByClusterIdCommamd command, CancellationToken cancellationToken)
    {
        return agClusterConfigRepository.DeleteByClusterIdAsync(command.ClusterId, cancellationToken);
    }
}
