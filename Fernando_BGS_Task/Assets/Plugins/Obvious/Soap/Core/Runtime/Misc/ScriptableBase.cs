﻿using System;
using UnityEngine;

namespace Obvious.Soap
{
    /// <summary>
    /// Base classes of all ScriptableObjects in Soap
    /// </summary>
    public abstract class ScriptableBase : ScriptableObject
    {
        internal virtual void Reset()
        {
            CategoryIndex = 0;
            Description = "";
        }
        public Action RepaintRequest;
        [HideInInspector]
        public int CategoryIndex = 0;
        [HideInInspector]
        public string Description = "";
        public abstract Type GetGenericType { get; }
    }
}