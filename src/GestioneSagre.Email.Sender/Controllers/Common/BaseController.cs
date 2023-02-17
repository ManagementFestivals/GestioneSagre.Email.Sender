using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace GestioneSagre.Email.Sender.Controllers.Common;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class BaseController : ControllerBase
{
}