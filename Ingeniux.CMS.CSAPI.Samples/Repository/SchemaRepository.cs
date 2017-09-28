using System.Collections.Generic;

namespace Ingeniux.CMS.CSAPI.Samples.Repository
{
    internal class SchemaRepository
    {
        private readonly IContentStoreContext _contentStoreContext;

        public SchemaRepository() : this(ContentStoreContext.Instance) { }
        public SchemaRepository(IContentStoreContext contentStoreContext)
        {
            _contentStoreContext = contentStoreContext;
        }

        public ISchema GetSchemaByRootName(string schemaRootName)
        {
            using (var session = _contentStoreContext.CreateSession(SessionType.ReadWrite))
            {
                return session.SchemasManager.SchemaByRootName(schemaRootName);
            }
        }

        public IEnumerable<ISchema> GetAllSchemas()
        {
            using (var session = _contentStoreContext.CreateSession(SessionType.ReadWrite))
            {
                return session.SchemasManager.Schemas(out int _);
            }
        }

        public IEnumerable<ISchemaField> GetSchemaFieldsByRootName(string schemaRootName)
        {
            using (var session = _contentStoreContext.CreateSession(SessionType.ReadWrite))
            {
                var schema = session.SchemasManager.SchemaByRootName(schemaRootName);
                return schema.Fields();
            }
        }
    }
}
