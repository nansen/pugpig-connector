using EPiServer.Core;
using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace EPiPugPigConnector.Manifest
{
    [EPiServerDataStore(AutomaticallyCreateStore = true, AutomaticallyRemapStore = true)]
    public class ManifestModel : IDynamicData
    {
        public ContentReference StartPoint { get; set; }
        public bool IncludeStartPage { get; set; }

        #region IDynamicData Members

        public Identity Id
        { get; set; }

        #endregion
    }
}
