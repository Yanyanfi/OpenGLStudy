using OpenGLStudy.Model.Base;
using OpenGLStudy.Models;
using OpenGLStudy.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Model;

internal class ResxPostexModel(string name, Texture texture) : PostexModel(texture, name)
{
    protected override void GetVerticesAndIndices(params object[] name)
    {
        GetVertices(name[0].ToString()!);
        GetIndices(name[0].ToString()!);
    }
    private void GetVertices(string name)
    {
        var prop =
            typeof(MeshResource).GetProperty(name + "Vertices", BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException(
                $"Property '{name}Vertices' not found in MeshResource."
            );
        string text = (string?)prop.GetValue(null) ?? throw new InvalidOperationException($"'{name}Vertices' is null.");
        vertices = text.Split([',', ' ', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(e => float.Parse(e.TrimEnd('f')))
            .ToArray();
    }
    private void GetIndices(string name)
    {
        var prop =
            typeof(MeshResource).GetProperty(name + "Indices", BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException(
                $"Property '{name}Vertices' not found in MeshResource."
            );
        string text = (string?)prop.GetValue(null) ?? throw new InvalidOperationException($"'{name}Indices' is null.");
        indices = text.Split([',', ' ', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(e => uint.Parse(e))
            .ToArray();
    }
}
