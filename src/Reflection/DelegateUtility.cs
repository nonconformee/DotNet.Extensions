using System;
using System.Reflection;

using RI.Utilities.Exceptions;
using RI.Utilities.Text;




namespace RI.Utilities.Reflection
{
    /// <summary>
    ///     Provides a utility for delegate types.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class DelegateUtility
    {
        #region Static Methods

        /// <summary>
        ///     Creates a delegate of a specified type from the given full method name.
        /// </summary>
        /// <param name="delegateType"> The type of the delegate to create. </param>
        /// <param name="fullMethodName"> The full method name. </param>
        /// <returns>
        ///     The created delegate.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="CreateFromFullMethodName" /> can be used to create a delegate pointing to a static method as returned by <see cref="DelegateExtensions.GetFullMethodName" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="delegateType" /> or <paramref name="fullMethodName" /> is null. </exception>
        /// <exception cref="EmptyStringArgumentException"> <paramref name="fullMethodName" /> is an empty string. </exception>
        /// <exception cref="ArgumentException"> <paramref name="fullMethodName" /> is an invalid full method name. </exception>
        /// <exception cref="TypeLoadException"> The type and/or method specified by <paramref name="fullMethodName" /> cannot be resolved. </exception>
        public static Delegate CreateFromFullMethodName (Type delegateType, string fullMethodName)
        {
            if (delegateType == null)
            {
                throw new ArgumentNullException(nameof(delegateType));
            }

            if (fullMethodName == null)
            {
                throw new ArgumentNullException(nameof(fullMethodName));
            }

            if (fullMethodName.IsNullOrEmptyOrWhitespace())
            {
                throw new EmptyStringArgumentException(nameof(fullMethodName));
            }

            int lastIndex = fullMethodName.LastIndexOf(".", StringComparison.Ordinal);

            if (lastIndex == -1)
            {
                throw new ArgumentException("The full method name is invalid: " + fullMethodName, nameof(fullMethodName));
            }

            string declaringType = fullMethodName.Substring(0, lastIndex);
            string methodName = fullMethodName.Substring(lastIndex + 1);

            Type type = Type.GetType(declaringType, false);
            MethodInfo method = type?.GetMethod(methodName, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            if (method == null)
            {
                throw new TypeLoadException("The type and/or method cannot be resolved to create a delegate: " + fullMethodName);
            }

            Delegate del = Delegate.CreateDelegate(delegateType, method, true);
            return del;
        }

        #endregion
    }
}
