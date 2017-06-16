// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Globalization;
using System.Resources;

namespace TerraFX
{
    /// <summary>Provides a set of methods for looking up localized resources.</summary>
    public static class Resources
    {
        #region Static Fields
        private static readonly ResourceManager _resourceManager = new ResourceManager(typeof(Resources));
        private static CultureInfo _culture;
        #endregion

        #region Static Properties
        /// <summary>Gets a localized <see cref="string" /> similar to <c>{0} is an instance of {1}</c>.</summary>
        public static string ArgumentExceptionForInvalidTypeMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(ArgumentExceptionForInvalidTypeMessage), Culture);
            }
        }

        /// <summary>Gets a localized <see cref="string" /> similar to <c>{0} is null</c>.</summary>
        public static string ArgumentNullExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(ArgumentNullExceptionMessage), Culture);
            }
        }

        /// <summary>Gets a localized <see cref="string" /> similar to <c>{0} has a value of {1}</c>.</summary>
        public static string ArgumentOutOfRangeExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(ArgumentOutOfRangeExceptionMessage), Culture);
            }
        }

        /// <summary>Gets or sets the <see cref="CultureInfo" /> used during resource lookup.</summary>
        /// <remarks>When this property has a value of <c>null</c>, <see cref="CultureInfo.CurrentUICulture" /> is used instead.</remarks>
        public static CultureInfo Culture
        {
            get
            {
                return _culture;
            }

            set
            {
                _culture = value;
            }
        }

        /// <summary>Gets the <see cref="ResourceManager" /> instance that is used to lookup the localized resources.</summary>
        public static ResourceManager ResourceManager
        {
            get
            {
                return _resourceManager;
            }
        }
        #endregion
    }
}
