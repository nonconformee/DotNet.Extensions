using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using RI.Utilities.Collections;
using RI.Utilities.ObjectModel;




namespace RI.Utilities.Threading
{
    /// <summary>
    ///     Implements a heavy-weight thread which encapsulates thread setup and execution and also captures exceptions.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="HeavyThread" /> is usually used for long-running and/or dedicated threads where a higher control and encapsulation of the threads execution is required, e.g. compared to <see cref="ThreadPool" />.
    ///     </para>
    ///     <para>
    ///         See <see cref="Start" /> and <see cref="Stop" /> for a description of the thread execution sequence.
    ///     </para>
    ///     <note type="important">
    ///         Some virtual methods are called from within locks to <see cref="SyncRoot" />.
    ///         Be careful in inheriting classes when calling outside code from those methods (e.g. through events, callbacks, or other virtual methods) to not produce deadlocks!
    ///     </note>
    /// </remarks>
    /// <threadsafety static="true" instance="true" />
    public abstract class HeavyThread : IDisposable, ISynchronizable
    {
        #region Constants

        /// <summary>
        ///     The default thread timeout.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The default value is 10000.
        ///     </para>
        /// </remarks>
        public const int DefaultThreadTimeout = 10000;

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="HeavyThread" />.
        /// </summary>
        protected HeavyThread ()
        {
            this.SyncRoot = new object();
            this.StartStopSyncRoot = new object();

            this.ThreadException = null;
            this.IsRunning = false;
            this.HasStoppedGracefully = null;

            this.Timeout = HeavyThread.DefaultThreadTimeout;

            this.Thread = null;
            this.StopRequested = false;
            this.StopEvent = null;
            this.StopTasks = null;
        }

        /// <summary>
        ///     Garbage collects this instance of <see cref="HeavyThread" />.
        /// </summary>
        ~HeavyThread ()
        {
            this.Dispose(false);
        }

        #endregion




        #region Instance Fields

        private bool? _hasStoppedGracefully;

        private bool _isRunning;

        private ManualResetEvent _stopEvent;

        private bool _stopRequested;

        private Thread _thread;

        private Exception _threadException;

        private int _timeout;

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets whether the thread has gracefully stopped without exception.
        /// </summary>
        /// <value>
        ///     true if the thread stopped gracefully and had no exception, false if the thread was forcibly terminated (due to timeout) and/or had an exception, or null if the thread was not started or is still running.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         The value of <see cref="HasStoppedGracefully" /> is reset to null by <see cref="Start" /> and set by <see cref="Stop" />.
        ///     </para>
        /// </remarks>
        public bool? HasStoppedGracefully
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this._hasStoppedGracefully;
                }
            }
            private set
            {
                lock (this.SyncRoot)
                {
                    this._hasStoppedGracefully = value;
                }
            }
        }

        /// <summary>
        ///     Gets whether the thread is running.
        /// </summary>
        /// <value>
        ///     true if the thread is running, false otherwise.
        /// </value>
        public bool IsRunning
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this._isRunning;
                }
            }
            private set
            {
                lock (this.SyncRoot)
                {
                    this._isRunning = value;
                }
            }
        }

        /// <summary>
        ///     Gets the exception of the thread.
        /// </summary>
        /// <value>
        ///     The exception of the thread or null if no exception occurred, the thread was not started, or the thread is still running.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         The value of <see cref="ThreadException" /> is reset to null by <see cref="Start" /> and set during the execution of the thread if an exception occurs.
        ///     </para>
        ///     <para>
        ///         <see cref="ThreadException" /> is set for any unhandled exception which occurs in the thread (<see cref="OnBegin" />, <see cref="OnRun" />, <see cref="OnEnd" />), except for <see cref="ThreadAbortException" />s.
        ///     </para>
        /// </remarks>
        public Exception ThreadException
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this._threadException;
                }
            }
            private set
            {
                lock (this.SyncRoot)
                {
                    this._threadException = value;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the timeout of the thread in milliseconds used for start and stop.
        /// </summary>
        /// <value>
        ///     The timeout of the thread in milliseconds used for start and stop.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         This timeout is used during <see cref="Start" /> while waiting for <see cref="OnBegin" /> to return and during <see cref="Stop" /> while waiting for <see cref="OnStopping" /> to take effect (signaling <see cref="OnRun" /> to return).
        ///     </para>
        ///     <para>
        ///         The default value is <see cref="DefaultThreadTimeout" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="value" /> is less than zero. </exception>
        public int Timeout
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this._timeout;
                }
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                lock (this.SyncRoot)
                {
                    this._timeout = value;
                }
            }
        }

        /// <summary>
        ///     Gets the event which is signaled when the thread is requested to stop.
        /// </summary>
        /// <value>
        ///     The event which is signaled when the thread is requested to stop or null if the thread is not running.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         <see cref="StopEvent" /> is reset by <see cref="Start" /> and set by <see cref="Stop" />.
        ///     </para>
        /// </remarks>
        protected ManualResetEvent StopEvent
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this._stopEvent;
                }
            }
            private set
            {
                lock (this.SyncRoot)
                {
                    this._stopEvent = value;
                }
            }
        }

        /// <summary>
        ///     Gets whether the thread has been requested to stop.
        /// </summary>
        /// <value>
        ///     true if the thread has been requested to stop, false otherwise or if the thread is not running.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         The value of <see cref="StopRequested" /> is reset to false by <see cref="Start" /> and set to true by <see cref="Stop" />.
        ///     </para>
        /// </remarks>
        protected bool StopRequested
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this._stopRequested;
                }
            }
            private set
            {
                lock (this.SyncRoot)
                {
                    this._stopRequested = value;
                }
            }
        }

        /// <summary>
        ///     Gets the actual thread instance used to run the thread.
        /// </summary>
        /// <value>
        ///     The actual thread instance used to run the thread or null if the thread is not running.
        /// </value>
        protected Thread Thread
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this._thread;
                }
            }
            private set
            {
                lock (this.SyncRoot)
                {
                    this._thread = value;
                }
            }
        }

        private object StartStopSyncRoot { get; }

        private HashSet<TaskCompletionSource<object>> StopTasks { get; set; }

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Checks whether the thread had an exception and throws a <see cref="HeavyThreadException" /> if so.
        /// </summary>
        /// <exception cref="HeavyThreadException"> The thread had an exception. </exception>
        public void CheckForException ()
        {
            lock (this.SyncRoot)
            {
                if (this.ThreadException != null)
                {
                    throw new HeavyThreadException(this.ThreadException);
                }
            }
        }

        /// <summary>
        ///     Determines whether the caller of this function is executed inside the thread or not.
        /// </summary>
        /// <returns>
        ///     true if the caller of this function is executed inside this thread, false otherwise or if the thread is not running.
        /// </returns>
        public bool IsInThread ()
        {
            lock (this.SyncRoot)
            {
                if (this.Thread == null)
                {
                    return false;
                }

                return this.Thread.ManagedThreadId == Thread.CurrentThread.ManagedThreadId;
            }
        }

        /// <summary>
        ///     Starts the thread.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The start sequence goes as follows:
        ///     </para>
        ///     <list type="number">
        ///         <item>
        ///             <para>
        ///                 <see cref="Thread" /> is created and initialized.
        ///             </para>
        ///         </item>
        ///         <item>
        ///             <para>
        ///                 <see cref="OnStarting" /> is called.
        ///             </para>
        ///         </item>
        ///         <item>
        ///             <para>
        ///                 <see cref="Thread" /> actually starts executing.
        ///             </para>
        ///         </item>
        ///         <item>
        ///             <para>
        ///                 <see cref="Start" /> is waiting until <see cref="OnBegin" /> finishes inside <see cref="Thread" /> or the timeout specified by <see cref="Timeout" /> occurs.
        ///             </para>
        ///         </item>
        ///         <item>
        ///             <para>
        ///                 <see cref="OnStarted" /> is called.
        ///             </para>
        ///         </item>
        ///         <item>
        ///             <para>
        ///                 <see cref="Start" /> returns while <see cref="OnRun" /> is executed inside <see cref="Thread" />.
        ///             </para>
        ///         </item>
        ///     </list>
        ///     <note type="note">
        ///         If <see cref="OnRun" /> returns before <see cref="Stop" /> was called, the thread sleeps and <see cref="OnEnd" /> is not executed until <see cref="Stop" /> is called.
        ///     </note>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> The thread is already running. </exception>
        /// <exception cref="TimeoutException"> The thread failed to return from <see cref="OnBegin" /> within <see cref="Timeout" />. </exception>
        /// <exception cref="HeavyThreadException"> An exception occurred inside the thread during execution of <see cref="OnBegin" />. </exception>
        [SuppressMessage("ReSharper", "AccessToDisposedClosure"),]
        [SuppressMessage("ReSharper", "EmptyGeneralCatchClause"),]
        public void Start ()
        {
            lock (this.StartStopSyncRoot)
            {
                this.VerifyNotRunning();

                bool success = false;

                try
                {
                    GC.ReRegisterForFinalize(this);

                    using (ManualResetEvent startEvent = new ManualResetEvent(false))
                    {
                        int timeout;

                        lock (this.SyncRoot)
                        {
                            timeout = this.Timeout;

                            this.IsRunning = false;
                            this.HasStoppedGracefully = null;
                            this.ThreadException = null;

                            this.StopRequested = false;
                            this.StopEvent = new ManualResetEvent(false);
                            this.StopTasks = new HashSet<TaskCompletionSource<object>>();

                            this.Thread = new Thread(() =>
                            {
                                ManualResetEvent stopEvent = this.StopEvent;
                                bool onRunStarted = false;
                                bool onEndStarted = false;

                                try
                                {
                                    this.OnBegin();
                                    startEvent.Set();
                                    onRunStarted = true;
                                    this.OnRun();
                                    stopEvent.WaitOne();
                                    onEndStarted = true;
                                    this.OnEnd();
                                }
                                catch (ThreadAbortException)
                                {
                                    throw;
                                }
                                catch (Exception exception)
                                {
                                    try
                                    {
                                        this.ThreadException = exception;
                                    }
                                    catch { }

                                    try
                                    {
                                        this.OnException(exception, false);
                                    }
                                    catch { }

                                    try
                                    {
                                        if (!onRunStarted)
                                        {
                                            startEvent?.Set();
                                        }
                                    }
                                    catch { }

                                    try
                                    {
                                        if (!onEndStarted)
                                        {
                                            stopEvent?.WaitOne();
                                        }
                                    }
                                    catch { }

                                    try
                                    {
                                        if (!onEndStarted)
                                        {
                                            this.OnEnd();
                                        }
                                    }
                                    catch { }
                                }
                            });

                            Thread currentThread = Thread.CurrentThread;

                            this.Thread.IsBackground = true;
                            this.Thread.Priority = currentThread.Priority;
                            this.Thread.CurrentCulture = currentThread.CurrentCulture;
                            this.Thread.CurrentUICulture = currentThread.CurrentUICulture;
                            this.Thread.SetApartmentState(ApartmentState.STA);

                            this.OnStarting();

                            if (this.Thread.Name == null)
                            {
                                this.Thread.Name = this.GetType()
                                                       .Name;
                            }

                            this.Thread.Start();
                        }

                        bool started = startEvent.WaitOne(timeout);

                        lock (this.SyncRoot)
                        {
                            if (this.ThreadException != null)
                            {
                                throw new HeavyThreadException(this.GetType()
                                                                   .Name + " failed to start (exception occurred).", this.ThreadException);
                            }

                            if (!started)
                            {
                                throw new TimeoutException(this.GetType()
                                                               .Name + " failed to start (timeout of " + timeout.ToString("D", CultureInfo.InvariantCulture) + " ms while waiting for start event).");
                            }

                            this.IsRunning = true;

                            this.OnStarted(true);
                        }

                        this.OnStarted(false);
                    }

                    success = true;
                }
                finally
                {
                    if (!success)
                    {
                        this.Stop();
                    }
                }
            }
        }

        /// <summary>
        ///     Stops the thread and frees all resources.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The stop sequence goes as follows:
        ///     </para>
        ///     <list type="number">
        ///         <item>
        ///             <para>
        ///                 <see cref="OnStopping" /> is called.
        ///             </para>
        ///         </item>
        ///         <item>
        ///             <para>
        ///                 <see cref="StopRequested" /> is set to true.
        ///                 <see cref="StopEvent" /> is signaled.
        ///                 All stop tasks, added by <see cref="AddStopTask" />, are completed.
        ///             </para>
        ///         </item>
        ///         <item>
        ///             <para>
        ///                 <see cref="Stop" /> waits until <see cref="Thread" /> ended (<see cref="OnRun" /> and <see cref="OnEnd" /> finished executing inside <see cref="Thread" />) or the timeout specified by <see cref="Timeout" /> occurs.
        ///             </para>
        ///         </item>
        ///         <item>
        ///             <para>
        ///                 If <see cref="Thread" /> did not end on its own so a timeout occurred (see step above), the thread is terminated using <see cref="System.Threading.Thread.Abort()" />.
        ///             </para>
        ///         </item>
        ///         <item>
        ///             <para>
        ///                 <see cref="OnStopped" /> is called.
        ///             </para>
        ///         </item>
        ///         <item>
        ///             <para>
        ///                 <see cref="Stop" /> returns.
        ///             </para>
        ///         </item>
        ///     </list>
        ///     <note type="important">
        ///         The thread cannot be stopped from inside itself (e.g. you cannot call <see cref="Stop" /> from <see cref="OnBegin" />, <see cref="OnRun" />, or <see cref="OnEnd" />).
        ///         This means that the eventual stop has to be controlled by the owner of <see cref="HeavyThread" />.
        ///         This ensures symmetry of start/stop (only who can start <see cref="HeavyThread" /> can also stop it).
        ///     </note>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> This function was called from inside the thread. </exception>
        public void Stop ()
        {
            this.VerifyNotFromThread(nameof(this.Stop));
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        ///     Adds a <see cref="TaskCompletionSource{T}" /> to this thread which is completed when the thread is requested to stop.
        /// </summary>
        /// <param name="tcs"> The <see cref="TaskCompletionSource{T}" /> to add. </param>
        /// <remarks>
        ///     <para>
        ///         A task can be added multiple times but will only be completed once.
        ///     </para>
        ///     <para>
        ///         All added tasks will be completed using <see cref="TaskCompletionSource{T}.TrySetResult" /> by <see cref="Stop" />.
        ///         Therefore, no tasks added after <see cref="Stop" /> was called will be completed.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="tcs" /> is null. </exception>
        protected void AddStopTask (TaskCompletionSource<object> tcs)
        {
            if (tcs == null)
            {
                throw new ArgumentNullException(nameof(tcs));
            }

            this.StopTasks.Add(tcs);
        }

        /// <summary>
        ///     Removes a <see cref="TaskCompletionSource{T}" /> which was previously added using <see cref="AddStopTask" />.
        /// </summary>
        /// <param name="tcs"> The <see cref="TaskCompletionSource{T}" /> to remove. </param>
        /// <remarks>
        ///     <para>
        ///         Removing a task which was not previously added has no effect.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="tcs" /> is null. </exception>
        protected void RemoveStopTask (TaskCompletionSource<object> tcs)
        {
            if (tcs == null)
            {
                throw new ArgumentNullException(nameof(tcs));
            }

            this.StopTasks.Remove(tcs);
        }

        /// <summary>
        ///     Ensures that the caller of this function is not executed inside the thread.
        /// </summary>
        /// <param name="operation"> The name of the performed operation. </param>
        /// <exception cref="InvalidOperationException"> The caller of this function is executed inside this thread. </exception>
        protected void VerifyNotFromThread (string operation)
        {
            if (this.IsInThread())
            {
                throw new InvalidOperationException(operation + " cannot be called from inside the thread of " + this.GetType()
                                                                                                                     .Name + ".");
            }
        }

        /// <summary>
        ///     Throws an <see cref="InvalidOperationException" /> if the thread is running.
        /// </summary>
        /// <exception cref="InvalidOperationException"> The thread is running. </exception>
        protected void VerifyNotRunning ()
        {
            if (this.IsRunning)
            {
                throw new InvalidOperationException(this.GetType()
                                                        .Name + " is already running.");
            }
        }

        /// <summary>
        ///     Throws an <see cref="InvalidOperationException" /> if the thread is not running.
        /// </summary>
        /// <exception cref="InvalidOperationException"> The thread is not running. </exception>
        protected void VerifyRunning ()
        {
            if (!this.IsRunning)
            {
                throw new InvalidOperationException(this.GetType()
                                                        .Name + " is not running.");
            }
        }

        #endregion




        #region Virtuals

        /// <summary>
        ///     Stops the thread and frees all resources.
        /// </summary>
        /// <param name="disposing"> true if called from <see cref="Stop" /> or <see cref="IDisposable.Dispose" />, false if called from the destructor. </param>
        /// <remarks>
        ///     <para>
        ///         See <see cref="Stop" /> for a description of the sequence when stopping/disposing the thread.
        ///     </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> This function was called from the thread. </exception>
        [SuppressMessage("ReSharper", "EmptyGeneralCatchClause"),]
        protected virtual void Dispose (bool disposing)
        {
            lock (this.StartStopSyncRoot)
            {
                this.VerifyNotFromThread(nameof(this.Dispose));

                Thread thread;
                int timeout;

                lock (this.SyncRoot)
                {
                    if (this.IsRunning)
                    {
                        this.OnStopping();

                        this.StopRequested = true;
                        this.StopEvent.Set();

                        this.StopTasks.ForEach(x => x.TrySetResult(this));
                        this.StopTasks.Clear();
                    }

                    thread = this.Thread;
                    timeout = this.Timeout;
                }

                bool terminated;

                try
                {
                    terminated = thread?.Join(timeout) ?? true;
                }
                catch
                {
                    terminated = false;
                }

                if (!terminated)
                {
                    try
                    {
                        thread?.Abort();
                    }
                    catch { }
                }

                lock (this.SyncRoot)
                {
                    this.StopEvent?.Close();
                    this.StopEvent = null;

                    this.StopTasks?.Clear();
                    this.StopTasks = null;

                    this.StopRequested = false;

                    this.Thread = null;

                    this.HasStoppedGracefully = terminated && (this.ThreadException == null);
                    this.IsRunning = false;

                    this.OnStopped();
                }
            }
        }

        /// <summary>
        ///     Called when the thread begins execution.
        /// </summary>
        /// <remarks>
        ///     <note type="important">
        ///         This method is intended for on-thread preparation of the threads operation.
        ///         Do not execute the actual operation of the thread inside <see cref="OnBegin" />.
        ///         See <see cref="Start" /> and <see cref="Stop" /> for more details.
        ///     </note>
        ///     <note type="note">
        ///         This method is called from the thread.
        ///     </note>
        /// </remarks>
        protected virtual void OnBegin () { }

        /// <summary>
        ///     Called when the thread ends execution.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This method is intended for on-thread cleanup of the threads operation.
        ///         The thread ends as soon as <see cref="OnEnd" /> returns.
        ///         See <see cref="Start" /> and <see cref="Stop" /> for more details.
        ///     </para>
        ///     <note type="note">
        ///         This method is called from the thread.
        ///     </note>
        ///     <note type="important">
        ///         Under very rare circumstances, this method is called twice.
        ///         Therefore, overrides of this method must be designed accordingly.
        ///     </note>
        /// </remarks>
        protected virtual void OnEnd () { }

        /// <summary>
        ///     Called when an exception occurred inside the thread.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        /// <param name="canContinue"> Indicates whether the thread is able to continue or not after the exception was handled by <see cref="OnException" />. </param>
        /// <remarks>
        ///     <note type="note">
        ///         This method is called from the thread.
        ///     </note>
        ///     <para>
        ///         <paramref name="canContinue" /> is only true if you call <see cref="OnException" /> yourself with <paramref name="canContinue" /> set to true.
        ///         It is false for any unhandled exception which is thrown inside the thread.
        ///     </para>
        /// </remarks>
        protected virtual void OnException (Exception exception, bool canContinue) { }

        /// <summary>
        ///     Called when the thread is running and supposed to perform its operations.
        /// </summary>
        /// <remarks>
        ///     <note type="note">
        ///         This method is called from the thread.
        ///     </note>
        /// </remarks>
        protected virtual void OnRun () { }

        /// <summary>
        ///     Called after the thread was started.
        /// </summary>
        /// <param name="withLock"> Indicates whether the method is called inside a lock to <see cref="SyncRoot" />. </param>
        /// <remarks>
        ///     <para>
        ///         After the thread was started, this method is called twice:
        ///         Once inside a lock to <see cref="SyncRoot" /> (<paramref name="withLock" /> is true) and then once outside (<paramref name="withLock" /> is false).
        ///     </para>
        ///     <note type="note">
        ///         This method is called by <see cref="Start" />.
        ///     </note>
        ///     <note type="important">
        ///         This method is called inside a lock to <see cref="SyncRoot" />.
        ///     </note>
        /// </remarks>
        protected virtual void OnStarted (bool withLock) { }

        /// <summary>
        ///     Called before the thread is started.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This method is intended for configuring the thread (priority, culture, etc.) before the actual thread is started.
        ///     </para>
        ///     <note type="note">
        ///         This method is called by <see cref="Start" />.
        ///     </note>
        ///     <note type="important">
        ///         This method is called inside a lock to <see cref="SyncRoot" />.
        ///     </note>
        /// </remarks>
        protected virtual void OnStarting () { }

        /// <summary>
        ///     Called after the thread has been stopped and its resources considered freed.
        /// </summary>
        /// <remarks>
        ///     <note type="note">
        ///         This method is called by <see cref="Stop" />.
        ///     </note>
        ///     <note type="important">
        ///         This method is called inside a lock to <see cref="SyncRoot" />.
        ///     </note>
        /// </remarks>
        protected virtual void OnStopped () { }

        /// <summary>
        ///     Called to stop the thread.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This method is intended to signalize the thread to cease its operation and return from <see cref="OnRun" />.
        ///     </para>
        ///     <note type="note">
        ///         This method is called by <see cref="Stop" />.
        ///     </note>
        ///     <note type="important">
        ///         This method is called inside a lock to <see cref="SyncRoot" />.
        ///     </note>
        ///     <note type="important">
        ///         If the thread does not end on its own (that is: return from <see cref="OnRun" />) after <see cref="OnStopping" /> was called, plus the time specified by <see cref="Timeout" />, the thread is terminated.
        ///     </note>
        /// </remarks>
        protected virtual void OnStopping () { }

        #endregion




        #region Interface: IDisposable

        /// <inheritdoc />
        void IDisposable.Dispose ()
        {
            this.Stop();
        }

        #endregion




        #region Interface: ISynchronizable

        /// <inheritdoc />
        bool ISynchronizable.IsSynchronized => true;

        /// <inheritdoc />
        public object SyncRoot { get; }

        #endregion
    }
}
