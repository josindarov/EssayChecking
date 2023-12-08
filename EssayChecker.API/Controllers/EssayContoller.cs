using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Essays;
using EssayChecker.API.Models.Foundation.Essays.Exceptions;
using EssayChecker.API.Services.Foundation.Essays;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace EssayChecker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EssayController : RESTFulController
{
    private readonly IEssayService essayService;

    public EssayController(IEssayService essayService) =>
        this.essayService = essayService;

    [HttpPost]
    public async ValueTask<ActionResult<Essay>> PostEssayAsync(Essay essay)
    {
        try
        {
            Essay postedEssay = await this.essayService.InsertEssayAsync(essay);
            return Created(postedEssay);
        }
        catch (EssayValidationException essayValidationException)
        {
            return BadRequest(essayValidationException.InnerException);
        }
        catch (EssayDependencyException essayDependencyException)
        {
            return InternalServerError(essayDependencyException);
        }
        catch (EssayServiceException essayServiceException)
        {
            return InternalServerError(essayServiceException);
        }
    }

    [HttpGet]
    public ActionResult<IQueryable<Essay>> GetAllEssays()
    {
        try
        {
            IQueryable<Essay> essays = this.essayService.RetrieveAllEssays();
            return Ok(essays);
        }
        catch (EssayDependencyException essayDependencyException)
        {
            return InternalServerError(essayDependencyException);
        }
        catch (EssayServiceException essayServiceException)
        {
            return InternalServerError(essayServiceException);
        }
    }

    [HttpGet("{id}")]
    public async ValueTask<ActionResult<Essay>> GetEssayByIdAsync(Guid id)
    {
        try
        {
            Essay essay = await this.essayService.RetrieveEssayById(id);
            return Ok(essay);
        }
        catch (EssayValidationException essayValidationException)
            when (essayValidationException.InnerException is NotFoundEssayException)
        {
            return NotFound(essayValidationException.InnerException);
        }
        catch (EssayValidationException essayValidationException)
        {
            return BadRequest(essayValidationException.InnerException);
        }
        catch (EssayDependencyException essayDependencyException)
        {
            return InternalServerError(essayDependencyException);
        }
        catch (EssayServiceException essayServiceException)
        {
            return InternalServerError(essayServiceException);
        }
    }
    
}