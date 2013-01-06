using HelloData.FrameWork.Data.Enum;

namespace HelloData.FrameWork.Data
{
    public class BaseVEntity : BaseEntity
    {
        public new TableTyleEnum TableType = TableTyleEnum.View;  
    }
}
