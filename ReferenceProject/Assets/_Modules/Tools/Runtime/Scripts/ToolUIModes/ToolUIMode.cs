﻿using System;
using UnityEngine;
using Unity.AppUI.UI;
using UnityEngine.UIElements;

namespace Unity.ReferenceProject.Tools
{
    public interface IToolUIModeHandler
    {
        bool IsOpened { get; }
        public VisualElement CreateVisualTree(ActionButton button, ToolUIController toolUIController);

        public void OpenTool();

        public void CloseTool();
        
        public ActionButton GetButton();
    }

    public abstract class ToolUIMode : ScriptableObject
    {
        public abstract IToolUIModeHandler CreateHandler();
    }

    public abstract class ToolUIModeHandler : IToolUIModeHandler
    {
        protected ActionButton Button { private set; get; }

        protected ToolUIController ToolUIController { private set; get; }

        public VisualElement CreateVisualTree(ActionButton button, ToolUIController toolUIController)
        {
            Button = button;
            ToolUIController = toolUIController;
            ToolUIController.SetCloseAction(CloseTool);
            ToolUIController.SetButtonDisplayStyleAction(SetButtonDisplayStyle);

            return CreateVisualTreeInternal();
        }

        public bool IsOpened => Button.selected;

        public void OpenTool()
        {
            Button.selected = true;
            OnToolOpenedInternal();
            ToolUIController.InvokeToolOpened();
        }

        public void CloseTool()
        {
            Button.selected = false;
            OnToolClosedInternal();
            ToolUIController.InvokeToolClosed();
        }

        void SetButtonDisplayStyle(DisplayStyle style)
        {
            Button.style.display = style;
            if (style == DisplayStyle.None)
            {
                CloseTool();
            }
        }

        protected abstract VisualElement CreateVisualTreeInternal();

        protected abstract void OnToolOpenedInternal();

        protected abstract void OnToolClosedInternal();
        
        public ActionButton GetButton()
        {
            return Button;
        }
    }
}
