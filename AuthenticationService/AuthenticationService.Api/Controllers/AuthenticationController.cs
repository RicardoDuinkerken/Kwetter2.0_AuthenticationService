using System.Net.Mime;
using AuthenticationService.Api.Dto;
using AuthenticationService.Core.Exceptions;
using AuthenticationService.Core.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
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
    
    [Authorize]
    [HttpPost]
        [Route("account/changePassword")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            _logger.LogInformation("ChangePassword Invoked for email: {E}", request.Email);
            try
            {
                return Ok(await _authService.ChangePassword(request.Email, request.Password));
            }
            catch (FailedLoginAttemptException)
            {
                _logger.LogWarning("Failed ChangePassword attempt on email: {E}", request.Email);
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch (Exception e)
            {
                _logger.LogError("{E}", e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [Authorize]
        [HttpPost]
        [Route("account/changeEmail")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailRequestDto request)
        {
            _logger.LogInformation("ChangeEmail Invoked for email: {E}", request.OldEmail);
            try
            {
                return Ok(await _authService.ChangeEmail(request.OldEmail, request.NewEmail));
            }
            catch (FailedLoginAttemptException)
            {
                _logger.LogWarning("Failed ChangeEmail attempt on email: {E}", request.OldEmail);
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch (Exception e)
            {
                _logger.LogError("{E}", e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [Authorize]
        [HttpPost]
        [Route("account/delete")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromBody] DeleteAccountRequestDto request)
        {
            _logger.LogInformation("Delete Invoked for email: {E}", request.Email);
            try
            {
                return Ok(await _authService.DeleteAccount(request.Email));
            }
            catch (FailedLoginAttemptException)
            {
                _logger.LogWarning("Failed DeleteAccount attempt on email: {E}", request.Email);
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch (Exception e)
            {
                _logger.LogError("{E}", e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
}