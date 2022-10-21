using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityExtension.Runtime.extension.Scripts.Runtime.Components.Auto
{
    public abstract class AutoUIBehavior : UIBehaviour
    {
        private readonly IList<FieldInfo> awakeFields;
        private readonly IList<FieldInfo> enableFields;
        private readonly IList<FieldInfo> startFields;

        protected AutoUIBehavior()
        {
            var fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<InjectAttribute>() != null);

            awakeFields = fields
                .Where(x => x.GetCustomAttribute<InjectAttribute>().Time == InjectionTime.Awake)
                .ToList();
            enableFields = fields
                .Where(x => x.GetCustomAttribute<InjectAttribute>().Time == InjectionTime.Enable)
                .ToList();
            startFields = fields
                .Where(x => x.GetCustomAttribute<InjectAttribute>().Time == InjectionTime.Start)
                .ToList();
        }

        #region Builtin Methods

        protected virtual void Awake()
        {
            foreach (var awakeField in awakeFields)
            {
                InjectField(awakeField);
            }
        }

        protected virtual void OnEnable()
        {
            foreach (var enableField in enableFields)
            {
                InjectField(enableField);
            }
        }

        protected virtual void Start()
        {
            foreach (var startField in startFields)
            {
                InjectField(startField);
            }
        }

        #endregion

        private void InjectField(FieldInfo field)
        {
            switch (field.GetCustomAttribute<InjectAttribute>().Place)
            {
                case InjectionPlace.Current:
                    field.SetValue(this, field.FieldType.IsArray ? GetComponents(field.FieldType.GetElementType()) : GetComponent(field.FieldType));
                    break;
                case InjectionPlace.Children:
                    field.SetValue(this, field.FieldType.IsArray ? GetComponentsInChildren(field.FieldType.GetElementType()) : GetComponentInChildren(field.FieldType));
                    break;
                case InjectionPlace.Parents:
                    field.SetValue(this, field.FieldType.IsArray ? GetComponentsInParent(field.FieldType.GetElementType()) : GetComponentInParent(field.FieldType));
                    break;
                default:
                    throw new NotImplementedException(field.GetCustomAttribute<InjectAttribute>().Place.ToString());
            }
        }
    }
}