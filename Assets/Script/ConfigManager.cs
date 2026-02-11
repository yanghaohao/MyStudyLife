using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// 配置管理器：统一读取JSON配置，供全项目调用
/// </summary>
public class ConfigManager : MonoBehaviour
{
    // 单例实例
    private static ConfigManager _instance;
    public static ConfigManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("ConfigManager");
                _instance = obj.AddComponent<ConfigManager>();
                DontDestroyOnLoad(obj); // 全局常驻
                _instance.LoadAllConfig(); // 初始化加载配置
            }
            return _instance;
        }
    }

    // 全局配置数据
    public GameConfig GameConfig { get; private set; }

    /// <summary>
    /// 加载所有配置文件
    /// </summary>
    private void LoadAllConfig()
    {
        // 读取Resources目录下的GameConfig.json
        TextAsset jsonFile = Resources.Load<TextAsset>("Config/GameConfig");
        if (jsonFile == null)
        {
            Debug.LogError("配置文件加载失败：Resources/Config/GameConfig.json 不存在");
            return;
        }

        // 解析JSON到实体类
        GameConfig = JsonUtility.FromJson<GameConfig>(jsonFile.text);
        if (GameConfig == null)
        {
            Debug.LogError("JSON配置解析失败，请检查格式是否正确");
        }
        else
        {
            Debug.Log("配置文件加载成功！");
        }
    }

    #region 快捷获取配置方法（简化调用）
    /// <summary>
    /// 获取文字文案
    /// </summary>
    /// <param name="key">文案key（如LoginSuccess）</param>
    /// <returns>文案内容</returns>
    public string GetTextConfig(string key)
    {
        if (GameConfig?.TextConfig == null) return $"[文案缺失：{key}]";
        
        // 反射获取文案（避免硬编码，方便扩展）
        var prop = typeof(TextConfig).GetProperty(key);
        return prop?.GetValue(GameConfig.TextConfig)?.ToString() ?? $"[文案缺失：{key}]";
    }

    /// <summary>
    /// 获取音频路径
    /// </summary>
    /// <param name="key">音频key（如BtnClick）</param>
    /// <returns>音频路径</returns>
    public string GetAudioConfig(string key)
    {
        if (GameConfig?.AudioConfig == null) return string.Empty;
        
        var prop = typeof(AudioConfig).GetProperty(key);
        return prop?.GetValue(GameConfig.AudioConfig)?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// 获取资源路径
    /// </summary>
    /// <param name="key">资源key（如DialogBg）</param>
    /// <returns>资源路径</returns>
    public string GetResourceConfig(string key)
    {
        if (GameConfig?.ResourceConfig == null) return string.Empty;
        
        var prop = typeof(ResourceConfig).GetProperty(key);
        return prop?.GetValue(GameConfig.ResourceConfig)?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// 获取弹窗配置
    /// </summary>
    public DialogConfig GetDialogConfig()
    {
        return GameConfig?.DialogConfig ?? new DialogConfig();
    }
    #endregion
}
