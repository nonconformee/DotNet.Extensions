using System;




namespace RI.Utilities.Text
{
    /// <summary>
    ///     Defines a delegate which can be used with the <see cref="StringExtensions" />.<see cref="StringExtensions.SplitWhere(string,StringSplitOptions,StringSplitPredicate)" /> and <see cref="StringExtensions" />.<see cref="StringExtensions.SplitWhere(string,StringSplitPredicate)" /> method to implement custom splitting of strings by testing between each character of the string whether it is a split position.
    /// </summary>
    /// <param name="str"> The whole string which is being splitted. </param>
    /// <param name="currentToken"> The current token, which is the string starting at the last split position up to and including the index specified by <paramref name="previous" />. </param>
    /// <param name="previous"> The index of the previous character. </param>
    /// <param name="next"> The index of the next character. </param>
    /// <returns>
    ///     true if a split position between <paramref name="previous" /> and <paramref name="next" /> has been found.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         Split testing always happens between characters so that when a split position is found (the return value of <see cref="StringSplitPredicate" /> is true), the character at the index <paramref name="previous" /> goes into the previous token while the character at the index <paramref name="next" /> goes into the next token.
    ///     </para>
    ///     <para>
    ///         <see cref="StringSplitPredicate" /> is called before the first character, between each following character, and after the last character of a string to test whether to split the string at that position.
    ///         Therefore, before the first character, <paramref name="previous" /> is -1 and <paramref name="next" /> is 0, after the first character <paramref name="previous" /> is 0 and <paramref name="next" /> is 1, and so forth, and after the last character <paramref name="previous" /> is n-1 and <paramref name="next" /> is n where n is the length of the string.
    ///     </para>
    /// </remarks>
    public delegate bool StringSplitPredicate (string str, string currentToken, int previous, int next);
}
