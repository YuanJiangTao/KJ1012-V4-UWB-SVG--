using KJ1012.Core.Data;

namespace KJ1012.AppSetting.IServices
{
   public interface IAppSettingService: ICustomService
    {
       void SaveDataSetting(DataSettings dataSettings);
       DataSettings GetDataSetting();
    }
}
