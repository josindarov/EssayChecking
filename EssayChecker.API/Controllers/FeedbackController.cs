using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Essays.Exceptions;
using EssayChecker.API.Models.Foundation.Feedbacks;
using EssayChecker.API.Models.Foundation.Feedbacks.Exceptions;
using EssayChecker.API.Services.Foundation.Feedbacks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace EssayChecker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController : RESTFulController
{
    private readonly IFeedbackService feedbackService;

    public FeedbackController(IFeedbackService feedbackService) =>
        this.feedbackService = feedbackService;

    [HttpPost]
    public async ValueTask<ActionResult<Feedback>> PostFeedbackAsync(Feedback feedback)
    {
        try
        {
            Feedback postedFeedback = await this.feedbackService
                .AddFeedbackAsync(feedback);

            return Created(postedFeedback);
        }
        catch (FeedbackValidationException feedbackValidationException)
        {
            return BadRequest(feedbackValidationException.InnerException);
        }
        catch (FeedbackDependencyException feedbackDependencyException)
        {
            return InternalServerError(feedbackDependencyException);
        }
        catch (FeedbackServiceException feedbackServiceException)
        {
            return InternalServerError(feedbackServiceException);
        }
    }

    [HttpGet]
    public ActionResult<IQueryable<Feedback>> GetAllFeedbacks()
    {
        try
        {
            IQueryable<Feedback> feedbacks = this.feedbackService
                .RetrieveAllFeedbacks();

            return Ok(feedbacks);
        }
        catch (FeedbackDependencyException feedbackDependencyException)
        {
            return InternalServerError(feedbackDependencyException);
        }
        catch (FeedbackServiceException feedbackServiceException)
        {
            return InternalServerError(feedbackServiceException);
        }
    }

    [HttpGet("{id}")]
    public async ValueTask<ActionResult<Feedback>> GetFeedbackByIdAsync(Guid id)
    {
        try
        {
            Feedback feedback = await this.feedbackService
                .RetrieveFeedbackByIdAsync(id);

            return Ok(feedback);
        }
        catch (FeedbackValidationException feedbackValidationException)
            when(feedbackValidationException.InnerException is NotFoundFeedbackException)
        {
            return NotFound(feedbackValidationException.InnerException);
        }
        catch (FeedbackValidationException feedbackValidationException)
        {
            return BadRequest(feedbackValidationException.InnerException);
        }
        catch (FeedbackDependencyException feedbackDependencyException)
        {
            return InternalServerError(feedbackDependencyException);
        }
        catch (FeedbackServiceException feedbackServiceException)
        {
            return InternalServerError(feedbackServiceException);
        }
    }
}