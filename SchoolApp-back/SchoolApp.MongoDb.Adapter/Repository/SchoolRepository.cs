using SchoolApp.CrossCutting.Extensions;
using SchoolApp.CrossCutting.ResultObjects;
using SchoolApp.Domain.DomainServices.Users;
using SchoolApp.Domain.Messages.Queries.School;
using SchoolApp.Domain.Messages.Queries.School.Dto;
using SchoolApp.Domain.Repositories;
using SchoolApp.MongoDb.Adapter.Mapper;
using MongoDB.Bson;
using MongoDB.Driver;
using SchoolApp.MongoDb.Adapter.Entities;
using SchoolApp.MongoDb.Adapter.Repository.Base;
using Serilog;

namespace SchoolApp.MongoDb.Adapter.Repository
{
    public class SchoolRepository : RepositoryBase<School>, ISchoolRepository
    {
        public SchoolRepository(IMongoClient mongoClient,
            IClientSessionHandle clientSessionHandle,
            UserServiceContext userServiceContext,
            IUserRepository userRepository,
            ILogger logger
        ) : base(mongoClient, clientSessionHandle, logger, userServiceContext, GetIndexes())
        {
        }

        private static IEnumerable<CreateIndexModel<School>> GetIndexes()
        {
            var uniqueTaxDocumentIndex = new CreateIndexModel<School>(
                Builders<School>.IndexKeys.Ascending(u => u.TaxDocument),
                new CreateIndexOptions { Unique = true });
            return new List<CreateIndexModel<School>> { uniqueTaxDocumentIndex };
        }

        public async Task<Domain.Entities.School?> GetAsync(string id, CancellationToken cancellationToken)
        {
            _logger.Information("Retrieving school entity by ID: {id}", id);
            try
            {
                var school = await this.GetByIdAsync(id, cancellationToken);
                _logger.Information("School entity retrieved successfully by ID: {id}", id);
                return school.ToSchoolDomain();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving school entity by ID: {id}", id);
                throw;
            }
        }

        public async Task<string> AddAsync(Domain.Entities.School school, CancellationToken cancellationToken)
        {
            _logger.Information("Adding school entity with TaxDocument: {taxDocument}", school.TaxDocument);
            try
            {
                var c = new School
                {
                    FullName = school.FullName,
                    TaxDocument = school.TaxDocument.RemoveSpacesAndSpecialCharacters(),
                    Avatar = school.Avatar,
                    Address = school.Address,
                    PhoneNumber = school.PhoneNumber,
                    Email = school.Email,
                    CreatedAt = DateTimeOffset.UtcNow,
                    Status = school.Status,
                };
                var result = await InsertAsync(c, cancellationToken);
                _logger.Information("School entity added successfully with ID: {id}", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding school entity with TaxDocument: {taxDocument}", school.TaxDocument);
                throw;
            }
        }

        public async Task<string> RemoveAsync(string id, CancellationToken cancellationToken)
        {
            _logger.Information("Removing school entity by ID: {id}", id);
            try
            {
                await base.DeleteAsync(id, cancellationToken);
                _logger.Information("School entity removed successfully by ID: {id}", id);
                return id;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error removing school entity by ID: {id}", id);
                throw;
            }
        }

        public async Task<string> UpdateAsync(string id, Domain.Entities.School school,
            CancellationToken cancellationToken)
        {
            _logger.Information("Updating school entity with ID: {id}", id);
            try
            {
                var c = new School
                {
                    Id = new ObjectId(id),
                    FullName = school.FullName,
                    TaxDocument = school.TaxDocument.RemoveSpacesAndSpecialCharacters(),
                    Avatar = school.Avatar,
                    Address = school.Address,
                    PhoneNumber = school.PhoneNumber,
                    Email = school.Email,
                    Status = school.Status,
                };
                await UpdateAsync(id, c, cancellationToken);
                _logger.Information("School entity updated successfully with ID: {id}", id);
                return id;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating school entity with ID: {id}", id);
                throw;
            }
        }

        public async Task<Domain.Entities.School?> GetByTaxDocumentAsync(string taxDocument,
            CancellationToken cancellationToken)
        {
            _logger.Information("Retrieving school entity by TaxDocument: {taxDocument}", taxDocument);
            try
            {
                var filter = FilterBuilder.Eq("TaxDocument", taxDocument.RemoveSpacesAndSpecialCharacters());
                var clinic = await GetSingleAsync(filter, cancellationToken);
                _logger.Information("School entity retrieved successfully by TaxDocument: {taxDocument}", taxDocument);
                return clinic.ToSchoolDomain();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving school entity by TaxDocument: {taxDocument}", taxDocument);
                throw;
            }
        }

        public async Task<Pagination<SchoolQueryListResult>> Handle(GetFilteredSchoolQuery request,
            CancellationToken cancellationToken)
        {
            _logger.Information("Handling GetFilteredSchoolQuery | Filter: {filter} | PageNumber: {pageNumber} | PageSize: {pageSize}", request.FilterValue,
                request.PagedRequest.PageNumber, request.PagedRequest.PageSize);
            try
            {
                var inputList = !string.IsNullOrWhiteSpace(request.FilterValue) ? request.FilterValue.Split(" ") : null;
                var filterList = new List<FilterDefinition<School>>();

                if (request.PagedRequest.Status != null)
                {
                    filterList.Add(FilterBuilder.Eq(s => s.Status, request.PagedRequest.Status.Value));
                }

                if (inputList != null)
                {
                    foreach (var item in inputList)
                    {
                        var fullNameFilter = FilterBuilder.Regex("FullName", new BsonRegularExpression(item, "i"));
                        var taxDocumentFilter = FilterBuilder.Regex("TaxDocument", new BsonRegularExpression(item, "i"));
                        filterList.Add(fullNameFilter);
                        filterList.Add(taxDocumentFilter);
                    }
                }

                var filter = filterList.Count != 0 ? FilterBuilder.Or(filterList) : FilterBuilder.Empty;

                var pagedResult = await GetByFilterPagedAsync(filter, request.PagedRequest.PageNumber,
                    request.PagedRequest.PageSize,
                    c => new SchoolQueryListResult()
                        { Avatar = c.Avatar, Email = c.Email, Status = c.Status, PhoneNumber = c.PhoneNumber, FullName = c.FullName, Id = c.Id.ToString() }, cancellationToken);

                _logger.Information("Handled GetFilteredSchoolQuery successfully | TotalItems: {totalItems} | PageNumber: {pageNumber} | PageSize: {pageSize}",
                    pagedResult.TotalItems, pagedResult.PageNumber, pagedResult.PageSize);

                return new Pagination<SchoolQueryListResult>(pagedResult.Items, pagedResult.TotalItems, pagedResult.PageNumber, pagedResult.PageSize);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error handling GetFilteredSchoolQuery | Filter: {filter} | PageNumber: {pageNumber} | PageSize: {pageSize}", request.FilterValue,
                    request.PagedRequest.PageNumber, request.PagedRequest.PageSize);
                throw;
            }
        }

        public async Task<SchoolQueryResult> Handle(GetSchoolByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Handling GetSchoolByIdQuery | ID: {id}", request.Id);
            try
            {
                var school = await this.GetByIdAsync(request.Id, cancellationToken);
                var result = school.ToSchoolQueryResult();
                _logger.Information("Handled GetSchoolByIdQuery successfully | ID: {id}", request.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error handling GetSchoolByIdQuery | ID: {id}", request.Id);
                throw;
            }
        }
    }
}