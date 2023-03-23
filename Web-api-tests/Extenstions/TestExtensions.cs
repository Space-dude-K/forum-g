using Microsoft.EntityFrameworkCore.Metadata;
using Web_api_tests.Helpers;

namespace Web_api_tests.Extensions
{
    public static class TestExtensions
    {
        public static List<T> GetPopulatedModelWithSeedDataFromConfig<T>(this IModel model)
        {
            var test = model.GetEntityTypes();
            List<T> result = new();

            //foreach (var prop in test.Where(p => p.ClrType.Name.StartsWith(modelFilter)))
            foreach (var prop in test.Where(p => p.ClrType.Equals(typeof(T))))
            {
                var seedData = prop.GetSeedData(true);

                foreach (var dicts in seedData)
                {
                    var typeInstance = Activator.CreateInstance(prop.ClrType);

                    foreach (var kvp in dicts)
                    {
                        TestHelper.SetObjectProperty(kvp.Key, kvp.Value, typeInstance);
                    }

                    result.Add((T)typeInstance);
                    /*output.WriteLine("Type: " + typeInstance.GetType().ToString());

                    foreach (var propOfObject in typeInstance.GetType().GetProperties())
                    {
                        var propName = propOfObject.Name;
                        var propValue = propOfObject.GetValue(typeInstance, null);

                        output.WriteLine("obj prop -> " + propName + " " + propValue);
                    }*/
                }
            }

            return result;
        }
        public static List<object[]> GetPopulatedModelWithSeedDataFromConfigForTestCase<T>(this IModel model)
        {
            List<object[]> result = new();

            foreach (var prop in model.GetEntityTypes().Where(p => p.ClrType.Equals(typeof(T))))
            {
                var seedData = prop.GetSeedData(true);

                foreach (var dicts in seedData)
                {
                    var typeInstance = Activator.CreateInstance(prop.ClrType);

                    foreach (var kvp in dicts)
                    {
                        TestHelper.SetObjectProperty(kvp.Key, kvp.Value, typeInstance);
                    }

                    result.Add(new object[] { (T)typeInstance });
                }
            }

            return result;
        }
    }
}