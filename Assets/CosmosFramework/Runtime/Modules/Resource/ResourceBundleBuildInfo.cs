﻿using System;

namespace Cosmos.Resource
{
    /// <summary>
    /// Build info of resource;
    /// </summary>
    [Serializable]
    public class ResourceBundleBuildInfo
    {
        /// <summary>
        /// Size of assetbundle;
        /// </summary>
        public long BundleSize;
        /// <summary>
        /// Hash of assetbundle
        /// </summary>
        public string BundleHash;
        /// <summary>
        /// 资源包的后缀，可空 [Nullable]
        /// </summary>
        public string BudleExtension;
        /// <summary>
        /// Resource bundle pack;
        /// </summary>
        public ResourceBundle ResourceBundle;
    }
}
