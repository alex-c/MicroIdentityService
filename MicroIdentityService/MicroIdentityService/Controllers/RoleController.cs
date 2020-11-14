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
    [Route("api/v1/roles")]
    public class RoleController : ControllerBase
    {
        private RoleService RoleService { get; }

        public RoleController(ILogger<RoleController> logger, RoleService roleService)
        {
            RoleService = roleService;
            Logger = logger;
        }

        [HttpGet]
        public IActionResult GetRoles([FromQuery] string filter = null, [FromQuery] int page = 1, [FromQuery] int elementsPerPage = 10, [FromQuery] Guid? domainId = null)
        {
            if (!ValidatePaginationParameters(page, elementsPerPage, out string errorMessage, out bool paginationDisabled))
            {
                return HandleBadRequest(errorMessage);
            }

            try
            {
                IEnumerable<Role> roles = RoleService.GetRoles(filter, domainId);
                IEnumerable<Role> paginatedIdentities = Paginate(roles, page, elementsPerPage, paginationDisabled);
                return Ok(new PaginatedResponse<RoleResponse>(paginatedIdentities.Select(i => new RoleResponse(i)), roles.Count()));
            }
            catch (EntityNotFoundException exception)
            {
                // Thrown if a domain ID is provided but no matching domain could be found
                return HandleResourceNotFoundException(exception);
            }
            catch (Exception exception)
            {
                return HandleUnexpectedException(exception);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetRole(Guid id)
        {
            try
            {
                Role role = RoleService.GetRole(id);
                return Ok(new RoleResponse(role));
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
        public IActionResult CreateRole([FromBody] RoleCreationRequest roleCreationRequest)
        {
            if (roleCreationRequest == null || string.IsNullOrWhiteSpace(roleCreationRequest.Name))
            {
                return HandleBadRequest("A valid role name needs to be provided.");
            }

            try
            {
                Role role = RoleService.CreateRole(roleCreationRequest.Name, roleCreationRequest.DomainId);
                return Created(GetNewResourceUri(role.Id), new RoleResponse(role));
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

        [HttpPatch("{id}")]
        public IActionResult UpdateRole(Guid id, [FromBody] RoleUpdateRequest roleUpdateRequest)
        {
            if (roleUpdateRequest == null || string.IsNullOrWhiteSpace(roleUpdateRequest.Name))
            {
                return HandleBadRequest("A valid role name needs to be provided.");
            }

            try
            {
                Role role = RoleService.UpdateRole(id, roleUpdateRequest.Name);
                return Ok(new RoleResponse(role));
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
        public IActionResult DeleteIdentity(Guid id)
        {
            RoleService.DeleteRole(id);
            return NoContent();
        }
    }
}
