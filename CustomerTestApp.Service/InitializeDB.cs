using CustomerTestApp.Service.Models;
using Newtonsoft.Json;

namespace CustomerTestApp.Service
{
    public static class InitializeDB
    {
        public static void Initialize(CustomerContext context)
        {
            using (context)
            {
                if (context.Customers.Any())
                {
                    return;
                }

                var customers = GetMockData();
                if (customers != null)
                {
                    context.Customers.AddRange(customers);
                    context.SaveChanges();
                }
            }
        }

        private static List<Models.Customer> GetMockData()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "MockData.json");
            if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<Models.Customer>>(jsonData);
            }
            return null;
        }
    }
}
