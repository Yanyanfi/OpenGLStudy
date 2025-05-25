using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Model;

internal abstract class ModelBase : IRenderable, IDisposable
{
    protected int vao, vbo, ebo;
    protected float[] vertices = null!;
    protected uint[] indices = null!;
    protected abstract void GenVao();
    protected abstract void GetVerticesAndIndices(params object[] args);
    protected ModelBase(params object[] args)
    {
        GetVerticesAndIndices(args);
        GenVao();
    }
    /// <summary>
    /// 设置着色器模式，材质等
    /// </summary>
    protected abstract void BeforeRender();
    protected virtual void AfterRender() { }
    public void Render()
    {
        BeforeRender();
        GL.BindVertexArray(vao);
        GL.DrawElements(BeginMode.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
        AfterRender();
    }

    #region Dispose
    protected bool disposedValue;
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
    ~ModelBase()
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
