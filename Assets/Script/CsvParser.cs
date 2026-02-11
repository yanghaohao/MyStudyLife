using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;

/// <summary>
/// CSV解析工具类（全局可访问）
/// </summary>
public static class CsvParser
{
    /// <summary>
    /// 解析CSV文本为行字典列表
    /// </summary>
    /// <param name="csvText">CSV文本内容</param>
    /// <returns>每行数据的字典（Key=表头，Value=单元格值）</returns>
    public static List<Dictionary<string, string>> Parse(string csvText)
    {
        var lines = csvText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var result = new List<Dictionary<string, string>>();

        if (lines.Length == 0) return result;

        // 解析表头（第一行）
        string[] headers = SplitCsvLine(lines[0]);

        // 解析数据行
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = SplitCsvLine(lines[i]);
            var row = new Dictionary<string, string>();

            for (int j = 0; j < headers.Length; j++)
            {
                row[headers[j]] = fields.Length > j ? fields[j] : string.Empty;
            }
            result.Add(row);
        }
        return result;
    }

    /// <summary>
    /// 处理CSV行拆分（兼容带引号的单元格，比如 "abc,def"）
    /// </summary>
    private static string[] SplitCsvLine(string line)
    {
        List<string> fields = new List<string>();
        StringBuilder currentField = new StringBuilder();
        bool inQuotes = false;

        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                // 逗号且不在引号内，分割字段
                fields.Add(currentField.ToString().Trim());
                currentField.Clear();
            }
            else
            {
                currentField.Append(c);
            }
        }

        // 添加最后一个字段
        fields.Add(currentField.ToString().Trim());
        return fields.ToArray();
    }
}
