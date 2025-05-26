using OpenGLStudy.Enums;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenGLStudy.Shaders;

internal class Shader : IDisposable
{
    public const int MaxDirLightCount = 4;
    public const int MaxPointLightCount = 8;
    private int mvp;
    private int mv;
    private int nmat;
    private int fnmat;
    private int vertMode;
    private int fragMode;
    private int enColorTex;
    private int enAmbientTex;
    private int enDiffuseTex;
    private int enSpecularTex;
    private int enNormalTex;
    #region MaterialAndLightLocaton
    private int materialAmbient;
    private int materialDiffuse;
    private int materialSpecular;
    private int materialShininess;
    private List<int> dirLightDirections = [];
    private List<int> dirLightAmbients = [];
    private List<int> dirLightSpeculars = [];
    private List<int> dirLightDiffuses = [];
    private int dirLightCountLocation;
    private List<int> pointLightPositions = [];
    private List<int> pointLightConstants = [];
    private List<int> pointLightLinears = [];
    private List<int> pointLightQuadratics = [];
    private List<int> pointLightAmbients = [];
    private List<int> pointLightSpeculars = [];
    private List<int> pointLightDiffuses = [];
    private int pointLightCountLocation;
    #endregion
    public int DirLightCount
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan<int>(value, MaxDirLightCount, "DirLightCount");
            field = value;
            GL.Uniform1(dirLightCountLocation, value);
        }
    }
    public int PointLightCount
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan<int>(value, MaxPointLightCount, "PointLightCount");
            field = value;
            GL.Uniform1(pointLightCountLocation, value);
        }
    }

    public int Handle { get; init; }
    public Matrix4 Model { get; private set; }
    public Matrix4 View { get; private set; }
    public Matrix4 Projection { get; private set; }
    public Matrix4 MvMatrix => mvMatrix;
    private Matrix4 mvpMatrix;
    private Matrix4 mvMatrix;
    private Matrix3 tnormalMatrix;

    public void SetMvpMatrix(MVPMatrixTarget target, Matrix4 matrix)
    {
        switch (target)
        {
            case MVPMatrixTarget.Model:
                Model = matrix;
                mvMatrix = Model * View;
                mvpMatrix = mvMatrix * Projection;
                if (new Matrix3(mvMatrix).Determinant != 0)
                    tnormalMatrix = Matrix3.Invert(new Matrix3(mvMatrix));
                GL.UniformMatrix4(mvp, false, ref mvpMatrix);
                GL.UniformMatrix4(mv, false, ref mvMatrix);
                GL.UniformMatrix3(nmat, true, ref tnormalMatrix);
                GL.UniformMatrix3(fnmat, true, ref tnormalMatrix);
                break;
            case MVPMatrixTarget.View:
                View = matrix;
                mvMatrix = Model * View;
                mvpMatrix = mvMatrix * Projection;
                if (new Matrix3(mvMatrix).Determinant != 0)
                    tnormalMatrix = Matrix3.Invert(new Matrix3(mvMatrix));
                GL.UniformMatrix4(mvp, false, ref mvpMatrix);
                GL.UniformMatrix4(mv, false, ref mvMatrix);
                GL.UniformMatrix3(nmat, true, ref tnormalMatrix);
                GL.UniformMatrix3(fnmat, true, ref tnormalMatrix);
                break;
            case MVPMatrixTarget.Projection:
                Projection = matrix;
                mvpMatrix = mvMatrix * Projection;
                GL.UniformMatrix4(mvp, false, ref mvpMatrix);
                break;
        }

        mvpMatrix = mvMatrix * Projection;
        GL.UniformMatrix4(mvp, false, ref mvpMatrix);
        GL.UniformMatrix4(mv, false, ref mvMatrix);
        GL.UniformMatrix4(mv, false, ref mvMatrix);
    }
    public void SetMaterial(MaterialTarget target, Vector3 value)
    {
        switch (target)
        {
            case MaterialTarget.Ambient:
                GL.Uniform3(materialAmbient, ref value);
                break;
            case MaterialTarget.Diffuse:
                GL.Uniform3(materialDiffuse, ref value);
                break;
            case MaterialTarget.Specular:
                GL.Uniform3(materialSpecular, ref value);
                break;
        }
    }
    public void SetMaterial(float shininess)
    {
        GL.Uniform1(materialShininess, shininess);
    }
    public void SetDirLightMaterial(DirLightTarget type, int index, Vector3 value)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, MaxDirLightCount);
        switch (type)
        {
            case DirLightTarget.Direction:
                GL.Uniform3(dirLightDirections[index], ref value); break;
            case DirLightTarget.Ambient:
                GL.Uniform3(dirLightAmbients[index], ref value); break;
            case DirLightTarget.Specular:
                GL.Uniform3(dirLightSpeculars[index], ref value); break;
            case DirLightTarget.Diffuse:
                GL.Uniform3(dirLightDiffuses[index], ref value); break;
        }
    }
    public void SetPointLightMaterial(PointLightTarget target, int index, Vector3 value)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, MaxPointLightCount);
        var location = target switch
        {
            PointLightTarget.Position => pointLightPositions[index],
            PointLightTarget.Ambient => pointLightAmbients[index],
            PointLightTarget.Diffuse => pointLightDiffuses[index],
            PointLightTarget.Specular => pointLightSpeculars[index],
            _ => throw new ArgumentException("枚举类型应该代表向量而不是浮点数")
        };
        GL.Uniform3(location, value);
    }
    public void SetPointLightMaterial(PointLightTarget target, int index, float value)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, MaxPointLightCount);
        var location = target switch
        {
            PointLightTarget.Constant => pointLightConstants[index],
            PointLightTarget.Linear => pointLightLinears[index],
            PointLightTarget.Quadratic => pointLightQuadratics[index],
            _ => throw new ArgumentException("枚举类型应该代表浮点数而不是向量")
        };
        GL.Uniform1(location, value);
    }
    public void SetMode(ShaderMode mode)
    {
        switch (mode)
        {
            case ShaderMode.VertexTexCoordUnlit:
                GL.Uniform1(vertMode, 1);
                GL.Uniform1(fragMode, 1);
                break;
            case ShaderMode.MaterialLight:
                GL.Uniform1(vertMode, 2);
                GL.Uniform1(fragMode, 2);
                break;
        }
    }
    public void SetTexMode(TexMode mode)
    {
        var color = (mode & TexMode.Color) == TexMode.Color ? 1 : 0;
        var ambient = (mode & TexMode.Ambient) == TexMode.Ambient ? 1 : 0;
        var diffuse = (mode & TexMode.Diffuse) == TexMode.Diffuse ? 1 : 0;
        var specular = (mode & TexMode.Specular) == TexMode.Specular ? 1 : 0;
        var normal = (mode & TexMode.Normal) == TexMode.Normal ? 1 : 0;
        GL.Uniform1(enColorTex, color);
        GL.Uniform1(enAmbientTex, ambient);
        GL.Uniform1(enDiffuseTex, diffuse);
        GL.Uniform1(enSpecularTex, specular);
        GL.Uniform1(enNormalTex, normal);
    }
    public static Shader? Instance { get; private set; }
    public Shader(string vertexPath, string fragmentPath)
    {
        vertexPath=Path.Combine(AppContext.BaseDirectory, vertexPath);
        fragmentPath = Path.Combine(AppContext.BaseDirectory, fragmentPath);
        string vertexShaderSource = File.ReadAllText(vertexPath);
        string fragmentShaderSource = File.ReadAllText(fragmentPath);
        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);
        GL.CompileShader(vertexShader);
        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);
        GL.CompileShader(fragmentShader);
        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int isSucess);
        if (isSucess == 0)
        {
            GL.GetShaderInfoLog(vertexShader, out string info);
            throw new Exception(info);
        }
        GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out isSucess);
        if (isSucess == 0)
        {
            GL.GetShaderInfoLog(fragmentShader, out string info);
            throw new Exception(info);
        }
        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);
        GL.LinkProgram(Handle);
        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out isSucess);
        if (isSucess == 0)
        {
            GL.GetProgramInfoLog(Handle, out string info);
            throw new Exception(info);
        }
        GL.DetachShader(Handle, vertexShader);
        GL.DetachShader(Handle, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
        Instance ??= this;
    }

    public void Use()
    {
        GL.UseProgram(Handle);
        GetUniformsLocation();
        AssignTextureUnit();
    }
    private void GetUniformsLocation()
    {
        mvp = GL.GetUniformLocation(Handle, "mvp");
        mv = GL.GetUniformLocation(Handle, "mv");
        nmat = GL.GetUniformLocation(Handle, "normal_mat");
        fnmat = GL.GetUniformLocation(Handle, "fnormal_mat");
        vertMode = GL.GetUniformLocation(Handle, "mode");
        fragMode = GL.GetUniformLocation(Handle, "fmode");
        enColorTex = GL.GetUniformLocation(Handle, "enColorTex");
        enAmbientTex = GL.GetUniformLocation(Handle, "enAmbientTex");
        enDiffuseTex = GL.GetUniformLocation(Handle, "enDiffuseTex");
        enSpecularTex = GL.GetUniformLocation(Handle, "enSpecularTex");
        enNormalTex = GL.GetUniformLocation(Handle, "enNormalTex");
        materialAmbient = GL.GetUniformLocation(Handle, "material.ambient");
        materialDiffuse = GL.GetUniformLocation(Handle, "material.diffuse");
        materialSpecular = GL.GetUniformLocation(Handle, "material.specular");
        materialShininess = GL.GetUniformLocation(Handle, "material.shininess");
        for (int i = 0; i < MaxDirLightCount; i++)
        {
            dirLightDirections.Add(GL.GetUniformLocation(Handle, $"dirLights[{i}].direction"));
            dirLightDiffuses.Add(GL.GetUniformLocation(Handle, $"dirLights[{i}].diffuse"));
            dirLightAmbients.Add(GL.GetUniformLocation(Handle, $"dirLights[{i}].ambient"));
            dirLightSpeculars.Add(GL.GetUniformLocation(Handle, $"dirLights[{i}].specular"));
        }
        dirLightCountLocation = GL.GetUniformLocation(Handle, "dirLightCount");
        for (int i = 0; i < MaxPointLightCount; i++)
        {
            pointLightPositions.Add(GL.GetUniformLocation(Handle, $"pointLights[{i}].position"));
            pointLightConstants.Add(GL.GetUniformLocation(Handle, $"pointLights[{i}].constant"));
            pointLightLinears.Add(GL.GetUniformLocation(Handle, $"pointLights[{i}].linear"));
            pointLightQuadratics.Add(GL.GetUniformLocation(Handle, $"pointLights[{i}].quadratic"));
            pointLightAmbients.Add(GL.GetUniformLocation(Handle, $"pointLights[{i}].ambient"));
            pointLightDiffuses.Add(GL.GetUniformLocation(Handle, $"pointLights[{i}].diffuse"));
            pointLightSpeculars.Add(GL.GetUniformLocation(Handle, $"pointLights[{i}].specular"));
        }
        pointLightCountLocation = GL.GetUniformLocation(Handle, "pointLightCount");
    }
    private void AssignTextureUnit()
    {
        var colorLocation = GL.GetUniformLocation(Handle, "texture0");
        var ambientLocation = GL.GetUniformLocation(Handle, "texture1");
        var diffuseLoaction = GL.GetUniformLocation(Handle, "texture2");
        var specularLocation = GL.GetUniformLocation(Handle, "texture3");
        var normalLocation = GL.GetUniformLocation(Handle, "texture4");
        GL.Uniform1(colorLocation, 0);
        GL.Uniform1(ambientLocation, 1);
        GL.Uniform1(diffuseLoaction, 2);
        GL.Uniform1(specularLocation, 3);
        GL.Uniform1(normalLocation, 4);
    }
    #region Dispose
    private bool disposedValue;
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
            GL.DeleteProgram(Handle);
            disposedValue = true;
        }
    }

    // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    // ~Shader()
    // {
    //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}

