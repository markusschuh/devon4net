using AutoMapper;
using OASP4Net.Business.Common;
using OASP4Net.Infrastructure.Test;


namespace OASP4Net.Test.xUnit.Test.UnitTest
{
    public class MyThaiStarUnitTest : BaseManagementTest
    {
        public override void ConfigureMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutomapperProfile());
            });
            Mapper = mockMapper.CreateMapper();
        }
    }
}
