using OpenGLStudy.Enums;
using OpenGLStudy.Model.Base;

namespace OpenGLStudy.Models;

internal class ModelManager : IDisposable
{
    public static ModelManager? Instance { get; private set; }
    public ModelManager() => Instance ??= this;
    public void AddModel(IRenderable model, ModelType modelType)
    {
        if (models.TryGetValue(modelType, out IRenderable? value))
        {
            if (value is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
        models[modelType] = model;
    }
    public IRenderable GetModel(ModelType modelType) => models[modelType];
    private Dictionary<ModelType, IRenderable> models = [];
    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: 释放托管状态(托管对象)
                foreach (var model in models.Values)
                {
                    if (model is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            disposedValue = true;
        }
    }

    // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    ~ModelManager()
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
