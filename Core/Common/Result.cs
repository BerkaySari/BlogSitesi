using System;
using System.Collections.Generic;

namespace Core.Common
{
    public class Result
    {

        public dynamic NotifyMessages { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

        public static Result AsSuccess(dynamic notifyList)
        {
            return new Result { IsSuccess = true, NotifyMessages = notifyList };
        }

        public static Result AsSuccess()
        {
            return new Result { IsSuccess = true };
        }

        public static Result AsError(string errorMessage)
        {
            return new Result { Message = errorMessage };
        }

        public static Result AsError(IList<string> errorMessages)
        {
            return new Result { Message = string.Join("\n", errorMessages) };
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }


        public new static Result<T> AsError(string errorMessage)
        {
            return new Result<T> { Message = errorMessage };
        }

        public new static Result<T> AsError(T data)
        {
            return new Result<T> { IsSuccess = false, Data = data };
        }



        public static Result<T> AsSuccess(T data)
        {
            return new Result<T> { IsSuccess = true, Data = data };
        }
        public static Result<T> AsSuccess(T data, dynamic notify)
        {
            return new Result<T> { IsSuccess = true, Data = data, NotifyMessages = notify };
        }
    }
}