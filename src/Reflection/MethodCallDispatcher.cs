using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using RI.Utilities.Collections;
using RI.Utilities.Exceptions;
using RI.Utilities.Text;




namespace RI.Utilities.Reflection
{
    /// <summary>
    ///     Provides a utility which allows dynamic dispatching of method calls based on a parameters type.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Method call dispatching is the dynamic forwarding of a method call based on the name of methods (including overloads) and the type of the first method parameter.
    ///     </para>
    ///     <para>
    ///         A <see cref="MethodCallDispatcher" /> can be created either for an object or a type.
    ///         If an object is used, the method calls are forwarded to the instance methods of that object.
    ///         If a type is used, the method calls are forwarded to the static methods of the type.
    ///     </para>
    ///     <para>
    ///         First, an instance of <see cref="MethodCallDispatcher" /> is created using <see cref="FromTarget" /> or <see cref="FromType" />.
    ///         Afterwards, calls to the methods or the overloads respectively, as specified by the methods name, can be dispatched.
    ///         This is done by checking which method overload (if any) has a first parameter which matches the dispatched parameters type.
    ///         That method is then called.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public sealed class MethodCallDispatcher
    {
        #region Static Properties/Indexer

        private static object GlobalSyncRoot { get; } = new object();

        private static Dictionary<Type, Dictionary<string, MethodCallDispatcher>> Prototypes { get; } = new Dictionary<Type, Dictionary<string, MethodCallDispatcher>>();

        #endregion




        #region Static Methods

        /// <summary>
        ///     Creates a method call dispatcher for a specified object.
        /// </summary>
        /// <param name="target"> The object. </param>
        /// <param name="methodName"> The name of the methods the calls are dispatched to. </param>
        /// <returns>
        ///     A new instance of <see cref="MethodCallDispatcher" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Only instance methods are considered for dispatching when using an object instead of a type.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="target" /> or <paramref name="methodName" /> is null. </exception>
        /// <exception cref="EmptyStringArgumentException"> <paramref name="methodName" /> is an empty string. </exception>
        public static MethodCallDispatcher FromTarget (object target, string methodName)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (methodName == null)
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            if (methodName.IsNullOrEmptyOrWhitespace())
            {
                throw new EmptyStringArgumentException(nameof(methodName));
            }

            return MethodCallDispatcher.CreateInternal(target, target.GetType(), methodName);
        }

        /// <summary>
        ///     Creates a method call dispatcher for a specified type.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <param name="methodName"> The name of the methods the calls are dispatched to. </param>
        /// <returns>
        ///     A new instance of <see cref="MethodCallDispatcher" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Only static methods are considered for dispatching when using a type instead of an object.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="type" /> or <paramref name="methodName" /> is null. </exception>
        /// <exception cref="EmptyStringArgumentException"> <paramref name="methodName" /> is an empty string. </exception>
        public static MethodCallDispatcher FromType (Type type, string methodName)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (methodName == null)
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            if (methodName.IsNullOrEmptyOrWhitespace())
            {
                throw new EmptyStringArgumentException(nameof(methodName));
            }

            return MethodCallDispatcher.CreateInternal(null, type, methodName);
        }

        private static MethodCallDispatcher CreateInternal (object target, Type type, string methodName)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (methodName == null)
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            if (methodName.IsNullOrEmptyOrWhitespace())
            {
                throw new EmptyStringArgumentException(nameof(methodName));
            }

            lock (MethodCallDispatcher.GlobalSyncRoot)
            {
                if (!MethodCallDispatcher.Prototypes.ContainsKey(type))
                {
                    MethodCallDispatcher.Prototypes.Add(type, new Dictionary<string, MethodCallDispatcher>(StringComparer.Ordinal));
                }

                if (!MethodCallDispatcher.Prototypes[type]
                                         .ContainsKey(methodName))
                {
                    MethodCallDispatcher.Prototypes[type]
                                        .Add(methodName, new MethodCallDispatcher(type, methodName));
                }

                MethodCallDispatcher prototype = MethodCallDispatcher.Prototypes[type][methodName];
                return target == null ? prototype : new MethodCallDispatcher(target, prototype);
            }
        }

        #endregion




        #region Instance Constructor/Destructor

        private MethodCallDispatcher (Type type, string methodName)
        {
            this.Type = type;
            this.MethodName = methodName;
            this.Target = null;

            this.Initialize(null);
        }

        private MethodCallDispatcher (object target, MethodCallDispatcher prototype)
        {
            this.Type = prototype.Type;
            this.MethodName = prototype.MethodName;
            this.Target = target;

            this.Initialize(prototype);
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the name of the methods the calls are dispatched to.
        /// </summary>
        /// <value>
        ///     The name of the methods the calls are dispatched to.
        /// </value>
        public string MethodName { get; }

        /// <summary>
        ///     Gets the object to which the method calls are dispatched.
        /// </summary>
        /// <value>
        ///     The object to which the method calls are dispatched or null if the calls are dispatched to a type instead of an object.
        /// </value>
        public object Target { get; }

        /// <summary>
        ///     Gets the type to which the method calls are dispatched.
        /// </summary>
        /// <value>
        ///     The type to which the method calls are dispatched.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         <see cref="Type" /> always returns a type, even if the method calls are dispatched to an object.
        ///         In such cases, <see cref="Type" /> returns the type of <see cref="Target" />.
        ///     </para>
        /// </remarks>
        public Type Type { get; }

        private List<MethodInfo> Methods { get; set; }

        private Dictionary<Type, MethodInfo> Routes { get; set; }

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Checks whether a method call can be dispatched based on the parameter object.
        /// </summary>
        /// <param name="parameter"> The parameter object. </param>
        /// <returns>
        ///     true if the type of <paramref name="parameter" /> can be dispatched, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="parameter" /> is null. </exception>
        public bool CanDispatchParameter (object parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            return this.CanDispatchType(parameter.GetType());
        }

        /// <summary>
        ///     Checks whether a method call can be dispatched based on the parameter type.
        /// </summary>
        /// <param name="type"> The parameter type. </param>
        /// <returns>
        ///     true if <paramref name="type" /> can be dispatched, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="type" /> is null. </exception>
        public bool CanDispatchType (Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return this.Routes.ContainsKey(type);
        }

        /// <summary>
        ///     Dispatches a method call.
        /// </summary>
        /// <param name="parameter"> The parameter of the dispatched method call. </param>
        /// <returns>
        ///     The return value of the called method.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="parameter" /> is forwarded to the first parameter of the method.
        ///         Additional parameters are not resolved and get their types default value.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="parameter" /> is null. </exception>
        public object Dispatch (object parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            this.DispatchInternal(parameter, out object returnValue, null);
            return returnValue;
        }

        /// <summary>
        ///     Dispatches a method call.
        /// </summary>
        /// <param name="parameter"> The parameter of the dispatched method call. </param>
        /// <param name="parameterResolver"> The dependency resolver used to resolve additional parameters after the first. </param>
        /// <returns>
        ///     The return value of the called method.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="parameter" /> is forwarded to the first parameter of the method.
        ///         Additional parameters are resolved using <paramref name="parameterResolver" /> (first using the parameters type, then, if unsuccessful, using the parameters name).
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="parameter" /> or <paramref name="parameterResolver" /> is null. </exception>
        public object Dispatch (object parameter, IServiceProvider parameterResolver)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (parameterResolver == null)
            {
                throw new ArgumentNullException(nameof(parameterResolver));
            }

            this.DispatchInternal(parameter, out object returnValue, (name, type) => parameterResolver.GetService(type));
            return returnValue;
        }

        /// <summary>
        ///     Dispatches a method call.
        /// </summary>
        /// <param name="parameter"> The parameter of the dispatched method call. </param>
        /// <param name="parameterResolver"> The function used to resolve additional parameters after the first. </param>
        /// <returns>
        ///     The return value of the called method.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="parameter" /> is forwarded to the first parameter of the method.
        ///         Additional parameters are resolved using <paramref name="parameterResolver" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="parameter" /> or <paramref name="parameterResolver" /> is null. </exception>
        public object Dispatch (object parameter, Func<string, Type, object> parameterResolver)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (parameterResolver == null)
            {
                throw new ArgumentNullException(nameof(parameterResolver));
            }

            this.DispatchInternal(parameter, out object returnValue, parameterResolver);
            return returnValue;
        }

        /// <summary>
        ///     Dispatches a method call.
        /// </summary>
        /// <param name="parameter"> The parameter of the dispatched method call. </param>
        /// <param name="returnValue"> Receives the return value of the called method. </param>
        /// <returns>
        ///     true if the method call could be dispatched, false otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="parameter" /> is forwarded to the first parameter of the method.
        ///         Additional parameters are not resolved and get their types default value.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="parameter" /> is null. </exception>
        public bool Dispatch (object parameter, out object returnValue)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            return this.DispatchInternal(parameter, out returnValue, null);
        }

        /// <summary>
        ///     Dispatches a method call.
        /// </summary>
        /// <param name="parameter"> The parameter of the dispatched method call. </param>
        /// <param name="returnValue"> Receives the return value of the called method. </param>
        /// <param name="parameterResolver"> The dependency resolver used to resolve additional parameters after the first. </param>
        /// <returns>
        ///     true if the method call could be dispatched, false otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="parameter" /> is forwarded to the first parameter of the method.
        ///         Additional parameters are resolved using <paramref name="parameterResolver" /> (first using the parameters type, then, if unsuccessful, using the parameters name).
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="parameter" /> or <paramref name="parameterResolver" /> is null. </exception>
        public bool Dispatch (object parameter, out object returnValue, IServiceProvider parameterResolver)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (parameterResolver == null)
            {
                throw new ArgumentNullException(nameof(parameterResolver));
            }

            return this.DispatchInternal(parameter, out returnValue, (name, type) => parameterResolver.GetService(type));
        }

        /// <summary>
        ///     Dispatches a method call.
        /// </summary>
        /// <param name="parameter"> The parameter of the dispatched method call. </param>
        /// <param name="returnValue"> Receives the return value of the called method. </param>
        /// <param name="parameterResolver"> The function used to resolve additional parameters after the first. </param>
        /// <returns>
        ///     true if the method call could be dispatched, false otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="parameter" /> is forwarded to the first parameter of the method.
        ///         Additional parameters are resolved using <paramref name="parameterResolver" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="parameter" /> or <paramref name="parameterResolver" /> is null. </exception>
        public bool Dispatch (object parameter, out object returnValue, Func<string, Type, object> parameterResolver)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (parameterResolver == null)
            {
                throw new ArgumentNullException(nameof(parameterResolver));
            }

            return this.DispatchInternal(parameter, out returnValue, parameterResolver);
        }

        private bool DispatchInternal (object parameter, out object returnValue, Func<string, Type, object> parameterResolver)
        {
            returnValue = null;

            Type parameterType = parameter.GetType();
            parameterResolver = parameterResolver ?? ((name, type) => type.GetDefaultValue());

            if (!this.Routes.ContainsKey(parameterType))
            {
                return false;
            }

            MethodInfo method = this.Routes[parameterType];
            ParameterInfo[] parameters = method.GetParameters();

            object[] parameterValues = new object[parameters.Length];
            parameterValues[0] = parameter;

            for (int i1 = 1; i1 < parameters.Length; i1++)
            {
                parameterValues[i1] = parameterResolver(parameters[i1]
                                                            .Name, parameters[i1]
                                                            .ParameterType);
            }

            returnValue = method.Invoke(this.Target, parameterValues);

            return true;
        }

        private void Initialize (MethodCallDispatcher prototype)
        {
            this.Methods = new List<MethodInfo>();
            this.Routes = new Dictionary<Type, MethodInfo>();

            if (prototype != null)
            {
                this.Methods.AddRange(prototype.Methods);
                this.Routes.AddRange(prototype.Routes);

                return;
            }

            this.Methods.AddRange(this.Type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                                      .Where(x => string.Equals(x.Name, this.MethodName, StringComparison.Ordinal)));

            foreach (MethodInfo method in this.Methods)
            {
                ParameterInfo[] parameters = method.GetParameters();

                if (parameters.Length == 0)
                {
                    throw new InvalidOperationException("Method " + this.Type.Name + "." + method.Name + " must have at least one parameter to be dispatchable.");
                }

                ParameterInfo parameter = parameters[0];
                Type type = parameter.ParameterType;

                if (this.Routes.ContainsKey(type))
                {
                    throw new InvalidOperationException("Method " + this.Type.Name + "." + method.Name + " cannot have overloads which has the same type for the first parameter more than once.");
                }

                this.Routes.Add(type, method);
            }
        }

        #endregion
    }
}
