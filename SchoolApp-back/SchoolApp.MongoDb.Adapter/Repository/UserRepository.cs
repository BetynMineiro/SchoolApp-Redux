using SchoolApp.CrossCutting.Extensions;
using SchoolApp.CrossCutting.ResultObjects;
using SchoolApp.Domain.DomainServices.Users;
using SchoolApp.Domain.Enum.Management;
using SchoolApp.Domain.Messages.Queries.User;
using SchoolApp.Domain.Messages.Queries.User.Dto;
using SchoolApp.Domain.Repositories;
using SchoolApp.MongoDb.Adapter.Mapper;
using MongoDB.Bson;
using MongoDB.Driver;
using SchoolApp.MongoDb.Adapter.Entities;
using SchoolApp.MongoDb.Adapter.Repository.Base;
using Serilog;

namespace SchoolApp.MongoDb.Adapter.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(
            IMongoClient mongoClient,
            IClientSessionHandle clientSessionHandle,
            UserServiceContext userServiceContext,  
            ILogger logger
        ) : base(mongoClient, clientSessionHandle, logger, userServiceContext, GetIndexes())
        {
        }

        private static IEnumerable<CreateIndexModel<User>> GetIndexes()
        {
            var uniqueTaxDocumentIndex = new CreateIndexModel<User>(
                Builders<User>.IndexKeys.Ascending(u => u.TaxDocument),
                new CreateIndexOptions { Unique = true });
            return new List<CreateIndexModel<User>> { uniqueTaxDocumentIndex };
        }

        public async Task<Domain.Entities.User?> GetAsync(string id, CancellationToken cancellationToken)
        {
            _logger.Information("Retrieving user entity by ID: {id}", id);
            try
            {
                var user = await this.GetByIdAsync(id, cancellationToken);
                _logger.Information("User entity retrieved successfully by ID: {id}", id);
                return user.ToUserDomain();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving user entity by ID: {id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Domain.Entities.User?>> GetAsync(IEnumerable<string> ids, CancellationToken cancellationToken)
        {
            _logger.Information("Retrieving user entities by IDs");
            try
            {
                var filter = FilterBuilder.In("Id", ids);
                var users = await GetByFilterAsync(filter, cancellationToken);
                _logger.Information("User entities retrieved successfully by IDs");
                return users.Select(u => u.ToUserDomain());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving user entities by IDs");
                throw;
            }
        }

        public async Task<string> AddAsync(Domain.Entities.User user, CancellationToken cancellationToken)
        {
            _logger.Information("Adding user entity with TaxDocument: {taxDocument}", user.TaxDocument);
            try
            {
                var p = new User()
                {
                    UserAuthId = user.UserAuthId,
                    TaxDocument = user.TaxDocument.RemoveSpacesAndSpecialCharacters(),
                    ProfileType = user.ProfileType,
                    FullName = user.FullName,
                    SocialName = user.SocialName,
                    BirthDay = user.BirthDay,
                    Avatar = user.Avatar,
                    PersonType = user.PersonType,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Gender = user.Gender,
                    Status = user.Status,
                    ManagedSchoolId = user.ManagedSchoolId,
                };
                var result = await InsertAsync(p, cancellationToken);
                _logger.Information("User entity added successfully with ID: {id}", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding user entity with TaxDocument: {taxDocument}", user.TaxDocument);
                throw;
            }
        }

        public async Task<string> RemoveAsync(string id, CancellationToken cancellationToken)
        {
            _logger.Information("Removing user entity by ID: {id}", id);
            try
            {
                await base.DeleteAsync(id, cancellationToken);
                _logger.Information("User entity removed successfully by ID: {id}", id);
                return id;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error removing user entity by ID: {id}", id);
                throw;
            }
        }

        public async Task<string> UpdateAsync(string id, Domain.Entities.User user, CancellationToken cancellationToken)
        {
            _logger.Information("Updating user entity with ID: {id}", id);
            try
            {
                var p = new User()
                {
                    Id = new ObjectId(id),
                    TaxDocument = user.TaxDocument.RemoveSpacesAndSpecialCharacters(),
                    ProfileType = user.ProfileType,
                    FullName = user.FullName,
                    SocialName = user.SocialName,
                    BirthDay = user.BirthDay,
                    Avatar = user.Avatar,
                    PersonType = user.PersonType,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Gender = user.Gender,
                    Status = user.Status,
                    ManagedSchoolId = user.ManagedSchoolId,
                };
                await UpdateAsync(id, p, cancellationToken);
                _logger.Information("User entity updated successfully with ID: {id}", id);
                return id;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating user entity with ID: {id}", id);
                throw;
            }
        }

        public async Task<Domain.Entities.User?> GetByTaxDocumentAsync(string? taxDocument, CancellationToken cancellationToken)
        {
            _logger.Information("Retrieving user entity by TaxDocument: {taxDocument}", taxDocument);
            try
            {
                var filter = FilterBuilder.Eq("TaxDocument", taxDocument.RemoveSpacesAndSpecialCharacters());
                var user = await GetSingleAsync(filter, cancellationToken);
                _logger.Information("User entity retrieved successfully by TaxDocument: {taxDocument}", taxDocument);
                return user.ToUserDomain();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving user entity by TaxDocument: {taxDocument}", taxDocument);
                throw;
            }
        }

        public async Task<Pagination<UserQueryResultForList>> Handle(GetFilteredUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Handling GetFilteredUsersQuery | Filter: {filter} | PageNumber: {pageNumber} | PageSize: {pageSize}", request.FilterValue, request.PagedRequest.PageNumber, request.PagedRequest.PageSize);
            try
            {
                var inputList = !string.IsNullOrWhiteSpace(request.FilterValue) ? request.FilterValue.Split(" ") : null;
                var filterList = new List<FilterDefinition<User>>();

                if (request.PagedRequest.Status != null)
                {
                    filterList.Add(FilterBuilder.Eq(s => s.Status, request.PagedRequest.Status.Value));
                }

                if (inputList != null)
                {
                    foreach (var item in inputList)
                    {
                        var fullNameFilter = FilterBuilder.Regex("FullName", new BsonRegularExpression(item, "i"));
                        var socialNameFilter = FilterBuilder.Regex("SocialName", new BsonRegularExpression(item, "i"));
                        var taxDocumentFilter = FilterBuilder.Regex("TaxDocument", new BsonRegularExpression(item, "i"));
                        var emailFilter = FilterBuilder.Regex("Email", new BsonRegularExpression(item, "i"));
                        var phoneNumberFilter = FilterBuilder.Regex("PhoneNumber", new BsonRegularExpression(item, "i"));

                        filterList.Add(fullNameFilter);
                        filterList.Add(socialNameFilter);
                        filterList.Add(taxDocumentFilter);
                        filterList.Add(emailFilter);
                        filterList.Add(phoneNumberFilter);
                    }
                }

                if (request.ProfileType != ProfileType.None)
                {
                    filterList.Add(FilterBuilder.Eq("ProfileType", request.ProfileType));
                }

                var filter = filterList.Count != 0 ? FilterBuilder.Or(filterList) : FilterBuilder.Empty;

                var pagedResult = await GetByFilterPagedAsync(filter, request.PagedRequest.PageNumber, request.PagedRequest.PageSize, cancellationToken);

                _logger.Information("Handled GetFilteredUsersQuery successfully | TotalItems: {totalItems} | PageNumber: {pageNumber} | PageSize: {pageSize}", pagedResult.TotalItems, pagedResult.PageNumber, pagedResult.PageSize);

                return new Pagination<UserQueryResultForList>(pagedResult.Items.ToUserQueryResultList(), pagedResult.TotalItems, pagedResult.PageNumber, pagedResult.PageSize);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error handling GetFilteredUsersQuery | Filter: {filter} | PageNumber: {pageNumber} | PageSize: {pageSize}", request.FilterValue, request.PagedRequest.PageNumber, request.PagedRequest.PageSize);
                throw;
            }
        }

        public async Task<UserQueryResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Handling GetUserByIdQuery | ID: {id}", request.Id);
            try
            {
                var user = await this.GetByIdAsync(request.Id, cancellationToken);
                var result = user.ToUserQueryResult();
                _logger.Information("Handled GetUserByIdQuery successfully | ID: {id}", request.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error handling GetUserByIdQuery | ID: {id}", request.Id);
                throw;
            }
        }

        public async Task<UserByAuthIdQueryResult> Handle(GetUserByUserAuthIdQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Handling GetUserByUserAuthIdQuery | UserAuthId: {userAuthId}", UserServiceContext.UserAuth.Id);
            try
            {
                var filter = FilterBuilder.Eq(c => c.UserAuthId, UserServiceContext.UserAuth.Id);
                var user = await this.GetSingleAsync(filter, cancellationToken);
                var result = user.ToUserByAuthIdQueryResult();
                _logger.Information("Handled GetUserByUserAuthIdQuery successfully | UserAuthId: {userAuthId}", UserServiceContext.UserAuth.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error handling GetUserByUserAuthIdQuery | UserAuthId: {userAuthId}", UserServiceContext.UserAuth.Id);
                throw;
            }
        }
    }
}