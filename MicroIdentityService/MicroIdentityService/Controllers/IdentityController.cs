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
using System.Threading.Tasks;

namespace MicroIdentityService.Controllers
{
    [Route("api/v1/identities")]
    public class IdentityController : ControllerBase
    {
        private IdentityService IdentityService { get; }

        public IdentityController(ILogger<IdentityController> logger, IdentityService identityService)
        {
            IdentityService = identityService;
            Logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetIdentities([FromQuery] string filter = null, [FromQuery] int page = 1, [FromQuery] int elementsPerPage = 10)
        {
            if (!ValidatePaginationParameters(page, elementsPerPage, out string errorMessage, out bool paginationDisabled))
            {
                return HandleBadRequest(errorMessage);
            }

            try
            {
                IEnumerable<Identity> identities = await IdentityService.GetIdentities(filter);
                IEnumerable<Identity> paginatedIdentities = Paginate(identities, page, elementsPerPage, paginationDisabled);
                return Ok(new PaginatedResponse<IdentityResponse>(paginatedIdentities.Select(i => new IdentityResponse(i)), identities.Count()));
            }
            catch (Exception exception)
            {
                return HandleUnexpectedException(exception);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIdentity(Guid id)
        {
            try
            {
                Identity identity = await IdentityService.GetIdentity(id);
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
        public async Task<IActionResult> CreateIdentity([FromBody] IdentityCreationRequest identityCreationRequest)
        {
            if (identityCreationRequest == null ||
                string.IsNullOrWhiteSpace(identityCreationRequest.Identifier) ||
                string.IsNullOrWhiteSpace(identityCreationRequest.Password))
            {
                return HandleBadRequest("A valid user identifier and password need to be provided.");
            }

            try
            {
                Identity identity = await IdentityService.CreateIdentity(identityCreationRequest.Identifier, identityCreationRequest.Password);
                return Created(GetNewResourceUri(identity.Id), new IdentityResponse(identity));
            }
            catch (ValidationException exception)
            {
                return HandleBadRequest(exception.Message);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIdentity(Guid id, [FromBody] IdentityUpdateRequest identityUpdateRequest)
        {
            try
            {
                await IdentityService.UpdateIdentity(id, identityUpdateRequest.Disabled);
                return NoContent();
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIdentity(Guid id)
        {
            await IdentityService.DeleteIdentity(id);
            return NoContent();
        }

        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetIdentityRoles(Guid id)
        {
            IEnumerable<Role> identityRoles = await IdentityService.GetIdentityRoles(id);
            return Ok(identityRoles.Select(r => new RoleResponse(r)));
        }

        [HttpPut("{id}/roles")]
        public async Task<IActionResult> UpdateIdentityRoles(Guid id, [FromBody] IdentityRolesUpdateRequest identityRolesUpdateRequest)
        {
            try
            {
                await IdentityService.UpdateIdentityRoles(id, identityRolesUpdateRequest.RoleIds);
                return NoContent();
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
    }
}
