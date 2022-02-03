namespace StarRepo.Data.Seed
{
    public static class ResourceHandler
    {
        public async static Task<T[]> ParseResourceAsync<T>(
            Func<string[], T> adapter,
            Type? targetType = null) 
            where T : class
        {
            targetType ??= typeof(T);
            var filename = $"Seed\\{targetType.UnderlyingSystemType.Name.ToLowerInvariant()}s.csv";
            var root = Path.GetDirectoryName(typeof(ResourceHandler).Assembly.Location);
            var result = (await File.ReadAllLinesAsync(Path.Combine(root!, filename)))
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => 
                adapter(line.Split(',')
                    .Select(part => part.Trim()).ToArray()))
                .ToArray();
            return result;
        }
    }
}
