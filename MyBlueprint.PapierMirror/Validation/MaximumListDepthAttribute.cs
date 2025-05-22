using MyBlueprint.PapierMirror.Models.Nodes;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyBlueprint.PapierMirror.Validation
{
    /// <summary>
    /// Specifies the maximum allowable list depth for the node.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class MaximumListDepthAttribute : ValidationAttribute
    {
        private int Depth { get; }
        private Type[] ListTypes { get; }

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
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            switch (value)
            {
                case null:
                    return ValidationResult.Success;
                case Node node:
                    var maxDepth = MaxDepth(node);

                    return maxDepth <= Depth ? ValidationResult.Success : new ValidationResult($"List cannot exceed maximum depth of {Depth}. Received max depth: {maxDepth}");
                default:
                    return new ValidationResult("Unexpected object");
            }
        }

        /// <summary>
        /// Returns the maximum depth of the node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int MaxDepth(Node node)
        {
            var subDepth = 0;
            foreach (var child in node.Content ?? [])
            {
                if (Array.IndexOf(ListTypes, child.GetType()) >= 0)
                {
                    subDepth = Math.Max(subDepth, MaxDepth(child) + 1);
                }
                else
                {
                    subDepth = Math.Max(subDepth, MaxDepth(child));
                }
            }

            return subDepth;
        }
    }
}