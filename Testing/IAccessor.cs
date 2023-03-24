namespace Testing
{
    public interface IClaimsPrincipalAccessor
    {
        IEnumerable<string>? Roles { get; set; }
    }
}