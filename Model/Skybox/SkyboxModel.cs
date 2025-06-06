using OpenGLStudy.Model.Base;
using OpenGLStudy.Shaders;
using OpenGLStudy.Textures;
using OpenTK.Graphics.OpenGL4;
using System.Text.RegularExpressions;

namespace OpenGLStudy.Model.Skybox;

internal class SkyboxModel : ModelBase
{
    private Texture[] textures = new Texture[6];
    private int[] vaos = new int[6];
    private int[] vbos = new int[6];
    private int[] ebos = new int[6];
    private static (float[], uint[])[] skyboxFaces = new (float[], uint[])[6];
    private int vaoIndex = 0;

    public SkyboxModel(string folderPath) : base(folderPath) { }
    public SkyboxModel(string folderPath, Transform offset) : base(folderPath, offset) { }

    protected override void BeforeRender()
    {
        Shader.Instance?.SetMode(ShaderMode.VertexTexCoordUnlit);
        vao = vaos[vaoIndex];
        indices = skyboxFaces[vaoIndex].Item2;
        textures[vaoIndex].Bind();
    }

    protected override void AfterRender()
    {
        if (++vaoIndex >= 6)
        {
            vaoIndex = 0;
            return;
        }
        Render();
    }

    protected override void GenVao()
    {
        for (int i = 0; i < 6; i++)
        {
            vaos[i] = GL.GenVertexArray();
            vbos[i] = GL.GenBuffer();
            ebos[i] = GL.GenBuffer();
            GL.BindVertexArray(vaos[i]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbos[i]);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                skyboxFaces[i].Item1.Length * sizeof(float),
                skyboxFaces[i].Item1,
                BufferUsageHint.StaticDraw
            );
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebos[i]);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                skyboxFaces[i].Item2.Length * sizeof(uint),
                skyboxFaces[i].Item2,
                BufferUsageHint.StaticDraw
            );
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.BindVertexArray(0);
        }
    }

    protected override void GetVerticesAndIndices(params object[] args)
    {
        InitializeVertices();
        string[] patterns = [@"\.right\.", @"\.left\.", @"\.top\.", @"\.bottom\.", @"\.front\.", @"\.back\."];
        var filePaths = Directory.EnumerateFiles(args[0].ToString()!, "*", SearchOption.TopDirectoryOnly)
            .Select(e => Path.GetRelativePath(AppContext.BaseDirectory, e));
        for (int i = 0; i < 6; i++)
        {
            var texPath = filePaths.First(e => Regex.IsMatch(e, patterns[i]));
            textures[i] = new(texPath, TextureWrapMode.MirroredRepeat);
        }
    }

    private static void InitializeVertices()
    {
        skyboxFaces[0] = ( // +X (Right)
            new float[] {
            1, -1, -1, 0, 1,
            1, -1,  1, 1, 1,
            1,  1,  1, 1, 0,
            1,  1, -1, 0, 0
            },
            new uint[] { 0, 1, 2, 2, 3, 0 }
        );

        skyboxFaces[1] = ( // -X (Left)
            new float[] {
           -1, -1,  1, 0, 1,
           -1, -1, -1, 1, 1,
           -1,  1, -1, 1, 0,
           -1,  1,  1, 0, 0
            },
            new uint[] { 0, 1, 2, 2, 3, 0 }
        );

        skyboxFaces[2] = ( // +Y (Top)
            new float[] {
           -1, 1, -1, 0, 1,
            1, 1, -1, 1, 1,
            1, 1,  1, 1, 0,
           -1, 1,  1, 0, 0
            },
            new uint[] { 0, 1, 2, 2, 3, 0 }
        );

        skyboxFaces[3] = ( // -Y (Bottom)
            new float[] {
           -1, -1,  1, 0, 1,
            1, -1,  1, 1, 1,
            1, -1, -1, 1, 0,
           -1, -1, -1, 0, 0
            },
            new uint[] { 0, 1, 2, 2, 3, 0 }
        );

        skyboxFaces[4] = ( // +Z (Front)
            new float[] {
           -1, -1, -1, 0, 1,
            1, -1, -1, 1, 1,
            1,  1, -1, 1, 0,
           -1,  1, -1, 0, 0
            },
            new uint[] { 0, 1, 2, 2, 3, 0 }
        );

        skyboxFaces[5] = ( // -Z (Back)
            new float[] {
            1, -1,  1, 0, 1,
           -1, -1,  1, 1, 1,
           -1,  1,  1, 1, 0,
            1,  1,  1, 0, 0
            },
            new uint[] { 0, 1, 2, 2, 3, 0 }
        );
    }

    #region Dispose
    protected override void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: 释放托管状态(托管对象)
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            for (int i = 0; i < 6; i++)
            {
                GL.DeleteBuffer(vbos[i]);
                GL.DeleteBuffer(ebos[i]);
                GL.DeleteVertexArray(vaos[i]);
                textures[i].Dispose();
            }
            disposedValue = true;
        }
    }
    #endregion
}
