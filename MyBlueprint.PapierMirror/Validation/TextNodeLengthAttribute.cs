using MyBlueprint.PapierMirror.Models.Nodes;
using System.ComponentModel.DataAnnotations;

namespace MyBlueprint.PapierMirror.Validation
{
    /// <summary>
    /// Specifies the maximum length of text nodes that are allowed in the ProseMirror <see cref="Node"/>.
    /// </summary>
    public sealed class TextNodeLengthAttribute : ValidationAttribute
    {
        private int Length { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextNodeLengthAttribute"/> class.
        /// </summary>
        /// <param name="length"></param>
        public TextNodeLengthAttribute(int length)
        {
            Length = length;
        }

        /// <inheritdoc />
        public override bool IsValid(object? value)
        {
            var count = 0;
            if (value is Node { Content: { } } node)
            {
                count += SumTextLength(node);
            }

            return count < Length;
        }

        /// <summary>
        /// Returns the total text length of a document.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static int SumTextLength(Node node)
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

                if (child is TextNode { Text: { } } textNode)
                {
                    count += textNode.Text.Length;
                }
            }

            return count;
        }
    }
}