using System.Collections.Generic;

namespace MicroIdentityService.Controllers.Contracts.Responses
{
    /// <summary>
    /// A generic response contract for paginated data.
    /// </summary>
    /// <typeparam name="T">Type of the data to paginate.</typeparam>
    public class PaginatedResponse<T>
    {

        /// <summary>
        /// The paginated data.
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Indicates the total number of elements in the collection, before pagination was applied.
        /// </summary>
        public int TotalElements { get; set; }

        /// <summary>
        /// Creates a paginated data response.
        /// </summary>
        /// <param name="data">The paginated data.</param>
        /// <param name="totalElements">The total element count.</param>
        public PaginatedResponse(IEnumerable<T> data, int totalElements)
        {
            Data = data;
            TotalElements = totalElements;
        }
    }
}
