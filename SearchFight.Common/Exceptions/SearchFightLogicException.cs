using System;

namespace SearchFight.Common.Exceptions
{
    public class SearchFightLogicException : SearchFightException
    {
        public SearchFightLogicException(string message) : base(message) { }
        public SearchFightLogicException(string message, Exception innerException) : base(message, innerException) { }
    }
}
