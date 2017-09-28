using System;
using System.Configuration;
using System.Data;

namespace Ingeniux.CMS.CSAPI.Samples.Repository
{
    internal interface IContentStoreContext
    {
        IUserSession CreateSession(SessionType sessionType = SessionType.ReadOnly);
    }

    // The ContentStore is a heavy weight object and should be created once
    //
    // This class uses the singleton design pattern. Be thoughtful when you use a singleton. They can lead to tightly coupled code if not very careful.
    internal class ContentStoreContext : IContentStoreContext
    {
        public static IContentStoreContext Instance { get; } = new ContentStoreContext();

        private readonly IContentStore _contentStore;
        private string _contentStoreUri => GetAppSetting("contentStoreUri", x => x);
        private string _contentStoreXmlPath => GetAppSetting("contentStoreXmlPath", x => x);
        private string _userId => GetAppSetting("userId", x => x);

        private ContentStoreContext()
        {
            _contentStore = new ContentStore(_contentStoreUri, _contentStoreXmlPath);
        }

        public IUserSession CreateSession(SessionType sessionType = SessionType.ReadOnly)
        {
            switch (sessionType)
            {
                case SessionType.ReadOnly:
                    return CreateReadSession();

                case SessionType.ReadWrite:
                    return CreateWriteSession();

                default:
                    throw new NotSupportedException($"{sessionType} is not supported.");
            }
        }

        private IUserSession CreateReadSession()
        {
            var currentUser = _contentStore.GetStartingUser(_userId);
            return _contentStore.OpenReadSession(currentUser);
        }

        private IUserSession CreateWriteSession()
        {
            var currentUser = _contentStore.GetStartingUser(_userId);
            return _contentStore.OpenWriteSession(currentUser);
        }

        private T GetAppSetting<T>(string key, Func<string, T> convert, bool isNullable = false)
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];

                var result = convert(value);

                if (!isNullable)
                {
                    if (result == null || string.IsNullOrWhiteSpace(result.ToString()))
                    {
                        throw new InvalidOperationException($"Verify that the app.config or web.config appSettings has an entry for {key}");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new DataException($"Unable to get configuration setting for {key}.", ex);
            }
        }
    }
}