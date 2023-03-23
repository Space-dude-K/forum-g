using System.Reflection;

namespace Web_api_tests.Helpers
{
    public static class TestHelper
    {
        public static void SetObjectProperty(string propertyName, object value, object obj)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);

            // Make sure object has the property we are after
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(obj, value, null);
            }
        }
    }
}