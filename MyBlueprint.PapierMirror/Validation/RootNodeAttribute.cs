using System;
using System.ComponentModel.DataAnnotations;

namespace MyBlueprint.PapierMirror.Validation
{
    /// <summary>
    /// Specifies the required type of the root node of the ProseMirror document.
    /// </summary>
    public sealed class RootNodeAttribute : ValidationAttribute
    {
        private Type RootType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RootNodeAttribute"/> class.
        /// </summary>
        /// <param name="rootType"></param>
        public RootNodeAttribute(Type rootType)
        {
            RootType = rootType;
        }

        /// <inheritdoc />
        public override bool IsValid(object? value)
        {
            if (value == null || value.GetType() == RootType)
            {
                return true;
            }

            return false;
        }
    }
}