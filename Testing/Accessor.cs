namespace Testing
{
    public class ClaimsPrincipalAccessor : IClaimsPrincipalAccessor
    {
        private readonly AsyncLocal<ContextHolder> _context = new();

        public IEnumerable<string>? Roles
        {
            get => _context.Value?.Roles;
            set
            {
                var holder = _context.Value;
                if (holder is not null)
                {
                    holder.Roles = null;
                }

                if (value is not null)
                {
                    _context.Value = new ContextHolder { Roles = value };
                }
            }
        }

        private class ContextHolder
        {
            public IEnumerable<string>? Roles;
        }
    }
}