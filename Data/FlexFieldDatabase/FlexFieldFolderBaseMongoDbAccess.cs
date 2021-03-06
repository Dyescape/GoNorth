using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoNorth.Config;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GoNorth.Data.FlexFieldDatabase
{
    /// <summary>
    /// Flex Field Folder Mongo Db  Base Access
    /// </summary>
    public class FlexFieldFolderBaseMongoDbAccess : BaseMongoDbAccess, IFlexFieldFolderDbAccess
    {
        /// <summary>
        /// Folder Collection
        /// </summary>
        protected IMongoCollection<FlexFieldFolder> _FolderCollection;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="collectionName">Name of the collection</param>
        /// <param name="configuration">Configuration</param>
        public FlexFieldFolderBaseMongoDbAccess(string collectionName, IOptions<ConfigurationData> configuration) : base(configuration)
        {
            _FolderCollection = _Database.GetCollection<FlexFieldFolder>(collectionName);
        }

        /// <summary>
        /// Creates a Flex Field folder
        /// </summary>
        /// <param name="folder">Folder to create</param>
        /// <returns>Created folder, with filled id</returns>
        public async Task<FlexFieldFolder> CreateFolder(FlexFieldFolder folder)
        {
            folder.Id = Guid.NewGuid().ToString();
            await _FolderCollection.InsertOneAsync(folder);

            return folder;
        }

        /// <summary>
        /// Returns a folder by its Id
        /// </summary>
        /// <param name="folderId">Folder id</param>
        /// <returns>Folder</returns>
        public async Task<FlexFieldFolder> GetFolderById(string folderId)
        {
            FlexFieldFolder folder = await _FolderCollection.Find(f => f.Id == folderId).FirstOrDefaultAsync();
            return folder;
        }

        /// <summary>
        /// Builds a flex field folder query for root folders
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="locale">Locale used for the collation</param>
        /// <returns>Flex Field folder Queryable</returns>
        private IFindFluent<FlexFieldFolder, FlexFieldFolder> BuildRootFolderQueryable(string projectId, string locale)
        {
            return _FolderCollection.Find(f => f.ProjectId == projectId && string.IsNullOrEmpty(f.ParentFolderId), new FindOptions {
                Collation = new Collation(locale, null, CollationCaseFirst.Off, CollationStrength.Primary)
            });
        }

        /// <summary>
        /// Returns all root folders for a project
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="start">Start of the query</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="locale">Locale used for the collation</param>
        /// <returns>Root Folders</returns>
        public async Task<List<FlexFieldFolder>> GetRootFoldersForProject(string projectId, int start, int pageSize, string locale)
        {
            List<FlexFieldFolder> folders = await BuildRootFolderQueryable(projectId, locale).SortBy(f => f.Name).Skip(start).Limit(pageSize).ToListAsync();
            return folders;
        }

        /// <summary>
        /// Returns the root folder count
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="locale">Locale used for the collation</param>
        /// <returns>Root Folder Count</returns>
        public async Task<int> GetRootFolderCount(string projectId, string locale)
        {
            int count = (int)await BuildRootFolderQueryable(projectId, locale).CountDocumentsAsync();
            return count;
        }

        /// <summary>
        /// Builds a flex field folder query for child folders
        /// </summary>
        /// <param name="folderId">Folder id for which the children should be requested</param>
        /// <param name="locale">Locale used for the collation</param>
        /// <returns>Flex Field folder Queryable</returns>
        private IFindFluent<FlexFieldFolder, FlexFieldFolder> BuildChildFolderQueryable(string folderId, string locale)
        {
            return _FolderCollection.Find(f => f.ParentFolderId == folderId, new FindOptions {
                Collation = new Collation(locale, null, CollationCaseFirst.Off, CollationStrength.Primary)
            });
        }

        /// <summary>
        /// Returns all Child folders for a folder
        /// </summary>
        /// <param name="folderId">Folder id for which the children should be requested</param>
        /// <param name="start">Start of the query</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="locale">Locale used for the collation</param>
        /// <returns>Child Folders</returns>
        public async Task<List<FlexFieldFolder>> GetChildFolders(string folderId, int start, int pageSize, string locale)
        {
            List<FlexFieldFolder> folders = await BuildChildFolderQueryable(folderId, locale).SortBy(f => f.Name).Skip(start).Limit(pageSize).ToListAsync();
            return folders;
        }

        /// <summary>
        /// Returns the child folder count
        /// </summary>
        /// <param name="folderId">Folder id for which the children should be requested</param>
        /// <param name="locale">Locale used for the collation</param>
        /// <returns>Count of child folders</returns>
        public async Task<int> GetChildFolderCount(string folderId, string locale)
        {
            int count = (int)await BuildChildFolderQueryable(folderId, locale).CountDocumentsAsync();
            return count;
        }

        /// <summary>
        /// Returns all folders to build a hierarchy of folders
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns>Folders with simple information</returns>
        public async Task<List<FlexFieldFolder>> GetFoldersForHierarchy(string projectId)
        {
            return await _FolderCollection.Find(f => f.ProjectId == projectId).Project(f => new FlexFieldFolder {
                Id = f.Id,
                ParentFolderId = f.ParentFolderId
            }).ToListAsync();
        }

        /// <summary>
        /// Updates a folder 
        /// </summary>
        /// <param name="folder">Folder</param>
        /// <returns>Task</returns>
        public async Task UpdateFolder(FlexFieldFolder folder)
        {
            await _FolderCollection.UpdateOneAsync(Builders<FlexFieldFolder>.Filter.Eq(f => f.Id, folder.Id), 
                                                   Builders<FlexFieldFolder>.Update.Set(p => p.Name, folder.Name).Set(p => p.Description, folder.Description).
                                                                                    Set(p => p.ImageFile, folder.ImageFile).Set(p => p.ThumbnailImageFile, folder.ThumbnailImageFile));
        }

        /// <summary>
        /// Moves a folder to a folder
        /// </summary>
        /// <param name="folderId">Folder to move</param>
        /// <param name="targetFolderId">Id of the folder to move the object to</param>
        /// <returns>Task</returns>
        public async Task MoveToFolder(string folderId, string targetFolderId)
        {
            await _FolderCollection.UpdateOneAsync(Builders<FlexFieldFolder>.Filter.Eq(f => f.Id, folderId), 
                                                   Builders<FlexFieldFolder>.Update.Set(p => p.ParentFolderId, targetFolderId));
        }

        /// <summary>
        /// Deletes a folder
        /// </summary>
        /// <param name="folder">Folder to delete</param>
        /// <returns>Task</returns>
        public async Task DeleteFolder(FlexFieldFolder folder)
        {
            DeleteResult result = await _FolderCollection.DeleteOneAsync(f => f.Id == folder.Id);
        }
    }
}