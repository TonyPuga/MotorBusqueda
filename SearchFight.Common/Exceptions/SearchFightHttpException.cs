using System;

namespace SearchFight.Common.Exceptions
{
    public class SearchFightHttpException : SearchFightException
    {
        public SearchFightHttpException(string message) : base(message) { }
        public SearchFightHttpException(string message, Exception innerException) : base(message, innerException) { }
    }
}
