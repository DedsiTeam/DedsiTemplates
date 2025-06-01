using Dedsi.CleanArchitecture.Domain;

namespace ApiGateway;

public class ApiGatewayDomainConsts : DedsiCleanArchitectureDomainConsts
{
    public const string ApplicationName = "ApiGateway";
    
    public const string ConnectionStringName = "ApiGatewayDB";

    public const string DbSchemaName = DefaultDbSchema;
}