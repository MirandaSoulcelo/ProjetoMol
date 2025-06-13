using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheProject.Application.DTOs;
using TheProject.Application.Features.Categories.Commands.AddCategory;
using TheProject.Application.Features.Categories.Queries.GetAllCategories;
using TheProject.Domain.Entities;

namespace TheProject.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<Response<List<CategoryDto>>>> GetAll()
        {
            var query = new GetAllCategoriesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<ActionResult<Response<CategoryDto>>> Add([FromBody] AddCategoryRequest request)
        {
            var command = new AddCategoryCommand
            {
                Name = request.Name
            };

            var result = await _mediator.Send(command);

            if (!result.Status)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}