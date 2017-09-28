using System;
using System.Collections.Generic;

namespace Ingeniux.CMS.CSAPI.Samples.Repository
{
    internal class PublishingTargetRepository
    {
        private readonly IContentStoreContext _contentStoreContext;

        public PublishingTargetRepository() : this(ContentStoreContext.Instance) { }
        public PublishingTargetRepository(IContentStoreContext contentStoreContext)
        {
            _contentStoreContext = contentStoreContext;
        }

        public IPublishingTarget GetPublishingTargetById(string publishingTargetId)
        {
            using (var session = _contentStoreContext.CreateSession())
            {
                return session.PublishingManager.Target(publishingTargetId);
            }
        }

        public IEnumerable<IPublishingTarget> GetAllPublishingTargets()
        {
            using (var session = _contentStoreContext.CreateSession())
            {
                return session.PublishingManager.Targets(out int _);
            }
        }
    }
}
