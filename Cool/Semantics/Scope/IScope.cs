using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Semantics
{
    /// <summary>
    /// Provide an interface to implement the scope concept in Cool language programming
    /// </summary>
    interface IScope
    {
        /// <summary>
        /// Determines if a variable is defined.
        /// </summary>
        /// <param name="name">Name of the variable to search.</param>
        /// <param name="type">If true is returned type contain the Type that contain this variable.</param>
        /// <returns>True if found, false otherwise</returns>
        bool IsDefined(string name, out TypeInfo type);

        /// <summary>
        ///  Determines if a function is defined.
        /// </summary>
        /// <param name="name">Name of the function to search.</param>
        /// <param name="args">Array contain the types of arguments of the function.</param>
        /// <param name="type">If true is returned type contain the Type that contain this function.</param>
        /// <returns>True if found, false otherwise</returns>
        bool IsDefined(string name, TypeInfo[] args, out TypeInfo type);

        /// <summary>
        /// Determines if a type is defined.
        /// </summary>
        /// <param name="name">Expexted name of the type</param>
        /// <param name="type">If true is returned type contain the type found</param>
        /// <returns>True if found, false otherwise</returns>
        bool IsDefinedType(string name, out TypeInfo type);

        /// <summary>
        /// Define a variable.
        /// </summary>
        /// <param name="name">Expected name of the variable.</param>
        /// <param name="type">Type that contain the implementation of this function.</param>
        /// <returns>True if the variable was define correctly, false otherwise.</returns>
        bool Define(string name, TypeInfo type);

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="name">Expected name of the function.</param>
        /// <param name="args">Array of type arguments of the function.</param>
        /// <param name="type">Represent the type of the value to return</param>
        /// <returns>True if the function was define correctly, false otherwise</returns>
        bool Define(string name, TypeInfo[] args, TypeInfo type);

        /// <summary>
        /// Change the type of the variable.
        /// </summary>
        /// <param name="name">Expected name of the variable to change.</param>
        /// <param name="type">The type to set the variable</param>
        /// <returns>True if the variable was changed correctly, false otherwise </returns>
        bool Change(string name, TypeInfo type);

        /// <summary>
        /// Create a child of this scope.
        /// </summary>
        /// <returns>A child scope to this scope</returns>
        IScope CreateChild();

        /// <summary>
        /// Property that represent the scope parent of the scope.
        /// </summary>
        IScope Parent { get; set; }

        /// <summary>
        /// Property that represent the type own of the scope.
        /// </summary>
        TypeInfo Type { get; set; }

    }
}
