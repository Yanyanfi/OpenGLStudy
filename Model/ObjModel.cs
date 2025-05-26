using Assimp;
using OpenGLStudy.Enums;
using OpenGLStudy.Model.Base;
using OpenGLStudy.Shaders;
using OpenGLStudy.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using TextureType = Assimp.TextureType;
using TextureWrapMode = OpenTK.Graphics.OpenGL4.TextureWrapMode;

namespace OpenGLStudy.Model;

/// <summary>
/// 加载 .obj 模型
/// </summary>
internal class ObjModel : ModelBase
{
    private int[] vaos = null!;
    private int[] vbos = null!;
    private int[] ebos = null!;
    private List<float[]> verticesList = [];
    private List<uint[]> indicesList = [];
    private List<Vector3> ambients = [];
    private List<Vector3> diffuses = [];
    private List<Vector3> speculars = [];
    private List<float> specularPowers = [];
    private List<TexMode> texModes = [];
    private Dictionary<int, Texture> diffuseTexs = [];
    private Dictionary<int, Texture> ambientTexs = [];
    private Dictionary<int, Texture> specularTexs = [];
    private Dictionary<int, Texture> normalTexs = [];
    private int vaoIndex;
    private bool useDiffuseTextureAsSpecular;
    private Transform? transform;
    /// <summary>
    /// 从 .obj 文件中加载 obj 模型
    /// </summary>
    /// <param name="path">.obj 文件路径</param>
    /// <param name="useDiffuseAsSpecular">当 == <see langword="true"/> 时，如果镜面反射贴图不存在，则用漫反射贴图替代</param>
    public ObjModel(string path, bool useDiffuseAsSpecular = false)
        : base(Path.Combine(AppContext.BaseDirectory, path), useDiffuseAsSpecular) { }
    public ObjModel(string path, Transform offset, bool useDiffuseAsSpecular = false)
        : base(Path.Combine(AppContext.BaseDirectory, path), useDiffuseAsSpecular, offset) { }
    protected override void BeforeRender()
    {
        var shader = Shader.Instance;
        shader?.SetMode(ShaderMode.MaterialLight);
        shader?.SetTexMode(texModes[vaoIndex]);
        diffuseTexs.TryGetValue(vaoIndex, out var dTexture);
        ambientTexs.TryGetValue(vaoIndex, out var aTexture);
        specularTexs.TryGetValue(vaoIndex, out var sTexture);
        normalTexs.TryGetValue(vaoIndex, out var nTexture);
        dTexture?.Bind();
        aTexture?.Bind();
        sTexture?.Bind();
        nTexture?.Bind();
        shader?.SetMaterial(MaterialTarget.Ambient, ambients[vaoIndex]);
        shader?.SetMaterial(MaterialTarget.Diffuse, diffuses[vaoIndex]);
        shader?.SetMaterial(MaterialTarget.Specular, speculars[vaoIndex]);
        shader?.SetMaterial(specularPowers[vaoIndex]);
        vao = vaos![vaoIndex];
        indices = indicesList[vaoIndex];
    }
    protected override void AfterRender()
    {
        Shader.Instance?.SetTexMode(TexMode.None);
        vaoIndex++;
        if (vaoIndex >= vaos.Length)
        {
            vaoIndex = 0;
            return;
        }
        Render();
    }
    protected override void GenVao()
    {
        var len = verticesList.Count;
        vaos = new int[len];
        vbos = new int[len];
        ebos = new int[len];
        for (int i = 0; i < len; i++)
        {
            vbos[i] = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbos[i]);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesList[i].Length * sizeof(float), verticesList[i], BufferUsageHint.StaticDraw);
            vaos[i] = GL.GenVertexArray();
            GL.BindVertexArray(vaos[i]);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 14 * sizeof(float), 0);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 14 * sizeof(float), 3 * sizeof(float));
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 14 * sizeof(float), 12 * sizeof(float));
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, 14 * sizeof(float), 6 * sizeof(float));
            GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, 14 * sizeof(float), 9 * sizeof(float));
            ebos[i] = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebos[i]);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                indicesList[i].Length * sizeof(uint),
                indicesList[i],
                BufferUsageHint.StaticDraw
            );
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(2);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(3);
            GL.EnableVertexAttribArray(4);
            GL.BindVertexArray(0);
        }
    }

    protected override void GetVerticesAndIndices(params object[] args)
    {
        transform = args.Length > 2 ? (Transform)args[2] : null;
        useDiffuseTextureAsSpecular = (bool)args[1];
        var objPath = args[0].ToString()!;
        Console.WriteLine($"正在加载模型：{Path.GetFileName(objPath)} ......");
        GetObjData(objPath);
        Console.WriteLine($"{Path.GetFileName(objPath)} 加载完成！\n");
    }
    private void GetObjData(string path)
    {
        var importer = new AssimpContext();
        var scene = importer.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs | PostProcessSteps.GenerateNormals | PostProcessSteps.CalculateTangentSpace);
        Console.WriteLine("mesh数量：" + scene.MeshCount);
        for (int i = 0; i < scene.Meshes.Count; i++)
        {
            Mesh? mesh = scene.Meshes[i];
            Console.WriteLine(mesh.Name);
            var material = scene.Materials[mesh.MaterialIndex];
            var parsedMaterial = ParseMaterial(material);
            ambients.Add(parsedMaterial.Ambient);
            diffuses.Add(parsedMaterial.Diffuse);
            speculars.Add(parsedMaterial.Specular);
            specularPowers.Add(Math.Max(parsedMaterial.Shininess, 1));
            verticesList.Add(GetVertices(mesh));
            indicesList.Add(GetIndices(mesh));
            Console.WriteLine("有法线：" + mesh.HasNormals);
            Console.WriteLine("texCoordChannelCount: " + mesh.TextureCoordinateChannelCount);
            Console.WriteLine("有法线贴图：" + material.HasTextureHeight);
            LoadTextures(path, material, i);
        }
    }
    private float[] GetVertices(Mesh mesh)
    {
        IEnumerable<float[]> verts = [];
        for (int i = 0; i < mesh.VertexCount; i++)
        {
            var pos1 = mesh.Vertices[i];
            var pos = transform switch
            {
                null => new Vector3(pos1.X, pos1.Y, pos1.Z),
                not null => (new Vector4(pos1.X, pos1.Y, pos1.Z, 1.0f) * transform.GetModelMatrix()).Xyz
            };
            var normal = mesh.HasNormals ? mesh.Normals[i] : new Vector3D();
            var tangent = mesh.HasTangentBasis ? mesh.Tangents[i] : new Vector3D();
            var biTangent = mesh.HasTangentBasis ? mesh.BiTangents[i] : new Vector3D();
            var uv = mesh.HasTextureCoords(0) ? mesh.TextureCoordinateChannels[0][i] : new Vector3D();
            verts = verts.Append([pos.X, pos.Y, pos.Z, normal.X, normal.Y, normal.Z, tangent.X, tangent.Y, tangent.Z, biTangent.X, biTangent.Y, biTangent.Z, uv.X, uv.Y]);
        }
        return verts.SelectMany(e => e).ToArray();
    }
    private uint[] GetIndices(Mesh mesh)
    {
        IEnumerable<uint[]> faceIndices = [];
        foreach (var face in mesh.Faces)
        {
            var uindices = face.Indices.Select(e => Convert.ToUInt32(e));
            faceIndices = faceIndices.Append(uindices.ToArray());
        }
        return faceIndices.SelectMany(e => e).ToArray();
    }
    private void LoadTextures(string objPath, Material material, int index)
    {
        texModes.Add(TexMode.None);
        var directory = Path.GetDirectoryName(objPath)!;
        LoadDiffuseTextures(directory, material, index);
        LoadNormalTextures(directory, material, index);
        Console.WriteLine("diffuseTexs.Count" + diffuseTexs.Count);
    }
    private void LoadDiffuseTextures(string directory, Material material, int index)
    {
        if (material.GetMaterialTexture(TextureType.Diffuse, 0, out var dTexture))
        {
            var texPath = Path.Combine(directory, dTexture.FilePath);
            if (File.Exists(texPath))
            {
                texModes[^1] |= TexMode.Diffuse;
                diffuseTexs[index] = new Texture(texPath, TextureWrapMode.Repeat, TextureUnit.Texture2);
                if (!material.HasTextureAmbient)
                {
                    texModes[^1] |= TexMode.Ambient;
                    ambientTexs[index] = new Texture(texPath, TextureWrapMode.Repeat, TextureUnit.Texture1);
                }
                if (!material.HasTextureSpecular && useDiffuseTextureAsSpecular)
                {
                    texModes[^1] |= TexMode.Specular;
                    specularTexs[index] = new Texture(texPath, TextureWrapMode.Repeat, TextureUnit.Texture3);
                }
            }
        }
    }
    private void LoadNormalTextures(string directory, Material material, int index)
    {
        if (material.GetMaterialTexture(TextureType.Height, 0, out var nTexture))
        {
            var texPath = Path.Combine(directory, nTexture.FilePath);
            if (File.Exists(texPath))
            {
                texModes[^1] |= TexMode.Normal;
                normalTexs[index] = new Texture(texPath, TextureWrapMode.Repeat, TextureUnit.Texture4);
            }
        }
    }
    private (Vector3 Ambient, Vector3 Diffuse, Vector3 Specular, float Shininess) ParseMaterial(Material material)
    {
        var am = material.ColorAmbient;
        var di = material.ColorDiffuse;
        var sp = material.ColorSpecular;
        var amVector = new Vector3(am.R, am.G, am.B);
        var diVector = new Vector3(di.R, di.G, di.B);
        var spVector = new Vector3(sp.R, sp.G, sp.B);
        var shininess = material.Shininess;
        return (amVector, diVector, spVector, shininess);
    }
    #region Dispose
    protected override void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                foreach (var tex in normalTexs)
                {
                    tex.Value.Dispose();
                }
                foreach (var tex in diffuseTexs)
                {
                    tex.Value.Dispose();
                }
            }
            for (int i = 0; i < vaos.Length; i++)
            {
                GL.DeleteVertexArray(vaos[i]);
                GL.DeleteBuffer(vbos[i]);
                GL.DeleteBuffer(ebos[i]);
            }
            disposedValue = true;
        }
    }
    #endregion
}
