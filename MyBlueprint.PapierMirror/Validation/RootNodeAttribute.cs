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
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var type = value.GetType();
            if (type == RootType)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"Invalid root node. Expected {RootType.Name}, got {type.Name}"); 
        }
    }
}