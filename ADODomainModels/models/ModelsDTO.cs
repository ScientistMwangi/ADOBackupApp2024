using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.WebApi;

namespace ADODomainModels.models
{
    internal class ModelsDTO
    {
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Avatar
    {
        public string Href { get; set; }
    }

    public class CreatedBy
    {
        public Descriptor Descriptor { get; set; }
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public Link Link { get; set; }
        public string Id { get; set; }
        public string UniqueName { get; set; }
        public object DirectoryAlias { get; set; }
        public object ProfileUrl { get; set; }
        public string ImageUrl { get; set; }
        public bool IsContainer { get; set; }
        public bool IsAadIdentity { get; set; }
        public bool Inactive { get; set; }
        public bool IsDeletedInOrigin { get; set; }
        public string DisplayNameForXmlSerialization { get; set; }
        public string UrlForXmlSerialization { get; set; }
    }

    public class Descriptor
    {
        public string SubjectType { get; set; }
        public string Identifier { get; set; }
    }

    public class LastMergeCommit
    {
        public string CommitId { get; set; }
        public object Author { get; set; }
        public object Committer { get; set; }
        public object Comment { get; set; }
        public bool CommentTruncated { get; set; }
        public object ChangeCounts { get; set; }
        public object Changes { get; set; }
        public object Parents { get; set; }
        public string Url { get; set; }
        public object RemoteUrl { get; set; }
        public object Links { get; set; }
        public object Statuses { get; set; }
        public object WorkItems { get; set; }
        public object Push { get; set; }
        public bool CommitTooManyChanges { get; set; }
    }

    public class LastMergeSourceCommit
    {
        public string CommitId { get; set; }
        public object Author { get; set; }
        public object Committer { get; set; }
        public object Comment { get; set; }
        public bool CommentTruncated { get; set; }
        public object ChangeCounts { get; set; }
        public object Changes { get; set; }
        public object Parents { get; set; }
        public string Url { get; set; }
        public object RemoteUrl { get; set; }
        public object Links { get; set; }
        public object Statuses { get; set; }
        public object WorkItems { get; set; }
        public object Push { get; set; }
        public bool CommitTooManyChanges { get; set; }
    }

    public class LastMergeTargetCommit
    {
        public string CommitId { get; set; }
        public object Author { get; set; }
        public object Committer { get; set; }
        public object Comment { get; set; }
        public bool CommentTruncated { get; set; }
        public object ChangeCounts { get; set; }
        public object Changes { get; set; }
        public object Parents { get; set; }
        public string Url { get; set; }
        public object RemoteUrl { get; set; }
        public object Links { get; set; }
        public object Statuses { get; set; }
        public object WorkItems { get; set; }
        public object Push { get; set; }
        public bool CommitTooManyChanges { get; set; }
    }

    public class Link
    {
        public Link Links { get; set; }
        public Avatar avatar { get; set; }
    }

    public class ProjectReference
    {
        public string Id { get; set; }
        public object Abbreviation { get; set; }
        public string Name { get; set; }
        public object Description { get; set; }
        public object Url { get; set; }
        public int State { get; set; }
        public int Revision { get; set; }
        public int Visibility { get; set; }
        public object DefaultTeamImageUrl { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }

    public class Repository
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public ProjectReference ProjectReference { get; set; }
        public object DefaultBranch { get; set; }
        public object Size { get; set; }
        public object RemoteUrl { get; set; }
        public object SshUrl { get; set; }
        public object WebUrl { get; set; }
        public object ValidRemoteUrls { get; set; }
        public bool IsFork { get; set; }
        public object ParentRepository { get; set; }
        public object Links { get; set; }
        public object IsDisabled { get; set; }
        public object IsInMaintenance { get; set; }
    }

    public class RootModel
    {
        public GitRepository Repository { get; set; }
        public int PullRequestId { get; set; }
        public int CodeReviewId { get; set; }
        public PullRequestStatus Status { get; set; }
        public CreatedBy CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ClosedDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SourceRefName { get; set; }
        public string TargetRefName { get; set; }
        public PullRequestAsyncStatus MergeStatus { get; set; }
        public PullRequestMergeFailureType MergeFailureType { get; set; }
        public string MergeFailureMessage { get; set; }
        public bool IsDraft { get; set; }
        public bool HasMultipleMergeBases { get; set; }
        public Guid MergeId { get; set; }
        public GitCommitRef LastMergeSourceCommit { get; set; }
        public GitCommitRef LastMergeTargetCommit { get; set; }
        public GitCommitRef LastMergeCommit { get; set; }
        public IdentityRefWithVote [] Reviewers { get; set; }

        public WebApiTagDefinition [] Labels { get; set; }
        public GitCommitRef[]? Commits { get; set; }
        public string Url { get; set; }
        public string RemoteUrl { get; set; }
        public ReferenceLinks Links { get; set; }
        public GitPullRequestCompletionOptions CompletionOptions { get; set; }
        public GitPullRequestMergeOptions MergeOptions { get; set; }
        public bool SupportsIterations { get; set; }
        public ResourceRef [] WorkItemRefs { get; set; }
        public DateTime CompletionQueueTime { get; set; }
        public IdentityRef? ClosedBy { get; set; }
        public IdentityRef ? AutoCompleteSetBy { get; set; }
        public object ArtifactId { get; set; }
        public GitForkRef ForkSource { get; set; }
    }


}
