// <copyright file="MappingProfile.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.LearningCredentials.Models.Automapper
{
    using AutoMapper;
    using LearningHub.Nhs.LearningCredentials.Models.Dsp;
    using LearningHub.Nhs.LearningCredentials.Models.Enums.Dsp;
    using LearningHub.Nhs.LearningCredentials.Models.System;
    using LearningHub.Nhs.LearningCredentials.Models.User;

    /// <summary>
    /// The mapping profile.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            this.CreateMap<Entities.User.UserProfile, UserProfileResponse>();
            this.CreateMap<UserProfileRequest, Entities.User.UserProfile>();

            this.CreateMap<Entities.System.ClientSystem, ClientSystemResponse>();

            this.CreateMap<Entities.Dsp.VerifiableCredential, VerifiableCredentialResponse>()
                .ForMember(d => d.PeriodUnit, x => x.MapFrom(s => (PeriodUnitEnum)s.PeriodUnitId));
            this.CreateMap<Entities.Dsp.UserVerifiableCredential, UserVerifiableCredentialResponse>()
                .ForMember(d => d.UserVerifiableCredentialId, x => x.MapFrom(s => s.Id))
                .ForMember(d => d.CredentialName, x => x.MapFrom(s => s.VerifiableCredential.CredentialName))
                .ForMember(d => d.Status, x => x.MapFrom(s => (UserVerifiableCredentialStatusEnum)s.UserVerifiableCredentialStatusId));
            this.CreateMap<UserVerifiableCredentialRequest, Entities.Dsp.UserVerifiableCredential>()
                .ForMember(d => d.UserVerifiableCredentialStatusId, x => x.MapFrom(s => (int)s.Status))
                .ForMember(d => d.Id, x => x.MapFrom(s => s.UserVerifiableCredentialId));
        }
    }
}
