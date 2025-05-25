using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Inputs;

internal static class InputState
{
    public static KeyboardState Keyboard { get; set; } = default!;
    public static MouseState Mouse { get; set; } = default!;
}
