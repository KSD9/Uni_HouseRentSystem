using System;
using System.Collections.Generic;
using System.Text;

namespace HouseRent.Common.Interfaces
{
    public interface IValidation
    {
        void Error(string key, string msg);
        bool IsValid { get; }
    }
}
