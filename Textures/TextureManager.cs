using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Textures;

internal class TextureManager
{
    private Dictionary<TextureType, Texture> _textureDictionary = [];
    public void AddTexture(TextureType key,string path,TextureWrapMode wrapMode=TextureWrapMode.Repeat)
    {
        if (_textureDictionary.TryGetValue(key, out var texture))
            texture.Dispose();
        _textureDictionary[key] = new Texture(path, wrapMode);
    }
    public Texture GetTexture(TextureType textureType) => _textureDictionary[textureType];
    public void BindTexture(TextureType textureType) => GetTexture(textureType)?.Bind();
    public void UnbindTexture() => GL.BindTexture(TextureTarget.Texture2D, 0);
    public void ClearAll()
    {
        foreach(var texture in _textureDictionary.Values)
        {
            texture.Dispose();
        }
        _textureDictionary.Clear();
    }
}
