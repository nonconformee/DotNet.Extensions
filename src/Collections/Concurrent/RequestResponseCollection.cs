using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using RI.Utilities.ObjectModel;




namespace RI.Utilities.Collections.Concurrent
{
    /// <summary>
    ///     Implements a base class for request/response collections.
    /// </summary>
    /// <typeparam name="TRequest"> The type of the requests. </typeparam>
    /// <typeparam name="TResponse"> The type of the responses. </typeparam>
    /// <typeparam name="TItem"> The type used to wrap the requests when presenting them to consumers. </typeparam>
    /// <remarks>
    ///     <para>
    ///         A request/response collection is a thread-safe, asynchronous, bidirectional producer/consumer collection.
    ///     </para>
    ///     <para>
    ///         On one side is one or more producers which can, from any thread, add requests to the collection.
    ///         After a request is added, a response is awaited.
    ///     </para>
    ///     <para>
    ///         On the other side is one or more consumers which can, from any thread, take requests from the collection.
    ///         A consumer then processes a request and issues the response, causing the awaiting request to continue with that response.
    ///     </para>
    ///     <para>
    ///         Therefore, requests and responses can be two different types (<typeparamref name="TRequest" /> and <typeparamref name="TResponse" />) to allow true bidirectional data flow from the producer to the consumer and back.
    ///     </para>
    ///     <para>
    ///         A consumer can only process one request at a time.
    ///         And one request is processed by exactly one consumer.
    ///     </para>
    ///     <para>
    ///         If there are consumers waiting for requests, a new request is issued to the consumer already waiting the longest.
    ///         If there are no consumers waiting for requests, a new request will be stored and issued to the next consumer which wants to take requests for processing.
    ///     </para>
    ///     <para>
    ///         Requests are presented to consumers using types deriving from <see cref="RequestResponseItem{TRequest,TResponse}" /> (<typeparamref name="TItem" />).
    ///         <see cref="RequestResponseItem{TRequest,TResponse}" /> wraps the request (available through the <see cref="RequestResponseItem{TRequest,TResponse}.Request" /> property) and provides methods to finish the request processing and issue a response (such as <see cref="RequestResponseItem{TRequest,TResponse}.Respond(TResponse)" />).
    ///     </para>
    /// </remarks>
    /// <threadsafety static="true" instance="true" />
    public abstract class RequestResponseCollection <TRequest, TResponse, TItem> : ISynchronizable
        where TItem : RequestResponseItem<TRequest, TResponse>, new()
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="RequestResponseCollection{TRequest,TResponse,TItem}" />.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <see cref="TaskCreationOptions.RunContinuationsAsynchronously" /> is used as continuation creation options.
        ///     </para>
        ///     <para>
        ///         The current task scheduler is used for executing continuations.
        ///     </para>
        /// </remarks>
        protected RequestResponseCollection ()
            : this(TaskCreationOptions.RunContinuationsAsynchronously, null) { }

        /// <summary>
        ///     Creates a new instance of <see cref="RequestResponseCollection{TRequest, TResponse, TItem}" />.
        /// </summary>
        /// <param name="completionCreationOptions"> The options which are used for creating continuations. </param>
        /// <remarks>
        ///     <para>
        ///         The current task scheduler is used for executing continuations.
        ///     </para>
        /// </remarks>
        protected RequestResponseCollection (TaskCreationOptions completionCreationOptions)
            : this(completionCreationOptions, null) { }

        /// <summary>
        ///     Creates a new instance of <see cref="RequestResponseCollection{TRequest, TResponse, TItem}" />.
        /// </summary>
        /// <param name="completionScheduler"> The task scheduler which is used for executing continuations. Can be null to use the current task scheduler. </param>
        /// <remarks>
        ///     <para>
        ///         <see cref="TaskCreationOptions.RunContinuationsAsynchronously" /> is used as continuation creation options.
        ///     </para>
        /// </remarks>
        protected RequestResponseCollection (TaskScheduler completionScheduler)
            : this(TaskCreationOptions.RunContinuationsAsynchronously, completionScheduler) { }

        /// <summary>
        ///     Creates a new instance of <see cref="RequestResponseCollection{TRequest, TResponse, TItem}" />.
        /// </summary>
        /// <param name="completionCreationOptions"> The options which are used for creating continuations. </param>
        /// <param name="completionScheduler"> The task scheduler which is used for executing continuations. Can be null to use the current task scheduler. </param>
        protected RequestResponseCollection (TaskCreationOptions completionCreationOptions, TaskScheduler completionScheduler)
        {
            this.SyncRoot = new object();

            this.CompletionCreationOptions = completionCreationOptions;
            this.CompletionScheduler = completionScheduler;

            this.Initialize();
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the options which are used for creating continuations.
        /// </summary>
        /// <value>
        ///     The options which are used for creating continuations.
        /// </value>
        public TaskCreationOptions CompletionCreationOptions { get; }

        /// <summary>
        ///     Gets the task scheduler which is used for executing continuations.
        /// </summary>
        /// <value>
        ///     The task scheduler which is used for executing continuations.
        /// </value>
        public TaskScheduler CompletionScheduler { get; }

        /// <summary>
        ///     Gets the number of currently waiting consumers.
        /// </summary>
        /// <value>
        ///     The number of currently waiting consumers.
        /// </value>
        public int WaitingConsumers
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this.ConsumerCount;
                }
            }
        }

        /// <summary>
        ///     Gets the number of currently waiting requests.
        /// </summary>
        /// <value>
        ///     The number of currently waiting requests.
        /// </value>
        public int WaitingRequests
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this.RequestCount;
                }
            }
        }

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Aborts all currently waiting requests with an exception.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        /// <returns>
        ///     The number of aborted requests.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="exception" /> is null. </exception>
        public int AbortRequests (Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            lock (this.SyncRoot)
            {
                int requests = this.RequestCount;

                this.GetRequests()
                    .ForEach(x => x.Abort(exception));

                this.ClearRequests();
                return requests;
            }
        }

        /// <summary>
        ///     Cancels all currently waiting requests.
        /// </summary>
        /// <returns>
        ///     The number of canceled requests.
        /// </returns>
        public int CancelRequests ()
        {
            lock (this.SyncRoot)
            {
                int requests = this.RequestCount;

                this.GetRequests()
                    .ForEach(x => x.Cancel());

                this.ClearRequests();
                return requests;
            }
        }

        /// <summary>
        ///     Dismisses all currently waiting consumers.
        /// </summary>
        /// <returns>
        ///     The number of dismissed consumers.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The tasks of all waiting consumers will be canceled.
        ///     </para>
        /// </remarks>
        public int DismissConsumers ()
        {
            lock (this.SyncRoot)
            {
                int consumers = this.ConsumerCount;

                this.GetConsumers()
                    .ForEach(x => x.TrySetCanceled());

                this.ClearConsumers();
                return consumers;
            }
        }

        /// <summary>
        ///     Issues the same response to all currently waiting requests and finishes them.
        /// </summary>
        /// <param name="response"> The response. </param>
        /// <returns>
        ///     The number of finished requests.
        /// </returns>
        public int RespondRequests (TResponse response)
        {
            lock (this.SyncRoot)
            {
                int requests = this.RequestCount;

                this.GetRequests()
                    .ForEach(x => x.Respond(response));

                this.ClearRequests();
                return requests;
            }
        }

        /// <summary>
        ///     Finishes all currently waiting requests with no response.
        /// </summary>
        /// <returns>
        ///     The number of finished requests.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This simply issues a response using the default value of <typeparamref name="TResponse" />.
        ///     </para>
        /// </remarks>
        public int RespondRequests ()
        {
            lock (this.SyncRoot)
            {
                int requests = this.RequestCount;

                this.GetRequests()
                    .ForEach(x => x.Respond());

                this.ClearRequests();
                return requests;
            }
        }

        /// <summary>
        ///     Puts a new request into the collection or issues it to an already waiting consumer.
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
        protected Task<TResponse> PutAsync (TRequest request, TimeSpan timeout, CancellationToken ct)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            TItem item;
            Task<TResponse> responseTask;

            lock (this.SyncRoot)
            {
                item = new TItem();
                item.Initialize(this.CompletionCreationOptions, request);

                responseTask = item.ResponseTask;

                TaskCompletionSource<TItem> consumer = this.GetAndRemoveNextConsumer();

                while (consumer != null)
                {
                    if (consumer.TrySetResult(item))
                    {
                        break;
                    }

                    consumer = this.GetAndRemoveNextConsumer();
                }

                if (consumer == null)
                {
                    this.AddRequest(item);
                }
            }

            Task timeoutTask = Task.Delay(timeout);
            Task cancelTask = Task.Delay(Timeout.InfiniteTimeSpan, ct);

            Task<Task> result = Task.WhenAny(responseTask, timeoutTask, cancelTask);

            Task<TResponse> waitTask = result.ContinueWith(task =>
            {
                lock (this.SyncRoot)
                {
                    this.RemoveRequest(item);

                    if (responseTask.IsCompleted)
                    {
                        return responseTask.Result;
                    }

                    if (ReferenceEquals(result.Result, timeoutTask))
                    {
                        item.NoLongerNeeded();
                        throw new TimeoutException();
                    }

                    if (ReferenceEquals(result.Result, cancelTask))
                    {
                        item.NoLongerNeeded();
                        throw new OperationCanceledException();
                    }

                    return responseTask.Result;
                }
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.LazyCancellation, this.CompletionScheduler ?? TaskScheduler.Current);

            return waitTask;
        }

        /// <summary>
        ///     Takes the next request from the collection for processing or waits for a request if non is waiting to be processed.
        /// </summary>
        /// <param name="timeout"> The timeout used to wait for the next request. </param>
        /// <param name="ct"> The cancellation token which can be used to cancel the request awaiting. </param>
        /// <returns>
        ///     The task used to await the next request.
        /// </returns>
        /// <exception cref="TimeoutException"> <paramref name="timeout" /> was reached without a response. </exception>
        /// <exception cref="OperationCanceledException"> <paramref name="ct" /> was triggered. </exception>
        protected Task<TItem> TakeAsync (TimeSpan timeout, CancellationToken ct)
        {
            TaskCompletionSource<TItem> tcs;
            Task<TItem> consumerTask;

            lock (this.SyncRoot)
            {
                TItem request = this.GetAndRemoveNextRequest();

                while (request != null)
                {
                    if (request.StillNeeded)
                    {
                        break;
                    }

                    request = this.GetAndRemoveNextRequest();
                }

                if (request != null)
                {
                    return Task.FromResult(request);
                }

                tcs = new TaskCompletionSource<TItem>(this.CompletionCreationOptions);
                consumerTask = tcs.Task;

                this.AddConsumer(tcs);
            }

            Task timeoutTask = Task.Delay(timeout);
            Task cancelTask = Task.Delay(Timeout.InfiniteTimeSpan, ct);

            Task<Task> result = Task.WhenAny(consumerTask, timeoutTask, cancelTask);

            Task<TItem> waitTask = result.ContinueWith(task =>
            {
                lock (this.SyncRoot)
                {
                    this.RemoveConsumer(tcs);

                    if (consumerTask.IsCompleted)
                    {
                        return consumerTask.Result;
                    }

                    if (ReferenceEquals(result.Result, timeoutTask))
                    {
                        throw new TimeoutException();
                    }

                    if (ReferenceEquals(result.Result, cancelTask))
                    {
                        throw new OperationCanceledException();
                    }

                    return consumerTask.Result;
                }
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.LazyCancellation, this.CompletionScheduler ?? TaskScheduler.Current);

            return waitTask;
        }

        #endregion




        #region Abstracts

        /// <summary>
        ///     Gets the number of consumers waiting to process requests.
        /// </summary>
        /// <value>
        ///     The number of consumers waiting to process requests.
        /// </value>
        protected abstract int ConsumerCount { get; }

        /// <summary>
        ///     Gets the number of requests waiting to be processed.
        /// </summary>
        /// <value>
        ///     The number of requests waiting to be processed.
        /// </value>
        protected abstract int RequestCount { get; }

        /// <summary>
        ///     Adds a waiting consumer to the collection.
        /// </summary>
        /// <param name="consumer"> The consumer. </param>
        protected abstract void AddConsumer (TaskCompletionSource<TItem> consumer);

        /// <summary>
        ///     Adds a waiting request to the collection.
        /// </summary>
        /// <param name="request"> The request. </param>
        protected abstract void AddRequest (TItem request);

        /// <summary>
        ///     Removes all waiting consumers from the collection.
        /// </summary>
        protected abstract void ClearConsumers ();

        /// <summary>
        ///     Removes all waiting requests from the collection.
        /// </summary>
        protected abstract void ClearRequests ();

        /// <summary>
        ///     Gets and removes the next waiting consumer.
        /// </summary>
        /// <returns>
        ///     The consumer waiting for request the longest or null if no consumer is waiting for processing requests.
        /// </returns>
        protected abstract TaskCompletionSource<TItem> GetAndRemoveNextConsumer ();

        /// <summary>
        ///     Gets and removes the next waiting request.
        /// </summary>
        /// <returns>
        ///     The next request to be processed or null if no request is waiting.
        /// </returns>
        protected abstract TItem GetAndRemoveNextRequest ();

        /// <summary>
        ///     Gets the sequence of all currently waiting consumers.
        /// </summary>
        /// <returns>
        ///     The sequence of all currently waiting consumers.
        /// </returns>
        protected abstract IEnumerable<TaskCompletionSource<TItem>> GetConsumers ();

        /// <summary>
        ///     Gets the sequence of all currently waiting requests.
        /// </summary>
        /// <returns>
        ///     The sequence of all currently waiting requests.
        /// </returns>
        protected abstract IEnumerable<TItem> GetRequests ();

        /// <summary>
        ///     Initializes the collection.
        /// </summary>
        protected abstract void Initialize ();

        /// <summary>
        ///     Removes a consumer from the collection.
        /// </summary>
        /// <param name="consumer"> The consumer. </param>
        protected abstract void RemoveConsumer (TaskCompletionSource<TItem> consumer);

        /// <summary>
        ///     Removes a request from the collection.
        /// </summary>
        /// <param name="request"> The request. </param>
        protected abstract void RemoveRequest (TItem request);

        #endregion




        #region Interface: ISynchronizable

        /// <inheritdoc />
        bool ISynchronizable.IsSynchronized => true;

        /// <inheritdoc />
        public object SyncRoot { get; }

        #endregion
    }
}
