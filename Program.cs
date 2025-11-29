using Warehouse.Presentation;
using Warehouse.DAO;

internal class Program
{
    static void Main(string[] args)
    {
        // Configure data sources for DAOs using CSV files in the Data folder
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;

        // Use relative paths that match the repository layout when running from the project's output folder
        var clothingCsv = Path.Combine(baseDir, "..", "..", "..", "Data", "clothing.csv");
        var footwearCsv = Path.Combine(baseDir, "..", "..", "..", "Data", "footwear.csv");

        try
        {
            // Configure DAO factory with CSV sources
            ItemDaoFactory.Instance.ConfigureSource(new ClothingCsvSource(clothingCsv));
            ItemDaoFactory.Instance.ConfigureSource(new FootwearCsvSource(footwearCsv));

            // Create and start the view
            var viewFactory = ViewFactory.Instance;
            var view = viewFactory.CreateView();
            view.Start();
        }
        catch (Exception ex)
        {
            // If the view is available, show crash info, otherwise write to stderr
            try
            {
                var view = ViewFactory.Instance.CreateView();
                view.Crash();
            }
            catch
            {
                // ignore
            }

            Console.Error.WriteLine($"Application failed to start: {ex.Message}");
        }
    }
}