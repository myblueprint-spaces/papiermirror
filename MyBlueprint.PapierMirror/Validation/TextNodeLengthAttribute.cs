using MyBlueprint.PapierMirror.Models.Nodes;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyBlueprint.PapierMirror.Validation
{
    /// <summary>
    /// Specifies the maximum length of text nodes that are allowed in the ProseMirror <see cref="Node"/>.
    /// </summary>
    public sealed class TextNodeLengthAttribute : ValidationAttribute
    {
        private int Length { get; }
        private Type TextNodeType { get; }
        private Func<Node, int> LengthFunc { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextNodeLengthAttribute"/> class.
        /// </summary>
        /// <param name="length"></param>
        public TextNodeLengthAttribute(int length) : this(length, typeof(TextNode), n => ((TextNode)n).Text?.Length ?? 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextNodeLengthAttribute"/> class.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="textNodeType"></param>
        /// <param name="textLengthFunc"></param>
        public TextNodeLengthAttribute(int length, Type textNodeType, Func<Node, int> textLengthFunc)
        {
            Length = length;
            TextNodeType = textNodeType;
            LengthFunc = textLengthFunc;
        }

        /// <inheritdoc />
        public override bool IsValid(object? value)
        {
            var count = 0;
            if (value is Node { Content: { } } node)
            {
                count += SumTextLength(node);
            }

            return count <= Length;
        }

        /// <summary>
        /// Returns the total text length of a document.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int SumTextLength(Node node)
        {
            var count = 0;
            if (node.Content == null)
            {
                return count;
            }

            foreach (var child in node.Content)
            {
                if (child.Content != null)
                {
                    count += SumTextLength(child);
                }

                if (child.GetType() == TextNodeType)
                {
                    var length = LengthFunc(child);
                    count += length;
                }
            }

            return count;
        }
    }
}