using Microsoft.Extensions.DependencyInjection;

namespace EventApp
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IPasswordEncryptor, PasswordEncryptor>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            return services;
        }
    }
}
