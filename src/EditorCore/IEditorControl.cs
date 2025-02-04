﻿using System.Collections.Generic;
using TextAdventures.Quest;

namespace QuestViva.EditorCore
{
    public interface IEditorControl
    {
        string ControlType { get; }
        string Caption { get; }
        int? Height { get; }
        int? Width { get; }
        string Attribute { get; }
        bool Expand { get; }
        string GetString(string tag);
        IEnumerable<string> GetListString(string tag);
        IDictionary<string, string> GetDictionary(string tag);
        int? GetInt(string tag);
        double? GetDouble(string tag);
        bool GetBool(string tag);
        bool IsControlVisible(IEditorData data);
        IEditorDefinition Parent { get; }
        bool IsControlVisibleInSimpleMode { get; }
        string Id { get; }
    }
}
