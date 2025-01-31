using Autofac;

namespace GetValueFromAPIExample.DependencyInjection
{
    public class ServiceRegistration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<ExternalAPIService>().As<IExternalAPIService>();
        }
    }
}
