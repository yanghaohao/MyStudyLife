using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

/// <summary>
/// 统一的更新数据类（替代dynamic，强类型更安全）
/// </summary>
public class ViewUpdateData
{
    // 语言切换标记
    public bool IsLangChange { get; set; }
    // 当前语言
    public LanguageType Lang { get; set; }
    // 业务数据（按需扩展）
    public object BusinessData { get; set; }
}

/// <summary>
/// View基类：统一多语言文案更新
/// </summary>
public class BaseView : BaseComponent, IView
{
    protected IViewModel _viewModel;
    protected LanguageManager _langMgr => LanguageManager.Instance;

    public virtual void BindViewModel(IViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.BindView(this);
    }

    /// <summary>
    /// 核心：更新UI（兼容业务数据+语言切换）
    /// </summary>
    public virtual void UpdateView(object data)
    {
        // 1. 判断是否是语言切换数据
        if (data is ViewUpdateData langData && langData.IsLangChange)
        {
            RefreshLangText(); // 刷新所有文案
            return;
        }

        // 2. 处理业务数据（子类重写时处理）
        OnUpdateBusinessData(data);
    }

    /// <summary>
    /// 子类重写：处理业务数据更新
    /// </summary>
    /// <param name="data">业务数据</param>
    protected virtual void OnUpdateBusinessData(object data) { }

    /// <summary>
    /// 子类重写：刷新当前View的所有多语言文案
    /// </summary>
    protected virtual void RefreshLangText() { }

    /// <summary>
    /// 快捷获取多语言文案（View层专用）
    /// </summary>
    protected string GetLangText(string key) => _langMgr.GetText(key);

    /// <summary>
    /// 销毁时释放ViewModel监听
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (_viewModel is BaseViewModel baseVM)
        {
            baseVM.Dispose();
        }
    }
}
