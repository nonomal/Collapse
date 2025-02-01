﻿using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

#nullable enable
namespace CollapseLauncher.Extension
{
    public delegate ConfiguredTaskAwaitable<TResult?> ActionTimeoutTaskAwaitableCallback<TResult>(CancellationToken token);
    internal static partial class TaskExtensions
    {
        internal static Task<TResult?>
            WaitForRetryAsync<TResult>(this ActionTimeoutTaskAwaitableCallback<TResult?> funcCallback,
                                       int?                                              timeout       = null,
                                       int?                                              timeoutStep   = null,
                                       int?                                              retryAttempt  = null,
                                       ActionOnTimeOutRetry?                             actionOnRetry = null,
                                       CancellationToken                                 fromToken     = default)
            => WaitForRetryAsync(() => funcCallback, timeout, timeoutStep, retryAttempt, actionOnRetry, fromToken);

        internal static async Task<TResult?>
            WaitForRetryAsync<TResult>(Func<ActionTimeoutTaskAwaitableCallback<TResult?>> funcCallback,
                                       int?                                               timeout       = null,
                                       int?                                               timeoutStep   = null,
                                       int?                                               retryAttempt  = null,
                                       ActionOnTimeOutRetry?                              actionOnRetry = null,
                                       CancellationToken                                  fromToken     = default)
            => await WaitForRetryAsync(funcCallback.AsTaskCallback(fromToken),
                                       timeout,
                                       timeoutStep,
                                       retryAttempt,
                                       actionOnRetry,
                                       fromToken);

        internal static ActionTimeoutTaskCallback<TResult?> AsTaskCallback<TResult>(this Func<ActionTimeoutTaskAwaitableCallback<TResult?>> func,
            CancellationToken fromToken)
        {
            return ActionTimeoutCallback;

            async Task<TResult?> ActionTimeoutCallback(CancellationToken innerToken)
            {
                ActionTimeoutTaskAwaitableCallback<TResult?> callback          = func.Invoke();
                ConfiguredTaskAwaitable<TResult?>            callbackAwaitable = callback.Invoke(fromToken);
                return await callbackAwaitable;
            }
        }
    }
}
