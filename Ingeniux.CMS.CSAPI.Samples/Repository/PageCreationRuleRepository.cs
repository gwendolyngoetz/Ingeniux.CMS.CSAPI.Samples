using System;

namespace Ingeniux.CMS.CSAPI.Samples.Repository
{
    internal class PageCreationRuleRepository
    {
        private readonly IContentStoreContext _contentStoreContext;

        public PageCreationRuleRepository() : this(ContentStoreContext.Instance) { }
        public PageCreationRuleRepository(IContentStoreContext contentStoreContext)
        {
            _contentStoreContext = contentStoreContext;
        }

        public void AddPageCreationRule(string pageId, string pageCreationRuleId)
        {
            using (var session = _contentStoreContext.CreateSession(SessionType.ReadWrite))
            {
                var pageCreationRule = session.PageCreationRulesManager.PageCreationRule(pageCreationRuleId);

                var page = session.Site.Page(pageId);
                page.SetPageCreationRule(pageCreationRule, false);
            }
        }

    }
}
