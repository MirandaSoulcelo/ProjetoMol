using Microsoft.AspNetCore.Mvc;
using TheProject.Application.DTOs;
using TheProject.Application.Interfaces;
using TheProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace TheProject.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{


    private readonly IProductsInterface _productsInterface;
    public ProductsController(IProductsInterface productsInterface)
    {
        _productsInterface = productsInterface;
    }



    [HttpGet("GetAll")]
    public async Task<ActionResult<Response<List<ProductsDTO>>>> GetAll(
     [FromQuery] string? search = null,
     [FromQuery] int page = 1,
     [FromQuery] int pageSize = 10)
    {
        var products = await _productsInterface.GetAll(search, page, pageSize);
        return Ok(products);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] ProductUptadeDTO dto)
    {
        

        var result = await _productsInterface.Update(dto);
        if (!result.Status)
            return BadRequest(result.Message);

        return Ok(result);
    }


    [HttpPost("Add")]
    public async Task<ActionResult<Response<Products>>> Add([FromBody] ProductUptadeDTO dto)
    {
        var result = await _productsInterface.Add(dto);
        if (!result.Status)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }



    [HttpDelete]
    [Authorize]
        public async Task<IActionResult> Delete([FromBody] ProductDeleteDTO dto)
    {
        try
        {
            var result = await _productsInterface.Delete(dto);

            if (result.Status)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response<bool>
            {
                Status = false,
                Message = $"Erro interno do servidor: {ex.Message}",
                Data = false
            });
        }
    }
}

