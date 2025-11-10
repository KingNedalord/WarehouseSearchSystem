using Warehouse.DAO;

namespace Warehouse.Services;
/// <summary>
/// Factory class for creating service layer objects.
/// 
/// Implements Singleton pattern and IServiceFactory interface.
/// Allows configuring services and creating them on demand.
/// 
/// Working principles:
/// 1. Service configuration through Configure()
/// 2. Service creation through CreateService()
/// 3. Fallback logic - creating service with dependencies if not configured
/// 
/// Usage example:
/// <code>
/// // Service configuration
/// ServiceFactory.Instance.Configure(new ItemService());
/// 
/// // Service creation
/// var service = ServiceFactory.Instance.CreateService();
/// 
/// // Or through static method for backward compatibility
/// var service = ServiceFactory.CreateItemService();
/// </code>
/// </summary>
public class ServiceFactory : IServiceFactory
{
    private static readonly ServiceFactory instance = new ServiceFactory();
    private IItemService service;

    private ServiceFactory() { }

    /// <summary>
    /// Single instance of ServiceFactory (Singleton pattern).
    /// </summary>
    public static ServiceFactory Instance => instance;

    /// <summary>
    /// Configures the application service.
    /// 
    /// After configuration, when calling CreateService() will return
    /// the registered instance instead of creating a new one.
    /// </summary>
    /// <param name="service">Service to register</param>
    public void Configure(IItemService service)
    {
        this.service = service;
    }

    /// <summary>
    /// Creates an application service.
    /// 
    /// Creation algorithm:
    /// 1. If service was configured - returns it
    /// 2. Otherwise, creates new ItemService with default dependencies
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