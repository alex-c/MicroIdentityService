using MicroIdentityService.Controllers.Contracts.Responses;
using MicroIdentityService.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroIdentityService.Controllers
{
    public class ControllerBase : Controller
    {
        /// <summary>
        /// Logger for controller-level logging.
        /// </summary>
        protected ILogger Logger { get; set; }

        /// <summary>
        /// Get's the URI of a newly created resource, to be included in `201 Created` responses.
        /// </summary>
        /// <param name="resourceId">The ID of the newly created resource.</param>
        /// <returns>Returns the newly created resource's URI.</returns>
        protected Uri GetNewResourceUri<T>(T resourceId)
        {
            return new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}/{resourceId}");
        }

        #region Pagination

        /// <summary>
        /// Validates pagination parameters. Delivers an error message on fail. Delivers whether pagination is disabled.
        /// Pagination is disabled when elementsPerPage is -1.
        /// </summary>
        /// <param name="page">The page of elements to return.</param>
        /// <param name="elementsPerPage">How many elements to return per page.</param>
        /// <param name="errorMessage">Error message to deliver on validation fail.</param>
        /// <param name="paginationDisabled">Delivers whether pagination is disabled.</param>
        /// <returns>Returns whether validation was successful.</returns>
        protected bool ValidatePaginationParameters(int page, int elementsPerPage, out string errorMessage, out bool paginationDisabled)
        {
            errorMessage = null;
            paginationDisabled = false;
            if (elementsPerPage == -1)
            {
                paginationDisabled = true;
            }
            else if (page <= 0 || elementsPerPage <= 0)
            {
                errorMessage = "Invalid paging parameters";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Paginates elements, if pagination is not disabled.
        /// </summary>
        /// <typeparam name="T">Type of the elements to paginate.</typeparam>
        /// <param name="elements">Elements to paginate.</param>
        /// <param name="page">The page of elements to return.</param>
        /// <param name="elementsPerPage">How many elements to return per page.</param>
        /// <param name="paginationDisabled">Whether pagination is disabled. Defaults to false.</param>
        /// <returns>Returns the paginated elements.</returns>
        protected IEnumerable<T> Paginate<T>(IEnumerable<T> elements, int page, int elementsPerPage, bool paginationDisabled = false)
        {
            if (paginationDisabled)
            {
                return elements;
            }
            return elements.Skip((page - 1) * elementsPerPage).Take(elementsPerPage);

        }

        #endregion

        #region Error Handling

        /// <summary>
        /// Handle bad requests.
        /// </summary>
        /// <param name="message">Message that should explain why the request is bad!</param>
        /// <returns>Returns a 400 error.</returns>
        protected IActionResult HandleBadRequest(string message)
        {
            return BadRequest(new ClientErrorResponse(message));
        }

        /// <summary>
        /// Handle "resource not found"-type of exceptions
        /// </summary>
        /// <param name="exception">The actual exception.</param>
        /// <returns>Returns a 404 error.</returns>
        protected IActionResult HandleResourceNotFoundException(EntityNotFoundException exception)
        {
            return NotFound(new ClientErrorResponse(exception.Message));
        }

        /// <summary>
        /// Handle "resource already exists"-type of exceptions.
        /// </summary>
        /// <param name="exception">The actual exception.</param>
        /// <returns>Returns a 409 error.</returns>
        protected IActionResult HandleResourceAlreadyExistsException(EntityAlreadyExsistsException exception)
        {
            return Conflict(new ClientErrorResponse(exception.Message));
        }

        /// <summary>
        /// Handle unexpected exceptions.
        /// </summary>
        /// <param name="exception">Unexpected exception that was caught.</param>
        /// <returns>Returns a 500 error.</returns>
        protected IActionResult HandleUnexpectedException(Exception exception)
        {
            Logger?.LogError(exception, "An unexpected exception was caught.");
            return new StatusCodeResult(500);
        }

        /// <summary>
        /// Handle unexpected exceptions with extra message.
        /// </summary>
        /// <param name="exception">Unexpected exception that was caught.</param>
        /// <param name="message">Extra message explaining the problem.</param>
        /// <returns>Returns a 500 error.</returns>
        protected IActionResult HandleUnexpectedException(Exception exception, string message)
        {
            Logger?.LogError(exception, message);
            return new StatusCodeResult(500);
        }

        #endregion
    }
}
