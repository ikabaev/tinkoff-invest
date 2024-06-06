using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Invest.Services
{
    public class TypeConfigurationSection
    {
        //[ConfigurationKeyName("Type")]
        public string? Type { get; set; }
        //[ConfigurationKeyName("Assembly")]
        public string? Assembly { get; set; }
    }
    public static class ProviderFactory
    {
        public static object? Create(IConfigurationSection section) 
        {
            var settings = section.Get<TypeConfigurationSection>();
            if (settings == null || settings.Type == null)
                throw new ArgumentNullException(nameof(section));


            if (settings.Assembly == null)
            {
                var type = Type.GetType(settings.Type) ?? throw new ArgumentNullException(nameof(section));
                var instance = Activator.CreateInstance(type, [section]);

                return instance;

            }
            else
            {
                var asm = Assembly.LoadFrom(settings.Assembly);
                var instance = asm.CreateInstance(settings.Type, true, BindingFlags.CreateInstance, null, args: [section], null, null);
                
                //var instance = Activator.CreateInstance(settings.Assembly, settings.Type, [section]);
                return instance;
            }
        }
    }
}
