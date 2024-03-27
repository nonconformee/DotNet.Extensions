using System;
using System.Collections.Generic;




namespace RI.Utilities.Reflection
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="Type" /> type.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class TypeExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Determines the best matching type from a list of candidates.
        /// </summary>
        /// <param name="type"> The type for which the best matching type from <paramref name="types" /> is to be determined. </param>
        /// <param name="matchingType"> The type from <paramref name="types" /> which matches <paramref name="type" /> best. null if no matching type is found. </param>
        /// <param name="inheritanceDepth"> The depth of inheritance from <paramref name="type" /> to <paramref name="matchingType" />. -1 if no matching type is found. </param>
        /// <param name="types"> The list of candidates. </param>
        /// <returns>
        ///     true if a matching type was found, false otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This method determines the best match for <paramref name="type" /> from <paramref name="types" />.
        ///         The best match is the one which has the lowest inheritance depth (<paramref name="inheritanceDepth" />) by appearing in the inheritance list of <paramref name="type" />.
        ///         Or in other words, the type which is closest to <paramref name="type" /> in the inheritance list of <paramref name="type" /> is choosen.
        ///     </para>
        ///     <para>
        ///         <paramref name="inheritanceDepth" /> is zero if <paramref name="type" /> and <paramref name="matchingType" /> are the same (which is, for example, if <paramref name="type" /> also appears in <paramref name="types" />).
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="type" /> is null. </exception>
        public static bool GetBestMatchingType (this Type type, out Type matchingType, out int inheritanceDepth, params Type[] types)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            matchingType = null;
            inheritanceDepth = -1;

            if (types == null)
            {
                return false;
            }

            if (types.Length == 0)
            {
                return false;
            }

            List<Type> inheritance = type.GetInheritance(true);
            inheritance.Reverse();

            List<int> depths = new List<int>();

            foreach (Type candidate in types)
            {
                int depth = -1;

                for (int i1 = 0; i1 < inheritance.Count; i1++)
                {
                    if (inheritance[i1] == candidate)
                    {
                        depth = i1;
                        break;
                    }
                }

                depths.Add(depth);
            }

            int minDepth = int.MaxValue;
            int minIndex = -1;

            for (int i1 = 0; i1 < depths.Count; i1++)
            {
                int depth = depths[i1];

                if ((depth < minDepth) && (depth != -1))
                {
                    minDepth = depth;
                    minIndex = i1;
                }
            }

            if (minIndex == -1)
            {
                return false;
            }

            matchingType = types[minIndex];
            inheritanceDepth = depths[minIndex];
            return true;
        }

        /// <summary>
        ///     Gets the default value of a type.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <returns>
        ///     The default value of the type.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="type" /> is null. </exception>
        public static object GetDefaultValue (this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        /// <summary>
        ///     Gets all types the specified type inherits from.
        /// </summary>
        /// <param name="type"> The type of which the inheritance list should be returned. </param>
        /// <param name="includeSelf"> Specifies whether <paramref name="type" /> is also included in the returned inheritance list. </param>
        /// <returns>
        ///     The list with all types <paramref name="type" /> inherits from.
        ///     The list is empty if the type is <see cref="object" /> and <paramref name="includeSelf" /> is false.
        ///     The list starts with the root type of the inheritance, which is always <see cref="object" />.
        /// </returns>
        /// <remarks>
        ///     <note type="note">
        ///         The returned inheritance list does only contain base classes but not interfaces.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="type" /> is null. </exception>
        public static List<Type> GetInheritance (this Type type, bool includeSelf)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            List<Type> types = new List<Type>();

            if (includeSelf)
            {
                types.Add(type);
            }

            while (type.BaseType != null)
            {
                type = type.BaseType;
                types.Add(type);
            }

            types.Reverse();

            return types;
        }

        /// <summary>
        ///     Determines whether a type is assignable to a generic type.
        /// </summary>
        /// <param name="type"> The type to check whether it can be assigned to a generic type. </param>
        /// <param name="genericType"> The generic type. </param>
        /// <returns> true if <paramref name="type" /> can be assigned to <paramref name="genericType" />, false otherwise. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="type" /> or <paramref name="genericType" /> is null. </exception>
        /// <exception cref="ArgumentException"> <paramref name="genericType" /> is not a generic type. </exception>
        public static bool IsAssignableToGenericType (this Type type, Type genericType)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (genericType == null)
            {
                throw new ArgumentNullException(nameof(genericType));
            }

            if (!genericType.IsGenericType)
            {
                throw new ArgumentException("Type is not a generic type.", nameof(genericType));
            }

            Type genericTypeComparison = genericType.GetGenericTypeDefinition();

            if (type.IsGenericType && (type.GetGenericTypeDefinition() == genericTypeComparison))
            {
                return true;
            }

            Type[] interfaces = type.GetInterfaces();

            foreach (Type @interface in interfaces)
            {
                if (@interface.IsGenericType && (@interface.GetGenericTypeDefinition() == genericTypeComparison))
                {
                    return true;
                }
            }

            if (type.BaseType == null)
            {
                return false;
            }

            return type.BaseType.IsAssignableToGenericType(genericType);
        }

        /// <summary>
        ///     Gets whether a type is a nullable type.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <returns>
        ///     true if the type is nullable, using <see cref="Nullable{T}" />, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="type" /> is null. </exception>
        public static bool IsNullable (this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsGenericType)
            {
                return false;
            }

            return type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        #endregion
    }
}
