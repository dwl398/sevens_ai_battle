using System;
using UnityEngine;

namespace Sevens.Editor
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SubclassSelectorAttribute : PropertyAttribute
    {
        private readonly bool includeMono;

        public SubclassSelectorAttribute(bool includeMono = false)
        {
            this.includeMono = includeMono;
        }

        public bool IsIncludeMono()
        {
            return includeMono;
        }
    }
}