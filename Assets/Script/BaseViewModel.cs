using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

/// <summary>
/// ViewModel基类：监听语言切换，通知View更新
/// </summary>
public class BaseViewModel : IViewModel
{
    protected IModel _model;
    protected IView _view;
    protected LanguageManager _langMgr => LanguageManager.Instance;

    public virtual void BindModel(IModel model)
    {
        _model = model;
        // 监听语言切换事件（绑定Model时注册）
        _langMgr.OnLanguageChanged += OnLangChanged;
    }

    public virtual void BindView(IView view)
    {
        _view = view;
    }

    /// <summary>
    /// 处理View事件
    /// </summary>
    public virtual void OnViewEvent(string eventName, object param) { }

    /// <summary>
    /// 更新Model数据
    /// </summary>
    public virtual void UpdateModel(object data) { }

    /// <summary>
    /// 语言切换回调：通知View更新文案
    /// </summary>
    protected virtual void OnLangChanged()
    {
        // 通知View刷新所有文案（可传递当前语言）
        _view?.UpdateView(new { IsLangChange = true, Lang = _langMgr.CurrentLang });
    }

    /// <summary>
    /// 通知View更新
    /// </summary>
    protected void NotifyViewUpdate(object data)
    {
        _view?.UpdateView(data);
    }

    /// <summary>
    /// 取消监听（防止内存泄漏）
    /// </summary>
    public virtual void Dispose()
    {
        _langMgr.OnLanguageChanged -= OnLangChanged;
    }
}
