using OpenGLStudy.Shaders;
using OpenGLStudy.Shaders.Enums;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Model;

internal abstract class MaterialModel : ModelBase
{
    public Vector3 Ambient { get; set; }
    public Vector3 Diffuse { get; set; }
    public Vector3 Specular { get; set; }
    public float Shininess { get; set; }
    protected MaterialModel(Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess, params object[] args) : base(args)
    {
        Ambient = ambient;
        Diffuse = diffuse;
        Specular = specular;
        Shininess = shininess;
    }
    protected override void BeforeRender()
    {
        var shader = Shader.Instance;
        shader?.SetMode(ShaderMode.MaterialLight);
        shader?.SetMaterial(Shininess);
        shader?.SetMaterial(MaterialTarget.Diffuse, Diffuse);
        shader?.SetMaterial(MaterialTarget.Ambient, Ambient);
        shader?.SetMaterial(MaterialTarget.Specular, Specular);
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
        GL.VertexAttribPointer(
            2,
            3,
            VertexAttribPointerType.Float,
            false,
            6 * sizeof(float),
            3 * sizeof(float)
        );
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

}
