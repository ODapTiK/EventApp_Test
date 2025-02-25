﻿using FluentValidation;

namespace EventApp
{
    public class UpdateParticipantUseCase : IUpdateParticipantUseCase
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IValidator<UpdateParticipantDTO> _validator;

        public UpdateParticipantUseCase(IParticipantRepository participantRepository, IValidator<UpdateParticipantDTO> validator)
        {
            _participantRepository = participantRepository;
            _validator = validator;
        }

        public async Task Execute(UpdateParticipantDTO updateParticipantDTO, Guid userId)
        {
            var validationResult = await _validator.ValidateAsync(updateParticipantDTO);
            if(!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var participant = await _participantRepository.FindParticipantAsync(userId, CancellationToken.None);

            if (participant == null) throw new ParticipantNotFoundException(nameof(ParticipantModel), userId);

            else
            {
                participant.Name = updateParticipantDTO.Name;
                participant.BirthDate = updateParticipantDTO.BirthDate.ToUniversalTime();
                participant.Surname = updateParticipantDTO.Surname;
                await _participantRepository.UpdateAsync(CancellationToken.None);
            }
        }
    }
}
