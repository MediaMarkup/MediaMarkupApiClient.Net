using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MediaMarkup.Api.Models;
using MediaMarkup.Core;

namespace MediaMarkup.Api
{
    public class Users : IUsers
    {
        public HttpClient ApiClient { get; set; }

        public Users(HttpClient apiClient)
        {
            ApiClient = apiClient;
        }

        /// <inheritdoc />
        public async Task<User> Create(UserCreateParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Users/Create/", parameters);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<User>();
            }

            throw new ApiException("Users.Create", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
        public async Task<User> GetById(string id, bool throwExceptionIfNull = false)
        {
            var response = await ApiClient.GetAsync($"Users/GetById?id={id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<User>();
            }

            if (throwExceptionIfNull)
            {
                throw new ApiException("Users.GetById", response.StatusCode, await response.Content.ReadAsStringAsync());
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<User> GetByEmail(string email, bool throwExceptionIfNull = false)
        {
            var response = await ApiClient.GetAsync($"users/email/{WebUtility.UrlEncode(email)}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<User>();
            }

            if (throwExceptionIfNull)
            {
                throw new ApiException("Users.GetByEmail", response.StatusCode, await response.Content.ReadAsStringAsync());
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<User> Update(UserUpdateParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Users/Update/", parameters);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<User>();
            }

            throw new ApiException("Users.Update", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        public async Task UpdatePassword(UserUpdatePasswordParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Users/UpdatePassword/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Users.UpdatePassword", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task Delete(string id, bool throwExceptionIfError = true)
        {
            var response = await ApiClient.DeleteAsync($"Users/Delete/?id={id}");

            if (!response.IsSuccessStatusCode && throwExceptionIfError)
            {
                throw new ApiException("Users.Delete", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
    }
}