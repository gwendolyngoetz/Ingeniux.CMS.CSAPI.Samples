using System;

namespace Ingeniux.CMS.CSAPI.Samples.Repository
{
    internal class RedirectRepository
    {
        private readonly IContentStoreContext _contentStoreContext;

        public RedirectRepository() : this(ContentStoreContext.Instance) { }
        public RedirectRepository(IContentStoreContext contentStoreContext)
        {
            _contentStoreContext = contentStoreContext;
        }

        public void AddRedirect(string pageId, string publishingTargetId, string oldUrl)
        {
            using (var session = _contentStoreContext.CreateSession(SessionType.ReadWrite))
            {
                try
                {
                    var page = session.Site.Page(pageId);
                    var publishingTarget = session.PublishingManager.Target(publishingTargetId);
                    publishingTarget.UrlMap().AddOrUpdateCustomUrl("add", page, oldUrl);

                }
                catch (UrlMapConflictException ex)
                {
                    throw new ApplicationException("This page already contains this redirect.", ex);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("An error occurred trying to add a redirect.", ex);
                }
            }
        }
    }
}
