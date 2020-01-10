using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Zomato.Repository.Test.Bootstrap;

namespace Zomato.Repository.Test.Bootstrap
{
    [CollectionDefinition("Register Dependency")]
    public class Fixture : ICollectionFixture<Initialize>
    {
    }
}