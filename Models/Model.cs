using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Models;

internal class Model : IDisposable
{
    private Dictionary<string, Mesh> meshDict;
    private bool disposedValue;

    public Model(params IEnumerable<string> meshName)
    {
        meshDict = [];
        LoadMeshes(meshName.Select(e => (e + "vertices", e + "Indices")));
    }

    public void Render()
    {
        foreach (var mesh in meshDict.Values)
        {
            mesh.Render();
        }
    }

    public void LoadMeshes(params IEnumerable<(string vertexResxName, string indexResxName)> names)
    {
        foreach (var name in names)
        {
            if (TryLoadMesh(name.vertexResxName, name.indexResxName) == false)
                throw new Exception($"Load mesh \"{name.vertexResxName}\" failed");
        }
    }

    private bool TryLoadMesh(string vertexResxName, string indexResxName)
    {
        if (TryLoadMeshFromResxFile(vertexResxName, indexResxName, out var meshFromResx))
        {
            if (meshDict.TryGetValue(vertexResxName, out var mesh))
                mesh.Dispose();
            meshDict[vertexResxName] = meshFromResx!;
            return true;
        }
        return false;
    }

    private bool TryLoadMeshFromResxFile(
        string vertexResxName,
        string indexResxName,
        out Mesh? mesh
    )
    {
        var vertexProp = typeof(MeshResource).GetProperty(
            vertexResxName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
        );
        var indexProp = typeof(MeshResource).GetProperty(
            indexResxName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
        );
        string? verticesText = vertexProp?.GetValue(null) as string;
        string? indicesText = indexProp?.GetValue(null) as string;
        if (verticesText is not null && indicesText is not null)
        {
            IEnumerable<float> vertices = verticesText
                .Split(
                    [' ', ',', '\n'],
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
                )
                .Select(e => float.Parse(e.TrimEnd('f')));
            IEnumerable<uint> indices = indicesText
                .Split(
                    [' ', ',', '\n'],
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
                )
                .Select(e => uint.Parse(e));
            mesh = new(vertices, indices);
            return true;
        }
        mesh = null;
        return false;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: 释放托管状态(托管对象)
                foreach(var mesh in meshDict.Values)
                    mesh.Dispose();
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            disposedValue = true;
        }
    }

    // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    ~Model()
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
