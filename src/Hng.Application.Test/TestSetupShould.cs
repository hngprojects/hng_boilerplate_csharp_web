using Xunit;

namespace Hng.Application.Test;

public class TestSetupShould
{
    [Fact]
    public void Pass_For_Setup_Sake()
    {
        Assert.False(1 * 1 == 2, "1 times 1 should not be equal to 2");
    }
}
