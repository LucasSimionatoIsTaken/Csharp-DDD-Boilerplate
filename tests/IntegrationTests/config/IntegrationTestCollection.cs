namespace IntegrationTests.config;

[CollectionDefinition("Integration Tests")]
public class IntegrationTestCollection : ICollectionFixture<TestServer>
{
}