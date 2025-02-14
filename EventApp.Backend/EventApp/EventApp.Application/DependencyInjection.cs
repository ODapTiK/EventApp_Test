using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EventApp
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblies([Assembly.GetExecutingAssembly()]);

            services.AddScoped<ICreateAdminUseCase, CreateAdminUseCase>();
            services.AddScoped<IDeleteAdminUseCase, DeleteAdminUseCase>();
            services.AddScoped<IAuthenticateAdminUseCase, AuthenticateAdminUseCase>();

            services.AddScoped<ICreateEventUseCase, CreateEventUseCase>();
            services.AddScoped<IUpdateEventUseCase, UpdateEventUseCase>();
            services.AddScoped<IDeleteEventUseCase, DeleteEventUseCase>();
            services.AddScoped<IGetEventByIdUseCase, GetEventByIdUseCase>();
            services.AddScoped<IGetEventsPageUseCase, GetEventsPageUseCase>();
            services.AddScoped<IGetEventsPageByParamsUseCase, GetEventsPageByParamsUseCase>();

            services.AddScoped<ICreateParticipantUseCase, CreateParticipantUseCase>();
            services.AddScoped<IUpdateParticipantUseCase, UpdateParticipantUseCase>();
            services.AddScoped<IDeleteParticipantUseCase, DeleteParticipantUseCase>();
            services.AddScoped<IAuthenticateParticipantUseCase, AuthenticateParticipantUseCase>();
            services.AddScoped<IGetParticipantInfoUseCase, GetParticipantInfoUseCase>();
            services.AddScoped<IGetParticipantEventsUseCase, GetParticipantEventsUseCase>();
            services.AddScoped<ISubscribeToEventUseCase, SubscribeToEventUseCase>();
            services.AddScoped<IUnsubscribeFromEventUseCase, UnsubscribeFromEventUseCase>();

            return services;
        }
    }
}
