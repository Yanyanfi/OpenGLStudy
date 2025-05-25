using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Shaders.Enums;

[Flags]
internal enum TexMode
{
    None = 0,
    Color = 1,
    Ambient = 2,
    Diffuse = 4,
    Specular = 8,
    Normal = 16,
}
