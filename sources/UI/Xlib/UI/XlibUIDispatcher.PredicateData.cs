// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;
using TerraFX.Interop.Xlib;

namespace TerraFX.UI;

public partial class XlibUIDispatcher
{
    private struct PredicateData
    {
        public GCHandle GCHandle;

        public bool IsClientMessage;

        public Atom TerraFXCreateWindowAtom;

        public Atom TerraFXQuitAtom;
    }
}
