using System;

namespace Hallo.Sip
{
    public interface IExceptionHandler
    {
        void Handle(Exception e);
    }
}