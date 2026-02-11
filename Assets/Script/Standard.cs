using System;

/// <summary>
/// View接口：负责UI显示和事件转发
/// </summary>
public interface IView
{
    void BindViewModel(IViewModel viewModel);
    void UpdateView(object data); // 更新UI显示
}

/// <summary>
/// ViewModel接口：处理业务逻辑，双向绑定View和Model
/// </summary>
public interface IViewModel
{
    void BindModel(IModel model);
    void BindView(IView view);
    void OnViewEvent(string eventName, object param); // 接收View的事件
    void UpdateModel(object data); // 更新Model数据
}

/// <summary>
/// Model接口：负责数据管理（本地/网络）
/// </summary>
public interface IModel
{
    void LoadData(Action<object> callback); // 加载数据
    void SaveData(object data); // 保存数据
}