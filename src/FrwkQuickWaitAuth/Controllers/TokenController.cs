using FrwkQuickWait.Domain.Entities;
using FrwkQuickWait.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FrwkQuickWaitAuth.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService tokenService;
        private readonly IUserService userService;
        public TokenController(ITokenService tokenService, IUserService userService)
        {
            this.tokenService = tokenService;
            this.userService = userService;

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody][Required] User model)
        {
            var response = await userService.GetUser(model);

            if (response == null)
                return NotFound(new { message = "Não existe usuário com essas especificações!"});

            var token = await tokenService.GenerateToken(response);

            return Ok(new { token } );
        }

    }
}
