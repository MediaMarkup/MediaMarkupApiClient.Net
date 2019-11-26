using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MediaMarkup.Api.Models;
using MediaMarkup.Core;
using Newtonsoft.Json;

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
        public async Task<UserInvitation> Create(UserCreateParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("users/invitations", parameters);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<UserInvitation>();
            }

            throw new ApiException("Users.Create", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
        public async Task<User> GetById(string id, bool throwExceptionIfNull = false)
        {
            var response = await ApiClient.GetAsync($"users/{id}");

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
        public async Task<User> Update(string id, UserUpdateParameters parameters)
        {
            var response = await ApiClient.PutAsJsonAsync($"users/{id}", parameters);

            if (response.IsSuccessStatusCode)
            {
                return await GetById(id);
            }

            throw new ApiException("Users.Update", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [Obsolete("This method is not supported. Please use Update method", true)]
        public async Task UpdatePassword(UserUpdatePasswordParameters parameters)
        {
            throw new NotImplementedException("Please use Update method. This method is obsolete");
        }

        /// <inheritdoc />
        public async Task Delete(string id, bool throwExceptionIfError = true)
        {
            var response = await ApiClient.DeleteAsync($"users/{id}");

            if (!response.IsSuccessStatusCode && throwExceptionIfError)
            {
                throw new ApiException("Users.Delete", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
    }
}