using OpenGLStudy.Model.Base;
using OpenGLStudy.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenGLStudy.Model.Debug;

internal class LineRenderer : IRenderable, IDisposable
{
    private float[] data = [];
    private int vao;
    private int vbo;
    public LineRenderer() => GenVao();
    public void AddLine(Vector3 startPos, Vector3 endPos, Vector3 color)
    {
        IEnumerable<float> v1 = [startPos.X, startPos.Y, startPos.Z, color.X, color.Y, color.Z];
        IEnumerable<float> v2 = [endPos.X, endPos.Y, endPos.Z, color.X, color.Y, color.Z];
        data = data.Concat(v1).Concat(v2).ToArray();
    }
    public void AddLines(params List<(Vector3 startPos, Vector3 endPos, Vector3 color)> lines)
    {
        lines.ForEach(e => AddLine(e.startPos, e.endPos, e.color));
    }
    public void Clear() => data = [];
    public void Render()
    {
        Shader.Instance?.SetMode(ShaderMode.DebugLine);
        GL.BindVertexArray(vao);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * data.Length, data, BufferUsageHint.DynamicDraw);
        
        GL.DrawArrays(PrimitiveType.Lines, 0, data.Length / 6);
        GL.BindVertexArray(0);
    }
    private void GenVao()
    {
        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();
        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.VertexAttribPointer(5, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(5);
        GL.BindVertexArray(0);
    }


    #region Dispose
    private bool disposedValue;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
            }
            GL.DeleteBuffer(vbo);
            GL.DeleteVertexArray(vao);
            disposedValue = true;
        }
    }

    // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    ~LineRenderer()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
