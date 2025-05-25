using OpenGLStudy.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Components;

internal class BiggerAndBigger : Component
{
    public BiggerAndBigger(float scalePerFrame, float frameTime)
    {
        ScalePerFrame = scalePerFrame;
        FrameTimeLimit = frameTime;
    }
    public float ScalePerFrame { get; set; }
    protected override void CustomUpdate(float deltaTime)
    {
        Owner.Transform.Scale *= ScalePerFrame;
    }
}
