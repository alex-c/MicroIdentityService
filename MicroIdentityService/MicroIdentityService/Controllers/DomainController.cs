using MicroIdentityService.Authorization;
using MicroIdentityService.Controllers.Contracts.Requests;
using MicroIdentityService.Controllers.Contracts.Responses;
using MicroIdentityService.Models;
using MicroIdentityService.Services;
using MicroIdentityService.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroIdentityService.Controllers
{
    [Route("api/v1/domains")]
    public class DomainController : ControllerBase
    {
        private DomainService DomainService { get; }

        public DomainController(ILogger<DomainController> logger, DomainService domainService)
        {
            DomainService = domainService;
            Logger = logger;
        }

        [HttpGet, Authorize(Policy = Policies.DOMAINS_GET)]
        public async Task<IActionResult> GetDomains([FromQuery] string filter = null, [FromQuery] int page = 1, [FromQuery] int elementsPerPage = 10)
        {
            if (!ValidatePaginationParameters(page, elementsPerPage, out string errorMessage, out bool paginationDisabled))
            {
                return HandleBadRequest(errorMessage);
            }

            try
            {
                IEnumerable<Domain> domains = await DomainService.GetDomains(filter);
                IEnumerable<Domain> paginatedIdentities = Paginate(domains, page, elementsPerPage, paginationDisabled);
                return Ok(new PaginatedResponse<DomainResponse>(paginatedIdentities.Select(i => new DomainResponse(i)), domains.Count()));
            }
            catch (Exception exception)
            {
                return HandleUnexpectedException(exception);
            }
        }

        [HttpGet("{id}"), Authorize(Policy = Policies.DOMAINS_GET)]
        public async Task<IActionResult> GetDomain(Guid id)
        {
            try
            {
                Domain domain = await DomainService.GetDomain(id);
                return Ok(new DomainResponse(domain));
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

        [HttpPost, Authorize(Policy = Policies.DOMAINS_CREATE)]
        public async Task<IActionResult> CreateDomain([FromBody] DomainCreationOrUpdateRequest domainCreationRequest)
        {
            if (domainCreationRequest == null || string.IsNullOrWhiteSpace(domainCreationRequest.Name))
            {
                return HandleBadRequest("A valid domain name needs to be provided.");
            }

            try
            {
                Domain domain = await DomainService.CreateDomain(domainCreationRequest.Name);
                return Created(GetNewResourceUri(domain.Id), new DomainResponse(domain));
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

        [HttpPatch("{id}"), Authorize(Policy = Policies.DOMAINS_UPDATE)]
        public async Task<IActionResult> UpdateDomain(Guid id, [FromBody] DomainCreationOrUpdateRequest domainUpdateRequest)
        {
            if (domainUpdateRequest == null || string.IsNullOrWhiteSpace(domainUpdateRequest.Name))
            {
                return HandleBadRequest("A valid domain name needs to be provided.");
            }

            try
            {
                Domain domain = await DomainService.UpdateDomain(id, domainUpdateRequest.Name);
                return Ok(new DomainResponse(domain));
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

        [HttpDelete("{id}"), Authorize(Policy = Policies.DOMAINS_DELETE)]
        public async Task<IActionResult> DeleteIdentity(Guid id)
        {
            await DomainService.DeleteDomain(id);
            return NoContent();
        }
    }
}
