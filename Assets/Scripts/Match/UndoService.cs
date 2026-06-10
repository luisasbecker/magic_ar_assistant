using System;
using System.Collections.Generic;

namespace MagicARAssistant.Match
{
    public sealed class UndoService
    {
        private readonly Stack<UndoRecord> _history = new();

        public int Count => _history.Count;

        public void Register(string description, Action undoAction)
        {
            if (undoAction == null)
            {
                return;
            }

            _history.Push(new UndoRecord(description, undoAction));
        }

        public bool TryUndo(out string description)
        {
            description = string.Empty;
            if (_history.Count == 0)
            {
                return false;
            }

            UndoRecord record = _history.Pop();
            description = record.Description;
            record.UndoAction.Invoke();
            return true;
        }

        public void Clear()
        {
            _history.Clear();
        }

        private readonly struct UndoRecord
        {
            public UndoRecord(string description, Action undoAction)
            {
                Description = description;
                UndoAction = undoAction;
            }

            public string Description { get; }
            public Action UndoAction { get; }
        }
    }
}

