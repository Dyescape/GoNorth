using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using GoNorth.Services.ImplementationStatusCompare;

namespace GoNorth.Data.NodeGraph
{
    /// <summary>
    /// Node Graph Link
    /// </summary>
    public class NodeLink : IImplementationListComparable, ICloneable
    {
        /// <summary>
        /// Id of the source node
        /// </summary>
        public string SourceNodeId { get; set; }

        /// <summary>
        /// Name of the source node port
        /// </summary>
        public string SourceNodePort { get; set; }

        /// <summary>
        /// Id of the target node
        /// </summary>
        public string TargetNodeId { get; set; }

        /// <summary>
        /// Name of the target node port
        /// </summary>
        public string TargetNodePort { get; set; }

        /// <summary>
        /// Vertices of the link
        /// </summary>
        public List<NodeLinkVertex> Vertices { get; set; }

        /// <summary>
        /// Label
        /// </summary>
        public string Label { get; set; }


        /// <summary>
        /// Id which is used in a list compare to detect deleted or new objects
        /// </summary>
        [JsonIgnore]
        public string ListComparableId { get { return SourceNodeId + SourceNodePort + TargetNodeId + TargetNodePort; } }

        /// <summary>
        /// Value which is used in a list compare for display
        /// </summary>
        [JsonIgnore]
        public CompareDifferenceValue ListComparableValue { get { return new CompareDifferenceValue("NodeConnection", CompareDifferenceValue.ValueResolveType.LanguageKey); } }

        /// <summary>
        /// Clones the object
        /// </summary>
        /// <returns>Cloned object</returns>
        public object Clone()
        {
            return new NodeLink {
                SourceNodeId = this.SourceNodeId,
                SourceNodePort = this.SourceNodePort,
                TargetNodeId = this.TargetNodeId,
                TargetNodePort = this.TargetNodePort,
                Vertices = this.Vertices != null ? this.Vertices.Select(v => v.Clone()).Cast<NodeLinkVertex>().ToList() : null,
                Label = this.Label
            };
        }
    }
}
