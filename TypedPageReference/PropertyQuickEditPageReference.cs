using System;
using EPiServer.Core;

namespace TypedPageReference
{
    [Serializable]
    public abstract class PropertyQuickEditPageReference<T> : PropertyTypedPageReference<T>
    {
        protected override PropertyTypedPageReferenceControl<T> CreateTypedPageReferenceControl()
        {
            return new PropertyQuickEditPageReferenceControl<T>
                       {
                           QuickEditLinkText = GetQuickEditLinkText(),
                           PopupWidth = PopupWidth,
                           PopupHeight = PopupHeight
                       };
        }

        protected virtual string GetQuickEditLinkText()
        {
            if (!string.IsNullOrEmpty(QuickEditLinkTextLanguageKey))
            {
                string linkText = LanguageManager.Instance.Translate(QuickEditLinkTextLanguageKey);
                if (!string.IsNullOrEmpty(linkText))
                    return linkText;
            }

            return "Quick edit";
        }

        protected virtual string QuickEditLinkTextLanguageKey
        {
            get { return null; }
        }

        protected virtual int PopupWidth
        {
            get { return 700; }
        }

        protected virtual int PopupHeight
        {
            get { return 500; }
        }
    }
}
