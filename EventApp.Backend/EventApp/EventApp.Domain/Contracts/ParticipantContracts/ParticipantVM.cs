using AutoMapper;

namespace EventApp
{
    public class ParticipantVM : IMapWith<ParticipantModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = string.Empty;
        public List<EventParticipant> Events { get; set; } = [];

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ParticipantModel, ParticipantVM>()
                .ForMember(vm => vm.Id, opt => opt.MapFrom(model => model.Id))
                .ForMember(vm => vm.Events, opt => opt.MapFrom(model => model.Events))
                .ForMember(vm => vm.Name, opt => opt.MapFrom(model => model.Name))
                .ForMember(vm => vm.Surname, opt => opt.MapFrom(model => model.Surname))
                .ForMember(vm => vm.BirthDate, opt => opt.MapFrom(model => model.BirthDate))
                .ForMember(vm => vm.Email, opt => opt.MapFrom(model => model.Email))
                .ReverseMap();
        }
    }
}
