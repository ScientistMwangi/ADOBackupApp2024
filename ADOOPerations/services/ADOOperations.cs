using ADODomainModels.models;
using CoreOPerations.interfaces;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Xml.Linq;

namespace CoreOPerations.services
{
    public class ADOOperations: IADOOperations
    {
        private GitHttpClient _gitHttpClient;
        private GitHttpClientModel _gitHttpClientModel;
        private Dictionary<string, List<long>> _currentSavedFilesFilenameAskey;
        private Dictionary<long, List<string>> _currentSavedFilesDateAsKey;
        public ADOOperations(GitHttpClientModel gitHttpClientModel)
        {
            _gitHttpClientModel = gitHttpClientModel;
            _gitHttpClient = GetGitClient(gitHttpClientModel);
            var currentFilesBackup = FileOperations.GetCurrentFilesHashTable();
            _currentSavedFilesFilenameAskey = currentFilesBackup.Item1;
            _currentSavedFilesDateAsKey = currentFilesBackup.Item2;
        }

        public async Task ADOCreatePullRequestBackup()
        {
            Console.WriteLine();
            if (_gitHttpClient == null) return;
            var pullRequestSearchCriteria = new GitPullRequestSearchCriteria
            {
                Status = PullRequestStatus.Active,
            };

            // Retrieve pull requests for the specified project and repository
            var pullRequests = await _gitHttpClient
                .GetPullRequestsAsync(
                _gitHttpClientModel.ProjectName,
                _gitHttpClientModel.RepositoryName, 
                pullRequestSearchCriteria);

            Console.WriteLine($" {pullRequests.Count} : active PRs found!");
            // Display information about each pull request
            int count = 1;
            foreach (var pullRequest in pullRequests)
            {
                try
                {
                    var prName = pullRequest.Title.ToLower();
                    Console.WriteLine($"Preparation to write pr to file...");

                    Console.WriteLine($"Pr details...{prName}, Source: {pullRequest.SourceRefName} " +
                        $"and target: {pullRequest.TargetRefName}");
                    // Check if there have been an update / it's new
                    if (_currentSavedFilesFilenameAskey.ContainsKey(prName))
                    {
                        var currentPRversions = _currentSavedFilesFilenameAskey[prName];
                        Console.WriteLine($"previous version of {prName} pr exist checking for an update...");

                        string incomingPrJson = JsonSerializer.Serialize(pullRequest);
                        Array.Sort(currentPRversions.ToArray());
                        var mostLastestVersion = currentPRversions[0];
                        //var fileNameWithTimeStamp = GenericHelper.GetCustomFileName(fileName);
                        var fileName = $"{prName}_{mostLastestVersion}.json";
                        string saveLatestPrVersion = FileOperations.ReadJsonFromFile(fileName);

                        if (incomingPrJson != saveLatestPrVersion)
                        {
                            Console.WriteLine($"Found new version of {prName}, creating a backup...");
                            CreatePrBackUp(incomingPrJson, fileName);
                        }
                        else
                        {
                            Console.WriteLine($"Already have the latest backup of {prName}.");
                        }

                    }
                    else
                    {
                        Console.WriteLine($"New pr found {prName}...");
                        // Just create a file backup
                        string incomingPrJson = JsonSerializer.Serialize(pullRequest);
                        CreatePrBackUp(incomingPrJson, prName);
                    }
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"Write pr to the file failed: Name:" +
                        $" {pullRequest.Title}, Source: {pullRequest.SourceRefName} " +
                        $"and target: {pullRequest.TargetRefName}" +
                        $" exception: {ex.Message} inner exception: {ex?.InnerException?.Message}");
                    continue;
                }
            }
        }

        public async Task RestorePrsByDateTime()
        {
            Console.WriteLine();
            var date2 = _gitHttpClientModel.StartDate;
            Console.WriteLine($"Checking backup for date: {date2}.....");
            DateTime date = (DateTime)_gitHttpClientModel.StartDate;
            long tickToNearestSeconds = date.Round(TimeSpan.TicksPerSecond).Ticks;
            if (_currentSavedFilesDateAsKey.ContainsKey(tickToNearestSeconds))
            {
                var files = _currentSavedFilesDateAsKey[tickToNearestSeconds];
                string source = string.Empty;
                string target = string.Empty;
                foreach(var file in files)
                {
                    var fileName = $"{file.ToLower()}_{tickToNearestSeconds}.json";
                    try
                    {
                        string prJsonBackup = FileOperations.ReadJsonFromFile(fileName);
                        var requestObject = JsonSerializer.Deserialize<RootModel>(prJsonBackup);
                        source = requestObject?.SourceRefName!;
                        target = requestObject?.TargetRefName!;
                        Console.WriteLine($"{requestObject?.Title!}: Pr restore request object created");
                        if(requestObject != null)
                            await CreateADOPullRequest(requestObject);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed restoring ADO the pr source: {source} and target: {target} " +
                            $"exception: {ex.Message} inner exception: {ex?.InnerException?.Message}");
                        continue;
                    }
                }
            }
            else
            {
                Console.WriteLine($"No backup found for date: {date}.....");
            }
        }

        /// <summary>
        ///  Assumption the pr's does not exist else it will throw and exception
        /// </summary>
        /// <param name="gitPullRequest"></param>
        /// <returns></returns>
        private async Task CreateADOPullRequest(RootModel rootModel)
        {
            Console.WriteLine();
            if (_gitHttpClient == null) return;
            var resquestModel = GetPrGitPullRequestMetadata(rootModel);
            try
            {
                Console.WriteLine($"Restoring ADO pr from the backup file name: {rootModel.Title.ToLower()}.........");
                resquestModel.Title = resquestModel.Title.ToLower();
                var createdPr = await _gitHttpClient
                    .CreatePullRequestAsync(resquestModel, _gitHttpClientModel.ProjectName, _gitHttpClientModel.RepositoryName);
                Console.WriteLine($"Pull request created successfully. ID: {createdPr.PullRequestId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed create the pr source: {resquestModel.SourceRefName} and target: {resquestModel.TargetRefName} exception: {ex.Message}  " +
                    $"  inner exception: {ex?.InnerException?.Message}");
            }
        }

        #region private methods

        private void CreatePrBackUp(string jsonContent, string fileName)
        {
            Console.WriteLine();
            var fileNameWithTimeStamp = GenericHelper.GetCustomFileName(fileName);
            Console.WriteLine($"Started creating file backup.....filename: {fileName}");
            FileOperations.WriteToFile(jsonContent, fileNameWithTimeStamp);
            Console.WriteLine($"Backup file created successfuly!");
        }

        private GitHttpClient GetGitClient(GitHttpClientModel gitHttpClientModel)
        {
            Console.WriteLine();
            // Replace these values with your own Azure DevOps organization URL and personal access token
            //https://dev.azure.com/kibuikamwangi2024/_usersSettings/about
            string organizationUrl = gitHttpClientModel.OrganizationUrl;
            string personalAccessToken = gitHttpClientModel.PersonalAccessToken;
            try
            {
                Console.WriteLine($"Getting ADO connetion using....");
                Console.WriteLine($"OrganizationUrl: {organizationUrl}, ");
                Console.WriteLine($"Project Name: {gitHttpClientModel.ProjectName}, ");
                Console.WriteLine($"Repository Name: {gitHttpClientModel.RepositoryName}, ");
                Console.WriteLine($"PAT: ###################################################### ");

                VssConnection connection = new VssConnection(
                    new Uri(organizationUrl),
                    new VssBasicCredential(string.Empty, personalAccessToken));
                // Get a reference to the GitHttpClient
                GitHttpClient gitClient = connection.GetClient<GitHttpClient>();
                return gitClient;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get ado connetion exception: {ex.Message}    inner exception: {ex?.InnerException?.Message}");
            }
            return null;
        }

        private static GitPullRequest GetPrGitPullRequestMetadata(RootModel rootModel)
        {
            var request = new GitPullRequest();

            request.AutoCompleteSetBy = rootModel.AutoCompleteSetBy;
            request.ClosedBy = rootModel.ClosedBy;
            request.ClosedDate = rootModel.ClosedDate;
            request.CodeReviewId = rootModel.CodeReviewId;
            request.Commits = rootModel.Commits;
            request.CompletionOptions = rootModel.CompletionOptions;
            request.CompletionQueueTime = rootModel.CompletionQueueTime;
            request.CreatedBy = rootModel.ClosedBy;
            request.CreationDate = rootModel.CreationDate;
            request.Description = rootModel.Description;
            request.ForkSource = rootModel.ForkSource;
            request.Labels = rootModel.Labels;
            request.LastMergeCommit = rootModel.LastMergeCommit;
            request.LastMergeSourceCommit = rootModel.LastMergeSourceCommit;
            request.LastMergeTargetCommit = rootModel.LastMergeTargetCommit;
            request.Links = rootModel.Links;
            request.MergeFailureMessage = rootModel.MergeFailureMessage;
            request.MergeFailureType = rootModel.MergeFailureType;
            request.MergeId = rootModel.MergeId;
            request.MergeOptions = rootModel.MergeOptions;
            request.MergeStatus = rootModel.MergeStatus;
            request.PullRequestId = rootModel.PullRequestId;
            request.RemoteUrl = rootModel.RemoteUrl;
            request.Repository = rootModel.Repository;
            request.Reviewers = rootModel.Reviewers;
            request.SourceRefName = rootModel.SourceRefName;
            request.Status = rootModel.Status;
            request.SupportsIterations = rootModel.SupportsIterations;
            request.TargetRefName = rootModel.TargetRefName;
            request.Title = rootModel.Title;
            request.Url = rootModel.Url;
            request.WorkItemRefs = rootModel.WorkItemRefs;

            return request;
        }

        #endregion
    }
}
