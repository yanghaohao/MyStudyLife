using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Model基类
/// </summary>
public class BaseModel : IModel
{

    // 配置管理器实例（所有Model可直接用）
    protected ConfigManager _configMgr => ConfigManager.Instance;
    protected LanguageManager _langMgr => LanguageManager.Instance;

    /// <summary>
    /// 加载数据（子类重写：本地读取/网络请求）
    /// </summary>
    /// <param name="callback"></param>
    public virtual void LoadData(Action<object> callback)
    {
        callback?.Invoke(null);
    }

    /// <summary>
    /// 保存数据（子类重写）
    /// </summary>
    public virtual void SaveData(object data) { }

    #region 快捷工具方法（多语言+配置）
    // 多语言文案
    protected string GetLangText(string key) => _langMgr.GetText(key);
    protected string GetLangText(string key, LanguageType lang) => _langMgr.GetText(key, lang);
    // 切换语言（Model层可触发）
    protected void SwitchLang(LanguageType lang) => _langMgr.SwitchLanguage(lang);
    // 原有配置方法
    protected string GetAudioPath(string key) => _configMgr.GetAudioConfig(key);
    protected string GetResourcePath(string key) => _configMgr.GetResourceConfig(key);
    protected DialogConfig GetDialogCfg() => _configMgr.GetDialogConfig();
    #endregion
}
