using System;
using System.Collections.Generic;
using System.Linq;




namespace RI.Utilities.Collections
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="Stack{T}" /> type.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class StackExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Gets all the items from a stack in the order they would be pop'ed without removing them.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="stack" />. </typeparam>
        /// <param name="stack"> The stack. </param>
        /// <returns>
        ///     The list which contains all the items of the stack in the order they would be pop'ed.
        ///     The list is empty if the stack contains no items.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="stack" /> is null. </exception>
        public static List<T> PeekAll <T> (this Stack<T> stack)
        {
            if (stack == null)
            {
                throw new ArgumentNullException(nameof(stack));
            }

            return stack.AsEnumerable()
                        .ToList();
        }

        /// <summary>
        ///     Gets all the items from a stack in the order they are pop'ed and removes them.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="stack" />. </typeparam>
        /// <param name="stack"> The stack. </param>
        /// <returns>
        ///     The list which contains all the items of the stack in the order they are pop'ed.
        ///     The list is empty if the stack contains no items.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="stack" /> is null. </exception>
        public static List<T> PopAll <T> (this Stack<T> stack)
        {
            if (stack == null)
            {
                throw new ArgumentNullException(nameof(stack));
            }

            List<T> items = new List<T>(stack.Count);

            while (stack.Count > 0)
            {
                items.Add(stack.Pop());
            }

            return items;
        }

        /// <summary>
        ///     Gets all the items from a stack in the order they are pop'ed and removes them.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="stack" />. </typeparam>
        /// <param name="stack"> The stack. </param>
        /// <param name="collection"> The collection the popped items are put into. </param>
        /// <returns>
        ///     The number of popped items.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="stack" /> or <paramref name="collection" /> is null. </exception>
        public static int PopInto <T> (this Stack<T> stack, ICollection<T> collection)
        {
            if (stack == null)
            {
                throw new ArgumentNullException(nameof(stack));
            }

            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            int count = stack.Count;

            while (stack.Count > 0)
            {
                collection.Add(stack.Pop());
            }

            return count;
        }

        /// <summary>
        ///     Pushes multiple items to a stack.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="stack" />. </typeparam>
        /// <param name="stack"> The stack. </param>
        /// <param name="items"> The sequence of items to push to the stack. </param>
        /// <returns>
        ///     The number of items pushed to the stack.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The items in <paramref name="items" /> are pushed in the order they are enumerated.
        ///     </para>
        ///     <para>
        ///         <paramref name="items" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="stack" /> or <paramref name="items" /> is null. </exception>
        public static int PushRange <T> (this Stack<T> stack, IEnumerable<T> items)
        {
            if (stack == null)
            {
                throw new ArgumentNullException(nameof(stack));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            int count = 0;

            foreach (T item in items)
            {
                stack.Push(item);
                count++;
            }

            return count;
        }

        #endregion
    }
}
