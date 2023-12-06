using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Essays.Exceptions;

public class FailedEssayStorageException : Xeption
{
    public FailedEssayStorageException(Exception innerException)
        :base("Failed essay storage error occured, contact support.", innerException)
    { }
}