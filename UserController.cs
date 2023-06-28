using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    [HttpGet("anonymous")]
    public IActionResult AnonymousAction()
    {
        if (HttpContext.Session.GetString("UserId") == null)
        {
            // Generate a unique identifier
            var userId = Guid.NewGuid().ToString();

            // Store the identifier in the session
            HttpContext.Session.SetString("UserId", userId);
        }

        return Ok();
    }
}