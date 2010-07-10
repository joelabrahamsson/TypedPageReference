using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using PageTypeBuilder;

namespace TypedPageReference
{
    [Serializable]
    public abstract class PropertyTypedPageReference<T> : PropertyPageReference
    {
        public override IPropertyControl CreatePropertyControl()
        {
            PropertyTypedPageReferenceControl<T> control = CreateTypedPageReferenceControl();
            SetControlsErrorMessages(control);

            return control;
        }

        protected virtual PropertyTypedPageReferenceControl<T> CreateTypedPageReferenceControl()
        {
            return new PropertyTypedPageReferenceControl<T>();
        }

        protected virtual void SetControlsErrorMessages(PropertyTypedPageReferenceControl<T> control)
        {
            control.WrongTypeErrorMessage = GetWrongTypeErrorMessage();
        }

        protected virtual string GetWrongTypeErrorMessage()
        {
            return
                string.Format(
                    CultureInfo.InvariantCulture,
                    GetWrongTypeErrorMessageTemplate(), 
                    GetValidPageTypesList());
        }

        protected virtual string GetWrongTypeErrorMessageTemplate()
        {
            if (!string.IsNullOrEmpty(WrongTypeErrorMessageLanguageKey))
            {
                string template = LanguageManager.Instance.Translate(WrongTypeErrorMessageLanguageKey);
                if (!string.IsNullOrEmpty(template))
                    return template;
            }

            return "The selected page is not of a valid page type. Valid page types for this property are: {0}";
        }

        protected virtual string GetValidPageTypesList()
        {
            StringBuilder validPageTypes = new StringBuilder();
            foreach (var valid in GetValidPageTypeNames())
            {
                validPageTypes.Append(valid);
                validPageTypes.Append(", ");
            }
            if (validPageTypes.Length > 2)
                validPageTypes = validPageTypes.Remove(validPageTypes.Length - 2, 2);

            return validPageTypes.ToString();
        }

        protected virtual string WrongTypeErrorMessageLanguageKey
        {
            get { return null; }
        }

        public virtual IEnumerable<Type> GetValidTypes()
        {
            var allPageTypes = PageType.List();
            foreach (var pageType in allPageTypes)
            {
                Type type = PageTypeResolver.Instance.GetPageTypeType(pageType.ID);
                
                if(type == null)
                    continue;

                if (typeof(T).IsAssignableFrom(type))
                    yield return type;
            }
        }

        public virtual IEnumerable<string> GetValidPageTypeNames()
        {
            foreach (var validType in GetValidTypes())
            {
                var pageTypeAttribute = GetAttribute(validType, typeof (PageTypeAttribute)) as PageTypeAttribute;

                if(pageTypeAttribute == null)
                    continue;

                if (!string.IsNullOrEmpty(pageTypeAttribute.Name))
                    yield return pageTypeAttribute.Name;
                else
                    yield return validType.Name;
            }
        }

        private static Attribute GetAttribute(Type type, Type attributeType)
        {
            Attribute attribute = null;

            object[] attributes = type.GetCustomAttributes(true);
            foreach (object attributeInType in attributes)
            {
                if (attributeType.IsAssignableFrom(attributeInType.GetType()))
                    attribute = (Attribute)attributeInType;
            }

            return attribute;
        }
    }
}
