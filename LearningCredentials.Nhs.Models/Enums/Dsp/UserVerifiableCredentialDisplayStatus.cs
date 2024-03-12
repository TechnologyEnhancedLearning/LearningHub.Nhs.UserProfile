// <copyright file="UserVerifiableCredentialDisplayStatus.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.LearningCredentials.Models.Enums.Dsp
{
    /// <summary>
    /// The user verifiable credential status enum.
    /// </summary>
    public enum UserVerifiableCredentialDisplayStatusEnum
    {
        /// <summary>
        /// Not available.
        /// </summary>
        NotAvailable = 1,

        /// <summary>
        /// Not sent to digital wallet.
        /// </summary>
        NotSentToDigitalWallet = 2,

        /// <summary>
        /// Sent to digital wallet.
        /// </summary>
        SentToDigitalWallet = 3,

        /// <summary>
        /// Later activity available.
        /// </summary>
        LaterActivityAvailable = 4,
    }
}
