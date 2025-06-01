using OpenGLStudy.Components.Cameras;
using OpenGLStudy.Enums;
using OpenGLStudy.Model.Base;
using OpenGLStudy.Models;

namespace OpenGLStudy.Components.Skybox;

internal class AddSkybox : Component
{
    Camera? camera;
    IRenderable skyboxModel;
    public AddSkybox()
    {
        skyboxModel = ModelManager.Instance!.GetModel(ModelType.Skybox1);
    }
    public override void Start()
    {
        if (!Scene.TryGetComponent<Camera>(out camera))
            throw new InvalidOperationException("场景中未找到 Camera 组件，无法添加 Skybox。");
        FrameTimeLimit = camera!.FrameTimeLimit;
        Owner.Model = skyboxModel;
        Owner.Transform.Scale = new(camera.FarPlane / (float)Math.Pow(3, 1 / 2.0));
    }
    protected override void CustomUpdate(float deltaTime)
    {
        Owner.Transform.Position = camera!.Owner.Transform.Position;
    }

}
