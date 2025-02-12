/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Collections.Generic;
using InfinityCode.ProjectContextActions.UnityTypes;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.ProjectContextActions
{
    [InitializeOnLoad]
    public class ItemDrawer
    {
        private static bool _isDirty;
        private static ProjectItem _item;
        private static string _lastHoveredGuid;
        private static List<Listener> _listeners;
        private static EditorWindow _mouseOverWindow;

        static ItemDrawer()
        {
            EditorApplication.projectWindowItemOnGUI += OnProjectItemGUI;
            EditorApplication.update += OnUpdate;
            _item = new ProjectItem();
        }

        private static int CompareListeners(Listener i1, Listener i2)
        {
            if (i1.Order == i2.Order) return 0;
            if (i1.Order > i2.Order) return 1;
            return -1;
        }

        private static void InvokeListeners()
        {
            foreach (Listener listener in _listeners)
            {
                if (listener.Action != null)
                {
                    try
                    {
                        listener.Action(_item);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
        }

        private static void OnProjectItemGUI(string guid, Rect rect)
        {
            if (_listeners == null) return;

            if (_isDirty)
            {
                _listeners.Sort(CompareListeners);
                _isDirty = false;
            }
            
            if (_mouseOverWindow != null && _mouseOverWindow.GetType() == ProjectBrowserRef.Type && _mouseOverWindow.wantsMouseMove == false)
            {
                _mouseOverWindow.wantsMouseMove = true;
            }

            _item.Set(guid, rect);
            bool needRepaint = false;

            if (_item.hovered && _lastHoveredGuid != _item.guid)
            {
                _lastHoveredGuid = _item.guid;
                needRepaint = true;
            }

            InvokeListeners();

            if (needRepaint && _mouseOverWindow != null) _mouseOverWindow.Repaint();
        }
        
        private static void OnUpdate()
        {
            _mouseOverWindow = EditorWindow.mouseOverWindow;
        }

        public static void Register(string id, Action<ProjectItem> action, float order = 0)
        {
            if (string.IsNullOrEmpty(id)) throw new Exception("ID cannot be empty");
            if (_listeners == null) _listeners = new List<Listener>();

            int hash = id.GetHashCode();
            foreach (Listener listener in _listeners)
            {
                if (listener.Hash == hash && listener.Id == id)
                {
                    listener.Action = action;
                    listener.Order = order;
                    return;
                }
            }
            _listeners.Add(new Listener
            {
                Id = id,
                Hash = hash,
                Action = action,
                Order = order
            });

            _isDirty = true;
        }

        private class Listener
        {
            public int Hash;
            public string Id;
            public Action<ProjectItem> Action;
            public float Order;
        }
    }
}