namespace Hackathon.Foundation.DependencyInjection.IoC
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;

    public class MvcControllerServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("Hackathon.Feature.*");
            serviceCollection.AddApiControllers("Hackathon.Feature.*");
            serviceCollection.AddClassesWithServiceAttribute("Hackathon.Feature.*");
            serviceCollection.AddMvcControllers("Hackathon.Foundation.*");
            serviceCollection.AddApiControllers("Hackathon.Foundation.*");
            serviceCollection.AddClassesWithServiceAttribute("Hackathon.Foundation.*");
        }
    }
}