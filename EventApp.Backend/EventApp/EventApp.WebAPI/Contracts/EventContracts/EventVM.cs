using AutoMapper;
using System;

namespace EventApp
{
    public class EventVM : IMapWith<EventModel>
    {

        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDateTime { get; set; }
        public string Venue { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int MaxParticipants { get; set; }
        public string ImageBase64 { get; set; } = string.Empty;
        public List<EventParticipant> Participants { get; set; } = [];

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventModel, EventVM>()
                .ForMember(vm => vm.Id, opt => opt.MapFrom(model => model.Id))
                .ForMember(vm => vm.Title, opt => opt.MapFrom(model => model.Title))
                .ForMember(vm => vm.Description, opt => opt.MapFrom(model => model.Description))
                .ForMember(vm => vm.EventDateTime, opt => opt.MapFrom(model => model.EventDateTime))
                .ForMember(vm => vm.Venue, opt => opt.MapFrom(model => model.Venue))
                .ForMember(vm => vm.Category, opt => opt.MapFrom(model => model.Category))
                .ForMember(vm => vm.MaxParticipants, opt => opt.MapFrom(model => model.MaxParticipants))
                .ForMember(vm => vm.Participants, opt => opt.MapFrom(model => model.Participants))
                .ForMember(vm => vm.ImageBase64, opt => opt.MapFrom(model => model.Image));
        }
    }
}
