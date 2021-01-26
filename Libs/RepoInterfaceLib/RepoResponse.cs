namespace RepoInterfaceLib
{
    public enum RepoOperationStatus
    {
        Success = 0,
        Failure
    }

    public class RepoResponse
    {
        public RepoOperationStatus OpStatus { private get; set; }

        public string Status => $"{OpStatus}";
        public string Message { get; set; }

        public bool IsOK => OpStatus == RepoOperationStatus.Success;
    }
}
