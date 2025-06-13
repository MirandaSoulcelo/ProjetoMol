using Microsoft.AspNetCore.Mvc;
using TheProject.Application.DTOs.UsersDTO;
using TheProject.Application.Interfaces;
using TheProject.Domain.Entities;

namespace TheProject.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{



    private readonly IUsersInterface _usersInterface;

    public UsersController(IUsersInterface usersInterface)
    {
        _usersInterface = usersInterface;
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<Response<List<UsersDTO>>>> GetAll()
    {
        var users = await _usersInterface.GetAll();
        return Ok(users);
    }

    [HttpPost("Add")]
    public async Task<ActionResult<Response<UsersDTO>>> Add([FromBody] UsersDTO dto)
    {
        var result = await _usersInterface.Add(dto);

        if (!result.Status)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }



    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UsersDTO dto)
    {
        // dto.Id = id; 

        var result = await _usersInterface.Update(dto);
        if (!result.Status)
            return BadRequest(result.Message);

        return Ok(result);
    }


    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] UserDeleteDTO dto)
    {
        try
        {
            var result = await _usersInterface.Delete(dto);

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
