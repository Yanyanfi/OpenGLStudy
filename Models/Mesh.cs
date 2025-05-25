using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL4;

namespace OpenGLStudy.Models;

internal class Mesh : IDisposable
{
    private int vao,vbo,ebo,
        indexCount;
    private bool disposedValue;

    public Mesh(IEnumerable<float> vertices, IEnumerable<uint> indices)
    {
        indexCount = indices.Count();
        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(
            BufferTarget.ArrayBuffer,
            vertices.Count() * sizeof(float),
            vertices.ToArray(),
            BufferUsageHint.StaticDraw
        );
        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(
            BufferTarget.ElementArrayBuffer,
            indices.Count() * sizeof(uint),
            indices.ToArray(),
            BufferUsageHint.StaticDraw
        );
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);
        GL.BindVertexArray(0);        
    }
    public void Render()
    {
        GL.BindVertexArray(vao);
        GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);
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
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
            GL.DeleteVertexArray(vao);
            disposedValue = true;
        }
    }

    // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    ~Mesh()
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
