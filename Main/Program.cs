using Controller;
using Controller.Commands;
using Controller.Interfaces;
using DataAccess;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Presentation;
using Presentation.Interfaces;
using Services;
using Services.Interfaces;

namespace Main;

internal static class Program
{
    static async Task Main()
    {
        var services = new ServiceCollection();

        services.AddDbContext<ItemContext>(options =>
            options.UseNpgsql(Configuration.SqlConnectionString));

        // register sources
        services.AddSingleton<ISourceReader<Clothing>, PostgreSqlSource<Clothing>>();
        services.AddSingleton<ISourceWriter<Clothing>, PostgreSqlSource<Clothing>>();
        services.AddSingleton<ISourceReader<Footwear>, PostgreSqlSource<Footwear>>();
        services.AddSingleton<ISourceWriter<Footwear>, PostgreSqlSource<Footwear>>();

        // register daos
        services.AddSingleton<IItemReader<Clothing>, ItemDao<Clothing>>();
        services.AddSingleton<IItemWriter<Clothing>, ItemDao<Clothing>>();
        services.AddSingleton<IItemReader<Footwear>, ItemDao<Footwear>>();
        services.AddSingleton<IItemWriter<Footwear>, ItemDao<Footwear>>();

        // register services
        services.AddSingleton<IUserService,  UserService>();
        services.AddSingleton<IAdminService, AdminService>();

        // register commands
        services.AddSingleton<AdminCommands>();
        services.AddSingleton<FindCommand>();
        services.AddSingleton<ExitCommand>();
        services.AddSingleton<HelpCommand>();
        services.AddSingleton<SwitchCommand>();
        services.AddSingleton<ForbiddenCommand>();
        services.AddSingleton<WrongCommand>();
        services.AddSingleton<ICommandProvider, CommandProvider>();

        // register controller
        services.AddSingleton<IController, ItemController>();

        // register view
        services.AddSingleton<IView, ConsoleView>();

        var provider = services.BuildServiceProvider();

        // entry point
        var view = provider.GetRequiredService<IView>();

        try
        {
            await view.StartAsync();
        }
        catch (Exception e)
        {
            view.Crash(e.Message);
        }
    }
}
