using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp.PixelFormats;

namespace OpenGLStudy.Textures;

internal class Texture : IDisposable
{
    public int Handle { get; init; }
    private TextureUnit textureUnit = TextureUnit.Texture0;
    public Texture(string imagePath, TextureWrapMode wrapMode)
    {
        imagePath = Path.Combine(AppContext.BaseDirectory, imagePath);
        using var image = SixLabors.ImageSharp.Image.Load<Rgba32>(imagePath);
        byte[] pixels = new byte[image.Width * image.Height * 4];
        image.CopyPixelDataTo(pixels);
        Handle = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, Handle);
        GL.TexImage2D(
            TextureTarget.Texture2D,
            0,
            PixelInternalFormat.Rgba,
            image.Width,
            image.Height,
            0,
            PixelFormat.Rgba,
            PixelType.UnsignedByte,
            pixels
        );
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        GL.TextureParameter(Handle, TextureParameterName.TextureWrapS, (int)wrapMode);
        GL.TextureParameter(Handle, TextureParameterName.TextureWrapT, (int)wrapMode);
        GL.TextureParameter(
            Handle,
            TextureParameterName.TextureMinFilter,
            (int)TextureMinFilter.Nearest
        );
        GL.TextureParameter(
            Handle,
            TextureParameterName.TextureMagFilter,
            (int)TextureMagFilter.Nearest
        );
        Unbind();
    }
    public Texture(string imagePath, TextureWrapMode wrapMode, TextureUnit textureUnit) : this(imagePath, wrapMode)
    {
        this.textureUnit = textureUnit;
    }
    public void Bind()
    {
        GL.ActiveTexture(textureUnit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }
    public void Unbind()
    {
        GL.BindTexture(TextureTarget.Texture2D, 0);
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
            GL.DeleteTexture(Handle);
            disposedValue = true;
        }
    }

    // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    // ~Texture()
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
