using System;
using System.Reflection;
using System.Collections.Generic;

// ������� ��������� ������� ���� save-system,
// ����� ������, ��� ����� ������� ��� � ���� �����

// ����� ������� ����� reflection)))
public abstract class OldData
{
    private static List<OldData> datas = new List<OldData>();
    /// <summary>
    /// ������� ����� Data c id ������ 0
    /// </summary>
    public static T New<T>() where T : OldData, new()
    {
        T newData = new();
        datas.Add(newData);
        newData.Id = 0;
        newData.LoadStandart();
        newData.LoadAfter();

        return newData;
    }
    /// <summary>
    /// ������� ����� Data c ����� id
    /// </summary>
    public static T New<T>(int id) where T : OldData, new()
    {
        T newData = new();
        datas.Add(newData);
        newData.Id = id;
        newData.LoadStandart();
        newData.LoadAfter();

        return newData;
    }
    /// <summary>
    /// ������� ����� Data � �� ����������� ��������� � ���� ������ �� id
    /// </summary>
    public static T Load<T>(int id) where T : OldData, new()
    {
        T newData = new();
        datas.Add(newData);
        newData.Id = id;
        newData.Load();

        return newData;
    }
    public static void SaveAll()
    {
        foreach (OldData data in datas)
        {
            data.Save();
        }
    }

    public int Id { get; private set; }
    public string Name { get; private set; }

    private Type type;
    protected PropertyInfo[] props { get; private set; }

    protected OldData() => Init();
    protected abstract void Init();
    protected void InitData<T>()
    {
        Name = typeof(T).Name;
        type = typeof(T);
        props = type.GetProperties();
    }
    
    protected virtual void Save()
    {

    }
    protected virtual void Load()
    {
        bool notLoaded = false;
        if (!notLoaded)
        {
            LoadStandart();
        }

        LoadAfter();
    }
    
    protected abstract void LoadStandart();
    protected abstract void LoadAfter();
}