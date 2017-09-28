namespace Ingeniux.CMS.CSAPI.Samples.Repository
{
    internal class WorkflowRepository
    {
        private readonly IContentStoreContext _contentStoreContext;

        public WorkflowRepository() : this(ContentStoreContext.Instance) { }
        public WorkflowRepository(IContentStoreContext contentStoreContext)
        {
            _contentStoreContext = contentStoreContext;
        }

        public void RemovePageFromWorkflow(string pageId)
        {
            using (var session = _contentStoreContext.CreateSession(SessionType.ReadWrite))
            {
                var page = session.Site.Page(pageId);
                page.RemoveFromWorkflow();
            }
        }

        public void AddPageToWorkflow(string pageId, string workflowDefinitionId)
        {
            using (var session = _contentStoreContext.CreateSession(SessionType.ReadWrite))
            {
                var workflowDefinition = session.WorkflowAdministrator.WorkflowDefinition(workflowDefinitionId);
                var page = session.Site.Page(pageId);
                page.AddToWorkflowDefault(workflowDefinition);
            }
        }
    }
}
