using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Essays;
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
        Essay postedEssay = await this.essayService.InsertEssayAsync(essay);
        return Created(postedEssay);
    }

    [HttpGet]
    public ActionResult<IQueryable<Essay>> GetAllEssays()
    {
        IQueryable<Essay> essays = this.essayService.RetrieveAllEssays();
        return Ok(essays);
    }

    [HttpGet("{id}")]
    public async ValueTask<ActionResult<Essay>> GetEssayByIdAsync(Guid id)
    {
        Essay essay = await this.essayService.RetrieveEssayById(id);
        return Ok(essay);
    }
    
}