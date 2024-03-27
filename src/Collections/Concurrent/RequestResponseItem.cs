using System;
using System.Threading;
using System.Threading.Tasks;

using RI.Utilities.ObjectModel;




namespace RI.Utilities.Collections.Concurrent
{
    /// <summary>
    ///     Implements a wrapper around requests which are managed by request/response collections (<see cref="RequestResponseCollection{TRequest,TResponse,TItem}" /> types and derivatives).
    /// </summary>
    /// <typeparam name="TRequest"> The type of the request. </typeparam>
    /// <typeparam name="TResponse"> The type of the response. </typeparam>
    /// <remarks>
    ///     <para>
    ///         Besides wrapping the request to process (<see cref="Request" />), <see cref="RequestResponseItem{TRequest,TResponse}" /> also provides methods to control and release the request processing, such as <see cref="Respond(TResponse)" /> or <see cref="Cancel" />.
    ///     </para>
    ///     <para>
    ///         See <see cref="RequestResponseCollection{TRequest,TResponse,TItem}" /> for more details.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="true" instance="true" />
    public class RequestResponseItem <TRequest, TResponse> : ISynchronizable, IDisposable
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="RequestResponseItem{TRequest,TResponse}" />.
        /// </summary>
        public RequestResponseItem ()
        {
            this.SyncRoot = new object();

            this.IsInitialized = false;

            this.CompletionCreationOptions = TaskCreationOptions.None;
            this.StillNeeded = true;
            this.IsFinished = false;
            this.Request = default;
            this.Response = default;
        }

        /// <summary>
        ///     Garbage collects this instance of <see cref="RequestResponseItem{TRequest,TResponse}" />.
        /// </summary>
        ~RequestResponseItem ()
        {
            ((IDisposable)this).Dispose();
        }

        #endregion




        #region Instance Fields

        private TaskCreationOptions _completionCreationOptions;

        private bool _isFinished;

        private bool _isInitialized;

        private TRequest _request;

        private TResponse _response;

        private bool _stillNeeded;

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the cancellation token which is triggered if a request/response is no longer needed (see <see cref="StillNeeded" />).
        /// </summary>
        /// <value>
        ///     The cancellation token which is triggered if a request/response is no longer needed (see <see cref="StillNeeded" />).
        /// </value>
        /// <exception cref="InvalidOperationException"> The item is not initialized. </exception>
        public CancellationToken CancellationToken
        {
            get
            {
                lock (this.SyncRoot)
                {
                    this.VerifyInitialized();
                    return this.CancellationTokenSource?.Token ?? new CancellationToken(true);
                }
            }
        }

        /// <summary>
        ///     Gets the options which are used for creating continuations.
        /// </summary>
        /// <value>
        ///     T
        ///     The options which are used for creating continuations.
        /// </value>
        /// <exception cref="InvalidOperationException"> The item is not initialized. </exception>
        public TaskCreationOptions CompletionCreationOptions
        {
            get
            {
                lock (this.SyncRoot)
                {
                    this.VerifyInitialized();
                    return this._completionCreationOptions;
                }
            }
            private set
            {
                lock (this.SyncRoot)
                {
                    this._completionCreationOptions = value;
                }
            }
        }

        /// <summary>
        ///     Gets whether the request is finished.
        /// </summary>
        /// <value>
        ///     true if the request is finished, false otherwise.
        /// </value>
        /// <exception cref="InvalidOperationException"> The item is not initialized. </exception>
        public bool IsFinished
        {
            get
            {
                lock (this.SyncRoot)
                {
                    this.VerifyInitialized();
                    return this._isFinished;
                }
            }
            private set
            {
                lock (this.SyncRoot)
                {
                    this._isFinished = value;
                }
            }
        }

        /// <summary>
        ///     Gets whether the item is initialized.
        /// </summary>
        /// <value>
        ///     true if the item is initialized, false otherwise.
        /// </value>
        public bool IsInitialized
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this._isInitialized;
                }
            }
            private set
            {
                lock (this.SyncRoot)
                {
                    this._isInitialized = value;
                }
            }
        }

        /// <summary>
        ///     Gets the request to be processed.
        /// </summary>
        /// <value>
        ///     The request to be processed.
        /// </value>
        /// <exception cref="InvalidOperationException"> The item is not initialized. </exception>
        public TRequest Request
        {
            get
            {
                lock (this.SyncRoot)
                {
                    this.VerifyInitialized();
                    return this._request;
                }
            }
            private set
            {
                lock (this.SyncRoot)
                {
                    this._request = value;
                }
            }
        }

        /// <summary>
        ///     Gets the issued response.
        /// </summary>
        /// <value>
        ///     The issued response, if any.
        /// </value>
        /// <exception cref="InvalidOperationException"> The item is not initialized. </exception>
        public TResponse Response
        {
            get
            {
                lock (this.SyncRoot)
                {
                    this.VerifyInitialized();
                    return this._response;
                }
            }
            private set
            {
                lock (this.SyncRoot)
                {
                    this._response = value;
                }
            }
        }

        /// <summary>
        ///     Gets whether the request is still required to be processed and a response is still being awaited.
        /// </summary>
        /// <value>
        ///     true if the request is still required to be processed and a response is still being awaited, false otherwise.
        /// </value>
        /// <exception cref="InvalidOperationException"> The item is not initialized. </exception>
        public bool StillNeeded
        {
            get
            {
                lock (this.SyncRoot)
                {
                    this.VerifyInitialized();
                    return this._stillNeeded;
                }
            }
            private set
            {
                lock (this.SyncRoot)
                {
                    this._stillNeeded = value;
                }
            }
        }

        internal Task<TResponse> ResponseTask
        {
            get
            {
                lock (this.SyncRoot)
                {
                    this.VerifyInitialized();
                    return this.ResponseCompletion.Task;
                }
            }
        }

        private CancellationTokenSource CancellationTokenSource { get; set; }

        private TaskCompletionSource<TResponse> ResponseCompletion { get; set; }

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Aborts the request with an exception.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="exception" /> is null. </exception>
        /// <exception cref="InvalidOperationException"> The item is not initialized or was already finished. </exception>
        public void Abort (Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            lock (this.SyncRoot)
            {
                this.VerifyInitialized();
                this.VerifyNotFinished();

                this.OnAbort(exception);

                this.IsFinished = true;
                this.StillNeeded = false;
                this.Response = default;
                this.ResponseCompletion.TrySetException(exception);
                this.CancellationTokenSource.Cancel();
            }
        }

        /// <summary>
        ///     Cancels the request.
        /// </summary>
        /// <exception cref="InvalidOperationException"> The item is not initialized or was already finished. </exception>
        public void Cancel ()
        {
            lock (this.SyncRoot)
            {
                this.VerifyInitialized();
                this.VerifyNotFinished();

                this.OnCancel();

                this.IsFinished = true;
                this.StillNeeded = false;
                this.Response = default;
                this.ResponseCompletion.TrySetCanceled();
                this.CancellationTokenSource.Cancel();
            }
        }

        /// <summary>
        ///     Finishes the response without a response.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This simply issues a response using the default value of <typeparamref name="TResponse" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> The item is not initialized or was already finished. </exception>
        public void Respond ()
        {
            lock (this.SyncRoot)
            {
                this.VerifyInitialized();
                this.VerifyNotFinished();

                this.OnRespond();

                this.IsFinished = true;
                this.StillNeeded = false;
                this.Response = default;
                this.ResponseCompletion.TrySetResult(this.Response);
                this.CancellationTokenSource.Cancel();
            }
        }

        /// <summary>
        ///     Finishes the response with a response.
        /// </summary>
        /// <param name="response"> The response. </param>
        /// <exception cref="InvalidOperationException"> The item is not initialized or was already finished. </exception>
        public void Respond (TResponse response)
        {
            lock (this.SyncRoot)
            {
                this.VerifyInitialized();
                this.VerifyNotFinished();

                this.OnRespond(response);

                this.IsFinished = true;
                this.StillNeeded = false;
                this.Response = response;
                this.ResponseCompletion.TrySetResult(response);
                this.CancellationTokenSource.Cancel();
            }
        }

        internal void Initialize (TaskCreationOptions completionCreationOptions, TRequest request)
        {
            lock (this.SyncRoot)
            {
                if (this.IsInitialized)
                {
                    throw new InvalidOperationException(this.GetType()
                                                            .Name + " is already initialized.");
                }

                this.IsInitialized = true;

                this.CompletionCreationOptions = completionCreationOptions;
                this.Request = request;

                this.ResponseCompletion = new TaskCompletionSource<TResponse>(request, this.CompletionCreationOptions);
                this.CancellationTokenSource = new CancellationTokenSource();

                this.OnInitialize(completionCreationOptions, request);
            }
        }

        internal void NoLongerNeeded ()
        {
            lock (this.SyncRoot)
            {
                if (this.IsFinished)
                {
                    return;
                }

                this.OnNoLongerNeeded();

                this.IsFinished = true;
                this.StillNeeded = false;
                this.Response = default;
                this.CancellationTokenSource?.Cancel();
            }
        }

        /// <summary>
        ///     Verifies that the item is initialized and throws a <see cref="InvalidOperationException" /> if not.
        /// </summary>
        /// <exception cref="InvalidOperationException"> The item is not initialized. </exception>
        protected void VerifyInitialized ()
        {
            if (!this.IsInitialized)
            {
                throw new InvalidOperationException(this.GetType()
                                                        .Name + " is not initialized.");
            }
        }

        /// <summary>
        ///     Verifies that the item is not finished and throws a <see cref="InvalidOperationException" /> otherwise.
        /// </summary>
        /// <exception cref="InvalidOperationException"> The item is finished. </exception>
        protected void VerifyNotFinished ()
        {
            if (this.IsFinished)
            {
                throw new InvalidOperationException(this.GetType()
                                                        .Name + " is already finished.");
            }
        }

        #endregion




        #region Virtuals

        /// <summary>
        ///     Called when the request is being aborted.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        protected virtual void OnAbort (Exception exception) { }

        /// <summary>
        ///     Called when the request is being canceled.
        /// </summary>
        protected virtual void OnCancel () { }

        /// <summary>
        ///     Called when the item is initialized.
        /// </summary>
        /// <param name="completionCreationOptions"> The completion creation options. </param>
        /// <param name="request"> The request associated with this item. </param>
        protected virtual void OnInitialize (TaskCreationOptions completionCreationOptions, TRequest request) { }

        /// <summary>
        ///     Called when the request/response is no longer needed (see <see cref="StillNeeded" />).
        /// </summary>
        protected virtual void OnNoLongerNeeded () { }

        /// <summary>
        ///     Called when the request is finished with a response.
        /// </summary>
        /// <param name="response"> The response. </param>
        protected virtual void OnRespond (TResponse response) { }

        /// <summary>
        ///     Called when the request is finished without a response.
        /// </summary>
        protected virtual void OnRespond () { }

        #endregion




        #region Interface: IDisposable

        /// <inheritdoc />
        void IDisposable.Dispose ()
        {
            if (this.IsInitialized && !this.IsFinished)
            {
                this.Respond();
            }

            this.CancellationTokenSource?.Dispose();
            this.CancellationTokenSource = null;
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
