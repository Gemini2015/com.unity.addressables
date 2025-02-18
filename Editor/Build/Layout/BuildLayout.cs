using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.AddressableAssets.Build.Layout
{
    /// <summary>
    /// A storage class used to gather data about an Addressable build.
    /// </summary>
    [Serializable]
    public class BuildLayout
    {
        /// <summary>
        /// Version of the Unity edtior used to perform the build.
        /// </summary>
        public string UnityVersion;

        /// <summary>
        /// Version of the Addressables package used to perform the build.
        /// </summary>
        public string PackageVersion;

        /// <summary>
        /// Information about the AssetBundleObject
        /// </summary>
        [Serializable]
        public class AssetBundleObjectInfo
        {
            /// <summary>
            /// The size, in bytes, of the AssetBundleObject
            /// </summary>
            public ulong Size;
        }

        /// <summary>
        /// Data about the AddressableAssetGroup that gets processed during a build.
        /// </summary>
        [Serializable]
        public class Group
        {
            /// <summary>
            /// The Name of the AdressableAssetGroup
            /// </summary>
            public string Name;

            /// <summary>
            /// The Guid of the AddressableAssetGroup
            /// </summary>
            public string Guid;

            /// <summary>
            /// The packing mode as defined by the BundledAssetGroupSchema on the AddressableAssetGroup
            /// </summary>
            public string PackingMode;

            /// <summary>
            /// A list of the AssetBundles associated with the Group
            /// </summary>
            [SerializeReference]
            public List<Bundle> Bundles = new List<Bundle>();

            /// <summary>
            /// Data about the AddressableAssetGroupSchemas associated with the Group
            /// </summary>
            [SerializeReference]
            public List<SchemaData> Schemas = new List<SchemaData>();
        }

        /// <summary>
        /// Data container for AddressableAssetGroupSchemas
        /// </summary>
        [Serializable]
        public class SchemaData
        {
            /// <summary>
            /// The Guid of the AddressableAssetGroupSchema
            /// </summary>
            public string Guid;

            /// <summary>
            /// The class type of the AddressableAssetGroupSchema
            /// </summary>
            public string Type;

            /// <summary>
            /// These key-value-pairs include data about the AddressableAssetGroupSchema, such as PackingMode and Compression.
            /// </summary>
            [SerializeReference]
            public List<Tuple<string, string>> KvpDetails = new List<Tuple<string, string>>();
        }

        /// <summary>
        /// Data store for AssetBundle information.
        /// </summary>
        [Serializable]
        public class Bundle
        {
            /// <summary>
            /// The name of the AssetBundle
            /// </summary>
            public string Name;

            /// <summary>
            /// The file size of the AssetBundle on disk, in bytes
            /// </summary>
            public ulong FileSize;

            /// <summary>
            /// The Compression method used for the AssetBundle.
            /// </summary>
            public string Compression;

            /// <summary>
            /// A reference to the Group data that this AssetBundle was generated from
            /// </summary>
            [SerializeReference]
            public Group Group;

            /// <summary>
            /// List of the Files referenced by the AssetBundle
            /// </summary>
            [SerializeReference]
            public List<File> Files = new List<File>();

            /// <summary>
            /// A list of the direct dependencies of the AssetBundle
            /// </summary>
            [SerializeReference]
            public List<Bundle> Dependencies;

            /// <summary>
            /// The full dependency list, flattened into a list
            /// </summary>
            [SerializeReference]
            public List<Bundle> ExpandedDependencies;
        }

        /// <summary>
        /// Data store for resource files generated by the build pipeline and referenced by a main File
        /// </summary>
        [Serializable]
        public class SubFile
        {
            /// <summary>
            /// The name of the sub-file
            /// </summary>
            public string Name;

            /// <summary>
            /// If the main File is a serialized file, this will be true.
            /// </summary>
            public bool IsSerializedFile;

            /// <summary>
            /// The size of the sub-file, in bytes
            /// </summary>
            public ulong Size;
        }

        /// <summary>
        /// Data store for the main File created for the AssetBundle
        /// </summary>
        [Serializable]
        public class File
        {
            /// <summary>
            /// The name of the File.
            /// </summary>
            public string Name;

            /// <summary>
            /// The AssetBundle data that relates to a built file.
            /// </summary>
            [SerializeReference]
            public Bundle Bundle;

            /// <summary>
            /// List of the resource files created by the build pipeline that a File references
            /// </summary>
            [SerializeReference]
            public List<SubFile> SubFiles = new List<SubFile>();

            /// <summary>
            /// A list of the explicit asset defined in the AssetBundle
            /// </summary>
            [SerializeReference]
            public List<ExplicitAsset> Assets = new List<ExplicitAsset>();

            /// <summary>
            /// A list of implicit assets built into the AssetBundle, typically through references by Assets that are explicitly defined.
            /// </summary>
            [SerializeReference]
            public List<DataFromOtherAsset> OtherAssets = new List<DataFromOtherAsset>();

            /// <summary>
            /// The final filename of the AssetBundle file
            /// </summary>
            public string WriteResultFilename;

            /// <summary>
            /// Data about the AssetBundleObject
            /// </summary>
            public AssetBundleObjectInfo BundleObjectInfo;

            /// <summary>
            /// The size of the data that needs to be preloaded for this File.
            /// </summary>
            public int PreloadInfoSize;

            /// <summary>
            /// The number of Mono scripts referenced by the File
            /// </summary>
            public int MonoScriptCount;

            /// <summary>
            /// The size of the Mono scripts referenced by the File
            /// </summary>
            public ulong MonoScriptSize;
        }

        /// <summary>
        /// Data store for Assets explicitly defined in an AssetBundle
        /// </summary>
        [Serializable]
        public class ExplicitAsset
        {
            /// <summary>
            /// The Asset Guid.
            /// </summary>
            public string Guid;

            /// <summary>
            /// The Asset path on disk
            /// </summary>
            public string AssetPath;

            /// <summary>
            /// The Addressable address defined in the Addressable Group window for an Asset.
            /// </summary>
            public string AddressableName;

            /// <summary>
            /// The size of the file on disk.
            /// </summary>
            public ulong SerializedSize;

            /// <summary>
            /// The size of the streamed Asset.
            /// </summary>
            public ulong StreamedSize;

            /// <summary>
            /// The file that the Asset was added to
            /// </summary>
            [SerializeReference]
            public File File;

            /// <summary>
            /// List of data from other Assets referenced by an Asset in the File
            /// </summary>
            [SerializeReference]
            public List<DataFromOtherAsset> InternalReferencedOtherAssets = new List<DataFromOtherAsset>();

            /// <summary>
            /// List of explicit Assets in the File
            /// </summary>
            [SerializeReference]
            public List<ExplicitAsset> InternalReferencedExplicitAssets = new List<ExplicitAsset>();

            /// <summary>
            /// List of Assets referenced by the File, but not included in the File.
            /// </summary>
            [SerializeReference]
            public List<ExplicitAsset> ExternallyReferencedAssets = new List<ExplicitAsset>();
        }

        /// <summary>
        /// Data store for implicit Asset references
        /// </summary>
        [Serializable]
        public class DataFromOtherAsset
        {
            /// <summary>
            /// The Guid of the Asset
            /// </summary>
            public string AssetGuid;

            /// <summary>
            /// The Asset path on disk
            /// </summary>
            public string AssetPath;

            /// <summary>
            /// A list of Assets that reference this data
            /// </summary>
            [SerializeReference]
            public List<ExplicitAsset> ReferencingAssets = new List<ExplicitAsset>();

            /// <summary>
            /// The number of Objects in the data
            /// </summary>
            public int ObjectCount;

            /// <summary>
            /// The size of the data on disk
            /// </summary>
            public ulong SerializedSize;

            /// <summary>
            /// The size of the streamed data
            /// </summary>
            public ulong StreamedSize;
        }

        /// <summary>
        /// The Addressable Groups that reference this data
        /// </summary>
        [SerializeReference]
        public List<Group> Groups = new List<Group>();

        /// <summary>
        /// List of AssetBundles this data was built into
        /// </summary>
        [SerializeReference]
        public List<Bundle> BuiltInBundles = new List<Bundle>();
    }

    /// <summary>
    /// Utility used to quickly reference data built with the build pipeline
    /// </summary>
    public class LayoutLookupTables
    {
        /// <summary>
        /// The AssetBundle name to the Bundle data map.
        /// </summary>
        public Dictionary<string, BuildLayout.Bundle> Bundles = new Dictionary<string, BuildLayout.Bundle>();

        /// <summary>
        /// File name to File data map.
        /// </summary>
        public Dictionary<string, BuildLayout.File> Files = new Dictionary<string, BuildLayout.File>();

        /// <summary>
        /// Guid to ExplicitAsset data map.
        /// </summary>
        public Dictionary<string, BuildLayout.ExplicitAsset> GuidToExplicitAsset = new Dictionary<string, BuildLayout.ExplicitAsset>();

        /// <summary>
        /// Group name to Group data map.
        /// </summary>
        public Dictionary<string, BuildLayout.Group> GroupLookup = new Dictionary<string, BuildLayout.Group>();
    }

    /// <summary>
    /// Helper methods for gathering data about a build layout.
    /// </summary>
    public class BuildLayoutHelpers
    {
        /// <summary>
        /// Gather a list of Explicit Assets defined in a BuildLayout
        /// </summary>
        /// <param name="layout">The BuildLayout generated during a build</param>
        /// <returns>A list of ExplicitAsset data.</returns>
        public static IEnumerable<BuildLayout.ExplicitAsset> EnumerateAssets(BuildLayout layout)
        {
            return EnumerateBundles(layout).SelectMany(b => b.Files).SelectMany(f => f.Assets);
        }

        /// <summary>
        /// Gather a list of Explicit Assets defined in a Bundle
        /// </summary>
        /// <param name="bundle">The Bundle data generated during a build</param>
        /// <returns>A list of ExplicitAssets defined in the Bundle</returns>
        public static IEnumerable<BuildLayout.ExplicitAsset> EnumerateAssets(BuildLayout.Bundle bundle)
        {
            return bundle.Files.SelectMany(f => f.Assets);
        }

        /// <summary>
        /// Gather a list of Bundle data defined in a BuildLayout
        /// </summary>
        /// <param name="layout">The BuildLayout generated during a build</param>
        /// <returns>A list of the Bundle data defined in a BuildLayout</returns>
        public static IEnumerable<BuildLayout.Bundle> EnumerateBundles(BuildLayout layout)
        {
            foreach (BuildLayout.Bundle b in layout.BuiltInBundles)
                yield return b;

            foreach (BuildLayout.Bundle b in layout.Groups.SelectMany(g => g.Bundles))
                yield return b;
        }

        /// <summary>
        /// Gather a list of File data defined in a BuildLayout
        /// </summary>
        /// <param name="layout">The BuildLayout generated during a build</param>
        /// <returns>A list of File data</returns>
        public static IEnumerable<BuildLayout.File> EnumerateFiles(BuildLayout layout)
        {
            return EnumerateBundles(layout).SelectMany(b => b.Files);
        }
    }
}
