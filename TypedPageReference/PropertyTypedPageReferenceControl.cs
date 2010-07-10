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
                AddErrorValidator(WrongTypeErrorMessage);

            base.ApplyEditChanges();
        }

        public virtual bool ValidateThatReferencedPageIsOfCorrectType(PageReference pageReference)
        {
            if (PageReference.IsNullOrEmpty(pageReference))
                return true;

            var referencedPage = DataFactory.Instance.GetPage(pageReference);
            return referencedPage is T;
        }
        
        public virtual string WrongTypeErrorMessage { get; set; }
    }
}
