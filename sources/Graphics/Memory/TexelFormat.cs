// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared/dxgiformat.h in the Windows SDK for Windows 10.0.19041.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Graphics
{

    /// <summary>
    /// TexelFormat specifies the format of the texels in a texture.
    /// </summary>
    public struct TexelFormat
    {
        /// <summary>Defines a RGBA4x8 TexelFormat.</summary>
        public static readonly TexelFormat RGBA4x8 = new TexelFormat(Channel.Type.UInt8, Channel.Kind.R, Channel.Type.UInt8, Channel.Kind.G, Channel.Type.UInt8, Channel.Kind.B, Channel.Type.UInt8, Channel.Kind.A);

        /// <summary>Defines a RFloat32 TexelFormat.</summary>
        public static readonly TexelFormat RFloat32 = new TexelFormat(Channel.Type.Float32, Channel.Kind.R);

        /// <summary>Defines a RFloat32 TexelFormat.</summary>
        public static readonly TexelFormat RUInt16 = new TexelFormat(Channel.Type.UInt16, Channel.Kind.R);

        /// <summary>Defines a RFloat32 TexelFormat.</summary>
        public static readonly TexelFormat RSInt16 = new TexelFormat(Channel.Type.SInt16, Channel.Kind.R);

        /// <summary>
        /// Channel holds the format info of a single color/intensity channel in a texture.
        /// </summary>
        public struct Channel
        {
            /// <summary>
            /// The texel format kind of a single channel, i.e. one of R,G,B,A,X
            /// </summary>
            public enum Type
            {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
                Unknown,
                Typeless,
                SInt8,
                SInt16,
                SInt32,
                SInt64,
                UInt8,
                UInt16,
                UInt32,
                UInt64,
                Float16,
                Float32,
                Float64,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            }

            /// <summary>
            /// Converts the Type enum into the # bits used by the indicated type
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static int BitsUsed(Type type)
            {
                switch (type)
                {
                    case Type.Unknown:
                        return -1;
                    case Type.Typeless:
                        return -1;
                    case Type.SInt8:
                        return 8;
                    case Type.SInt16:
                        return 16;
                    case Type.SInt32:
                        return 32;
                    case Type.SInt64:
                        return 64;
                    case Type.UInt8:
                        return 8;
                    case Type.UInt16:
                        return 16;
                    case Type.UInt32:
                        return 32;
                    case Type.UInt64:
                        return 64;
                    case Type.Float16:
                        return 16;
                    case Type.Float32:
                        return 32;
                    case Type.Float64:
                        return 64;
                    default:
                        return -1;
                }
            }


        /// <summary>
        /// The texel format kind of a single channel, i.e. one of R,G,B,A,X
        /// </summary>
        public enum Kind
            {
                /// <summary>
                /// Red
                /// </summary>
                R,

                /// <summary>
                /// Green
                /// </summary>
                G,

                /// <summary>
                /// Blue
                /// </summary>
                B,

                /// <summary>
                /// Alpha or Opacity == (1 - Transparency)
                /// </summary>
                A,

                /// <summary>
                /// Generic kind with user defined semantics
                /// </summary>
                X,
            }

            private Type _type; 

            private Kind _kind; // R, G, B, A, X

            /// <summary>
            /// The texel format Type of a single channel, e.g. Float, UInt16
            /// </summary>
            public Type ChannelType => _type;
            /// <summary>
            /// The texel format Kind of a single channel, i.e. one of R,G,B,A,X
            /// </summary>
            public Kind ChannelKind => _kind;

            /// <summary>
            /// Constructor for a Texel.Channel 
            /// </summary>
            /// <param name="type"></param>
            /// <param name="kind"></param>
            public Channel (Type type, Kind kind)
            {
                _type = type;
                _kind = kind;
            }

        }

        private Channel[] _channels;

        /// <summary>
        /// The Channels of this TexelFormat
        /// </summary>
        public Channel[] Channels => _channels;

        /// <summary>
        /// Single Channel constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="kind"></param>
        public TexelFormat(Channel.Type type, Channel.Kind kind)
        {
            _channels = new Channel[] { new Channel(type, kind) };
        }

        /// <summary>
        /// Four Channel constructor
        /// </summary>
        /// <param name="type0"></param>
        /// <param name="kind0"></param>
        /// <param name="type1"></param>
        /// <param name="kind1"></param>
        /// <param name="type2"></param>
        /// <param name="kind2"></param>
        /// <param name="type3"></param>
        /// <param name="kind3"></param>
        public TexelFormat(Channel.Type type0, Channel.Kind kind0, Channel.Type type1, Channel.Kind kind1, Channel.Type type2, Channel.Kind kind2, Channel.Type type3, Channel.Kind kind3)
        {
            _channels = new Channel[] {
                new Channel(type0, kind0),
                new Channel(type1, kind1),
                new Channel(type2, kind2),
                new Channel(type3, kind3),
            };
        }
    }
}
