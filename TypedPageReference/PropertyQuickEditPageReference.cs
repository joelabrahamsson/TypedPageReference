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
                           PopupWidth = GetPopupWidth(),
                           PopupHeight = GetPopupHeight()
                       };
        }

        protected virtual string GetQuickEditLinkText()
        {
            if (!string.IsNullOrEmpty(GetQuickEditLinkTextLanguageKey()))
            {
                string linkText = LanguageManager.Instance.Translate((GetQuickEditLinkTextLanguageKey()));
                if (!string.IsNullOrEmpty(linkText))
                    return linkText;
            }

            return "Quick edit";
        }

        protected virtual string GetQuickEditLinkTextLanguageKey()
        {
            return null;
        }

        protected virtual int GetPopupWidth()
        {
            return 700;
        }

        protected virtual int GetPopupHeight()
        {
            return 500;
        }
    }
}
