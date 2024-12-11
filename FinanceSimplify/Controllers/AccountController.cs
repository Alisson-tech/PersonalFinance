using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceSimplify.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : Controller
{
    [HttpGet]
    public ActionResult GetAll()
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public ActionResult GetId(int id)
    {
        return Ok();
    }

    [HttpPost]
    public ActionResult Create()
    {
        return Ok();
    }

    [HttpPut("{id}")]
    public ActionResult Edit(int id)
    {
        return Ok();
    }


    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        return Ok();
    }

}
