using Warehouse.Presentation;
using Warehouse.DAO;

internal class Program
{
    static void Main(string[] args)
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;

        // Use relative paths that match the repository layout when running from the project's output folder
        var clothingCsv = Path.Combine(baseDir, "..", "..", "..", "Data", "clothing.csv");
        var footwearCsv = Path.Combine(baseDir, "..", "..", "..", "Data", "footwear.csv");

        try
        {
            ItemDaoFactory.Instance.ConfigureSource(new ClothingCsvSource(clothingCsv));
            ItemDaoFactory.Instance.ConfigureSource(new FootwearCsvSource(footwearCsv));

            var viewFactory = ViewFactory.Instance;
            var view = viewFactory.CreateView();
            view.Start();
        }
        catch (Exception ex)
        {
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
        finally
        {
            ItemDaoFactory.Instance.CloseSources();
        }
    }
}