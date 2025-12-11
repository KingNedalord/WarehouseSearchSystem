using Warehouse.DAO;

namespace Warehouse.Services;
/// <summary>
/// Factory class for creating service layer objects.
/// 
/// Implements Singleton pattern and IServiceFactory interface.
/// Allows configuring services and creating them on demand.
/// </summary>
public class ServiceFactory : IServiceFactory
{
    private static readonly ServiceFactory instance = new();
    private IItemService? service;

    private ServiceFactory() { }

    /// <summary>
    /// Single instance of ServiceFactory (Singleton pattern).
    /// </summary>
    public static ServiceFactory Instance => instance;

    /// <summary>
    /// Configures the application service.
    /// </summary>
    /// <param name="service">Service to register</param>
    public void Configure(IItemService service)
    {
        this.service = service;
    }

    /// <summary>
    /// Creates an application service.
    /// </summary>
    /// <returns>IItemService instance</returns>
    public IItemService CreateService()
    {
        if (service != null)
        {
            return service;
        }

        return new ItemService(ItemDaoFactory.Instance);
    }
}