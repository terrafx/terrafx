// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Globalization;
using System.Resources;

namespace TerraFX
{
    /// <summary>Provides a set of methods for looking up localized resources.</summary>
    public static class Resources
    {
        private static readonly ResourceManager s_resourceManager = new ResourceManager(typeof(Resources));
        private static CultureInfo? s_culture;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>{0} is an instance of {1}</c>.</summary>
        public static string ArgumentExceptionForInvalidTypeMessage => ResourceManager.GetString(nameof(ArgumentExceptionForInvalidTypeMessage), Culture)!;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>{0} is null</c>.</summary>
        public static string ArgumentNullExceptionMessage => ResourceManager.GetString(nameof(ArgumentNullExceptionMessage), Culture)!;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>{0} has a value of {1}</c>.</summary>
        public static string ArgumentOutOfRangeExceptionMessage => ResourceManager.GetString(nameof(ArgumentOutOfRangeExceptionMessage), Culture)!;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>Cannot enumerate audio devices when not running</c>.</summary>
        public static string CannotEnumerateAudioDevicesWhenNotRunningMessage => ResourceManager.GetString(nameof(CannotEnumerateAudioDevicesWhenNotRunningMessage), Culture)!;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>Could not connect stream for playback</c>.</summary>
        public static string CouldNotConnectPlaybackStreamMessage => ResourceManager.GetString(nameof(CouldNotConnectPlaybackStreamMessage), Culture)!;

        /// <summary>Gets or sets the <see cref="CultureInfo" /> used during resource lookup.</summary>
        /// <remarks>When this property has a value of <c>null</c>, <see cref="CultureInfo.CurrentUICulture" /> is used instead.</remarks>
        public static CultureInfo? Culture
        {
            get
            {
                return s_culture;
            }

            set
            {
                s_culture = value;
            }
        }

        /// <summary>Gets a localized <see cref="string" /> similar to <c>Device is already started</c>.</summary>
        public static string DeviceAlreadyStartedMessage => ResourceManager.GetString(nameof(DeviceAlreadyStartedMessage), Culture)!;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>Device is not completed</c>.</summary>
        public static string DeviceNotCompletedMessage => ResourceManager.GetString(nameof(DeviceNotCompletedMessage), Culture)!;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>{0} failed with an error code of {1}</c>.</summary>
        public static string ExternalExceptionMessage => ResourceManager.GetString(nameof(ExternalExceptionMessage), Culture)!;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>{0} has a value of {1}</c>.</summary>
        public static string InvalidOperationExceptionMessage => ResourceManager.GetString(nameof(InvalidOperationExceptionMessage), Culture)!;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>Invalid sample format</c>.</summary>
        public static string InvalidSampleFormatMessage => ResourceManager.GetString(nameof(InvalidSampleFormatMessage), Culture)!;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>An I/O error occurred</c>.</summary>
        public static string IOExceptionMessage => ResourceManager.GetString(nameof(IOExceptionMessage), Culture)!;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>{0} was not found in the collection</c>.</summary>
        public static string KeyNotFoundExceptionMessage => ResourceManager.GetString(nameof(KeyNotFoundExceptionMessage), Culture)!;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>One or more of the requested {0} is unavailable</c>.</summary>
        public static string NotSupportedExceptionForMissingFeatureMessage => ResourceManager.GetString(nameof(NotSupportedExceptionForMissingFeatureMessage), Culture)!;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>The collection is read-only</c>.</summary>
        public static string NotSupportedExceptionForReadOnlyCollectionMessage => ResourceManager.GetString(nameof(NotSupportedExceptionForReadOnlyCollectionMessage), Culture)!;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>{0} is disposed</c>.</summary>
        public static string ObjectDisposedExceptionMessage => ResourceManager.GetString(nameof(ObjectDisposedExceptionMessage), Culture)!;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>Provider is already started</c>.</summary>
        public static string ProviderAlreadyStartedMessage => ResourceManager.GetString(nameof(ProviderAlreadyStartedMessage), Culture)!;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>Provider cannot be stopped in the {0} state</c>.</summary>
        public static string ProviderCannotBeStoppedMessage => ResourceManager.GetString(nameof(ProviderCannotBeStoppedMessage), Culture)!;

        /// <summary>Gets a localized <see cref="string" /> similar to <c>The timeout of {0} was reached before the operation could be completed</c>.</summary>
        public static string TimeoutExceptionMessage => ResourceManager.GetString(nameof(ObjectDisposedExceptionMessage), Culture)!;

        /// <summary>Gets the <see cref="ResourceManager" /> instance that is used to lookup the localized resources.</summary>
        public static ResourceManager ResourceManager => s_resourceManager;
    }
}
