using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// Represents an abstract expression in the Gwent namespace.
/// </summary>
/// Represents an abstract expression in the Gwent namespace.
/// </summary>
namespace Gwent
{
    /// <summary>
    /// Represents an abstract expression in the Gwent namespace.
    /// </summary>
    public abstract class Expression
    {
        /// <summary>
        /// Evaluates the expression and returns the result.
        /// </summary>
        /// <returns>The result of the expression evaluation.</returns>
        public abstract object Evaluate();

        /// <summary>
        /// Gets the scope of the expression.
        /// </summary>
        /// <param name="father">The current scope.</param>
        public abstract void SetScope(Scope current);

        /// <summary>
        /// Checks the semantic validity of the expression.
        /// </summary>
        /// <returns>True if the expression is semantically valid, false otherwise.</returns>
        public abstract Scope.DataType CheckSemantic();

#pragma warning disable CS8632 // La anotación para tipos de referencia que aceptan valores NULL solo debe usarse en el código dentro de un contexto de anotaciones "#nullable".
        protected abstract Scope? Context{ get; set; }
#pragma warning restore CS8632 // La anotación para tipos de referencia que aceptan valores NULL solo debe usarse en el código dentro de un contexto de anotaciones "#nullable".
    }
}