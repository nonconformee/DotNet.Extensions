using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;




namespace RI.Utilities.Collections.Concurrent
{
    /// <summary>
    ///     Implements a request/response queue.
    /// </summary>
    /// <typeparam name="TRequest"> The type of the requests. </typeparam>
    /// <typeparam name="TResponse"> The type of the responses. </typeparam>
    /// <remarks>
    ///     <para>
    ///         A request/response queue is a thread-safe, asynchronous, bidirectional producer/consumer queue.
    ///     </para>
    ///     <para>
    ///         Operations work like this:
    ///         A request is put into the queue by any thread and the response is then awaited.
    ///         A consumer on any thread takes requests out of the queue, processes it, and provides the response, causing the request to continue.
    ///     </para>
    ///     <para>
    ///         See <see cref="RequestResponseCollection{TRequest,TResponse,TItem}" /> and <see cref="RequestResponseItem{TRequest,TResponse}" /> for more details.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="true" instance="true" />
    public sealed class RequestResponseQueue <TRequest, TResponse> : RequestResponseCollection<TRequest, TResponse, RequestResponseItem<TRequest, TResponse>>
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="RequestResponseQueue{TRequest, TResponse}" />.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <see cref="TaskCreationOptions.RunContinuationsAsynchronously" /> is used as continuation creation options.
        ///     </para>
        ///     <para>
        ///         The current task scheduler is used for executing continuations.
        ///     </para>
        /// </remarks>
        public RequestResponseQueue () { }

        /// <summary>
        ///     Creates a new instance of <see cref="RequestResponseQueue{TRequest, TResponse}" />.
        /// </summary>
        /// <param name="completionCreationOptions"> The options which are used for creating continuations. </param>
        /// <remarks>
        ///     <para>
        ///         The current task scheduler is used for executing continuations.
        ///     </para>
        /// </remarks>
        public RequestResponseQueue (TaskCreationOptions completionCreationOptions)
            : base(completionCreationOptions) { }

        /// <summary>
        ///     Creates a new instance of <see cref="RequestResponseQueue{TRequest, TResponse}" />.
        /// </summary>
        /// <param name="completionScheduler"> The task scheduler which is used for executing continuations. Can be null to use the current task scheduler. </param>
        /// <remarks>
        ///     <para>
        ///         <see cref="TaskCreationOptions.RunContinuationsAsynchronously" /> is used as continuation creation options.
        ///     </para>
        /// </remarks>
        public RequestResponseQueue (TaskScheduler completionScheduler)
            : base(completionScheduler) { }

        /// <summary>
        ///     Creates a new instance of <see cref="RequestResponseQueue{TRequest, TResponse}" />.
        /// </summary>
        /// <param name="completionCreationOptions"> The options which are used for creating continuations. </param>
        /// <param name="completionScheduler"> The task scheduler which is used for executing continuations. Can be null to use the current task scheduler. </param>
        public RequestResponseQueue (TaskCreationOptions completionCreationOptions, TaskScheduler completionScheduler)
            : base(completionCreationOptions, completionScheduler) { }

        #endregion




        #region Instance Properties/Indexer

        private List<TaskCompletionSource<RequestResponseItem<TRequest, TResponse>>> Consumers { get; set; }

        private List<RequestResponseItem<TRequest, TResponse>> Requests { get; set; }

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Awaits the next request and returns it for processing.
        /// </summary>
        /// <returns>
        ///     The task used to await the next request.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The returned request for processing is wrapped in a <see cref="RequestResponseItem{TRequest,TResponse}" />.
        ///         To finish the processing and issue the response, <see cref="RequestResponseItem{TRequest,TResponse}.Respond(TResponse)" /> must be called.
        ///     </para>
        /// </remarks>
        public Task<RequestResponseItem<TRequest, TResponse>> DequeueAsync ()
        {
            return this.DequeueAsync(Timeout.InfiniteTimeSpan, CancellationToken.None);
        }

        /// <summary>
        ///     Awaits the next request and returns it for processing.
        /// </summary>
        /// <param name="timeout"> The timeout used to wait for the next request. </param>
        /// <returns>
        ///     The task used to await the next request.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The returned request for processing is wrapped in a <see cref="RequestResponseItem{TRequest,TResponse}" />.
        ///         To finish the processing and issue the response, <see cref="RequestResponseItem{TRequest,TResponse}.Respond(TResponse)" /> must be called.
        ///     </para>
        /// </remarks>
        /// <exception cref="TimeoutException"> <paramref name="timeout" /> was reached without a response. </exception>
        public Task<RequestResponseItem<TRequest, TResponse>> DequeueAsync (TimeSpan timeout)
        {
            return this.DequeueAsync(timeout, CancellationToken.None);
        }

        /// <summary>
        ///     Awaits the next request and returns it for processing.
        /// </summary>
        /// <param name="ct"> The cancellation token which can be used to cancel the request awaiting. </param>
        /// <returns>
        ///     The task used to await the next request.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The returned request for processing is wrapped in a <see cref="RequestResponseItem{TRequest,TResponse}" />.
        ///         To finish the processing and issue the response, <see cref="RequestResponseItem{TRequest,TResponse}.Respond(TResponse)" /> must be called.
        ///     </para>
        /// </remarks>
        /// <exception cref="OperationCanceledException"> <paramref name="ct" /> was triggered. </exception>
        public Task<RequestResponseItem<TRequest, TResponse>> DequeueAsync (CancellationToken ct)
        {
            return this.DequeueAsync(Timeout.InfiniteTimeSpan, ct);
        }

        /// <summary>
        ///     Awaits the next request and returns it for processing.
        /// </summary>
        /// <param name="timeout"> The timeout used to wait for the next request. </param>
        /// <param name="ct"> The cancellation token which can be used to cancel the request awaiting. </param>
        /// <returns>
        ///     The task used to await the next request.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The returned request for processing is wrapped in a <see cref="RequestResponseItem{TRequest,TResponse}" />.
        ///         To finish the processing and issue the response, <see cref="RequestResponseItem{TRequest,TResponse}.Respond(TResponse)" /> must be called.
        ///     </para>
        /// </remarks>
        /// <exception cref="TimeoutException"> <paramref name="timeout" /> was reached without a response. </exception>
        /// <exception cref="OperationCanceledException"> <paramref name="ct" /> was triggered. </exception>
        public Task<RequestResponseItem<TRequest, TResponse>> DequeueAsync (TimeSpan timeout, CancellationToken ct)
        {
            return this.TakeAsync(timeout, ct);
        }

        /// <summary>
        ///     Enqueues a new request for processing and awaits the response.
        /// </summary>
        /// <param name="request"> The request. </param>
        /// <returns>
        ///     The task used to await the response.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="request" /> is null. </exception>
        public Task<TResponse> EnqueueAsync (TRequest request)
        {
            return this.EnqueueAsync(request, Timeout.InfiniteTimeSpan, CancellationToken.None);
        }

        /// <summary>
        ///     Enqueues a new request for processing and awaits the response.
        /// </summary>
        /// <param name="request"> The request. </param>
        /// <param name="timeout"> The timeout used to wait for the response. </param>
        /// <returns>
        ///     The task used to await the response.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="request" /> is null. </exception>
        /// <exception cref="TimeoutException"> <paramref name="timeout" /> was reached without a response. </exception>
        public Task<TResponse> EnqueueAsync (TRequest request, TimeSpan timeout)
        {
            return this.EnqueueAsync(request, timeout, CancellationToken.None);
        }

        /// <summary>
        ///     Enqueues a new request for processing and awaits the response.
        /// </summary>
        /// <param name="request"> The request. </param>
        /// <param name="ct"> The cancellation token which can be used to cancel the request processing. </param>
        /// <returns>
        ///     The task used to await the response.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="request" /> is null. </exception>
        /// <exception cref="OperationCanceledException"> <paramref name="ct" /> was triggered. </exception>
        public Task<TResponse> EnqueueAsync (TRequest request, CancellationToken ct)
        {
            return this.EnqueueAsync(request, Timeout.InfiniteTimeSpan, ct);
        }

        /// <summary>
        ///     Enqueues a new request for processing and awaits the response.
        /// </summary>
        /// <param name="request"> The request. </param>
        /// <param name="timeout"> The timeout used to wait for the response. </param>
        /// <param name="ct"> The cancellation token which can be used to cancel the request processing. </param>
        /// <returns>
        ///     The task used to await the response.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="request" /> is null. </exception>
        /// <exception cref="TimeoutException"> <paramref name="timeout" /> was reached without a response. </exception>
        /// <exception cref="OperationCanceledException"> <paramref name="ct" /> was triggered. </exception>
        public Task<TResponse> EnqueueAsync (TRequest request, TimeSpan timeout, CancellationToken ct)
        {
            return this.PutAsync(request, timeout, ct);
        }

        #endregion




        #region Overrides

        /// <inheritdoc />
        protected override int ConsumerCount => this.Consumers.Count;

        /// <inheritdoc />
        protected override int RequestCount => this.Requests.Count;

        /// <inheritdoc />
        protected override void AddConsumer (TaskCompletionSource<RequestResponseItem<TRequest, TResponse>> consumer)
        {
            this.Consumers.Add(consumer);
        }

        /// <inheritdoc />
        protected override void AddRequest (RequestResponseItem<TRequest, TResponse> request)
        {
            this.Requests.Add(request);
        }

        /// <inheritdoc />
        protected override void ClearConsumers ()
        {
            this.Consumers.Clear();
        }

        /// <inheritdoc />
        protected override void ClearRequests ()
        {
            this.Requests.Clear();
        }

        /// <inheritdoc />
        protected override TaskCompletionSource<RequestResponseItem<TRequest, TResponse>> GetAndRemoveNextConsumer ()
        {
            TaskCompletionSource<RequestResponseItem<TRequest, TResponse>> consumer = null;

            if (this.Consumers.Count > 0)
            {
                consumer = this.Consumers[0];
                this.Consumers.RemoveAt(0);
            }

            return consumer;
        }

        /// <inheritdoc />
        protected override RequestResponseItem<TRequest, TResponse> GetAndRemoveNextRequest ()
        {
            RequestResponseItem<TRequest, TResponse> request = null;

            if (this.Requests.Count > 0)
            {
                request = this.Requests[0];
                this.Requests.RemoveAt(0);
            }

            return request;
        }

        /// <inheritdoc />
        protected override IEnumerable<TaskCompletionSource<RequestResponseItem<TRequest, TResponse>>> GetConsumers ()
        {
            return this.Consumers;
        }

        /// <inheritdoc />
        protected override IEnumerable<RequestResponseItem<TRequest, TResponse>> GetRequests ()
        {
            return this.Requests;
        }

        /// <inheritdoc />
        protected override void Initialize ()
        {
            this.Requests = new List<RequestResponseItem<TRequest, TResponse>>();
            this.Consumers = new List<TaskCompletionSource<RequestResponseItem<TRequest, TResponse>>>();
        }

        /// <inheritdoc />
        protected override void RemoveConsumer (TaskCompletionSource<RequestResponseItem<TRequest, TResponse>> consumer)
        {
            this.Consumers.Remove(consumer);
        }

        /// <inheritdoc />
        protected override void RemoveRequest (RequestResponseItem<TRequest, TResponse> request)
        {
            this.Requests.Remove(request);
        }

        #endregion
    }
}
