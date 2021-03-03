using ReactiveUI;
using AYP.Helpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace AYP.Helpers.Extensions
{
    public static class ReactiveCommandExtension
    {
        public static IDisposable ExecuteWithSubscribe<TParam, TResult>(this ReactiveCommand<TParam, TResult> reactiveCommand, TParam parameter = default)
        {
            return reactiveCommand.Execute(parameter).Subscribe();
        }
    }
}
