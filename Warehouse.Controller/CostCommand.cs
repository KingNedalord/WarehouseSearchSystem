using Warehouse.Services;

/// <summary>
/// Command for finding items within a specified price range
/// </summary>
public class CostCommand : ICommand
{
    private readonly IItemService service;

    /// <summary>
    /// Creates a new CostCommand with default service factory
    /// </summary>
    public CostCommand() : this(ServiceFactory.Instance)
    {
    }

    /// <summary>
    /// Creates a new CostCommand with injected service factory
    /// </summary>
    /// <param name="serviceFactory">Service factory to use</param>
    public CostCommand(IServiceFactory serviceFactory)
    {
        service = (serviceFactory ?? ServiceFactory.Instance).CreateService();
    }

    /// <summary>
    /// Creates a new CostCommand with injected service
    /// </summary>
    /// <param name="service">Item service to use</param>
    public CostCommand(IItemService service)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Executes the cost command to find items within a price range
    /// </summary>
    /// <param name="request">Request containing minimum and maximum price parameters</param>
    /// <returns>Response containing items within the specified price range</returns>
    public Response Execute(Request request)
    {
        throw new NotImplementedException();
    }
}