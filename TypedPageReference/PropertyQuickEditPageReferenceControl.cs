using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;

namespace TypedPageReference
{
    public class PropertyQuickEditPageReferenceControl<T> : PropertyTypedPageReferenceControl<T>
    {
        public override void CreateEditControls()
        {
            base.CreateEditControls();

            if (PageReference.IsNullOrEmpty(EditControl.PageLink))
                return;

            PageData linkedPage = DataFactory.Instance.GetPage(EditControl.PageLink);

            Controls.Add(GetQuickEditLink(linkedPage));
        }

        private HyperLink GetQuickEditLink(PageData linkedPage)
        {
            var quickEditLink = new HyperLink();
            string editLink = 
                string.Format("javascript:window.open('{0}EditPanel.aspx?id={1}&mode=simpleeditmode','mywindow','width={2},height={3}');",
                              UriSupport.ResolveUrlFromUIAsRelativeOrAbsolute("edit/"), 
                              linkedPage.PageLink,
                              PopupWidth, PopupHeight);

            quickEditLink.Attributes["onclick"] = editLink;
            quickEditLink.NavigateUrl = "javascript:void(0);";
            quickEditLink.Text = QuickEditLinkText;
            return quickEditLink;
        }

        public virtual string QuickEditLinkText { get; set; }

        public virtual int PopupWidth { get; set; }

        public virtual int PopupHeight { get; set; }
    }
}
