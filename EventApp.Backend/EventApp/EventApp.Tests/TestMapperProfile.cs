using AutoMapper;

namespace EventApp
{
    public class TestMapperProfile : Profile
    {
        public TestMapperProfile()
        {
            CreateMap<EventModel, EventVM>()
                .ForMember(vm => vm.Id, opt => opt.MapFrom(model => model.Id))
                .ForMember(vm => vm.Title, opt => opt.MapFrom(model => model.Title))
                .ForMember(vm => vm.Description, opt => opt.MapFrom(model => model.Description))
                .ForMember(vm => vm.EventDateTime, opt => opt.MapFrom(model => model.EventDateTime))
                .ForMember(vm => vm.Venue, opt => opt.MapFrom(model => model.Venue))
                .ForMember(vm => vm.Category, opt => opt.MapFrom(model => model.Category))
                .ForMember(vm => vm.MaxParticipants, opt => opt.MapFrom(model => model.MaxParticipants))
                .ForMember(vm => vm.Image, opt => opt.MapFrom(model => model.Image))
                .ForMember(vm => vm.Participants, opt => opt.MapFrom(model => model.Participants.Select(ep => new EventParticipant
                {
                    ParticipantId = ep.ParticipantId,
                    RegistrationDate = ep.RegistrationDate,
                    EventId = ep.EventId,
                    Participant = new ParticipantModel
                    {
                        Name = ep.Participant.Name,
                        Surname = ep.Participant.Surname
                    }
                }).ToList()));
        }
    }
}
