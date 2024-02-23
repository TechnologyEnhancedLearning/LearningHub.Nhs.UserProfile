// <copyright file="ErrorViewModel.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Models
{
	/// <summary>
	/// Defines the <see cref="ErrorViewModel" />.
	/// </summary>
	public class ErrorViewModel
	{
		/// <summary>
		/// Gets or sets the RequestId.
		/// </summary>
		public string? RequestId { get; set; }

		/// <summary>
		/// Gets a value indicating whether the ShowRequestId.
		/// </summary>
		public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
	}
}