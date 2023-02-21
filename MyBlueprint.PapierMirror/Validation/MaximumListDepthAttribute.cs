using MyBlueprint.PapierMirror.Models.Nodes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyBlueprint.PapierMirror.Validation
{
    /// <summary>
    /// Specifies the maximum allowable list depth for the node.
    /// </summary>
    public sealed class MaximumListDepthAttribute : ValidationAttribute
    {
        private int Depth { get; }
        private Type[] ListTypes { get; }
        private int MaxNodeDepth { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaximumListDepthAttribute"/> class.
        /// </summary>
        /// <param name="depth"></param>
        public MaximumListDepthAttribute(int depth) : this(depth, typeof(BulletList), typeof(OrderedList)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaximumListDepthAttribute"/> class.
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="listTypes"></param>
        public MaximumListDepthAttribute(int depth, params Type[] listTypes)
        {
            Depth = depth;
            ListTypes = listTypes;
        }

        /// <inheritdoc />
        public override bool IsValid(object? value)
        {
            return value switch
            {
                null => true,
                Node node => MaxDepth(node) <= Depth,
                _ => false
            };
        }

        /// <summary>
        /// Returns the maximum depth of the node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int MaxDepth(Node node)
        {
            var subDepth = 0;
            foreach (var child in node.Content ?? Enumerable.Empty<Node>())
            {
                if (Array.IndexOf(ListTypes, child.GetType()) >= 0)
                {
                    if (node.GetType() == typeof(Document))
                    {
                        if (subDepth > MaxNodeDepth)
                        {
                            MaxNodeDepth = subDepth;
                        }

                        subDepth = 0;
                    }

                    subDepth += MaxDepth(child) + 1;
                }
                else
                {
                    subDepth += MaxDepth(child);
                }
            }

            if (node.GetType() == typeof(Document))
            {
                return MaxNodeDepth;
            }

            return subDepth;
        }
    }
}