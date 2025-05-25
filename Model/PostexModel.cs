using OpenGLStudy.Shaders;
using OpenGLStudy.Textures;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Model;

internal abstract class PostexModel : ModelBase
{
    private Texture texture;
    protected PostexModel(Texture texture, params string[] args) : base(args)
    {
        this.texture = texture;
    }
    protected override void BeforeRender()
    {
        Shader.Instance?.SetMode(ShaderMode.VertexTexCoordUnlit);
        texture.Bind();
    }
    protected override void GenVao()
    {
        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(
            BufferTarget.ElementArrayBuffer,
            indices.Length * sizeof(uint),
            indices,
            BufferUsageHint.StaticDraw
        );
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);
        GL.BindVertexArray(0);
    }
}
