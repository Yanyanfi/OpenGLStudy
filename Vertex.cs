using OpenTK.Graphics.ES11;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy;

internal class Vertex : IEnumerable<float>
{
    public Vertex(float x, float y, float z, float texCoordsX, float texCoordsY)
    {
        Position = new(x, y, z);
        TexCoords = new(texCoordsX, texCoordsY);
    }
    public Vector3 Position { get; set; }
    public Vector2 TexCoords { get; set; }
    
    public static int Size => sizeof(float) * 5;

    public IEnumerator<float> GetEnumerator()
    {
        yield return Position.X;
        yield return Position.Y;
        yield return Position.Z;
        yield return TexCoords.X;
        yield return TexCoords.Y;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
