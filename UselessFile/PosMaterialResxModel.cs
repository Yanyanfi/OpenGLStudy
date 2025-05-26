using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGLStudy.Enums;
using OpenGLStudy.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenGLStudy.UselessFile;

internal class PosMaterialResxModel : ResxModel, IDisposable
{
    private bool disposedValue;
    private int vbo,
        ebo;
    private Vector3 ambient,
        diffuse,
        specular;
    private float shininess;

    public PosMaterialResxModel(
        string name,
        Vector3 ambient,
        Vector3 diffuse,
        Vector3 specular,
        float shininess
    )
        : base(name)
    {
        this.ambient = ambient;
        this.diffuse = diffuse;
        this.specular = specular;
        this.shininess = shininess;
    }

    public override void Render()
    {
        var shader = Shader.Instance;
        shader?.SetMode(ShaderMode.MaterialLight);
        shader?.SetMaterial(shininess);
        shader?.SetMaterial(MaterialTarget.Diffuse, diffuse);
        shader?.SetMaterial(MaterialTarget.Ambient, ambient);
        shader?.SetMaterial(MaterialTarget.Specular, specular);
        GL.BindVertexArray(vao);
        GL.DrawElements(BeginMode.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
    }

    protected override void GenVao()
    {
        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);
        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(
            BufferTarget.ArrayBuffer,
            sizeof(float) * vertices.Length,
            vertices,
            BufferUsageHint.StaticDraw
        );
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(
            BufferTarget.ElementArrayBuffer,
            indices.Length * sizeof(uint),
            indices,
            BufferUsageHint.StaticDraw
        );
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(2);
        GL.BindVertexArray(0);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: 释放托管状态(托管对象)
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(ebo);
            GL.DeleteBuffer(vbo);
            disposedValue = true;
        }
    }

    // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    ~PosMaterialResxModel()
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
}
