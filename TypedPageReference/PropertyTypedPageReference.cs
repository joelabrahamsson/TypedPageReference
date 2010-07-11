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

        public virtual IEnumerable<PageType> GetValidPageTypes()
        {
            var allPageTypes = PageType.List();
            foreach (var pageType in allPageTypes)
            {
                Type type = PageTypeResolver.Instance.GetPageTypeType(pageType.ID);
                
                if(type == null)
                    continue;

                if (typeof(T).IsAssignableFrom(type))
                    yield return pageType;
            }
        }

        public virtual IEnumerable<string> GetValidPageTypeNames()
        {
            foreach (var validPageType in GetValidPageTypes())
            {
                yield return validPageType.Name;
            }
        }
    }
}
