using OpenGLStudy.Inputs;
using OpenGLStudy.Shaders;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Components;

internal class CameraPerspectiveSwitcher : Component
{
    private Camera? camera;
    public override void Start() => Owner.TryGetComponent<Camera>(out camera);
    public override void Update(float deltaTime)
    {
        if (InputState.Keyboard.IsKeyPressed(Keys.V) && camera is not null)
        {
            camera.EnableThirdPerson = !camera.EnableThirdPerson;
        }
    }
}

