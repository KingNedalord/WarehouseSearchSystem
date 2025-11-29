// using Warehouse.Presentation;
// using Warehouse.DAO;

// internal class Program
// {
//     static void Main(string[] args)
//     {
//         var baseDir = AppDomain.CurrentDomain.BaseDirectory;

//         // Paths to CSV files relative to repo layout
//         var clothingCsv = Path.Combine(baseDir, "..", "..", "Data", "clothing.csv");
//         var footwearCsv = Path.Combine(baseDir, "..", "..", "Data", "footwear.csv");

//         try
//         {
//             ItemDaoFactory.Instance.ConfigureSource(new ClothingCsvSource(clothingCsv));
//             ItemDaoFactory.Instance.ConfigureSource(new FootwearCsvSource(footwearCsv));

//             var view = ViewFactory.Instance.CreateView();
//             view.Start();
//         }
//         catch (Exception ex)
//         {
//             try { ViewFactory.Instance.CreateView().Crash(); } catch { }
//             Console.Error.WriteLine($"Runner failed to start: {ex.Message}");
//         }
//     }
// }
