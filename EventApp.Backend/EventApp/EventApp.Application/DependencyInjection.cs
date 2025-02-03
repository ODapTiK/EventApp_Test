using Microsoft.Extensions.DependencyInjection;

namespace EventApp
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<IParticipantService, ParticipantService>();

            return services;
        }
    }
}
