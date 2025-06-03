using OpenGLStudy.Model.Debug;

namespace OpenGLStudy.Components.Debug;

internal class ModelSpaceAxes : Component
{
    private LineRenderer lineRenderer = null!;
    public override void Start()
    {
        Owner.EnableRenderLine = true;
        lineRenderer = Owner.LineRenderer;
        GenerateLines();
    }
    private void GenerateLines()
    {
        lineRenderer.AddLines
        (
            (new(0, 0, 0), new(1, 0, 0), new(1, 0, 0)),
            (new(0, 0, 0), new(0, 1, 0), new(0, 1, 0)),
            (new(0, 0, 0), new(0, 0, 1), new(0, 0, 1))
        );
    }

}
