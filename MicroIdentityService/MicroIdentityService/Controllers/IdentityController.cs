using MicroIdentityService.Controllers.Contracts.Requests;
using MicroIdentityService.Controllers.Contracts.Responses;
using MicroIdentityService.Models;
using MicroIdentityService.Services;
using MicroIdentityService.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroIdentityService.Controllers
{
    [Route("api/v1/identities")]
    public class IdentityController : ControllerBase
    {
        private IdentifierValidationService IdentifierValidationService { get; }

        private IdentityService IdentityService { get; }

        public IdentityController(ILogger<IdentityController> logger,
            IdentifierValidationService identifierValidationService,
            IdentityService identityService)
        {
            IdentifierValidationService = identifierValidationService;
            IdentityService = identityService;
            Logger = logger;
        }

        [HttpGet]
        public IActionResult GetIdentities([FromQuery] int page = 1, [FromQuery] int elementsPerPage = 10)
        {
            try
            {
                IEnumerable<Identity> identities = IdentityService.GetIdentities();
                IEnumerable<Identity> paginatedIdentities = identities.Skip((page - 1) * elementsPerPage).Take(elementsPerPage);
                return Ok(new PaginatedResponse<IdentityResponse>(paginatedIdentities.Select(i => new IdentityResponse(i)), identities.Count()));
            }
            catch (Exception exception)
            {
                return HandleUnexpectedException(exception);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetIdentity(Guid id)
        {
            try
            {
                Identity identity = IdentityService.GetIdentity(id);
                return Ok(new IdentityResponse(identity));
            }
            catch (EntityNotFoundException exception)
            {
                return HandleResourceNotFoundException(exception);
            }
            catch (Exception exception)
            {
                return HandleUnexpectedException(exception);
            }
        }

        [HttpPost]
        public IActionResult CreateIdentity([FromBody] IdentityCreationRequest identityCreationRequest)
        {
            if (identityCreationRequest == null ||
                string.IsNullOrWhiteSpace(identityCreationRequest.Identifier) ||
                string.IsNullOrWhiteSpace(identityCreationRequest.Password))
            {
                return HandleBadRequest("A valid user identifier and password need to be provided.");
            }

            try
            {
                IdentifierValidationService.Validate(identityCreationRequest.Identifier);
                Identity identity = IdentityService.CreateIdentity(identityCreationRequest.Identifier, identityCreationRequest.Password);
                return Created(GetNewResourceUri(identity.Id), new IdentityResponse(identity));
            }
            catch (IdentifierValidationException)
            {
                return HandleBadRequest("The provided user identifier is not valid.");
            }
            catch (EntityAlreadyExsistsException exception)
            {
                return HandleResourceAlreadyExistsException(exception);
            }
            catch (Exception exception)
            {
                return HandleUnexpectedException(exception);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteIdentity(Guid id)
        {
            IdentityService.DeleteIdentity(id);
            return Ok();
        }
    }
}
