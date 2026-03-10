
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]

public class UserController : ControllerBase
{
    private static List<User> users = new List<User>();

    [HttpGet]
    public ActionResult GetUsers()
    {
        return Ok(users);
    }

    [HttpGet("{id}")]
    public ActionResult GetUserById(int id)
    {
        var user = users.FirstOrDefault(user => user.Id == id);

        return user != null ? Ok(user) : NotFound($"User with id: {id} not found.");
    }

    [HttpPost]
    public ActionResult CreateUser([FromBody] User newUser)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        users.Add(newUser);
        return Ok(newUser);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateUser(int id, [FromBody] User updatedUser)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = users.FirstOrDefault(user => user.Id == id);

        if (user == null)
        {
            return NotFound($"User with id: {id} not found.");
        }

        user.Name = updatedUser.Name;
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id)
    {
        var user = users.FirstOrDefault(user => user.Id == id);
        if (user == null)
        {
            return NotFound($"User with id: {id} not found");
        }

        users.Remove(user);
        return NoContent();
    }
}