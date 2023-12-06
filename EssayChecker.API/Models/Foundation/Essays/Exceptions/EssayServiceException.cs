using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Essays.Exceptions;

public class EssayServiceException : Xeption
{
    public EssayServiceException(Exception innerException)
        : base("Service error occured, contact suport", innerException)
    { }
}