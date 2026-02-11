using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

// 总配置实体
[Serializable]
public class GameConfig
{
    public AudioConfig AudioConfig;    // 音频路径配置
    public TextConfig TextConfig;      // 文字文案配置
    public ResourceConfig ResourceConfig; // 资源路径配置
    public DialogConfig DialogConfig;  // 弹窗样式配置
}

[Serializable]
public class AudioConfig
{
    public string BtnClick;    // 按钮点击音效路径
    public string DialogOpen;  // 弹窗打开音效路径
    public string SuccessTip;  // 成功提示音效路径
}

[Serializable]
public class TextConfig
{
    public string LoginSuccess; // 登录成功文案
    public string LoginFail;    // 登录失败文案
    public string NetworkError; // 网络错误文案
}

[Serializable]
public class ResourceConfig
{
    public string DialogBg; // 弹窗背景图片路径
    public string VideoBg;  // 视频背景路径
}

[Serializable]
public class DialogConfig
{
    public int Width;          // 弹窗宽度
    public int Height;         // 弹窗高度
    public int TitleFontSize;  // 标题字体大小
    public int ContentFontSize;// 内容字体大小
    public string ConfirmBtnText; // 确认按钮文案
    public string CancelBtnText;  // 取消按钮文案
}
