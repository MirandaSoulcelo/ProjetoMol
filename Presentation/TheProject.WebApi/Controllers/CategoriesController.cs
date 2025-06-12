using Microsoft.AspNetCore.Mvc;
using TheProject.Application.DTOs;
using TheProject.Application.Interfaces;
using TheProject.Domain.Entities;

namespace TheProject.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesInterface _categoriesInterface;

        public CategoriesController(ICategoriesInterface categoriesInterface)
        {
            _categoriesInterface = categoriesInterface;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<Response<List<CategoryDto>>>> GetAll()
        {
            var categories = await _categoriesInterface.GetAll();
            return Ok(categories);
        }

        [HttpPost("Add")]
        public async Task<ActionResult<Response<CategoryDto>>> Add([FromBody] AddCategoryRequest request)
        {
            var result = await _categoriesInterface.Add(request.Name);
            
            if (!result.Status)
            {
                return BadRequest(result);
            }
            
            return Ok(result);
        }
    }
}