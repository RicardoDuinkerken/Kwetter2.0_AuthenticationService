using System.Net.Mime;
using AuthenticationService.Api.Dto;
using AuthenticationService.Core.Exceptions;
using AuthenticationService.Core.Services.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthenticationService.Api.Controllers;

[ApiController]
[Route("/")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IAuthenticationService _authService;
    
    public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authService)
    {
        _logger = logger;
        _authService = authService;
    }
    
    [HttpPost]
    [Route("login")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] AuthenticationRequestDto request)
    {
        _logger.LogInformation("Login Invoked for email: {E}", request.Email);
        try
        {
            return Ok(await _authService.Login(request.Email, request.Password));
        }
        catch (FailedLoginAttemptException)
        {
            _logger.LogWarning("Failed login attempt on email: {E}", request.Email);
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
        catch (Exception e)
        {
            _logger.LogError("{E}", e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpPost]
    [Route("register")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] CreateAccountRequestDto request)
    {
        _logger.LogInformation("Register Invoked for email: {E}", request.Email);
            
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!request.Password.Equals(request.ConfirmPassword))
            throw new ArgumentException("Password inputs do not match");

        try
        {
            return Ok(await _authService.Register(request.Email, request.Password));
        }
        catch (Exception e)
        {
            _logger.LogError("{E}", e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}