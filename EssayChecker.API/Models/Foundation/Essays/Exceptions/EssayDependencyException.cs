using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Essays.Exceptions;

public class EssayDependencyException : Xeption
{
    public EssayDependencyException(Exception innerException)
        : base("Essay dependency error occured, contact support.", innerException)
    { }
}