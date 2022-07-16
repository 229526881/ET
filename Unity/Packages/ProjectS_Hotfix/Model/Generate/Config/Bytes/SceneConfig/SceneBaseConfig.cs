//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;



namespace ET.cfg.SceneConfig
{

public sealed partial class SceneBaseConfig :  Bright.Config.BeanBase 
{
    public SceneBaseConfig(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        SceneName = _buf.ReadString();
        SceneRecastNavDataFileName = _buf.ReadString();
        PostInit();
    }

    public static SceneBaseConfig DeserializeSceneBaseConfig(ByteBuf _buf)
    {
        return new SceneConfig.SceneBaseConfig(_buf);
    }

    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 场景名
    /// </summary>
    public string SceneName { get; private set; }
    /// <summary>
    /// 场景寻路数据文件名
    /// </summary>
    public string SceneRecastNavDataFileName { get; private set; }

    public const int __ID__ = -1905665057;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "SceneName:" + SceneName + ","
        + "SceneRecastNavDataFileName:" + SceneRecastNavDataFileName + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}
