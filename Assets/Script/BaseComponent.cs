using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Collections; // 【新增】协程必需的命名空间
using System.Collections.Generic;

/// <summary>
/// 所有UI组件的基类，封装通用方法
/// </summary>
public class BaseComponent : MonoBehaviour
{
    // 缓存子物体，避免重复GetComponent查找（性能优化）
    private Dictionary<string, Component> _componentCache = new Dictionary<string, Component>();

    #region 生命周期（统一封装，子类按需重写）
    protected virtual void Awake()
    {
        InitCache();       // 初始化组件缓存
        InitView();        // 初始化UI视图
        InitListener();    // 初始化事件监听
    }

    protected virtual void Start()
    {
        InitData();        // 初始化数据
    }

    protected virtual void OnDestroy()
    {
        ClearListener();   // 清理事件监听
        ClearCache();      // 清理缓存
    }

    // 可选重写的初始化方法（子类按需实现）
    protected virtual void InitCache() { }
    protected virtual void InitView() { }
    protected virtual void InitListener() { }
    protected virtual void InitData() { }
    protected virtual void ClearListener() { }
    #endregion

    #region 通用工具方法（简化调用）
    /// <summary>
    /// 查找子物体组件（带缓存，避免重复查找）
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="path">子物体路径（如 "Content/Button_Confirm"）</param>
    /// <returns>目标组件</returns>
    protected T GetChildComponent<T>(string path) where T : Component
    {
        string key = $"{typeof(T)}_{path}";
        if (_componentCache.TryGetValue(key, out Component comp))
        {
            return comp as T;
        }

        Transform targetTrans = transform.Find(path);
        if (targetTrans == null)
        {
            Debug.LogError($"[{gameObject.name}] 找不到子物体：{path}");
            return null;
        }

        T targetComp = targetTrans.GetComponent<T>();
        if (targetComp == null)
        {
            Debug.LogError($"[{gameObject.name}] 子物体 {path} 没有 {typeof(T)} 组件");
            return null;
        }

        _componentCache.Add(key, targetComp);
        return targetComp;
    }

    /// <summary>
    /// 快速绑定按钮点击事件
    /// </summary>
    /// <param name="buttonPath">按钮路径</param>
    /// <param name="onClick">点击回调</param>
    protected void BindButtonClick(string buttonPath, UnityAction onClick)
    {
        Button btn = GetChildComponent<Button>(buttonPath);
        if (btn != null)
        {
            btn.onClick.AddListener(onClick);
        }
    }

    /// <summary>
    /// 显示/隐藏UI
    /// </summary>
    /// <param name="isShow">是否显示</param>
    protected void SetVisible(bool isShow)
    {
        gameObject.SetActive(isShow);
    }

    /// <summary>
    /// 延迟执行方法（简化Invoke调用）
    /// </summary>
    /// <param name="action">要执行的方法</param>
    /// <param name="delayTime">延迟时间（秒）</param>
    protected void DelayExecute(Action action, float delayTime)
    {
        StartCoroutine(DelayExecuteCoroutine(action, delayTime));
    }

    // 【修正】返回值为非泛型的 IEnumerator（协程专用）
    private IEnumerator DelayExecuteCoroutine(Action action, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        action?.Invoke();
    }

    /// <summary>
    /// 清理组件缓存
    /// </summary>
    private void ClearCache()
    {
        _componentCache.Clear();
    }
    #endregion
}