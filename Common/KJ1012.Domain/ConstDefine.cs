namespace KJ1012.Domain
{
    public static class ConstDefine
    {

        public static byte CheckUserId = 255;

        public const string CollectionCenterToMqttTopic = "Sunny/CollectionCenter";

        public const string WebServiceToMqttTopic = "Sunny/WebService";
        public const string ProcessCenterToMqttTopic = "Sunny/ProcessCenter";

        public const string XlsxFilePath = "static\\xlsx\\";
        public const string PdfFilePath = "static\\pdf\\";
        public const string MqttProcessClientId = "V20001";
        public const string MqttCollectionClientId = "V20002";
        public const string MqttWebClientId = "V20003";
        public const string DataSettingAesKey = "www.zz-xl.com123";

        //public const string downMemberCountRedisKey = "downMemberCount";
        public static int DownMemberCountKey = 0;
        public static bool IsHostServer = false;
        public static bool IsDataHostServer = false;
        public static int ServerModel = 0;
        public static int Precision = 10;

    }
}
