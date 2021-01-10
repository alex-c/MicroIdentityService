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
    [Route("api/v1/api-keys")]
    public class ApiKeyController : ControllerBase
    {
        private ApiKeyService ApiKeyService { get; }

        public ApiKeyController(ILogger<ApiKeyController> logger, ApiKeyService apiKeyService)
        {
            ApiKeyService = apiKeyService;
            Logger = logger;
        }

        [HttpGet, Authorize(Policy = Policies.API_KEYS_GET)]
        public async Task<IActionResult> GetApiKeys([FromQuery] string filter = null, [FromQuery] int page = 1, [FromQuery] int elementsPerPage = 10)
        {
            if (!ValidatePaginationParameters(page, elementsPerPage, out string errorMessage, out bool paginationDisabled))
            {
                return HandleBadRequest(errorMessage);
            }

            try
            {
                IEnumerable<ApiKey> keys = await ApiKeyService.GetApiKeys(filter);
                IEnumerable<ApiKey> paginatedKeys = Paginate(keys, page, elementsPerPage, paginationDisabled);
                return Ok(new PaginatedResponse<ApiKeyResponse>(paginatedKeys.Select(k => new ApiKeyResponse(k)), keys.Count()));
            }
            catch (Exception exception)
            {
                return HandleUnexpectedException(exception);
            }
        }

        [HttpPost, Authorize(Policy = Policies.API_KEYS_CREATE)]
        public async Task<IActionResult> CreateApiKey([FromBody] ApiKeyCreationRequest apiKeyCreationRequest)
        {
            if (apiKeyCreationRequest == null || string.IsNullOrWhiteSpace(apiKeyCreationRequest.Name))
            {
                return HandleBadRequest("A valid key name has to be supplied.");
            }

            ApiKey key = await ApiKeyService.CreateApiKey(apiKeyCreationRequest.Name);
            return Created(GetNewResourceUri(key.Id), new ApiKeyResponse(key));
        }

        [HttpPut("{id}/status"), Authorize(Policy = Policies.API_KEYS_UPDATE)]
        public async Task<IActionResult> UpdateApiKeyStatus(Guid id, [FromBody] ApiKeyStatusUpdateRequest apiKeyStatusUpdateRequest)
        {
            if (apiKeyStatusUpdateRequest == null)
            {
                return HandleBadRequest("Missing status data.");
            }

            try
            {
                await ApiKeyService.UpdateApiKeyStatus(id, apiKeyStatusUpdateRequest.Enabled);
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

        [HttpDelete("{id}"), Authorize(Policy = Policies.API_KEYS_DELETE)]
        public async Task<IActionResult> DeleteApiKey(Guid id)
        {
            await ApiKeyService.DeleteApiKey(id);
            return NoContent();
        }

        #region Api key permissions

        [HttpGet("{id}/permissions"), Authorize(Policy = Policies.API_KEYS_GET_PERMISSIONS)]
        public async Task<IActionResult> GetApiKeyPermissions(Guid id)
        {
            IEnumerable<string> apiKeyPermissions = await ApiKeyService.GetApiKeyPermissions(id);
            return Ok(apiKeyPermissions);
        }

        [HttpPut("{id}/permissions"), Authorize(Policy = Policies.API_KEYS_SET_PERMISSIONS)]
        public async Task<IActionResult> UpdateApiKeyPermissions(Guid id, [FromBody] ApiKeyPermissionsUpdateRequest apiKeyPermissionsUpdateRequest)
        {
            try
            {
                await ApiKeyService.UpdateApiKeyPermissions(id, apiKeyPermissionsUpdateRequest.Permissions);
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

        #endregion
    }
}
