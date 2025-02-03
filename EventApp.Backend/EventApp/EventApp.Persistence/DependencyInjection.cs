using Microsoft.Extensions.DependencyInjection;

namespace EventApp
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IParticipantRepository, ParticipantRepository>();
            services.AddTransient<IPasswordEncryptor, PasswordEncryptor>();
            services.AddTransient<IJwtProvider, JwtProvider>();
            return services;
        }
    }
}
