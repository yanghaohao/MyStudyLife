using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// 多语言管理器：负责语言切换、文案读取、通知UI更新
/// </summary>
public class LanguageManager : MonoBehaviour
{
    // 单例实例
    private static LanguageManager _instance;
    public static LanguageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("LanguageManager");
                _instance = obj.AddComponent<LanguageManager>();
                DontDestroyOnLoad(obj);
                _instance.Init();
            }
            return _instance;
        }
    }

    // 当前语言
    private LanguageType _currentLang = LanguageType.zh_CN;
    public LanguageType CurrentLang
    {
        get => _currentLang;
        set
        {
            if (_currentLang != value)
            {
                _currentLang = value;
                // 保存到本地（下次启动生效）
                PlayerPrefs.SetString("CurrentLanguage", _currentLang.ToString());
                // 通知所有UI更新语言
                OnLanguageChanged?.Invoke();
            }
        }
    }

    // 多语言配置缓存（Key -> 语言 -> 文案）
    private Dictionary<string, Dictionary<LanguageType, string>> _langDict = new Dictionary<string, Dictionary<LanguageType, string>>();

    // 语言切换事件（ViewModel/View监听）
    public event Action OnLanguageChanged;

    /// <summary>
    /// 初始化：加载配置 + 读取本地保存的语言
    /// </summary>
    private void Init()
    {
        // 加载多语言配置表
        LoadLanguageConfig();
        // 读取本地保存的语言（优先用户设置）
        string saveLang = PlayerPrefs.GetString("CurrentLanguage", "zh_CN");
        if (Enum.TryParse(saveLang, out LanguageType lang))
        {
            _currentLang = lang;
        }
    }

    /// <summary>
    /// 加载CSV多语言配置
    /// </summary>
    private void LoadLanguageConfig()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Config/LanguageConfig");
        if (csvFile == null)
        {
            Debug.LogError("多语言配置文件缺失：Resources/Config/LanguageConfig.csv");
            return;
        }

        // 用之前的CsvParser解析
        List<Dictionary<string, string>> rows = CsvParser.Parse(csvFile.text);
        foreach (var row in rows)
        {
            if (!row.ContainsKey("Key")) continue;
            string key = row["Key"];
            var langRow = new Dictionary<LanguageType, string>();

            // 遍历所有语言类型，填充文案
            foreach (LanguageType lang in Enum.GetValues(typeof(LanguageType)))
            {
                // 把枚举的zh_CN转为CSV的zh-CN
                string csvColName = lang.ToString().Replace('_', '-');
                if (row.ContainsKey(csvColName))
                {
                    langRow[lang] = row[csvColName];
                }
                else
                {
                    langRow[lang] = $"[缺失文案：{key}]";
                }
            }

            _langDict[key] = langRow;
        }
    }

    /// <summary>
    /// 获取指定Key的文案（当前语言）
    /// </summary>
    /// <param name="key">文案Key</param>
    /// <returns>对应语言的文案</returns>
    public string GetText(string key)
    {
        if (_langDict.TryGetValue(key, out var langRow) && langRow.TryGetValue(_currentLang, out string text))
        {
            return text;
        }
        return $"[文案缺失：{key}]";
    }

    /// <summary>
    /// 获取指定Key+指定语言的文案
    /// </summary>
    public string GetText(string key, LanguageType lang)
    {
        if (_langDict.TryGetValue(key, out var langRow) && langRow.TryGetValue(lang, out string text))
        {
            return text;
        }
        return $"[文案缺失：{key}]";
    }

    /// <summary>
    /// 切换语言（全局生效）
    /// </summary>
    /// <param name="lang">目标语言</param>
    public void SwitchLanguage(LanguageType lang)
    {
        CurrentLang = lang;
        Debug.Log($"语言切换为：{lang}");
    }
}