using EPiServer;
using EPiServer.Core;
using EPiServer.Web.PropertyControls;

namespace TypedPageReference
{
    public class PropertyTypedPageReferenceControl<T> : PropertyPageReferenceControl
    {
        public override void ApplyEditChanges()
        {
            if (!ValidateThatReferencedPageIsOfCorrectType(EditControl.PageLink))
                AddErrorValidator(GetWrongTypeErrorMessage());

            base.ApplyEditChanges();
        }

        public virtual bool ValidateThatReferencedPageIsOfCorrectType(PageReference pageReference)
        {
            if (PageReference.IsNullOrEmpty(pageReference))
                return true;

            var referencedPage = DataFactory.Instance.GetPage(pageReference);
            return referencedPage is T;
        }

        protected virtual string GetWrongTypeErrorMessage()
        {
            

            return WrongTypeErrorMessage;
        }

        public virtual string WrongTypeErrorMessage { get; set; }
    }
}
