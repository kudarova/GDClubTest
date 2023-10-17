using System.Collections.Generic;

[System.Serializable]
public abstract class Data : Updatable
{
    private static List<Data> data = new List<Data>();
    // решил отделить entity от data, уж слишком разные это данные, которые требуют разного подхода
    /// <summary>
    /// Load Data 
    /// </summary>
    public static void Load<T>(out T dat) where T : Data, new()
    {
        if (TryLoad(out dat))
        {
            LinkToUpdate(dat);
            dat.LateLoad();
            return;
        }
        
        Save(out dat);
    }
    /// <summary>
    /// Clean Load
    /// </summary>
    public static bool TryLoad<T>(out T dat) where T : Data
    {
        T loaded = SerializationManager.Load<T>("/" + typeof(T).Name);
        if (loaded is not null)
        {
            dat = loaded;
            dat.Instance = dat;
            dat = (T)dat.Instance;
            data.Add(dat);
            return true;
        }

        dat = null;
        return false;
    }
    protected static void Save<T>(out T dat) where T : Data, new()
    {
        dat = new();
        dat.Instance = dat;
        dat = (T)dat.Instance;
        dat.LocalSave();
        data.Add(dat);
    }

    public static void LoadAll()
    {
        for (int i = 0; i < data.Count; i++) data[i].LocalLoad();
    }
    public static void SaveAll()
    {
        for (int i = 0; i < data.Count; i++) data[i].LocalSave();
    }

    private Data instance;
    public Data Instance { get { return instance; } private set { instance ??= value; } }
    protected void DataRemove()
    {
        if (Instance != null)
        {
            data.Remove(Instance);
            instance = null;
        }
    }

    protected virtual void LocalLoad()
    {
        Data loaded = SerializationManager.Load<Data>("/" + GetType().Name);
        if (loaded is not null)
        {
            instance = loaded;
            LinkToUpdate(instance);
            instance.LateLoad();
        }
    }
    protected abstract void LateLoad();
    protected virtual void LocalSave() => SerializationManager.Save("/" + GetType().Name, this);
}