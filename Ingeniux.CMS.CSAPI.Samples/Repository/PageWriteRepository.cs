using System.Collections.Generic;
using Ingeniux.CMS.Enums;

namespace Ingeniux.CMS.CSAPI.Samples.Repository
{
    internal class PageWriteRepository
    {
        private readonly IContentStoreContext _contentStoreContext;

        public PageWriteRepository() : this(ContentStoreContext.Instance) { }
        public PageWriteRepository(IContentStoreContext contentStoreContext)
        {
            _contentStoreContext = contentStoreContext;
        }

        public void MovePageUnderNewParent(string pageIdToMove, string newParentPageId)
        {
            using (var session = _contentStoreContext.CreateSession(SessionType.ReadWrite))
            {
                var page = session.Site.Page(pageIdToMove);
                var newParent = session.Site.Page(newParentPageId);
                session.Site.MovePage(page, newParent, EnumCopyActions.IGX_MAKE_CHILD);
            }
        }

        public IPage CreatePage(string parentPageId, string name, string schemaRootName)
        {
            using (var session = _contentStoreContext.CreateSession(SessionType.ReadWrite))
            {
                var componentSchema = session.SchemasManager.SchemaByRootName(schemaRootName);
                var parentPage = session.Site.Page(parentPageId);

                var page = session.Site.CreatePage(componentSchema, name, parentPage);
                return page;
            }
        }

        public IPage AddChildPageToParentComponent(string parentPageId, string parentPageElementName, string childPageId)
        {
            using (var session = _contentStoreContext.CreateSession(SessionType.ReadWrite))
            {
                var parentPage = session.Site.Page(parentPageId);

                parentPage.Element(parentPageElementName).Value = childPageId;
                return parentPage;
            }
        }
        
        public void AddChildPageToParentList(string parentPageId, string parentPageListElementName, string childPageId)
        {
            using (var session = _contentStoreContext.CreateSession(SessionType.ReadWrite))
            {
                var parentPage = session.Site.Page(parentPageId);
                var childPage = session.Site.Page(childPageId);

                var listElement = parentPage.Element(parentPageListElementName) as ListElement;

                if (listElement == null)
                {
                    return;
                }

                // Remove the blank placeholder which exists on empty lists
                if (listElement._Elements.Count == 1 && listElement._Elements[0].GetType() == typeof(ComponentElement) && string.IsNullOrWhiteSpace(listElement._Elements[0].Value))
                {
                    listElement.ClearAllListItems();
                }

                listElement.AddListItem(new ComponentElement(listElement.ChildElementName)
                {
                    Value = childPage.Id,
                    Expanded = true
                });
            }
        }

        public void CheckInPageAndMarkForPublish(string pageId, string publishingTargetId)
        {
            using (var session = _contentStoreContext.CreateSession(SessionType.ReadWrite))
            {
                var publishingTarget = session.PublishingManager.Target(publishingTargetId);

                var page = session.Site.Page(pageId);
                page.CheckIn(new List<IPublishingTarget>() { publishingTarget }, false);
                page.MarkForPublish(publishingTarget, false, true);
            }
        }
    }
}
