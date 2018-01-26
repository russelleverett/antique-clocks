using Microsoft.AspNetCore.Authorization;
using System;

namespace Website.Infrastructure.Attributes {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class LoginRequiredAttribute : Attribute { }
}
