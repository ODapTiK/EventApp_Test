using AutoMapper;

namespace EventApp
{
    public class GetParticipantInfoUseCase : IGetParticipantInfoUseCase
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IMapper _mapper;

        public GetParticipantInfoUseCase(IParticipantRepository participantRepository, IMapper mapper)
        {
            _participantRepository = participantRepository;
            _mapper = mapper;
        }

        public async Task<ParticipantVM> Execute(Guid userId)
        {
            if (userId.Equals(Guid.Empty)) throw new FluentValidation.ValidationException("Participant id can not be empty!");

            var participant = await _participantRepository.GetAsync(userId, CancellationToken.None);

            if(participant == null) throw new ParticipantNotFoundException(nameof(ParticipantModel), userId);

            return _mapper.Map<ParticipantVM>(participant);
        }
    }
}
