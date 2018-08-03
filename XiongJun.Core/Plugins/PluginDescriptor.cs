using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using XiongJun.Core.Infrastructure;

namespace XiongJun.Core.Plugins
{
    /// <summary>
    /// Represents a plugin descriptor
    /// </summary>
    public sealed class PluginDescriptor : IDescriptor, IComparable<PluginDescriptor>
    {
        #region Ctors

        /// <summary>
        /// Ctor
        /// </summary>
        public PluginDescriptor()
        {
            this.SupportedVersions = new List<string>();
            this.LimitedToStores = new List<int>();
            this.LimitedToCustomerRoles = new List<int>();
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="referencedAssembly">Referenced assembly</param>
        public PluginDescriptor(Assembly referencedAssembly) : this()
        {
            this.ReferencedAssembly = referencedAssembly;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the instance of the plugin
        /// </summary>
        /// <returns>Plugin instance</returns>
        public IPlugin Instance()
        {
            return Instance<IPlugin>();
        }

        /// <summary>
        /// Get the instance of the plugin
        /// </summary>
        /// <typeparam name="T">Type of the plugin</typeparam>
        /// <returns>Plugin instance</returns>
        public T Instance<T>() where T : class, IPlugin
        {
            object instance = null;
            try
            {
                instance = EngineContext.Current.Resolve(PluginType);
            }
            catch
            {
                //try resolve
            }
            if (instance == null)
            {
                //not resolved
                instance = EngineContext.Current.ResolveUnregistered(PluginType);
            }
            var typedInstance = instance as T;
            if (typedInstance != null)
                typedInstance.PluginDescriptor = this;
            return typedInstance;
        }

        /// <summary>
        /// Compares this instance with a specified PluginDescriptor object
        /// </summary>
        /// <param name="other">The PluginDescriptor to compare with this instance</param>
        /// <returns>An integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the specified parameter</returns>
        public int CompareTo(PluginDescriptor other)
        {
            return DisplayOrder != other.DisplayOrder ? DisplayOrder.CompareTo(other.DisplayOrder) : String.Compare(FriendlyName, other.FriendlyName, StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns the plugin as a string
        /// </summary>
        /// <returns>Value of the FriendlyName</returns>
        public override string ToString()
        {
            return FriendlyName;
        }

        /// <summary>
        /// Determines whether this instance and another specified PluginDescriptor object have the same SystemName
        /// </summary>
        /// <param name="value">The PluginDescriptor to compare to this instance</param>
        /// <returns>True if the SystemName of the value parameter is the same as the SystemName of this instance; otherwise, false</returns>
        public override bool Equals(object value)
        {
            return SystemName?.Equals((value as PluginDescriptor)?.SystemName) ?? false;
        }

        /// <summary>
        /// Returns the hash code for this plugin descriptor
        /// </summary>
        /// <returns>A 32-bit signed integer hash code</returns>
        public override int GetHashCode()
        {
            return SystemName.GetHashCode();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the plugin group
        /// </summary>
        [JsonProperty(PropertyName = "Group")]
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets the plugin friendly name
        /// </summary>
        [JsonProperty(PropertyName = "FriendlyName")]
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the plugin system name
        /// </summary>
        [JsonProperty(PropertyName = "SystemName")]
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the version
        /// </summary>
        [JsonProperty(PropertyName = "Version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the supported versions of nopCommerce
        /// </summary>
        [JsonProperty(PropertyName = "SupportedVersions")]
        public IList<string> SupportedVersions { get; set; }

        /// <summary>
        /// Gets or sets the author
        /// </summary>
        [JsonProperty(PropertyName = "Author")]
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        [JsonProperty(PropertyName = "DisplayOrder")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the name of the assembly file
        /// </summary>
        [JsonProperty(PropertyName = "FileName")]
        public string AssemblyFileName { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the list of store identifiers in which this plugin is available. If empty, then this plugin is available in all stores
        /// </summary>
        [JsonProperty(PropertyName = "LimitedToStores")]
        public IList<int> LimitedToStores { get; set; }

        /// <summary>
        /// Gets or sets the list of customer role identifiers for which this plugin is available. If empty, then this plugin is available for all ones.
        /// </summary>
        [JsonProperty(PropertyName = "LimitedToCustomerRoles")]
        public IList<int> LimitedToCustomerRoles { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether plugin is installed
        /// </summary>
        [JsonIgnore]
        public bool Installed { get; set; }

        /// <summary>
        /// Gets or sets the plugin type
        /// </summary>
        [JsonIgnore]
        public Type PluginType { get; set; }

        /// <summary>
        /// Gets or sets the original assembly file that a shadow copy was made from it
        /// </summary>
        [JsonIgnore]
        public FileInfo OriginalAssemblyFile { get; internal set; }

        /// <summary>
        /// Gets or sets the assembly that has been shadow copied that is active in the application
        /// </summary>
        [JsonIgnore]
        public Assembly ReferencedAssembly { get; internal set; }

        #endregion

    }
}
