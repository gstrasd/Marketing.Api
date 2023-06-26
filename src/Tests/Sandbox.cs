using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class Sandbox
    {
        [Fact]
        public void Miscellaneous()
        {
            var foo = "this is a string much larger than the 64 characters I am using to test the range operator";


            Assert.True(true);
        }
    }
}
