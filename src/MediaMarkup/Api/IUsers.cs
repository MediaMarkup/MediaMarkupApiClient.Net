using System.Threading.Tasks;
using MediaMarkup.Api.Models;

namespace MediaMarkup.Api
{
    public interface IUsers
    {
        /// <summary>
        /// Creates a new user with the specified parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<User> Create(UserCreateParameters parameters);

        /// <summary>
        /// Invites a new user with the specified parameters.
        /// User received an invitation email with expiry to complete account creation and password
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<UserInvitation> Invite(UserInviteParameters parameters);

        /// <summary>
        /// Gets a user by the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="throwExceptionIfNull">If the result is null for any reaason (not found or errors), specify true to throw an exception which will provide additional information</param>
        /// <returns></returns>
        Task<User> GetById(string id, bool throwExceptionIfNull = false);

        /// <summary>
        /// Gets a user by the specified email address.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="throwExceptionIfNull">If the result is null for any reaason (not found or errors), specify true to throw an exception which will provide additional information</param>
        /// <returns></returns>
        Task<User> GetByEmail(string email, bool throwExceptionIfNull = false);

        /// <summary>
        /// Updates the specified user with the supplied parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<User> Update(string id, UserUpdateParameters parameters);

        /// <summary>
        /// Updates the specified user's password.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task UpdatePassword(UserUpdatePasswordParameters parameters);

        /// <summary>
        /// Deletes a user by the specified user id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="throwExceptionIfError">Set to throw an exception if there is an error, eg the user is not found. Default is true</param>
        /// <returns></returns>
        Task Delete(string id, bool throwExceptionIfError = true);
    }
}